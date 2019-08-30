using Tweetinvi.Core.Parameters;
using Tweetinvi.Models;
using Tweetinvi.Models.DTO;
using Tweetinvi.Parameters;

namespace Tweetinvi.Core.QueryGenerators
{
    public interface IUserQueryGenerator
    {
        string GetAuthenticatedUserQuery (IGetAuthenticatedUserParameters parameters);

        string GetFriendIdsQuery (IUserIdentifier user, int maxFriendsToRetrieve);

        string GetFollowerIdsQuery (IUserIdentifier user, int maxFollowersToRetrieve);

        string GetFavoriteTweetsQuery (IGetUserFavoritesQueryParameters parameters);

        // Block User
        string GetBlockUserQuery (IUserIdentifier user);

        string GetUnBlockUserQuery (IUserIdentifier user);

        string GetBlockedUserIdsQuery ();
        string GetBlockedUsersQuery ();

        // Download Profile Image
        string DownloadProfileImageURL (IUserDTO userDTO, ImageSize size = ImageSize.normal);
        string DownloadProfileImageInHttpURL (IUserDTO userDTO, ImageSize size = ImageSize.normal);

        // Spam
        string GetReportUserForSpamQuery (IUserIdentifier user);
    }
}