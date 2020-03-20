using System.Globalization;
using System.Linq;
using System.Text;
using Tweetinvi.Controllers.Properties;
using Tweetinvi.Controllers.Shared;
using Tweetinvi.Core.Extensions;
using Tweetinvi.Core.QueryGenerators;
using Tweetinvi.Models;
using Tweetinvi.Parameters;

namespace Tweetinvi.Controllers.Tweet
{
    public class TweetQueryGenerator : ITweetQueryGenerator
    {
        private readonly IQueryParameterGenerator _queryParameterGenerator;
        private readonly IUserQueryParameterGenerator _userQueryParameterGenerator;

        public TweetQueryGenerator(
            IQueryParameterGenerator queryParameterGenerator,
            IUserQueryParameterGenerator userQueryParameterGenerator)
        {
            _queryParameterGenerator = queryParameterGenerator;
            _userQueryParameterGenerator = userQueryParameterGenerator;
        }

        // Get Tweet
        public string GetTweetQuery(IGetTweetParameters parameters, TweetMode? tweetMode)
        {
            var query = new StringBuilder(Resources.Tweet_Get);

            query.AddParameterToQuery("id", parameters.Tweet?.Id.ToString() ?? parameters.Tweet?.IdStr);
            query.AddParameterToQuery("include_card_uri", parameters.IncludeCardUri);
            query.AddParameterToQuery("include_entities", parameters.IncludeEntities);
            query.AddParameterToQuery("include_ext_alt_text", parameters.IncludeExtAltText);
            query.AddParameterToQuery("include_my_retweet", parameters.IncludeMyRetweet);
            query.AddParameterToQuery("trim_user", parameters.TrimUser);

            query.AddFormattedParameterToQuery(_queryParameterGenerator.GenerateTweetModeParameter(tweetMode));
            query.AddFormattedParameterToQuery(parameters.FormattedCustomQueryParameters);

            return query.ToString();
        }

        public string GetTweetsQuery(IGetTweetsParameters parameters, TweetMode? tweetMode)
        {
            var query = new StringBuilder(Resources.Tweet_Lookup);

            var validTweetIdentifiers = parameters.Tweets.Where(x => GetTweetId(x) != null);
            var tweetIds = validTweetIdentifiers.Select(GetTweetId);

            query.AddParameterToQuery("id", string.Join(",", tweetIds));
            query.AddParameterToQuery("include_card_uri", parameters.IncludeCardUri);
            query.AddParameterToQuery("include_entities", parameters.IncludeEntities);
            query.AddParameterToQuery("include_ext_alt_text", parameters.IncludeExtAltText);
            query.AddParameterToQuery("trim_user", parameters.TrimUser);

            query.AddFormattedParameterToQuery(_queryParameterGenerator.GenerateTweetModeParameter(tweetMode));
            query.AddFormattedParameterToQuery(parameters.FormattedCustomQueryParameters);

            return query.ToString();
        }

