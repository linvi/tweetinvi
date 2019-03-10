using System.Collections.Generic;
using System.Threading.Tasks;
using Tweetinvi.Models;
using Tweetinvi.Parameters;

namespace Tweetinvi
{
    public class TweetAsync
    {
        // Tweet Factory
        public static Task<ITweet> GetTweet(long tweetId)
        {
            return Sync.ExecuteTaskAsync(() => Tweet.GetTweet(tweetId));
        }

        // Tweet Controller
        public static Task<ITweet> PublishTweet(IPublishTweetParameters parameters)
        {
            return Sync.ExecuteTaskAsync(() => Tweet.PublishTweet(parameters));
        }

        public static Task<ITweet> PublishTweet(string text)
        {
            return Sync.ExecuteTaskAsync(() => Tweet.PublishTweet(text));
        }

        // Publish TweetInReplyTo From Text
        public static Task<ITweet> PublishTweetInReplyTo(string text, ITweetIdentifier tweetToReplyTo)
        {
            return Sync.ExecuteTaskAsync(() => Tweet.PublishTweetInReplyTo(text, tweetToReplyTo));
        }

        public static Task<ITweet> PublishTweetInReplyTo(string text, long tweetToReplyToId)
        {
            return Sync.ExecuteTaskAsync(() => Tweet.PublishTweetInReplyTo(text, tweetToReplyToId));
        }

        // Publish Retweet
        public static Task<ITweet> PublishRetweet(ITweet tweet)
        {
            return Sync.ExecuteTaskAsync(() => Tweet.PublishRetweet(tweet));
        }

        public static Task<ITweet> PublishRetweet(long tweetId)
        {
            return Sync.ExecuteTaskAsync(() => Tweet.PublishRetweet(tweetId));
        }

        // Get Retweet
        public static Task<IEnumerable<ITweet>> GetRetweets(ITweet tweet)
        {
            return Sync.ExecuteTaskAsync(() => Tweet.GetRetweets(tweet));
        }

        public static Task<IEnumerable<ITweet>> GetRetweets(long tweetId)
        {
            return Sync.ExecuteTaskAsync(() => Tweet.GetRetweets(tweetId));
        }

        // Destroy Tweet
        public static Task<bool> DestroyTweet(ITweet tweet)
        {
            return Sync.ExecuteTaskAsync(() => Tweet.DestroyTweet(tweet));
        }

        public static Task<bool> DestroyTweet(long tweetId)
        {
            return Sync.ExecuteTaskAsync(() => Tweet.DestroyTweet(tweetId));
        }

        // Favorite Tweet
        public static Task<bool> FavoriteTweet(ITweet tweet)
        {
            return Sync.ExecuteTaskAsync(() => Tweet.FavoriteTweet(tweet));
        }

        public static Task<bool> FavoriteTweet(long tweetId)
        {
            return Sync.ExecuteTaskAsync(() => Tweet.FavoriteTweet(tweetId));
        }

        // Generate OEmbedTweet
        public static Task<IOEmbedTweet> GenerateOEmbedTweet(ITweet tweet)
        {
            return Sync.ExecuteTaskAsync(() => Tweet.GetOEmbedTweet(tweet));
        }

        public static Task<IOEmbedTweet> GenerateOEmbedTweet(long tweetId)
        {
            return Sync.ExecuteTaskAsync(() => Tweet.GetOEmbedTweet(tweetId));
        }
    }
}
