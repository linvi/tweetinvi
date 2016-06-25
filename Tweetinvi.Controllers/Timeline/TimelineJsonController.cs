using Tweetinvi.Core.Factories;
using Tweetinvi.Core.Parameters;
using Tweetinvi.Core.Web;
using Tweetinvi.Models;
using Tweetinvi.Parameters;

namespace Tweetinvi.Controllers.Timeline
{
    public interface ITimelineJsonController
    {
        // Home Timeline
        string GetHomeTimeline(int maximumNumberOfTweetsToRetrieve);
        string GetHomeTimeline(IHomeTimelineParameters timelineParameters);

        // User Timeline
        string GetUserTimeline(IUserIdentifier userIdentifier, int maximumNumberOfTweets = 40);
        string GetUserTimeline(long userId, int maximumNumberOfTweets = 40);
        string GetUserTimeline(string userScreenName, int maximumNumberOfTweets = 40);
        string GetUserTimeline(IUserIdentifier userIdentifier, IUserTimelineParameters timelineParameters);

        // Mention Timeline
        string GetMentionsTimeline(int maximumNumberOfTweets = 40);
        string GetMentionsTimeline(IMentionsTimelineParameters timelineParameters);
    }

    public class TimelineJsonController : ITimelineJsonController
    {
        private readonly ITimelineQueryGenerator _timelineQueryGenerator;
        private readonly ITwitterAccessor _twitterAccessor;
        private readonly IUserFactory _userFactory;
        private readonly ITimelineQueryParameterGenerator _timelineQueryParameterGenerator;

        public TimelineJsonController(
            ITimelineQueryGenerator timelineQueryGenerator,
            ITwitterAccessor twitterAccessor,
            IUserFactory userFactory,
            ITimelineQueryParameterGenerator timelineQueryParameterGenerator)
        {
            _timelineQueryGenerator = timelineQueryGenerator;
            _twitterAccessor = twitterAccessor;
            _userFactory = userFactory;
            _timelineQueryParameterGenerator = timelineQueryParameterGenerator;
        }

        // Home Timeline
        public string GetHomeTimeline(int maximumNumberOfTweetsToRetrieve)
        {
            var timelineRequestParameter = _timelineQueryParameterGenerator.CreateHomeTimelineParameters();
            timelineRequestParameter.MaximumNumberOfTweetsToRetrieve = maximumNumberOfTweetsToRetrieve;
            return GetHomeTimeline(timelineRequestParameter);
        }

        public string GetHomeTimeline(IHomeTimelineParameters timelineParameters)
        {
            string query = _timelineQueryGenerator.GetHomeTimelineQuery(timelineParameters);
            return _twitterAccessor.ExecuteGETQueryReturningJson(query);
        }

        // User Timeline
        public string GetUserTimeline(IUserIdentifier userIdentifier, int maximumNumberOfTweets = 40)
        {
            var requestParameters = _timelineQueryParameterGenerator.CreateUserTimelineParameters();
            requestParameters.MaximumNumberOfTweetsToRetrieve = maximumNumberOfTweets;

            return GetUserTimeline(userIdentifier, requestParameters);
        }

        public string GetUserTimeline(long userId, int maximumNumberOfTweets = 40)
        {
            var userIdentifier = _userFactory.GenerateUserIdentifierFromId(userId);
            return GetUserTimeline(userIdentifier, maximumNumberOfTweets);
        }

        public string GetUserTimeline(string userScreenName, int maximumNumberOfTweets = 40)
        {
            var userIdentifier = _userFactory.GenerateUserIdentifierFromScreenName(userScreenName);
            return GetUserTimeline(userIdentifier, maximumNumberOfTweets);
        }

        public string GetUserTimeline(IUserIdentifier userIdentifier, IUserTimelineParameters parameters)
        {
            if (parameters == null)
            {
                parameters = _timelineQueryParameterGenerator.CreateUserTimelineParameters();
            }

            var queryParameters = _timelineQueryParameterGenerator.CreateUserTimelineQueryParameters(userIdentifier, parameters);

            return GetUserTimeline(queryParameters);
        }

        public string GetUserTimeline(IUserTimelineQueryParameters timelineParameters)
        {
            var query = _timelineQueryGenerator.GetUserTimelineQuery(timelineParameters);
            return _twitterAccessor.ExecuteGETQueryReturningJson(query);
        }

        // Mentions Timeline
        public string GetMentionsTimeline(int maximumNumberOfTweets = 40)
        {
            var requestParameters = _timelineQueryParameterGenerator.CreateMentionsTimelineParameters();
            requestParameters.MaximumNumberOfTweetsToRetrieve = maximumNumberOfTweets;

            return GetMentionsTimeline(requestParameters);
        }

        public string GetMentionsTimeline(IMentionsTimelineParameters timelineParameters)
        {
            var query = _timelineQueryGenerator.GetMentionsTimelineQuery(timelineParameters);
            return _twitterAccessor.ExecuteGETQueryReturningJson(query);
        }
    }
}