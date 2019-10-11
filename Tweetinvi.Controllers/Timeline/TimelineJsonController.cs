using System.Threading.Tasks;
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
        Task<string> GetHomeTimeline(int maximumNumberOfTweetsToRetrieve);
        Task<string> GetHomeTimeline(IHomeTimelineParameters timelineParameters);

        // User Timeline
        Task<string> GetUserTimeline(IUserIdentifier user, int maximumNumberOfTweets = 40);
        Task<string> GetUserTimeline(long userId, int maximumNumberOfTweets = 40);
        Task<string> GetUserTimeline(string userScreenName, int maximumNumberOfTweets = 40);
        Task<string> GetUserTimeline(IUserIdentifier user, IUserTimelineParameters timelineParameters);

        // Mention Timeline
        Task<string> GetMentionsTimeline(int maximumNumberOfTweets = 40);
        Task<string> GetMentionsTimeline(IMentionsTimelineParameters timelineParameters);
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
        public Task<string> GetHomeTimeline(int maximumNumberOfTweetsToRetrieve)
        {
            var timelineRequestParameter = _timelineQueryParameterGenerator.CreateHomeTimelineParameters();
            timelineRequestParameter.PageSize = maximumNumberOfTweetsToRetrieve;
            return GetHomeTimeline(timelineRequestParameter);
        }

        public Task<string> GetHomeTimeline(IHomeTimelineParameters timelineParameters)
        {
            string query = _timelineQueryGenerator.GetHomeTimelineQuery(timelineParameters);
            return _twitterAccessor.ExecuteGETQueryReturningJson(query);
        }

        // User Timeline
        public Task<string> GetUserTimeline(IUserIdentifier user, int maximumNumberOfTweets = 40)
        {
            var requestParameters = _timelineQueryParameterGenerator.CreateUserTimelineParameters();
            requestParameters.PageSize = maximumNumberOfTweets;

            return GetUserTimeline(user, requestParameters);
        }

        public Task<string> GetUserTimeline(long userId, int maximumNumberOfTweets = 40)
        {
            var user = _userFactory.GenerateUserIdentifierFromId(userId);
            return GetUserTimeline(user, maximumNumberOfTweets);
        }

        public Task<string> GetUserTimeline(string userScreenName, int maximumNumberOfTweets = 40)
        {
            var user = _userFactory.GenerateUserIdentifierFromScreenName(userScreenName);
            return GetUserTimeline(user, maximumNumberOfTweets);
        }

        public Task<string> GetUserTimeline(IUserIdentifier user, IUserTimelineParameters parameters)
        {
            if (parameters == null)
            {
                parameters = _timelineQueryParameterGenerator.CreateUserTimelineParameters();
            }

            var queryParameters = _timelineQueryParameterGenerator.CreateUserTimelineQueryParameters(user, parameters);

            return GetUserTimeline(queryParameters);
        }

        public Task<string> GetUserTimeline(IUserTimelineQueryParameters timelineParameters)
        {
            var query = _timelineQueryGenerator.GetUserTimelineQuery(timelineParameters);
            return _twitterAccessor.ExecuteGETQueryReturningJson(query);
        }

        // Mentions Timeline
        public Task<string> GetMentionsTimeline(int maximumNumberOfTweets = 40)
        {
            var requestParameters = _timelineQueryParameterGenerator.CreateMentionsTimelineParameters();
            requestParameters.PageSize = maximumNumberOfTweets;

            return GetMentionsTimeline(requestParameters);
        }

        public Task<string> GetMentionsTimeline(IMentionsTimelineParameters timelineParameters)
        {
            var query = _timelineQueryGenerator.GetMentionsTimelineQuery(timelineParameters);
            return _twitterAccessor.ExecuteGETQueryReturningJson(query);
        }
    }
}