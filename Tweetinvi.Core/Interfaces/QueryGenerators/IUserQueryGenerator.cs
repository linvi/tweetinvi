using Tweetinvi.Core.Enum;
using Tweetinvi.Core.Interfaces.DTO;
using Tweetinvi.Core.Interfaces.Models;
using Tweetinvi.Core.Parameters.QueryParameters;

namespace Tweetinvi.Core.Interfaces.QueryGenerators
{
    public interface IUserQueryGenerator
    {
        // Friend Ids
        string GetFriendIdsQuery(IUserIdentifier userDTO, int maxFriendsToRetrieve);
        string GetFriendIdsQuery(long userId, int maxFriendsToRetrieve);
        string GetFriendIdsQuery(string screenName, int maxFriendsToRetrieve);

        // Followers Ids
        string GetFollowerIdsQuery(IUserIdentifier userDTO, int maxFollowersToRetrieve);
        string GetFollowerIdsQuery(long userId, int maxFollowersToRetrieve);
        string GetFollowerIdsQuery(string screenName, int maxFollowersToRetrieve);

        // Favourites
        string GetFavoriteTweetsQuery(IGetUserFavoritesQueryParameters parameters);

        // Block User
        string GetBlockUserQuery(IUserIdentifier userDTO);
        string GetBlockUserQuery(long userId);
        string GetBlockUserQuery(string userScreenName);

        // Unblock User
        string GetUnBlockUserQuery(IUserIdentifier userDTO);
        string GetUnBlockUserQuery(long userId);
        string GetUnBlockUserQuery(string userScreenName);

        // Get Blocked User
        string GetBlockedUserIdsQuery();
        string GetBlockedUsersQuery();

        // Download Profile Image
        string DownloadProfileImageURL(IUserDTO userDTO, ImageSize size = ImageSize.normal);
        string DownloadProfileImageInHttpURL(IUserDTO userDTO, ImageSize size = ImageSize.normal);

        // Spam
        string GetReportUserForSpamQuery(IUserIdentifier userIdentifier);
        string GetReportUserForSpamQuery(long userId);
        string GetReportUserForSpamQuery(string userScreenName);
    }
}