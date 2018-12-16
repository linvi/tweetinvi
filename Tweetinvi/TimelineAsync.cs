using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Tweetinvi.Models;
using Tweetinvi.Parameters;

namespace Tweetinvi
{
    public static class TimelineAsync
    {
        // Home Timeline
        public static ConfiguredTaskAwaitable<IEnumerable<ITweet>> GetHomeTimeline(int maximumTweets = 40)
        {
            return Sync.ExecuteTaskAsync(() => Timeline.GetHomeTimeline(maximumTweets));
        }

        public static ConfiguredTaskAwaitable<IEnumerable<ITweet>> GetHomeTimeline(IHomeTimelineParameters timelineParameters)
        {
            return Sync.ExecuteTaskAsync(() => Timeline.GetHomeTimeline(timelineParameters));
        }

        // User Timeline
        public static ConfiguredTaskAwaitable<IEnumerable<ITweet>> GetUserTimeline(IUserIdentifier user, int maximumTweets = 40)
        {
            return Sync.ExecuteTaskAsync(() => Timeline.GetUserTimeline(user, maximumTweets));
        }

        public static ConfiguredTaskAwaitable<IEnumerable<ITweet>> GetUserTimeline(long userId, int maximumTweets = 40)
        {
            return Sync.ExecuteTaskAsync(() => Timeline.GetUserTimeline(userId, maximumTweets));
        }

        public static ConfiguredTaskAwaitable<IEnumerable<ITweet>> GetUserTimeline(string userScreenName, int maximumTweets = 40)
        {
            return Sync.ExecuteTaskAsync(() => Timeline.GetUserTimeline(userScreenName, maximumTweets));
        }

        public static ConfiguredTaskAwaitable<IEnumerable<ITweet>> GetUserTimeline(IUserIdentifier user, IUserTimelineParameters timelineParameters)
        {
            return Sync.ExecuteTaskAsync(() => Timeline.GetUserTimeline(user, timelineParameters));
        }

        // Mention Timeline
        public static ConfiguredTaskAwaitable<IEnumerable<IMention>> GetMentionsTimeline(int maximumTweets = 40)
        {
            return Sync.ExecuteTaskAsync(() => Timeline.GetMentionsTimeline(maximumTweets));
        }

        public static ConfiguredTaskAwaitable<IEnumerable<IMention>> GetMentionsTimeline(IMentionsTimelineParameters timelineParameters)
        {
            return Sync.ExecuteTaskAsync(() => Timeline.GetMentionsTimeline(timelineParameters));
        }
    }
}
