using System;
using System.Collections.Generic;
using Tweetinvi.Core.Injectinvi;
using Tweetinvi.Core.Interfaces;
using Tweetinvi.Core.Interfaces.Controllers;
using Tweetinvi.Core.Interfaces.Models;
using Tweetinvi.Core.Parameters;

namespace Tweetinvi
{
    public static class Timeline
    {
        [ThreadStatic]
        private static ITimelineController _timelineController;

        /// <summary>
        /// Controller handling any Timeline request
        /// </summary>
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
        private static IFactory<IRetweetsOfMeTimelineParameters> _retweetsOfMeTimelineParameterFactory;

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
            _retweetsOfMeTimelineParameterFactory = TweetinviContainer.Resolve<IFactory<IRetweetsOfMeTimelineParameters>>();
        }

        // Parameter generator

        /// <summary>
        /// Parameters available to refine the results from the Home Timeline
        /// </summary>
        public static IHomeTimelineParameters CreateHomeTimelineParameter()
        {
            return _homeTimelineParameterFactory.Create();
        }

        /// <summary>
        /// Parameters available to refine the results from a User Timeline
        /// </summary>
        public static IUserTimelineParameters CreateUserTimelineParameter()
        {
            return _userTimelineParameterFactory.Create();
        }

        /// <summary>
        /// Parameters available to refine the results from the Mentions Timeline
        /// </summary>
        public static IMentionsTimelineParameters CreateMentionsTimelineParameters()
        {
            return _mentionsTimelineParameterFactory.Create();
        }

        /// <summary>
        /// Parameters available to refine the results from the Retweets of Me Timeline
        /// </summary>
        public static IRetweetsOfMeTimelineParameters CreateRetweetsOfMeTimelineParameters()
        {
            return _retweetsOfMeTimelineParameterFactory.Create();
        }

        // Home Timeline

        /// <summary>
        /// Get the tweets visible on the authenticated user timeline.
        /// </summary>
        public static IEnumerable<ITweet> GetHomeTimeline(int maximumTweets = 40)
        {
            return TimelineController.GetHomeTimeline(maximumTweets);
        }

        /// <summary>
        /// Get the tweets visible on the authenticated user timeline.
        /// </summary>
        public static IEnumerable<ITweet> GetHomeTimeline(IHomeTimelineParameters homeTimelineParameters)
        {
            return TimelineController.GetHomeTimeline(homeTimelineParameters);
        }

        // User Timeline

        /// <summary>
        /// Get the tweets visible on the specified user Timeline
        /// </summary>
        public static IEnumerable<ITweet> GetUserTimeline(IUserIdentifier userIdentifier, int maximumTweets = 40)
        {
            return TimelineController.GetUserTimeline(userIdentifier, maximumTweets);
        }

        /// <summary>
        /// Get the tweets visible on the specified user Timeline
        /// </summary>
        public static IEnumerable<ITweet> GetUserTimeline(long userId, int maximumTweets = 40)
        {
            return TimelineController.GetUserTimeline(userId, maximumTweets);
        }

        /// <summary>
        /// Get the tweets visible on the specified user Timeline
        /// </summary>
        public static IEnumerable<ITweet> GetUserTimeline(string userScreenName, int maximumTweets = 40)
        {
            return TimelineController.GetUserTimeline(userScreenName, maximumTweets);
        }

        /// <summary>
        /// Get the tweets visible on the specified user Timeline
        /// </summary>
        public static IEnumerable<ITweet> GetUserTimeline(long userId, IUserTimelineParameters userTimelineParameters)
        {
            return TimelineController.GetUserTimeline(userId, userTimelineParameters);
        }

        /// <summary>
        /// Get the tweets visible on the specified user Timeline
        /// </summary>
        public static IEnumerable<ITweet> GetUserTimeline(string userScreenName, IUserTimelineParameters userTimelineParameters)
        {
            return TimelineController.GetUserTimeline(userScreenName, userTimelineParameters);
        }

        /// <summary>
        /// Get the tweets visible on the specified user Timeline
        /// </summary>
        public static IEnumerable<ITweet> GetUserTimeline(IUserIdentifier userIdentifier, IUserTimelineParameters userTimelineParameters)
        {
            return TimelineController.GetUserTimeline(userIdentifier, userTimelineParameters);
        }

        // Mention Timeline

        /// <summary>
        /// Get the tweets visible on your mentions timeline
        /// </summary>
        public static IEnumerable<IMention> GetMentionsTimeline(int maximumTweets = 40)
        {
            return TimelineController.GetMentionsTimeline(maximumTweets);
        }

        /// <summary>
        /// Get the tweets visible on your mentions timeline
        /// </summary>
        public static IEnumerable<IMention> GetMentionsTimeline(IMentionsTimelineParameters mentionsTimelineParameters)
        {
            return TimelineController.GetMentionsTimeline(mentionsTimelineParameters);
        }

        // Retweets of Me Timeline

        /// <summary>
        /// Get the tweets visible on your retweets of me Timeline
        /// </summary>
        public static IEnumerable<ITweet> GetRetweetsOfMeTimeline(IRetweetsOfMeTimelineParameters parameters = null)
        {
            return TimelineController.GetRetweetsOfMeTimeline(parameters);
        }
    }
}