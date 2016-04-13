using Tweetinvi.Core.Interfaces.Models;

namespace Tweetinvi.Core.Parameters.QueryParameters
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