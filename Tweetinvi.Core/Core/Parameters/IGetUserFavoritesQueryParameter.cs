using Tweetinvi.Models;
using Tweetinvi.Parameters;

namespace Tweetinvi.Core.Parameters
{
    /// <summary>
    /// https://dev.twitter.com/rest/reference/get/favorites/list
    /// </summary>
    public interface IGetUserFavoritesQueryParameters
    {
        /// <summary>
        /// User from whom you want to receive the favorites.
        /// </summary>
        IUserIdentifier UserIdentifier { get; set; }

        /// <summary>
        /// Optional parameters to get favourites.
        /// </summary>
        IGetUserFavoritesParameters Parameters { get; set; }
    }
}