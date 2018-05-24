using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using Tweetinvi.Controllers.Properties;
using Tweetinvi.Controllers.Shared;
using Tweetinvi.Core;
using Tweetinvi.Core.Extensions;
using Tweetinvi.Core.Helpers;
using Tweetinvi.Core.QueryGenerators;
using Tweetinvi.Models;
using Tweetinvi.Models.DTO;
using Tweetinvi.Parameters;

namespace Tweetinvi.Controllers.Tweet
{
    public class TweetQueryGenerator : ITweetQueryGenerator
    {
        private readonly IQueryParameterGenerator _queryParameterGenerator;
        private readonly ITweetQueryValidator _tweetQueryValidator;
        private readonly ITweetinviSettingsAccessor _tweetinviSettingsAccessor;
        private readonly ITwitterStringFormatter _twitterStringFormatter;

        public TweetQueryGenerator(
            IQueryParameterGenerator queryParameterGenerator,
            ITweetQueryValidator tweetQueryValidator,
            ITweetinviSettingsAccessor tweetinviSettingsAccessor,
            ITwitterStringFormatter twitterStringFormatter)
        {
            _queryParameterGenerator = queryParameterGenerator;
            _tweetQueryValidator = tweetQueryValidator;
            _tweetinviSettingsAccessor = tweetinviSettingsAccessor;
            _twitterStringFormatter = twitterStringFormatter;
        }

        private string CleanupString(string source)
        {
            return _twitterStringFormatter.TwitterEncode(source);
        }

        // Get Tweet
        public string GetTweetQuery(long tweetId)
        {
            _tweetQueryValidator.ThrowIfTweetCannotBeUsed(tweetId);

            var query = new StringBuilder(string.Format(Resources.Tweet_Get, tweetId));
            query.AddFormattedParameterToQuery(_queryParameterGenerator.GenerateTweetModeParameter(_tweetinviSettingsAccessor.CurrentThreadSettings.TweetMode));

            return query.ToString();
        }

        public string GetTweetsQuery(IEnumerable<long> tweetIds)
        {
            if (tweetIds == null)
            {
                throw new ArgumentNullException("Tweet Ids cannot be null.");
            }

            var tweetIdsAsList = new List<long>(tweetIds);

            if (!tweetIdsAsList.Any())
            {
                throw new ArgumentException("Tweet Ids cannot be empty.");
            }

            // Uses URL encoded comma (%2C) so that the parameter doesn't need to be URL encoded
            var idsParameter = string.Join("%2C", tweetIdsAsList);

            var query = new StringBuilder(string.Format(Resources.Tweet_Lookup, idsParameter));
            query.AddFormattedParameterToQuery(_queryParameterGenerator.GenerateTweetModeParameter(_tweetinviSettingsAccessor.CurrentThreadSettings.TweetMode));

            return query.ToString();
        }

        // Publish Tweet
        public string GetPublishTweetQuery(IPublishTweetParameters queryParameters)
        {
            _tweetQueryValidator.ThrowIfTweetCannotBePublished(queryParameters);

            var text = queryParameters.Text;

            var useExtendedMode = _tweetinviSettingsAccessor.CurrentThreadSettings.TweetMode == TweetMode.Extended;

            var quotedTweetUrl = GetQuotedTweetUrl(queryParameters);

            if (!useExtendedMode && quotedTweetUrl != null)
            {
                text = text.TrimEnd() + " " + quotedTweetUrl;
            }

            var query = new StringBuilder(string.Format(Resources.Tweet_Publish, CleanupString(text)));

            if (queryParameters.Parameters != null)
            {
                if (queryParameters.InReplyToTweet != null)
                {
                    query.AddParameterToQuery("in_reply_to_status_id", queryParameters.InReplyToTweet.Id);

                    // Extended Tweet prefix auto-population
                    query.AddParameterToQuery("auto_populate_reply_metadata", queryParameters.AutoPopulateReplyMetadata);
                    if (queryParameters.ExcludeReplyUserIds != null)
                    {
                        query.AddParameterToQuery("exclude_reply_user_ids", String.Join(",", queryParameters.ExcludeReplyUserIds));
                    }
                }

                query.AddParameterToQuery("possibly_sensitive", queryParameters.PossiblySensitive);

                if (queryParameters.Coordinates != null)
                {
                    query.AddParameterToQuery("lat", queryParameters.Coordinates.Latitude.ToString(CultureInfo.InvariantCulture));
                    query.AddParameterToQuery("long", queryParameters.Coordinates.Longitude.ToString(CultureInfo.InvariantCulture));
                }

                query.AddParameterToQuery("place_id", queryParameters.PlaceId);
                query.AddParameterToQuery("display_coordinates", queryParameters.DisplayExactCoordinates);
                query.AddParameterToQuery("trim_user", queryParameters.TrimUser);
                query.AddParameterToQuery("tweet_mode", _tweetinviSettingsAccessor.CurrentThreadSettings.TweetMode.ToString().ToLowerInvariant());

                if (useExtendedMode && quotedTweetUrl != null)
                {
                    query.AddParameterToQuery("attachment_url", quotedTweetUrl);
                }

                if (queryParameters.MediaIds.Count > 0)
                {
                    var mediaIdsParameter = string.Join(",", queryParameters.MediaIds.Select(x => x.ToString(CultureInfo.InvariantCulture)));
                    query.AddParameterToQuery("media_ids", mediaIdsParameter);
                }

                query.Append(_queryParameterGenerator.GenerateAdditionalRequestParameters(queryParameters.FormattedCustomQueryParameters));
            }

            return query.ToString();
        }

