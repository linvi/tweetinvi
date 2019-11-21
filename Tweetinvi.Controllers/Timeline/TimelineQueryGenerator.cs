using System.Text;
using Tweetinvi.Controllers.Properties;
using Tweetinvi.Controllers.Shared;
using Tweetinvi.Core;
using Tweetinvi.Core.Extensions;
using Tweetinvi.Core.QueryGenerators;
using Tweetinvi.Parameters;

namespace Tweetinvi.Controllers.Timeline
{
    public interface ITimelineQueryGenerator
    {
        string GetHomeTimelineQuery(IGetHomeTimelineParameters parameters, TweetMode? tweetMode);
        string GetUserTimelineQuery(IGetUserTimelineParameters parameters, TweetMode? tweetMode);

        // Mention Timeline
        string GetMentionsTimelineQuery(IMentionsTimelineParameters mentionsTimelineParameters);

        // Retweets of Me Timeline
        string GetRetweetsOfMeTimelineQuery(IGetRetweetsOfMeTimelineParameters parameters, TweetMode? tweetMode);
    }

    public class TimelineQueryGenerator : ITimelineQueryGenerator
    {
        private readonly IUserQueryParameterGenerator _userQueryParameterGenerator;
        private readonly IQueryParameterGenerator _queryParameterGenerator;
        private readonly ITimelineQueryParameterGenerator _timelineQueryParameterGenerator;
        private readonly ITweetinviSettingsAccessor _tweetinviSettingsAccessor;

        public TimelineQueryGenerator(
            IUserQueryParameterGenerator userQueryParameterGenerator,
            IQueryParameterGenerator queryParameterGenerator,
            ITimelineQueryParameterGenerator timelineQueryParameterGenerator,
            ITweetinviSettingsAccessor tweetinviSettingsAccessor)
        {
            _userQueryParameterGenerator = userQueryParameterGenerator;
            _queryParameterGenerator = queryParameterGenerator;
            _timelineQueryParameterGenerator = timelineQueryParameterGenerator;
            _tweetinviSettingsAccessor = tweetinviSettingsAccessor;
        }

        // Home Timeline
        public string GetHomeTimelineQuery(IGetHomeTimelineParameters parameters, TweetMode? tweetMode)
        {
            var query = new StringBuilder(Resources.Timeline_GetHomeTimeline);

            AddTimelineParameters(query, parameters);

            query.AddParameterToQuery("contributor_details", parameters.IncludeContributorDetails);
            query.AddParameterToQuery("exclude_replies", parameters.ExcludeReplies);
            query.AddParameterToQuery("tweet_mode", tweetMode?.ToString().ToLowerInvariant());
            query.AddFormattedParameterToQuery(parameters.FormattedCustomQueryParameters);

            return query.ToString();
        }

        // User Timeline
        public string GetUserTimelineQuery(IGetUserTimelineParameters parameters, TweetMode? tweetMode)
        {
            var query = new StringBuilder(Resources.Timeline_GetUserTimeline);

            query.AddFormattedParameterToQuery(_userQueryParameterGenerator.GenerateIdOrScreenNameParameter(parameters.User));

            AddTimelineParameters(query, parameters);

            query.AddParameterToQuery("contributor_details", parameters.IncludeContributorDetails);
            query.AddParameterToQuery("exclude_replies", parameters.ExcludeReplies);
            query.AddParameterToQuery("include_rts", parameters.IncludeRetweets);
            query.AddParameterToQuery("tweet_mode", tweetMode?.ToString().ToLowerInvariant());
            query.AddFormattedParameterToQuery(parameters.FormattedCustomQueryParameters);

            return query.ToString();
        }

        // Mentions Timeline
        public string GetMentionsTimelineQuery(IMentionsTimelineParameters mentionsTimelineParameters)
        {
            var includeContributorDetailsQueryParameter = GenerateIncludeContributorsDetailsParameter(mentionsTimelineParameters.IncludeContributorDetails);
            var timelineRequestParameter = GenerateTimelineRequestParameter(mentionsTimelineParameters);
            var requestParameters = $"{includeContributorDetailsQueryParameter}{timelineRequestParameter}";

            return string.Format(Resources.Timeline_GetMentionsTimeline, requestParameters);
        }

        // Retweets of Me Timeline
        public string GetRetweetsOfMeTimelineQuery(IGetRetweetsOfMeTimelineParameters parameters, TweetMode? tweetMode)
        {
            var query = new StringBuilder(Resources.Timeline_GetRetweetsOfMeTimeline);

            AddTimelineParameters(query, parameters);
            query.AddParameterToQuery("include_user_entities", parameters.IncludeUserEntities);

            query.AddParameterToQuery("tweet_mode", tweetMode?.ToString().ToLowerInvariant());
            query.AddFormattedParameterToQuery(parameters.FormattedCustomQueryParameters);

            return query.ToString();
        }

        private void AddTimelineParameters(StringBuilder query, ITimelineRequestParameters parameters)
        {
            _queryParameterGenerator.AddMinMaxQueryParameters(query, parameters);
            query.AddParameterToQuery("include_entities", parameters.IncludeEntities);
            query.AddParameterToQuery("trim_user", parameters.TrimUser);
        }

        // Base Timeline Query Generator
        private string GenerateTimelineRequestParameter(ITimelineRequestParameters timelineRequestParameters)
        {
            var requestParameter = new StringBuilder();

            requestParameter.Append(_queryParameterGenerator.GenerateCountParameter(timelineRequestParameters.PageSize));
            requestParameter.Append(_queryParameterGenerator.GenerateTrimUserParameter(timelineRequestParameters.TrimUser));
            requestParameter.Append(_queryParameterGenerator.GenerateSinceIdParameter(timelineRequestParameters.SinceId));
            requestParameter.Append(_queryParameterGenerator.GenerateMaxIdParameter(timelineRequestParameters.MaxId));
            requestParameter.Append(_queryParameterGenerator.GenerateIncludeEntitiesParameter(timelineRequestParameters.IncludeEntities));

            requestParameter.Append(_queryParameterGenerator.GenerateAdditionalRequestParameters(timelineRequestParameters.FormattedCustomQueryParameters));
            requestParameter.AddFormattedParameterToParametersList(_queryParameterGenerator.GenerateTweetModeParameter(_tweetinviSettingsAccessor.CurrentThreadSettings.TweetMode));

            return requestParameter.ToString();
        }

        private string GenerateIncludeContributorsDetailsParameter(bool includeContributorDetails)
        {
            return _timelineQueryParameterGenerator.GenerateIncludeContributorDetailsParameter(includeContributorDetails);
        }
    }
}