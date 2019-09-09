using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Tweetinvi.Core.Parameters;
using Tweetinvi.Core.Web;
using Tweetinvi.Models;
using Tweetinvi.Models.DTO;
using Tweetinvi.Models.DTO.QueryDTO;
using Tweetinvi.Parameters;

namespace Tweetinvi.Core.Controllers
{
    public interface IUserController
    {
        Task<ITwitterResult<IUserDTO, IAuthenticatedUser>> GetAuthenticatedUser(IGetAuthenticatedUserParameters parameters, ITwitterRequest request);
        Task<ITwitterResult<IUserDTO, IUser>> GetUser(IGetUserParameters parameters, ITwitterRequest request);
        Task<ITwitterResult<IUserDTO[], IUser[]>> GetUsers(IGetUsersParameters parameters, ITwitterRequest request);

        TwitterCursorResult<long, IIdsCursorQueryResultDTO> GetFriendIds(IGetFriendIdsParameters parameters, ITwitterRequest request);
        TwitterCursorResult<long, IIdsCursorQueryResultDTO> GetFollowerIds(IGetFollowerIdsParameters parameters, ITwitterRequest request);
        
        Task<ITwitterResult<IUserDTO>> BlockUser(IBlockUserParameters parameters, ITwitterRequest request);
        Task<ITwitterResult<IUserDTO>> UnblockUser(IUnblockUserParameters parameters, ITwitterRequest request);
        Task<ITwitterResult<IUserDTO>> ReportUserForSpam(IReportUserForSpamParameters parameters, ITwitterRequest request);
        TwitterCursorResult<long,IIdsCursorQueryResultDTO> GetBlockedUserIds(IGetBlockedUserIdsParameters parameters, ITwitterRequest request);

        // Favourites
        Task<IEnumerable<ITweet>> GetFavoriteTweets(IGetUserFavoritesQueryParameters parameters);
        Task<IEnumerable<ITweet>> GetFavoriteTweets(IUserIdentifier user, IGetUserFavoritesParameters parameters);

        // Get Blocked Users

        Task<IEnumerable<IUser>> GetBlockedUsers(int maxUsers = 2147483647);

        // Stream Profile Image
        Stream GetProfileImageStream(IUser user, ImageSize imageSize = ImageSize.normal);
        Stream GetProfileImageStream(IUserDTO userDTO, ImageSize imageSize = ImageSize.normal);
    }
}