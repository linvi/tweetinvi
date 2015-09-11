using System.Collections.Generic;
using System.Threading.Tasks;
using Tweetinvi.Core.Interfaces.Models;

namespace Tweetinvi.Core.Interfaces.Async
{
    public interface ITweetAsync
    {
        // Retweet
        Task<ITweet> PublishRetweetAsync();
        Task<List<ITweet>> GetRetweetsAsync();

        // Favourite
        Task FavouriteAsync();
        Task UnFavouriteAsync();

        // Oembed
        Task<IOEmbedTweet> GenerateOEmbedTweetAsync();

        // Destroy
        Task<bool> DestroyAsync();
    }
}