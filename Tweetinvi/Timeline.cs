using System;
using System.Collections.Generic;
using Tweetinvi.Core.Injectinvi;
using Tweetinvi.Core.Interfaces;
using Tweetinvi.Core.Interfaces.Controllers;
using Tweetinvi.Core.Interfaces.Models;
using Tweetinvi.Core.Interfaces.Parameters;

namespace Tweetinvi
{
    public static class Timeline
    {
        [ThreadStatic]
        private static ITimelineController _timelineController;
        public static ITimelineController TimelineController
        {
            get
            {
                if (_timelineController == null)
                {
                    Initialize();
                }

                return _timelineController;
            }
        }

        private static IFactory<IHomeTimelineParameters> _homeTimelineParameterFactory;
        private static IFactory<IUserTimelineParameters> _userTimelineParameterFactory;
        private static IFactory<IMentionsTimelineParameters> _mentionsTimelineParameterFactory;
        private static IFactory<IRetweetsOfMeTimelineRequestParameters> _retweetsOfMeTimelineParameterFactory;

        static Timeline()
        {
            Initialize();
        }

        private static void Initialize()
        {
            _timelineController = TweetinviContainer.Resolve<ITimelineController>();
            _homeTimelineParameterFactory = TweetinviContainer.Resolve<IFactory<IHomeTimelineParameters>>();
            _userTimelineParameterFactory = TweetinviContainer.Resolve<IFactory<IUserTimelineParameters>>();
            _mentionsTimelineParameterFactory = TweetinviContainer.Resolve<IFactory<IMentionsTimelineParameters>>();
            _retweetsOfMeTimelineParameterFactory = TweetinviContainer.Resolve<IFactory<IRetweetsOfMeTimelineRequestParameters>>();
        }

        // Parameter generator
        public static IHomeTimelineParameters CreateHomeTimelineRequestParameter()
        {
            return _homeTimelineParameterFactory.Create();
        }

        public static IUserTimelineParameters CreateUserTimelineRequestParameter()
        {
            return _userTimelineParameterFactory.Create();
        }

        public static IMentionsTimelineParameters CreateMentionsTimelineRequestParameters()
        {
            return _mentionsTimelineParameterFactory.Create();
        }

        public static IRetweetsOfMeTimelineRequestParameters CreateRetweetsOfMeTimelineRequestParameters()
        {
            return _retweetsOfMeTimelineParameterFactory.Create();
        }

        // Home Timeline
        public static IEnumerable<ITweet> GetHomeTimeline(int maximumTweets = 40)
        {
            return TimelineController.GetHomeTimeline(maximumTweets);
        }

        public static IEnumerable<ITweet> GetHomeTimeline(IHomeTimelineParameters homeTimelineParameters)
        {
            return TimelineController.GetHomeTimeline(homeTimelineParameters);
        }

        // User Timeline
        public static IEnumerable<ITweet> GetUserTimeline(IUserIdentifier userIdentifier, int maximumTweets = 40)
        {
            return TimelineController.GetUserTimeline(userIdentifier, maximumTweets);
        }

        public static IEnumerable<ITweet> GetUserTimeline(long userId, int maximumTweets = 40)
        {
            return TimelineController.GetUserTimeline(userId, maximumTweets);
        }

        public static IEnumerable<ITweet> GetUserTimeline(string userScreenName, int maximumTweets = 40)
        {
            return TimelineController.GetUserTimeline(userScreenName, maximumTweets);
        }

        public static IEnumerable<ITweet> GetUserTimeline(long userId, IUserTimelineParameters userTimelineParameters)
        {
            return TimelineController.GetUserTimeline(userId, userTimelineParameters);
        }

        public static IEnumerable<ITweet> GetUserTimeline(string userScreenName, IUserTimelineParameters userTimelineParameters)
        {
            return TimelineController.GetUserTimeline(userScreenName, userTimelineParameters);
        }

        public static IEnumerable<ITweet> GetUserTimeline(IUserIdentifier userIdentifier, IUserTimelineParameters userTimelineParameters)
        {
            return TimelineController.GetUserTimeline(userIdentifier, userTimelineParameters);
        }

        // Mention Timeline
        public static IEnumerable<IMention> GetMentionsTimeline(int maximumTweets = 40)
        {
            return TimelineController.GetMentionsTimeline(maximumTweets);
        }

        public static IEnumerable<IMention> GetMentionsTimeline(IMentionsTimelineParameters mentionsTimelineParameters)
        {
            return TimelineController.GetMentionsTimeline(mentionsTimelineParameters);
        }

        // Retweets of Me Timeline
        public static IEnumerable<ITweet> GetRetweetsOfMeTimeline(IRetweetsOfMeTimelineRequestParameters parameters = null)
        {
            return TimelineController.GetRetweetsOfMeTimeline(parameters);
        }
    }
}