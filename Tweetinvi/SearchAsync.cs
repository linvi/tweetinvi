using System.Collections.Generic;
using System.Threading.Tasks;
using Tweetinvi.Core.Interfaces;
using Tweetinvi.Core.Parameters;

namespace Tweetinvi
{
    public static class SearchAsync
    {
        public static async Task<IEnumerable<ITweet>> SearchTweets(string searchQuery)
        {
            return await Sync.ExecuteTaskAsync(() => Search.SearchTweets(searchQuery));
        }

        public static async Task<IEnumerable<ITweet>> SearchTweets(ITweetSearchParameters tweetSearchParameters)
        {
            return  await Sync.ExecuteTaskAsync(() => Search.SearchTweets(tweetSearchParameters));
        }

        public static async Task<IEnumerable<ITweet>> SearchDirectRepliesTo(ITweet tweet)
        {
            return await Sync.ExecuteTaskAsync(() => Search.SearchDirectRepliesTo(tweet));
        }

        public static async Task<IEnumerable<ITweet>> SearchRepliesTo(ITweet tweet, bool recursiveReplies)
        {
            return await Sync.ExecuteTaskAsync(() =>Search.SearchRepliesTo(tweet, recursiveReplies));
        }
    }
}