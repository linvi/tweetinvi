using Tweetinvi.Core.Enum;
using Tweetinvi.Core.Interfaces.DTO;
using Tweetinvi.Core.Interfaces.Models;
using Tweetinvi.Core.Parameters.QueryParameters;

namespace Tweetinvi.Core.Interfaces.QueryGenerators
{
    public interface IUserQueryGenerator
    {
        // Friend Ids
        string GetFriendIdsQuery(IUserIdentifier userIdentifier, int maxFriendsToRetrieve);

        // Followers Ids
        string GetFollowerIdsQuery(IUserIdentifier userIdentifier, int maxFollowersToRetrieve);

        // Favourites
        string GetFavoriteTweetsQuery(IGetUserFavoritesQueryParameters parameters);

        // Block User
        string GetBlockUserQuery(IUserIdentifier userIdentifier);

        // Unblock User
        string GetUnBlockUserQuery(IUserIdentifier userIdentifier);

        // Get Blocked User
        string GetBlockedUserIdsQuery();
        string GetBlockedUsersQuery();

        // Download Profile Image
        string DownloadProfileImageURL(IUserDTO userDTO, ImageSize size = ImageSize.normal);
        string DownloadProfileImageInHttpURL(IUserDTO userDTO, ImageSize size = ImageSize.normal);

        // Spam
        string GetReportUserForSpamQuery(IUserIdentifier userIdentifier);
    }
}