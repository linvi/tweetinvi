using System.Collections.Generic;
using System.Threading.Tasks;
using Tweetinvi.Core.Interfaces.Models;

namespace Tweetinvi.Core.Interfaces.Async
{
    public interface ITweetAsync
    {
        // Retweet

        /// <summary>
        /// Retweet the current tweet from the currently authenticated user.
        /// </summary>
        Task<ITweet> PublishRetweetAsync();

        /// <summary>
        /// Get the retweets of the current tweet
        /// </summary>
        Task<List<ITweet>> GetRetweetsAsync();

        // Favourite

        /// <summary>
        /// Favorites the tweet
        /// </summary>
        Task FavoriteAsync();

        /// <summary>
        /// Remove the tweet from favourites
        /// </summary>
        Task UnFavoriteAsync();

        // Oembed

        /// <summary>
        /// Generate an OEmbedTweet
        /// </summary>
        Task<IOEmbedTweet> GenerateOEmbedTweetAsync();

        // Destroy

        /// <summary>
        /// Delete a tweet from Twitter
        /// </summary>
        Task<bool> DestroyAsync();
    }
}