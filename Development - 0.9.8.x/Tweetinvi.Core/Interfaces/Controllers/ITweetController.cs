using System.Collections.Generic;
using Tweetinvi.Core.Interfaces.DTO;
using Tweetinvi.Core.Interfaces.Models;

namespace Tweetinvi.Core.Interfaces.Controllers
{
    public interface ITweetController
    {
        // Publish Tweet
        bool PublishTweet(ITweet tweetToPublish);
        ITweet PublishTweet(ITweetDTO tweetToPublish);

        // Publish Tweet in reply to
        bool PublishTweetInReplyTo(ITweet tweetToPublish, ITweet tweetToReplyTo);
        ITweet PublishTweetInReplyTo(ITweetDTO tweetToPublish, ITweetDTO tweetToReplyTo);
        bool PublishTweetInReplyTo(ITweet tweetToPublish, long tweetIdToReplyTo);
        ITweet PublishTweetInReplyTo(ITweetDTO tweetToPublish, long tweetIdToReplyTo);

        // Publish Tweet With Geo
        bool PublishTweetWithGeo(ITweet tweetToPublish, ICoordinates coordinates);
        ITweet PublishTweetWithGeo(ITweetDTO tweetToPublish, ICoordinates coordinates);
        bool PublishTweetWithGeo(ITweet tweetToPublish, double longitude, double latitude);
        ITweet PublishTweetWithGeo(ITweetDTO tweetToPublish, double longitude, double latitude);

        // Publish Tweet
        bool PublishTweetWithGeoInReplyTo(ITweet tweetToPublish, ICoordinates coordinates, long tweetIdToReplyTo);
        ITweet PublishTweetWithGeoInReplyTo(ITweetDTO tweetToPublish, ICoordinates coordinates, long tweetIdToReplyTo);
        bool PublishTweetWithGeoInReplyTo(ITweet tweetToPublish, double longitude, double latitude, long tweetIdToReplyTo);
        ITweet PublishTweetWithGeoInReplyTo(ITweetDTO tweetToPublish, double longitude, double latitude, long tweetIdToReplyTo);

        // Publish Retweet
        ITweet PublishRetweet(ITweet tweetToPublish);
        ITweet PublishRetweet(ITweetDTO tweetToPublish);
        ITweet PublishRetweet(long tweetId);

        // Get Retweets
        IEnumerable<ITweet> GetRetweets(ITweet tweet);
        IEnumerable<ITweet> GetRetweets(ITweetDTO tweetDTO);
        IEnumerable<ITweet> GetRetweets(long tweetId);

        // Destroy Tweet
        bool DestroyTweet(ITweet tweet);
        bool DestroyTweet(ITweetDTO tweetDTO);
        bool DestroyTweet(long tweetId);

        // Generate OembedTweet
        IOEmbedTweet GenerateOEmbedTweet(ITweet tweet);
        IOEmbedTweet GenerateOEmbedTweet(ITweetDTO tweetDTO);
        IOEmbedTweet GenerateOEmbedTweet(long tweetId);

        // Favorite Tweet
        bool FavoriteTweet(ITweet tweet);
        bool FavoriteTweet(ITweetDTO tweetDTO);
        bool FavoriteTweet(long tweetId);

        bool UnFavoriteTweet(ITweet tweet);
        bool UnFavoriteTweet(ITweetDTO tweetDTO);
        bool UnFavoriteTweet(long tweetId);

        // Update Published Tweet
        void UpdateTweetIfTweetSuccessfullyBeenPublished(ITweet sourceTweet, ITweetDTO publishedTweetDTO);
    }
}