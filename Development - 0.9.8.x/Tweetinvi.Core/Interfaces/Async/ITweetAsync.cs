using System.Collections.Generic;
using System.Threading.Tasks;
using Tweetinvi.Core.Interfaces.Models;

namespace Tweetinvi.Core.Interfaces.Async
{
    public interface ITweetAsync
    {
        Task<bool> PublishAsync();
        
        // Reply
        Task<ITweet> PublishReplyAsync(string text);
        Task<bool> PublishInReplyToAsync(long replyToTweetId);
        Task<bool> PublishInReplyToAsync(ITweet replyToTweet);
        
        // Retweet
        Task<ITweet> PublishRetweetAsync();
        Task<List<ITweet>> GetRetweetsAsync();

        // With Geo
        Task<bool> PublishWithGeoAsync(ICoordinates coordinates);
        Task<bool> PublishWithGeoAsync(double longitude, double latitude);
        Task<bool> PublishWithGeoInReplyToAsync(double latitude, double longitude, long replyToTweetId);
        Task<bool> PublishWithGeoInReplyToAsync(double latitude, double longitude, ITweet replyToTweet);

        // Favourite
        Task FavouriteAsync();
        Task UnFavouriteAsync();

        // Oembed
        Task<IOEmbedTweet> GenerateOEmbedTweetAsync();

        // Destroy
        Task<bool> DestroyAsync();
    }
}
