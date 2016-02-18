using System;
using System.Text;
using Tweetinvi.Controllers.Properties;
using Tweetinvi.Controllers.Shared;
using Tweetinvi.Core.Interfaces.QueryGenerators;
using Tweetinvi.Core.Interfaces.QueryValidators;
using Tweetinvi.Core.Parameters;
using Tweetinvi.Core.Parameters.QueryParameters;

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
        string GetRetweetsOfMeTimelineQuery(IRetweetsOfMeTimelineParameters retweetsOfMeTimelineParameters);
    }

    public class TimelineQueryGenerator : ITimelineQueryGenerator
    {
        private readonly IUserQueryParameterGenerator _userQueryParameterGenerator;
        private readonly IUserQueryValidator _userQueryValidator;
        private readonly IQueryParameterGenerator _queryParameterGenerator;
        private readonly ITimelineQueryParameterGenerator _timelineQueryParameterGenerator;

        public TimelineQueryGenerator(
            IUserQueryParameterGenerator userQueryGenerator,
            IUserQueryValidator userQueryValidator,
            IQueryParameterGenerator queryParameterGenerator,
            ITimelineQueryParameterGenerator timelineQueryParameterGenerator)
        {
            _userQueryParameterGenerator = userQueryGenerator;
            _userQueryValidator = userQueryValidator;
            _queryParameterGenerator = queryParameterGenerator;
            _timelineQueryParameterGenerator = timelineQueryParameterGenerator;
        }

        // Helper

        // Home Timeline
        public string GetHomeTimelineQuery(IHomeTimelineParameters timelineParameters)
        {
            var homeTimelineRequestQueryParameter = GenerateHomeTimelineParameters(timelineParameters);
            var includeContributorDetailsQueryParameter = GenerateIncludeContributorsDetailsParameter(timelineParameters.IncludeContributorDetails);
            var timelineRequestQueryParameter = GenerateTimelineRequestParameter(timelineParameters);
            var requestParameters = string.Format("{0}{1}{2}", homeTimelineRequestQueryParameter, includeContributorDetailsQueryParameter, timelineRequestQueryParameter);
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
                throw new ArgumentNullException("Timeline Query parameter cannot be null");
            }

            var queryParameters = userTimelineQueryParameters.Parameters;
            var userIdentifier = userTimelineQueryParameters.UserIdentifier;

            if (queryParameters == null)
            {
                throw new ArgumentNullException("Timeline request parameter cannot be null");
            }

            if (!_userQueryValidator.CanUserBeIdentified(userIdentifier))
            {
                throw new ArgumentNullException("User identifier cannot be null");
            }

            var userTimelineRequestParameter = GenerateUserTimelineRequestParameters(userTimelineQueryParameters);
            var includeContributorDetailsQueryParameter = GenerateIncludeContributorsDetailsParameter(queryParameters.IncludeContributorDetails);
            var timelineRequestParameter = GenerateTimelineRequestParameter(queryParameters);
            var requestParameters = string.Format("{0}{1}{2}", userTimelineRequestParameter, includeContributorDetailsQueryParameter, timelineRequestParameter);

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
            var requestParameters = string.Format("{0}{1}", includeContributorDetailsQueryParameter, timelineRequestParameter);

            return string.Format(Resources.Timeline_GetMentionsTimeline, requestParameters);
        }
        
        // Retweets of Me Timeline
        public string GetRetweetsOfMeTimelineQuery(IRetweetsOfMeTimelineParameters retweetsOfMeTimelineParameters)
        {
            var includeUserEntitiesParameter = _timelineQueryParameterGenerator.GenerateIncludeUserEntitiesParameter(retweetsOfMeTimelineParameters.IncludeUserEntities);
            var timelineRequestParameter = GenerateTimelineRequestParameter(retweetsOfMeTimelineParameters);
            var requestParameters = string.Format("{0}{1}", timelineRequestParameter, includeUserEntitiesParameter);
               
            return string.Format(Resources.Timeline_GetRetweetsOfMeTimeline, requestParameters);
        }

        // Base Timeline Query Generator
        private string GenerateTimelineRequestParameter(ITimelineRequestParameters timelineRequestParameters)
        {
            var requestParameter = new StringBuilder();

            requestParameter.Append(_queryParameterGenerator.GenerateCountParameter(timelineRequestParameters.MaximumNumberOfTweetsToRetrieve));
            requestParameter.Append(_queryParameterGenerator.GenerateTrimUserParameter(timelineRequestParameters.TrimUser));
            requestParameter.Append(_queryParameterGenerator.GenerateSinceIdParameter(timelineRequestParameters.SinceId));
            requestParameter.Append(_queryParameterGenerator.GenerateMaxIdParameter(timelineRequestParameters.MaxId));
            requestParameter.Append(_queryParameterGenerator.GenerateIncludeEntitiesParameter(timelineRequestParameters.IncludeEntities));
            requestParameter.Append(_queryParameterGenerator.GenerateAdditionalRequestParameters(timelineRequestParameters.FormattedCustomQueryParameters));

            return requestParameter.ToString();
        }

        private string GenerateIncludeContributorsDetailsParameter(bool includeContributorDetails)
        {
            return _timelineQueryParameterGenerator.GenerateIncludeContributorDetailsParameter(includeContributorDetails);
        }
    }
}