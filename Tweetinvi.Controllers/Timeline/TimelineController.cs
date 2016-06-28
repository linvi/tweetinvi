using System.Collections.Generic;
using Tweetinvi.Core.Controllers;
using Tweetinvi.Core.Factories;
using Tweetinvi.Core.Parameters;
using Tweetinvi.Models;
using Tweetinvi.Parameters;

namespace Tweetinvi.Controllers.Timeline
{
    public class TimelineController : ITimelineController
    {
        private readonly ITweetFactory _tweetFactory;
        private readonly ITimelineQueryExecutor _timelineQueryExecutor;
        private readonly IUserFactory _userFactory;
        private readonly ITimelineQueryParameterGenerator _timelineQueryParameterGenerator;

        public TimelineController(
            ITweetFactory tweetFactory,
            ITimelineQueryExecutor timelineQueryExecutor,
            IUserFactory userFactory,
            ITimelineQueryParameterGenerator timelineQueryParameterGenerator)
        {
            _tweetFactory = tweetFactory;
            _timelineQueryExecutor = timelineQueryExecutor;
            _userFactory = userFactory;
            _timelineQueryParameterGenerator = timelineQueryParameterGenerator;
        }

        // Home Timeline
        public IEnumerable<ITweet> GetHomeTimeline(int maximumNumberOfTweetsToRetrieve)
        {
            var timelineRequestParameter = _timelineQueryParameterGenerator.CreateHomeTimelineParameters();
            timelineRequestParameter.MaximumNumberOfTweetsToRetrieve = maximumNumberOfTweetsToRetrieve;

            return GetHomeTimeline(timelineRequestParameter);
        }

        public IEnumerable<ITweet> GetHomeTimeline(IHomeTimelineParameters parameters)
        {
            if (parameters == null)
            {
                parameters = _timelineQueryParameterGenerator.CreateHomeTimelineParameters();
            }

            var timelineDTO = _timelineQueryExecutor.GetHomeTimeline(parameters);
            return _tweetFactory.GenerateTweetsFromDTO(timelineDTO);
        }

        // User Timeline
        public IEnumerable<ITweet> GetUserTimeline(long userId, int maximumNumberOfTweets = 40)
        {
            var userIdentifier = _userFactory.GenerateUserIdentifierFromId(userId);
            return GetUserTimeline(userIdentifier, maximumNumberOfTweets);
        }

        public IEnumerable<ITweet> GetUserTimeline(string userScreenName, int maximumNumberOfTweets = 40)
        {
            var userIdentifier = _userFactory.GenerateUserIdentifierFromScreenName(userScreenName);
            return GetUserTimeline(userIdentifier, maximumNumberOfTweets);
        }

        public IEnumerable<ITweet> GetUserTimeline(IUserIdentifier userIdentifier, int maximumNumberOfTweets = 40)
        {
            var requestParameters = _timelineQueryParameterGenerator.CreateUserTimelineParameters();
            requestParameters.MaximumNumberOfTweetsToRetrieve = maximumNumberOfTweets;

            return GetUserTimeline(userIdentifier, requestParameters);
        }

        public IEnumerable<ITweet> GetUserTimeline(long userId, IUserTimelineParameters parameters)
        {
            return GetUserTimeline(new UserIdentifier(userId), parameters);
        }

        public IEnumerable<ITweet> GetUserTimeline(string userScreenName, IUserTimelineParameters parameters)
        {
            return GetUserTimeline(new UserIdentifier(userScreenName), parameters);
        }

        public IEnumerable<ITweet> GetUserTimeline(IUserIdentifier userIdentifier, IUserTimelineParameters parameters)
        {
            if (parameters == null)
            {
                parameters = _timelineQueryParameterGenerator.CreateUserTimelineParameters();
            }

            var queryParameters = _timelineQueryParameterGenerator.CreateUserTimelineQueryParameters(userIdentifier, parameters);
            return GetUserTimeline(queryParameters);
        }

        private IEnumerable<ITweet> GetUserTimeline(IUserTimelineQueryParameters queryParameters)
        {
            var tweetsDTO = _timelineQueryExecutor.GetUserTimeline(queryParameters);

            return _tweetFactory.GenerateTweetsFromDTO(tweetsDTO);
        }

        // Mention Timeline
        public IEnumerable<IMention> GetMentionsTimeline(int maximumNumberOfTweets = 40)
        {
            var timelineRequestParameter = _timelineQueryParameterGenerator.CreateMentionsTimelineParameters();
            timelineRequestParameter.MaximumNumberOfTweetsToRetrieve = maximumNumberOfTweets;

            return GetMentionsTimeline(timelineRequestParameter);
        }

        public IEnumerable<IMention> GetMentionsTimeline(IMentionsTimelineParameters parameters)
        {
            if (parameters == null)
            {
                parameters = _timelineQueryParameterGenerator.CreateMentionsTimelineParameters();
            }

            var timelineDTO = _timelineQueryExecutor.GetMentionsTimeline(parameters);
            return _tweetFactory.GenerateMentionsFromDTO(timelineDTO);
        }

        // Retweets Of Me Timeline
        public IEnumerable<ITweet> GetRetweetsOfMeTimeline(IRetweetsOfMeTimelineParameters parameters)
        {
            if (parameters == null)
            {
                parameters = _timelineQueryParameterGenerator.CreateRetweetsOfMeTimelineParameters();
            }

            var timelineDTO = _timelineQueryExecutor.GetRetweetsOfMeTimeline(parameters);
            return _tweetFactory.GenerateTweetsFromDTO(timelineDTO);
        }
    }
}