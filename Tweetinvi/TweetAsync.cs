using System.Collections.Generic;
using System.Threading.Tasks;
using Tweetinvi.Models;
using Tweetinvi.Parameters;

namespace Tweetinvi
{
    public class TweetAsync
    {
        // Tweet Factory
        public static async Task<ITweet> GetTweet(long tweetId)
        {
            return await Sync.ExecuteTaskAsync(() => Tweet.GetTweet(tweetId));
        }

        // Tweet Controller
        public static async Task<ITweet> PublishTweet(string text, IPublishTweetOptionalParameters parameters = null)
        {
            return await Sync.ExecuteTaskAsync(() => Tweet.PublishTweet(text, parameters));
        }

        public static async Task<ITweet> PublishTweetWithImage(string text, byte[] media)
        {
            return await Sync.ExecuteTaskAsync(() => Tweet.PublishTweetWithImage(text, media));
        }

        public static async Task<ITweet> PublishTweetWithVideo(string text, byte[] media)
        {
            return await Sync.ExecuteTaskAsync(() => Tweet.PublishTweetWithVideo(text, media));
        }

        // Publish TweetInReplyTo From Text
        public static async Task<ITweet> PublishTweetInReplyTo(string text, ITweetIdentifier tweetToReplyTo)
        {
            return await Sync.ExecuteTaskAsync(() => Tweet.PublishTweetInReplyTo(text, tweetToReplyTo));
        }

        public static async Task<ITweet> PublishTweetInReplyTo(string text, long tweetToReplyToId)
        {
            return await Sync.ExecuteTaskAsync(() => Tweet.PublishTweetInReplyTo(text, tweetToReplyToId));
        }

        // Publish Retweet
        public static async Task<ITweet> PublishRetweet(ITweet tweet)
        {
            return await Sync.ExecuteTaskAsync(() => Tweet.PublishRetweet(tweet));
        }

        public static async Task<ITweet> PublishRetweet(long tweetId)
        {
            return await Sync.ExecuteTaskAsync(() => Tweet.PublishRetweet(tweetId));
        }

        // Get Retweet
        public static async Task<IEnumerable<ITweet>> GetRetweets(ITweet tweet)
        {
            return await Sync.ExecuteTaskAsync(() => Tweet.GetRetweets(tweet));
        }

        public static async Task<IEnumerable<ITweet>> GetRetweets(long tweetId)
        {
            return await Sync.ExecuteTaskAsync(() => Tweet.GetRetweets(tweetId));
        }

        // Destroy Tweet
        public static async Task<bool> DestroyTweet(ITweet tweet)
        {
            return await Sync.ExecuteTaskAsync(() => Tweet.DestroyTweet(tweet));
        }

        public static async Task<bool> DestroyTweet(long tweetId)
        {
            return await Sync.ExecuteTaskAsync(() => Tweet.DestroyTweet(tweetId));
        }

        // Favorite Tweet
        public static async Task<bool> FavoriteTweet(ITweet tweet)
        {
            return await Sync.ExecuteTaskAsync(() => Tweet.FavoriteTweet(tweet));
        }

        public static async Task<bool> FavoriteTweet(long tweetId)
        {
            return await Sync.ExecuteTaskAsync(() => Tweet.FavoriteTweet(tweetId));
        }

        // Generate OEmbedTweet
        public static async Task<IOEmbedTweet> GenerateOEmbedTweet(ITweet tweet)
        {
            return await Sync.ExecuteTaskAsync(() => Tweet.GetOEmbedTweet(tweet));
        }

        public static async Task<IOEmbedTweet> GenerateOEmbedTweet(long tweetId)
        {
            return await Sync.ExecuteTaskAsync(() => Tweet.GetOEmbedTweet(tweetId));
        }
    }
}