        private string GetQuotedTweetUrl(IPublishTweetParameters parameters)
        {
            if (parameters.QuotedTweet?.CreatedBy?.ScreenName == null)
            {
                return null;
            }

            return string.Format("https://twitter.com/{0}/status/{1}",
                        parameters.QuotedTweet.CreatedBy.ScreenName,
                        parameters.QuotedTweet.Id.ToString(CultureInfo.InvariantCulture));
        }

        // Publish Retweet
        public string GetPublishRetweetQuery(ITweetDTO tweetDTO)
        {
            _tweetQueryValidator.ThrowIfTweetCannotBeUsed(tweetDTO);
            return GetPublishRetweetQuery(tweetDTO.Id);
        }

        public string GetPublishRetweetQuery(long tweetId)
        {
            _tweetQueryValidator.ThrowIfTweetCannotBeUsed(tweetId);

            var query = new StringBuilder(string.Format(Resources.Tweet_Retweet_Publish, tweetId));
            query.AddFormattedParameterToQuery(_queryParameterGenerator.GenerateTweetModeParameter(_tweetinviSettingsAccessor.CurrentThreadSettings.TweetMode));

            return query.ToString();
        }

        // Get Retweets

        public string GetRetweetsQuery(ITweetIdentifier tweetIdentifier, int maxRetweetsToRetrieve)
        {
            _tweetQueryValidator.ThrowIfTweetCannotBeUsed(tweetIdentifier);

            var query = new StringBuilder(string.Format(Resources.Tweet_Retweet_GetRetweets, tweetIdentifier.Id));

            query.AddParameterToQuery("count", maxRetweetsToRetrieve);
            query.AddFormattedParameterToQuery(_queryParameterGenerator.GenerateTweetModeParameter(_tweetinviSettingsAccessor.CurrentThreadSettings.TweetMode));

            return query.ToString();
        }

        #region Get Retweeter Ids

        public string GetRetweeterIdsQuery(ITweetIdentifier tweetIdentifier, int maxRetweetersToRetrieve = 100)
        {
            _tweetQueryValidator.ThrowIfTweetCannotBeUsed(tweetIdentifier);

            var query = new StringBuilder(string.Format(Resources.Tweet_GetRetweeters, tweetIdentifier.Id));

            query.AddParameterToQuery("id", tweetIdentifier.Id);
            query.AddParameterToQuery("count", maxRetweetersToRetrieve);

            return query.ToString();
        }

        #endregion

        // UnRetweet
        public string GetUnRetweetQuery(ITweetIdentifier tweetIdentifier)
        {
            _tweetQueryValidator.ThrowIfTweetCannotBeUsed(tweetIdentifier);
            return GetUnRetweetQuery(tweetIdentifier.Id);
        }

        public string GetUnRetweetQuery(long tweetId)
        {
            _tweetQueryValidator.ThrowIfTweetCannotBeUsed(tweetId);

            var query = new StringBuilder(string.Format(Resources.Tweet_UnRetweet, tweetId));
            query.AddFormattedParameterToQuery(_queryParameterGenerator.GenerateTweetModeParameter(_tweetinviSettingsAccessor.CurrentThreadSettings.TweetMode));

            return query.ToString();
        }

        // Destroy Tweet
        public string GetDestroyTweetQuery(ITweetDTO tweetDTO)
        {
            _tweetQueryValidator.ThrowIfTweetCannotBeDestroyed(tweetDTO);
            return GetDestroyTweetQuery(tweetDTO.Id);
        }

        public string GetDestroyTweetQuery(long tweetId)
        {
            _tweetQueryValidator.ThrowIfTweetCannotBeUsed(tweetId);

            var query = new StringBuilder(string.Format(Resources.Tweet_Destroy, tweetId));

            query.AddFormattedParameterToQuery(_queryParameterGenerator.GenerateTweetModeParameter(_tweetinviSettingsAccessor.CurrentThreadSettings.TweetMode));

            return query.ToString();
        }

        // Favorite Tweet
        public string GetFavoriteTweetQuery(ITweetDTO tweetDTO)
        {
            _tweetQueryValidator.ThrowIfTweetCannotBeUsed(tweetDTO);
            return GetFavoriteTweetQuery(tweetDTO.Id);
        }

        public string GetFavoriteTweetQuery(long tweetId)
        {
            _tweetQueryValidator.ThrowIfTweetCannotBeUsed(tweetId);
            return string.Format(Resources.Tweet_Favorite_Create, tweetId);
        }

        // Unfavourite Tweet
        public string GetUnFavoriteTweetQuery(ITweetDTO tweetDTO)
        {
            _tweetQueryValidator.ThrowIfTweetCannotBeUsed(tweetDTO);
            return GetUnFavoriteTweetQuery(tweetDTO.Id);
        }

        public string GetUnFavoriteTweetQuery(long tweetId)
        {
            _tweetQueryValidator.ThrowIfTweetCannotBeUsed(tweetId);
            return string.Format(Resources.Tweet_Favorite_Destroy, tweetId);
        }

        // OEmbed Tweet
        public string GetGenerateOEmbedTweetQuery(ITweetDTO tweetDTO)
        {
            _tweetQueryValidator.ThrowIfTweetCannotBeUsed(tweetDTO);
            return GetGenerateOEmbedTweetQuery(tweetDTO.Id);
        }

        public string GetGenerateOEmbedTweetQuery(long tweetId)
        {
            _tweetQueryValidator.ThrowIfTweetCannotBeUsed(tweetId);
            return string.Format(Resources.Tweet_GenerateOEmbed, tweetId);
        }
    }
}