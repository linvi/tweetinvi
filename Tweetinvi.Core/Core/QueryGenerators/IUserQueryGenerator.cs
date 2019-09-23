using Tweetinvi.Models;
using Tweetinvi.Models.DTO;
using Tweetinvi.Parameters;

namespace Tweetinvi.Core.QueryGenerators
{
    public interface IUserQueryGenerator
    {
        string GetAuthenticatedUserQuery(IGetAuthenticatedUserParameters parameters);
        string GetUserQuery(IGetUserParameters parameters);
        string GetUsersQuery(IGetUsersParameters parameters, TweetMode? tweetMode);


        string GetFriendIdsQuery(IGetFriendIdsParameters parameters);


        string GetFollowUserQuery(IFollowUserParameters parameters);
        string GetUnFollowUserQuery(IUnFollowUserParameters parameters);
        string GetFollowerIdsQuery(IGetFollowerIdsParameters parameters);

        // Block User
        string GetBlockUserQuery(IBlockUserParameters parameters);
        string GetUnblockUserQuery(IUnblockUserParameters parameters);
        string GetReportUserForSpamQuery(IReportUserForSpamParameters parameters);
        string GetBlockedUserIdsQuery(IGetBlockedUserIdsParameters parameters);
        string GetBlockedUsersQuery(IGetBlockedUsersParameters parameters);

        // Download Profile Image
        string DownloadProfileImageURL(IUserDTO userDTO, ImageSize size = ImageSize.normal);
        string DownloadProfileImageInHttpURL(IUserDTO userDTO, ImageSize size = ImageSize.normal);
    }
}