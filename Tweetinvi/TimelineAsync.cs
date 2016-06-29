using System.Collections.Generic;
using System.Threading.Tasks;
using Tweetinvi.Models;
using Tweetinvi.Parameters;

namespace Tweetinvi
{
    public static class TimelineAsync
    {
        // Home Timeline
        public static async Task<IEnumerable<ITweet>> GetHomeTimeline(int maximumTweets = 40)
        {
            return await Sync.ExecuteTaskAsync(() => Timeline.GetHomeTimeline(maximumTweets));
        }

        public static async Task<IEnumerable<ITweet>> GetHomeTimeline(IHomeTimelineParameters timelineParameters)
        {
            return await Sync.ExecuteTaskAsync(() => Timeline.GetHomeTimeline(timelineParameters));
        }

        // User Timeline
        public static async Task<IEnumerable<ITweet>> GetUserTimeline(IUser user, int maximumTweets = 40)
        {
            return await Sync.ExecuteTaskAsync(() => Timeline.GetUserTimeline(user, maximumTweets));
        }

        public static async Task<IEnumerable<ITweet>> GetUserTimeline(IUserIdentifier userIdentifier, int maximumTweets = 40)
        {
            return await Sync.ExecuteTaskAsync(() => Timeline.GetUserTimeline(userIdentifier, maximumTweets));
        }

        public static async Task<IEnumerable<ITweet>> GetUserTimeline(long userId, int maximumTweets = 40)
        {
            return await Sync.ExecuteTaskAsync(() => Timeline.GetUserTimeline(userId, maximumTweets));
        }

        public static async Task<IEnumerable<ITweet>> GetUserTimeline(string userScreenName, int maximumTweets = 40)
        {
            return await Sync.ExecuteTaskAsync(() => Timeline.GetUserTimeline(userScreenName, maximumTweets));
        }

        public static async Task<IEnumerable<ITweet>> GetUserTimeline(IUserIdentifier userIdentifier, IUserTimelineParameters timelineParameters)
        {
            return await Sync.ExecuteTaskAsync(() => Timeline.GetUserTimeline(userIdentifier, timelineParameters));
        }

        // Mention Timeline
        public static async Task<IEnumerable<IMention>> GetMentionsTimeline(int maximumTweets = 40)
        {
            return await Sync.ExecuteTaskAsync(() => Timeline.GetMentionsTimeline(maximumTweets));
        }

        public static async Task<IEnumerable<IMention>> GetMentionsTimeline(IMentionsTimelineParameters timelineParameters)
        {
            return await Sync.ExecuteTaskAsync(() => Timeline.GetMentionsTimeline(timelineParameters));
        }
    }
}