        // Publish Tweet
        public string GetPublishTweetQuery(IPublishTweetParameters parameters, TweetMode? tweetMode)
        {
            var text = parameters.Text;
            var useExtendedTweetMode = tweetMode == null || tweetMode == TweetMode.Extended;

            var quotedTweetUrl = GetQuotedTweetUrl(parameters);
            var attachmentUrl = parameters.QuotedTweetUrl;

            if (quotedTweetUrl != null)
            {
                // if there is a quoted tweet we need to pass the url in the text or attachment url
                // attachment_url is only available under tweetMode
                if (useExtendedTweetMode && attachmentUrl == null)
                {
                    attachmentUrl = quotedTweetUrl;
                }
                else
                {
                    text = text.TrimEnd() + " " + quotedTweetUrl;
                }
            }

            var query = new StringBuilder(Resources.Tweet_Publish);

            query.AddParameterToQuery("status", text);
            query.AddParameterToQuery("auto_populate_reply_metadata", parameters.AutoPopulateReplyMetadata);
            query.AddParameterToQuery("attachment_url", attachmentUrl);
            query.AddParameterToQuery("card_uri", parameters.CardUri);
            query.AddParameterToQuery("display_coordinates", parameters.DisplayExactCoordinates);

            if (parameters.ExcludeReplyUserIds != null)
            {
                query.AddParameterToQuery("exclude_reply_user_ids", string.Join(",", parameters.ExcludeReplyUserIds));
            }

            query.AddParameterToQuery("in_reply_to_status_id", GetTweetId(parameters.InReplyToTweet));
            query.AddParameterToQuery("lat", parameters.Coordinates?.Latitude.ToString(CultureInfo.InvariantCulture));
            query.AddParameterToQuery("long", parameters.Coordinates?.Longitude.ToString(CultureInfo.InvariantCulture));

            if (parameters.MediaIds.Count > 0)
            {
                var mediaIdsParameter = string.Join(",", parameters.MediaIds.Select(x => x.ToString(CultureInfo.InvariantCulture)));
                query.AddParameterToQuery("media_ids", mediaIdsParameter);
            }

            query.AddParameterToQuery("place_id", parameters.PlaceId);
            query.AddParameterToQuery("possibly_sensitive", parameters.PossiblySensitive);
            query.AddParameterToQuery("trim_user", parameters.TrimUser);
            query.AddParameterToQuery("tweet_mode", tweetMode?.ToString().ToLowerInvariant());

            query.AddFormattedParameterToQuery(parameters.FormattedCustomQueryParameters);

            return query.ToString();
        }

        private string GetQuotedTweetUrl(IPublishTweetParameters parameters)
        {
            if (parameters.QuotedTweet?.CreatedBy?.ScreenName == null)
            {
                return null;
            }

            var quotedTweetId = GetTweetId(parameters.QuotedTweet);
            return $"https://twitter.com/{parameters.QuotedTweet.CreatedBy.ScreenName}/status/{quotedTweetId}";
        }

        public string GetDestroyTweetQuery(IDestroyTweetParameters parameters, TweetMode? tweetMode)
        {
            var query = new StringBuilder(string.Format(Resources.Tweet_Destroy, _queryParameterGenerator.GenerateTweetIdentifier(parameters.Tweet)));

            query.AddParameterToQuery("trim_user", parameters.TrimUser);

            var tweetModeParameter = _queryParameterGenerator.GenerateTweetModeParameter(tweetMode);
            query.AddFormattedParameterToQuery(tweetModeParameter);
            query.AddFormattedParameterToQuery(parameters.FormattedCustomQueryParameters);

            return query.ToString();
        }

        public string GetFavoriteTweetsQuery(IGetUserFavoriteTweetsParameters parameters, TweetMode? tweetMode)
        {
            var userParameter = _userQueryParameterGenerator.GenerateIdOrScreenNameParameter(parameters.User);
            var query = new StringBuilder(Resources.User_GetFavourites + userParameter);

            query.AddParameterToQuery("include_entities", parameters.IncludeEntities);
            _queryParameterGenerator.AddMinMaxQueryParameters(query, parameters);

            var tweetModeParameter = _queryParameterGenerator.GenerateTweetModeParameter(tweetMode);
            query.AddFormattedParameterToQuery(tweetModeParameter);
            query.AddFormattedParameterToQuery(parameters.FormattedCustomQueryParameters);

            return query.ToString();
        }

        public string GetRetweetsQuery(IGetRetweetsParameters parameters, TweetMode? tweetMode)
        {
            var tweetId = GetTweetId(parameters.Tweet);
            var query = new StringBuilder(string.Format(Resources.Tweet_Retweet_GetRetweets, tweetId));

            query.AddParameterToQuery("count", parameters.PageSize);
            query.AddParameterToQuery("trim_user", parameters.TrimUser);

            query.AddFormattedParameterToQuery(_queryParameterGenerator.GenerateTweetModeParameter(tweetMode));
            query.AddFormattedParameterToQuery(parameters.FormattedCustomQueryParameters);

            return query.ToString();
        }

