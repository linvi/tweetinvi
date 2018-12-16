using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Tweetinvi.Models;
using Tweetinvi.Parameters;

namespace Tweetinvi
{
    public static class SearchAsync
    {
        public static ConfiguredTaskAwaitable<IEnumerable<ITweet>> SearchTweets(string searchQuery)
        {
            return Sync.ExecuteTaskAsync(() => Search.SearchTweets(searchQuery));
        }

        public static ConfiguredTaskAwaitable<IEnumerable<ITweet>> SearchTweets(ISearchTweetsParameters searchTweetsParameters)
        {
            return  Sync.ExecuteTaskAsync(() => Search.SearchTweets(searchTweetsParameters));
        }

        public static ConfiguredTaskAwaitable<IEnumerable<ITweet>> SearchDirectRepliesTo(ITweet tweet)
        {
            return Sync.ExecuteTaskAsync(() => Search.SearchDirectRepliesTo(tweet));
        }

        public static ConfiguredTaskAwaitable<IEnumerable<ITweet>> SearchRepliesTo(ITweet tweet, bool recursiveReplies)
        {
            return Sync.ExecuteTaskAsync(() => Search.SearchRepliesTo(tweet, recursiveReplies));
        }

        public static ConfiguredTaskAwaitable<IEnumerable<IUser>> SearchUsers(string query, int maximumNumberOfResults = 20, int page = 0)
        {
            return Sync.ExecuteTaskAsync(() => Search.SearchUsers(query, maximumNumberOfResults, page));
        }
    }
}