using System;
using System.Text;
using Tweetinvi.Controllers.Properties;
using Tweetinvi.Controllers.Shared;
using Tweetinvi.Core;
using Tweetinvi.Core.Extensions;
using Tweetinvi.Core.Parameters;
using Tweetinvi.Core.QueryGenerators;
using Tweetinvi.Core.QueryValidators;
using Tweetinvi.Parameters;

namespace Tweetinvi.Controllers.Timeline
{
    public interface ITimelineQueryGenerator
    {
        // Home Timeline
        string GetHomeTimelineQuery(IHomeTimelineParameters timelineParameters);

        // User Timeline
        string GetUserTimelineQuery(IUserTimelineQueryParameters userTimelineQueryParameters);

        // Mention Timeline
        string GetMentionsTimelineQuery(IMentionsTimelineParameters mentionsTimelineParameters);

        // Retweets of Me Timeline
        string GetRetweetsOfMeTimelineQuery(IGetRetweetsOfMeTimelineParameters parameters, TweetMode? tweetMode);
    }

    public class TimelineQueryGenerator : ITimelineQueryGenerator
    {
        private readonly IUserQueryParameterGenerator _userQueryParameterGenerator;
        private readonly IUserQueryValidator _userQueryValidator;
        private readonly IQueryParameterGenerator _queryParameterGenerator;
        private readonly ITimelineQueryParameterGenerator _timelineQueryParameterGenerator;
        private readonly ITweetinviSettingsAccessor _tweetinviSettingsAccessor;

        public TimelineQueryGenerator(
            IUserQueryParameterGenerator userQueryGenerator,
            IUserQueryValidator userQueryValidator,
            IQueryParameterGenerator queryParameterGenerator,
            ITimelineQueryParameterGenerator timelineQueryParameterGenerator,
            ITweetinviSettingsAccessor tweetinviSettingsAccessor)
        {
            _userQueryParameterGenerator = userQueryGenerator;
            _userQueryValidator = userQueryValidator;
            _queryParameterGenerator = queryParameterGenerator;
            _timelineQueryParameterGenerator = timelineQueryParameterGenerator;
            _tweetinviSettingsAccessor = tweetinviSettingsAccessor;
        }

        // Helper

        // Home Timeline
        public string GetHomeTimelineQuery(IHomeTimelineParameters timelineParameters)
        {
            var homeTimelineRequestQueryParameter = GenerateHomeTimelineParameters(timelineParameters);
            var includeContributorDetailsQueryParameter = GenerateIncludeContributorsDetailsParameter(timelineParameters.IncludeContributorDetails);
            var timelineRequestQueryParameter = GenerateTimelineRequestParameter(timelineParameters);
            var requestParameters = $"{homeTimelineRequestQueryParameter}{includeContributorDetailsQueryParameter}{timelineRequestQueryParameter}";
            return string.Format(Resources.Timeline_GetHomeTimeline, requestParameters);
        }

        private string GenerateHomeTimelineParameters(IHomeTimelineParameters timelineParameters)
        {
            return _timelineQueryParameterGenerator.GenerateExcludeRepliesParameter(timelineParameters.ExcludeReplies);
        }

        // User Timeline
        public string GetUserTimelineQuery(IUserTimelineQueryParameters userTimelineQueryParameters)
        {
            if (userTimelineQueryParameters == null)
            {
                throw new ArgumentNullException(nameof(userTimelineQueryParameters));
            }

            var queryParameters = userTimelineQueryParameters.Parameters;
            var user = userTimelineQueryParameters.UserIdentifier;

            if (queryParameters == null)
            {
                throw new ArgumentNullException(nameof(queryParameters));
            }

            _userQueryValidator.ThrowIfUserCannotBeIdentified(user);

            var userTimelineRequestParameter = GenerateUserTimelineRequestParameters(userTimelineQueryParameters);
            var includeContributorDetailsQueryParameter = GenerateIncludeContributorsDetailsParameter(queryParameters.IncludeContributorDetails);
            var timelineRequestParameter = GenerateTimelineRequestParameter(queryParameters);
            var requestParameters = $"{userTimelineRequestParameter}{includeContributorDetailsQueryParameter}{timelineRequestParameter}";

            return string.Format(Resources.Timeline_GetUserTimeline, requestParameters);
        }

        private string GenerateUserTimelineRequestParameters(IUserTimelineQueryParameters timelineQueryParameters)
        {
            var queryParameters = timelineQueryParameters.Parameters;

            var requestParameter = new StringBuilder();

            requestParameter.Append(_userQueryParameterGenerator.GenerateIdOrScreenNameParameter(timelineQueryParameters.UserIdentifier));
            requestParameter.Append(_timelineQueryParameterGenerator.GenerateIncludeRTSParameter(queryParameters.IncludeRTS));
            requestParameter.Append(_timelineQueryParameterGenerator.GenerateExcludeRepliesParameter(queryParameters.ExcludeReplies));

            return requestParameter.ToString();
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