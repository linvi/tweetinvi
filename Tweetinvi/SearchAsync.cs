using System.Collections.Generic;
using System.Threading.Tasks;
using Tweetinvi.Models;
using Tweetinvi.Parameters;

namespace Tweetinvi
{
    public static class SearchAsync
    {
        public static async Task<IEnumerable<ITweet>> SearchTweets(string searchQuery)
        {
            return await Sync.ExecuteTaskAsync(() => Search.SearchTweets(searchQuery));
        }

        public static async Task<IEnumerable<ITweet>> SearchTweets(ISearchTweetsParameters searchTweetsParameters)
        {
            return  await Sync.ExecuteTaskAsync(() => Search.SearchTweets(searchTweetsParameters));
        }

        public static async Task<IEnumerable<ITweet>> SearchDirectRepliesTo(ITweet tweet)
        {
            return await Sync.ExecuteTaskAsync(() => Search.SearchDirectRepliesTo(tweet));
        }

        public static async Task<IEnumerable<ITweet>> SearchRepliesTo(ITweet tweet, bool recursiveReplies)
        {
            return await Sync.ExecuteTaskAsync(() => Search.SearchRepliesTo(tweet, recursiveReplies));
        }

        public static async Task<IEnumerable<IUser>> SearchUsers(string query, int maximumNumberOfResults = 20, int page = 0)
        {
            return await Sync.ExecuteTaskAsync(() => Search.SearchUsers(query, maximumNumberOfResults, page));
        }
    }
}