        public string GetPublishRetweetQuery(IPublishRetweetParameters parameters, TweetMode? tweetMode)
        {
            var tweetId = GetTweetId(parameters.Tweet);
            var query = new StringBuilder(string.Format(Resources.Tweet_Retweet_Publish, tweetId));

            query.AddParameterToQuery("trim_user", parameters.TrimUser);

            query.AddFormattedParameterToQuery(_queryParameterGenerator.GenerateTweetModeParameter(tweetMode));
            query.AddFormattedParameterToQuery(parameters.FormattedCustomQueryParameters);

            return query.ToString();
        }

        public string GetDestroyRetweetQuery(IDestroyRetweetParameters parameters, TweetMode? tweetMode)
        {
            var tweetId = GetTweetId(parameters.Tweet);
            var query = new StringBuilder(string.Format(Resources.Tweet_DestroyRetweet, tweetId));

            query.AddParameterToQuery("trim_user", parameters.TrimUser);

            query.AddFormattedParameterToQuery(_queryParameterGenerator.GenerateTweetModeParameter(tweetMode));
            query.AddFormattedParameterToQuery(parameters.FormattedCustomQueryParameters);

            return query.ToString();
        }

        public string GetRetweeterIdsQuery(IGetRetweeterIdsParameters parameters)
        {
            var query = new StringBuilder(Resources.Tweet_GetRetweeters);

            query.AddParameterToQuery("id", GetTweetId(parameters.Tweet));
            _queryParameterGenerator.AppendCursorParameters(query, parameters);

            query.AddFormattedParameterToQuery(parameters.FormattedCustomQueryParameters);

            return query.ToString();
        }

        // Favorites
        public string GetCreateFavoriteTweetQuery(IFavoriteTweetParameters parameters)
        {
            var query = new StringBuilder(Resources.Tweet_Favorite_Create);

            query.AddParameterToQuery("id", GetTweetId(parameters.Tweet));
            query.AddParameterToQuery("include_entities", parameters.IncludeEntities);
            query.AddFormattedParameterToQuery(parameters.FormattedCustomQueryParameters);

            return query.ToString();
        }

        public string GetUnfavoriteTweetQuery(IUnfavoriteTweetParameters parameters)
        {
            var query = new StringBuilder(Resources.Tweet_Favorite_Destroy);

            query.AddParameterToQuery("id", GetTweetId(parameters.Tweet));
            query.AddParameterToQuery("include_entities", parameters.IncludeEntities);
            query.AddFormattedParameterToQuery(parameters.FormattedCustomQueryParameters);

            return query.ToString();
        }

        public string GetOEmbedTweetQuery(IGetOEmbedTweetParameters parameters)
        {
            var query = new StringBuilder(Resources.Tweet_GenerateOEmbed);

            query.AddParameterToQuery("id", GetTweetId(parameters.Tweet));
            query.AddParameterToQuery("maxwidth", parameters.MaxWidth);
            query.AddParameterToQuery("hide_media", parameters.HideMedia);
            query.AddParameterToQuery("hide_thread", parameters.HideThread);
            query.AddParameterToQuery("omit_script", parameters.OmitScript);
            query.AddParameterToQuery("align", _queryParameterGenerator.GenerateOEmbedAlignmentParameter(parameters.Alignment));
            query.AddParameterToQuery("related", string.Join(",", parameters.RelatedUsernames ?? new string[0]));
            query.AddFormattedParameterToQuery(_queryParameterGenerator.GenerateLanguageParameter(parameters.Language));
            query.AddParameterToQuery("theme", _queryParameterGenerator.GenerateOEmbedThemeParameter(parameters.Theme));
            query.AddParameterToQuery("link_color", parameters.LinkColor);
            query.AddParameterToQuery("widget_type", parameters.WidgetType);
            query.AddParameterToQuery("dnt", parameters.EnablePersonalisationAndSuggestions);

            query.AddFormattedParameterToQuery(parameters.FormattedCustomQueryParameters);

            return query.ToString();
        }

        private string GetTweetId(ITweetIdentifier tweetIdentifier)
        {
            if (tweetIdentifier == null)
            {
                return null;
            }

            var tweetId = tweetIdentifier.IdStr;
            if (string.IsNullOrEmpty(tweetId))
            {
                tweetId = tweetIdentifier.Id.ToString(CultureInfo.InvariantCulture);
            }

            return tweetId;
        }

    }
}