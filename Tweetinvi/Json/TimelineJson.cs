using System;
using Tweetinvi.Controllers.Timeline;
using Tweetinvi.Core.Interfaces;
using Tweetinvi.Core.Interfaces.Models;
using Tweetinvi.Core.Parameters;

namespace Tweetinvi.Json
{
    public static class TimelineJson
    {
        [ThreadStatic]
        private static ITimelineJsonController _timelineJsonController;
        public static ITimelineJsonController TimelineJsonController
        {
            get
            {
                if (_timelineJsonController == null)
                {
                    Initialize();
                }

                return _timelineJsonController;
            }
        }

        static TimelineJson()
        {
            Initialize();
        }

        private static void Initialize()
        {
            _timelineJsonController = TweetinviContainer.Resolve<ITimelineJsonController>();
        }

        // Home Timeline
        public static string GetHomeTimeline(int maximumTweets = 40)
        {
            return TimelineJsonController.GetHomeTimeline(maximumTweets);
        }

        public static string GetHomeTimeline(IHomeTimelineParameters timelineParameters)
        {
            return TimelineJsonController.GetHomeTimeline(timelineParameters);
        }

        // User Timeline
        public static string GetUserTimeline(IUser user, int maximumTweets = 40)
        {
            return TimelineJsonController.GetUserTimeline(user, maximumTweets);
        }

        public static string GetUserTimeline(IUserIdentifier userDTO, int maximumTweets = 40)
        {
            return TimelineJsonController.GetUserTimeline(userDTO, maximumTweets);
        }

        public static string GetUserTimeline(long userId, int maximumTweets = 40)
        {
            return TimelineJsonController.GetUserTimeline(userId, maximumTweets);
        }

        public static string GetUserTimeline(string userScreenName, int maximumTweets = 40)
        {
            return TimelineJsonController.GetUserTimeline(userScreenName, maximumTweets);
        }

        public static string GetMentionsTimeline(int maximumTweets = 40)
        {
            return TimelineJsonController.GetMentionsTimeline(maximumTweets);
        }
    }
}
