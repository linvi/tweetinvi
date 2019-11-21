using System;
using System.Threading.Tasks;
using Tweetinvi.Controllers.Timeline;
using Tweetinvi.Models;

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

        // User Timeline
        public static Task<string> GetUserTimeline(IUserIdentifier user, int maximumTweets = 40)
        {
            return TimelineJsonController.GetUserTimeline(user, maximumTweets);
        }

        public static Task<string> GetUserTimeline(long userId, int maximumTweets = 40)
        {
            return TimelineJsonController.GetUserTimeline(userId, maximumTweets);
        }

        public static Task<string> GetUserTimeline(string userScreenName, int maximumTweets = 40)
        {
            return TimelineJsonController.GetUserTimeline(userScreenName, maximumTweets);
        }

        public static Task<string> GetMentionsTimeline(int maximumTweets = 40)
        {
            return TimelineJsonController.GetMentionsTimeline(maximumTweets);
        }
    }
}
