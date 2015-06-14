using System.Collections.Generic;
using System.Threading.Tasks;
using Tweetinvi.Core.Interfaces;
using Tweetinvi.Core.Interfaces.Models;

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
        public static async Task<ITweet> PublishTweet(string text)
        {
            return await Sync.ExecuteTaskAsync(() => Tweet.PublishTweet(text));
        }

        public static async Task<ITweet> PublishTweetWithMedia(string text, byte[] media)
        {
            return await Sync.ExecuteTaskAsync(() => Tweet.PublishTweetWithMedia(text, media));
        }

        // Publish TweetInReplyTo From Text
        public static async Task<ITweet> PublishTweetInReplyTo(string text, ITweet tweetToReplyTo)
        {
            return await Sync.ExecuteTaskAsync(() => Tweet.PublishTweetInReplyTo(text, tweetToReplyTo));
        }

        public static async Task<ITweet> PublishTweetInReplyTo(string text, long tweetToReplyToId)
        {
            return await Sync.ExecuteTaskAsync(() => Tweet.PublishTweetInReplyTo(text, tweetToReplyToId));
        }

        // Publish TweetInReplyTo With Geo info
        public static async Task<ITweet> PublishTweetWithGeoInReplyTo(string text, ICoordinates coordinates, ITweet tweetToReplyTo)
        {
            return await Sync.ExecuteTaskAsync(() => Tweet.PublishTweetWithGeoInReplyTo(text, coordinates, tweetToReplyTo));
        }

        public static async Task<ITweet> PublishTweetWithGeoInReplyTo(string text, ICoordinates coordinates, long tweetIdToReplyTo)
        {
            return await Sync.ExecuteTaskAsync(() => Tweet.PublishTweetWithGeoInReplyTo(text, coordinates, tweetIdToReplyTo));
        }

        public static async Task<ITweet> PublishTweetWithGeoInReplyTo(string text, double longitude, double latitude, ITweet tweetToReplyTo)
        {
            return await Sync.ExecuteTaskAsync(() => Tweet.PublishTweetWithGeoInReplyTo(text, longitude, latitude, tweetToReplyTo));
        }

        public static async Task<ITweet> PublishTweetWithGeoInReplyTo(string text, double longitude, double latitude, long tweetIdToReplyTo)
        {
            return await Sync.ExecuteTaskAsync(() => Tweet.PublishTweetWithGeoInReplyTo(text, longitude, latitude, tweetIdToReplyTo));
        }

        public static async Task<bool> PublishTweetWithGeoInReplyTo(ITweet tweet, ICoordinates coordinates, ITweet tweetToReplyTo)
        {
            return await Sync.ExecuteTaskAsync(() => Tweet.PublishTweetWithGeoInReplyTo(tweet, coordinates, tweetToReplyTo));
        }

        public static async Task<bool> PublishTweetWithGeoInReplyTo(ITweet tweet, ICoordinates coordinates, long tweetIdToReplyTo)
        {
            return await Sync.ExecuteTaskAsync(() => Tweet.PublishTweetWithGeoInReplyTo(tweet, coordinates, tweetIdToReplyTo));
        }

        public static async Task<bool> PublishTweetWithGeoInReplyTo(ITweet tweet, double longitude, double latitude, ITweet tweetToReplyTo)
        {
            return await Sync.ExecuteTaskAsync(() => Tweet.PublishTweetWithGeoInReplyTo(tweet, longitude, latitude, tweetToReplyTo));
        }

        public static async Task<bool> PublishTweetWithGeoInReplyTo(ITweet tweet, double longitude, double latitude, long tweetIdToReplyTo)
        {
            return await Sync.ExecuteTaskAsync(() => Tweet.PublishTweetWithGeoInReplyTo(tweet, longitude, latitude, tweetIdToReplyTo));
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
            return await Sync.ExecuteTaskAsync(() => Tweet.GenerateOEmbedTweet(tweet));
        }

        public static async Task<IOEmbedTweet> GenerateOEmbedTweet(long tweetId)
        {
            return await Sync.ExecuteTaskAsync(() => Tweet.GenerateOEmbedTweet(tweetId));
        }
    }
}
