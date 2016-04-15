using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using Tweetinvi.Controllers.Properties;
using Tweetinvi.Controllers.Shared;
using Tweetinvi.Core.Extensions;
using Tweetinvi.Core.Helpers;
using Tweetinvi.Core.Interfaces.DTO;
using Tweetinvi.Core.Interfaces.Models;
using Tweetinvi.Core.Interfaces.QueryGenerators;
using Tweetinvi.Core.Parameters;

namespace Tweetinvi.Controllers.Tweet
{
    public class TweetQueryGenerator : ITweetQueryGenerator
    {
        private readonly IQueryParameterGenerator _queryParameterGenerator;
        private readonly ITweetQueryValidator _tweetQueryValidator;
        private readonly ITwitterStringFormatter _twitterStringFormatter;

        public TweetQueryGenerator(
            IQueryParameterGenerator queryParameterGenerator,
            ITweetQueryValidator tweetQueryValidator,
            ITwitterStringFormatter twitterStringFormatter)
        {
            _queryParameterGenerator = queryParameterGenerator;
            _tweetQueryValidator = tweetQueryValidator;
            _twitterStringFormatter = twitterStringFormatter;
        }

        private string CleanupString(string source)
        {
            return _twitterStringFormatter.TwitterEncode(source);
        }

        // Get Tweet
        public string GetTweetQuery(long tweetId)
        {
            return string.Format(Resources.Tweet_Get, tweetId);
        }

        public string GetTweetsQuery(IEnumerable<long> tweetIds)
        {
            if (tweetIds.IsNullOrEmpty())
            {
                return null;
            }

            var idsParameter = string.Join("%2C", tweetIds);
            return string.Format(Resources.Tweet_Lookup, idsParameter);
        }

        // Publish Tweet
        public string GetPublishTweetQuery(IPublishTweetParameters queryParameters)
        {
            _tweetQueryValidator.ThrowIfTweetCannotBePublished(queryParameters);

            var text = queryParameters.Text;

            if (queryParameters.QuotedTweet != null)
            {
                var quotedTweet = queryParameters.QuotedTweet;
                if (quotedTweet.CreatedBy != null)
                {
                    text += string.Format("https://twitter.com/{0}/status/{1}", 
                        quotedTweet.CreatedBy.ScreenName, 
                        quotedTweet.Id.ToString(CultureInfo.InvariantCulture));
                }
            }

            var query = new StringBuilder(string.Format(Resources.Tweet_Publish, CleanupString(text)));
            
            if (queryParameters.Parameters != null)
            {
                if (queryParameters.InReplyToTweet != null)
                {
                    query.AddParameterToQuery("in_reply_to_status_id", queryParameters.InReplyToTweet.Id);
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

                if (queryParameters.MediaIds.Count > 0)
                {
                    var mediaIdsParameter = string.Join("%2C", queryParameters.MediaIds.Select(x => x.ToString(CultureInfo.InvariantCulture)));
                    query.AddParameterToQuery("media_ids", mediaIdsParameter);
                }

                query.Append(_queryParameterGenerator.GenerateAdditionalRequestParameters(queryParameters.FormattedCustomQueryParameters));
            }

            return query.ToString();
        }

        // Publish Retweet
        public string GetPublishRetweetQuery(ITweetDTO tweetDTO)
        {
            if (!_tweetQueryValidator.IsTweetPublished(tweetDTO))
            {
                return null;
            }

            return GetPublishRetweetQuery(tweetDTO.Id);
        }

        public string GetPublishRetweetQuery(long tweetId)
        {
            return string.Format(Resources.Tweet_Retweet_Publish, tweetId);
        }

        // Get Retweets

        public string GetRetweetsQuery(ITweetIdentifier tweetIdentifier, int maxRetweetsToRetrieve)
        {
            _tweetQueryValidator.ValidateTweetIdentifier(tweetIdentifier);

            var query = new StringBuilder(string.Format(Resources.Tweet_Retweet_GetRetweets, tweetIdentifier.Id));
            query.AddParameterToQuery("count", maxRetweetsToRetrieve);

            return query.ToString();
        }

        #region Get Retweeter Ids

        public string GetRetweeterIdsQuery(ITweetIdentifier tweetIdentifier, int maxRetweetersToRetrieve = 100)
        {
           _tweetQueryValidator.ValidateTweetIdentifier(tweetIdentifier);
            
            var query = new StringBuilder(string.Format(Resources.Tweet_GetRetweeters, tweetIdentifier.Id));

            query.AddParameterToQuery("id", tweetIdentifier.Id);
            query.AddParameterToQuery("count", maxRetweetersToRetrieve);

            return query.ToString();
        }

        #endregion

        // UnRetweet
        public string GetUnRetweetQuery(ITweetIdentifier tweetIdentifier)
        {
            if (tweetIdentifier == null)
            {
                return null;
            }

            return GetUnRetweetQuery(tweetIdentifier.Id);
        }

        public string GetUnRetweetQuery(long tweetId)
        {
            return string.Format(Resources.Tweet_UnRetweet, tweetId);
        }

        // Destroy Tweet
        public string GetDestroyTweetQuery(ITweetDTO tweetDTO)
        {
            if (!_tweetQueryValidator.CanTweetDTOBeDestroyed(tweetDTO))
            {
                return null;
            }

            return GetDestroyTweetQuery(tweetDTO.Id);
        }

        public string GetDestroyTweetQuery(long tweetId)
        {
            return string.Format(Resources.Tweet_Destroy, tweetId);
        }

        // Favorite Tweet
        public string GetFavoriteTweetQuery(ITweetDTO tweetDTO)
        {
            if (!_tweetQueryValidator.IsTweetPublished(tweetDTO))
            {
                return null;
            }

            return GetFavoriteTweetQuery(tweetDTO.Id);
        }

        public string GetFavoriteTweetQuery(long tweetId)
        {
            return string.Format(Resources.Tweet_Favorite_Create, tweetId);
        }

        // Unfavourite Tweet
        public string GetUnFavoriteTweetQuery(ITweetDTO tweetDTO)
        {
            if (!_tweetQueryValidator.IsTweetPublished(tweetDTO))
            {
                return null;
            }

            return GetUnFavoriteTweetQuery(tweetDTO.Id);
        }

        public string GetUnFavoriteTweetQuery(long tweetId)
        {
            return string.Format(Resources.Tweet_Favorite_Destroy, tweetId);
        }

        // OEmbed Tweet
        public string GetGenerateOEmbedTweetQuery(ITweetDTO tweetDTO)
        {
            if (!_tweetQueryValidator.IsTweetPublished(tweetDTO))
            {
                return null;
            }

            return GetGenerateOEmbedTweetQuery(tweetDTO.Id);
        }

        public string GetGenerateOEmbedTweetQuery(long tweetId)
        {
            return string.Format(Resources.Tweet_GenerateOEmbed, tweetId);
        }

    }
}