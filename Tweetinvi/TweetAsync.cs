using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Tweetinvi.Models;
using Tweetinvi.Parameters;

namespace Tweetinvi
{
    public class TweetAsync
    {
        // Tweet Factory
        public static ConfiguredTaskAwaitable<ITweet> GetTweet(long tweetId)
        {
            return Sync.ExecuteTaskAsync(() => Tweet.GetTweet(tweetId));
        }

        // Tweet Controller
        public static ConfiguredTaskAwaitable<ITweet> PublishTweet(string text, IPublishTweetOptionalParameters parameters = null)
        {
            return Sync.ExecuteTaskAsync(() => Tweet.PublishTweet(text, parameters));
        }

        // Publish TweetInReplyTo From Text
        public static ConfiguredTaskAwaitable<ITweet> PublishTweetInReplyTo(string text, ITweetIdentifier tweetToReplyTo)
        {
            return Sync.ExecuteTaskAsync(() => Tweet.PublishTweetInReplyTo(text, tweetToReplyTo));
        }

        public static ConfiguredTaskAwaitable<ITweet> PublishTweetInReplyTo(string text, long tweetToReplyToId)
        {
            return Sync.ExecuteTaskAsync(() => Tweet.PublishTweetInReplyTo(text, tweetToReplyToId));
        }

        // Publish Retweet
        public static ConfiguredTaskAwaitable<ITweet> PublishRetweet(ITweet tweet)
        {
            return Sync.ExecuteTaskAsync(() => Tweet.PublishRetweet(tweet));
        }

        public static ConfiguredTaskAwaitable<ITweet> PublishRetweet(long tweetId)
        {
            return Sync.ExecuteTaskAsync(() => Tweet.PublishRetweet(tweetId));
        }

        // Get Retweet
        public static ConfiguredTaskAwaitable<IEnumerable<ITweet>> GetRetweets(ITweet tweet)
        {
            return Sync.ExecuteTaskAsync(() => Tweet.GetRetweets(tweet));
        }

        public static ConfiguredTaskAwaitable<IEnumerable<ITweet>> GetRetweets(long tweetId)
        {
            return Sync.ExecuteTaskAsync(() => Tweet.GetRetweets(tweetId));
        }

        // Destroy Tweet
        public static ConfiguredTaskAwaitable<bool> DestroyTweet(ITweet tweet)
        {
            return Sync.ExecuteTaskAsync(() => Tweet.DestroyTweet(tweet));
        }

        public static ConfiguredTaskAwaitable<bool> DestroyTweet(long tweetId)
        {
            return Sync.ExecuteTaskAsync(() => Tweet.DestroyTweet(tweetId));
        }

        // Favorite Tweet
        public static ConfiguredTaskAwaitable<bool> FavoriteTweet(ITweet tweet)
        {
            return Sync.ExecuteTaskAsync(() => Tweet.FavoriteTweet(tweet));
        }

        public static ConfiguredTaskAwaitable<bool> FavoriteTweet(long tweetId)
        {
            return Sync.ExecuteTaskAsync(() => Tweet.FavoriteTweet(tweetId));
        }

        // Generate OEmbedTweet
        public static ConfiguredTaskAwaitable<IOEmbedTweet> GenerateOEmbedTweet(ITweet tweet)
        {
            return Sync.ExecuteTaskAsync(() => Tweet.GetOEmbedTweet(tweet));
        }

        public static ConfiguredTaskAwaitable<IOEmbedTweet> GenerateOEmbedTweet(long tweetId)
        {
            return Sync.ExecuteTaskAsync(() => Tweet.GetOEmbedTweet(tweetId));
        }
    }
}
