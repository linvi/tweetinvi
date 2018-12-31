using System.Collections.Generic;
using System.Threading.Tasks;
using Tweetinvi.Models;
using Tweetinvi.Parameters;

namespace Tweetinvi
{
    public static class TimelineAsync
    {
        // Home Timeline
        public static Task<IEnumerable<ITweet>> GetHomeTimeline(int maximumTweets = 40)
        {
            return Sync.ExecuteTaskAsync(() => Timeline.GetHomeTimeline(maximumTweets));
        }

        public static Task<IEnumerable<ITweet>> GetHomeTimeline(IHomeTimelineParameters timelineParameters)
        {
            return Sync.ExecuteTaskAsync(() => Timeline.GetHomeTimeline(timelineParameters));
        }

        // User Timeline
        public static Task<IEnumerable<ITweet>> GetUserTimeline(IUserIdentifier user, int maximumTweets = 40)
        {
            return Sync.ExecuteTaskAsync(() => Timeline.GetUserTimeline(user, maximumTweets));
        }

        public static Task<IEnumerable<ITweet>> GetUserTimeline(long userId, int maximumTweets = 40)
        {
            return Sync.ExecuteTaskAsync(() => Timeline.GetUserTimeline(userId, maximumTweets));
        }

        public static Task<IEnumerable<ITweet>> GetUserTimeline(string userScreenName, int maximumTweets = 40)
        {
            return Sync.ExecuteTaskAsync(() => Timeline.GetUserTimeline(userScreenName, maximumTweets));
        }

        public static Task<IEnumerable<ITweet>> GetUserTimeline(IUserIdentifier user, IUserTimelineParameters timelineParameters)
        {
            return Sync.ExecuteTaskAsync(() => Timeline.GetUserTimeline(user, timelineParameters));
        }

        // Mention Timeline
        public static Task<IEnumerable<IMention>> GetMentionsTimeline(int maximumTweets = 40)
        {
            return Sync.ExecuteTaskAsync(() => Timeline.GetMentionsTimeline(maximumTweets));
        }

        public static Task<IEnumerable<IMention>> GetMentionsTimeline(IMentionsTimelineParameters timelineParameters)
        {
            return Sync.ExecuteTaskAsync(() => Timeline.GetMentionsTimeline(timelineParameters));
        }
    }
}
