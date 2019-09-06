using System;
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

        // Friends
        TwitterCursorResult<long, IIdsCursorQueryResultDTO> GetFriendIds(
            IGetFriendIdsParameters parameters,
            ITwitterRequest request);

        Task<IEnumerable<IUser>> GetFriends(IUserIdentifier user, int maxFriendsToRetrieve = 250);
        Task<IEnumerable<IUser>> GetFriends(long userId, int maxFriendsToRetrieve = 250);
        Task<IEnumerable<IUser>> GetFriends(string userScreenName, int maxFriendsToRetrieve = 250);

        // Followers
        Task<IEnumerable<long>> GetFollowerIds(IUserIdentifier user, int maxFollowersToRetrieve = 5000);
        Task<IEnumerable<long>> GetFollowerIds(long userId, int maxFollowersToRetrieve = 5000);
        Task<IEnumerable<long>> GetFollowerIds(string userScreenName, int maxFollowersToRetrieve = 5000);

        Task<IEnumerable<IUser>> GetFollowers(IUserIdentifier user, int maxFollowersToRetrieve = 250);
        Task<IEnumerable<IUser>> GetFollowers(long userId, int maxFollowersToRetrieve = 250);
        Task<IEnumerable<IUser>> GetFollowers(string userScreenName, int maxFollowersToRetrieve = 250);

        // Favourites
        Task<IEnumerable<ITweet>> GetFavoriteTweets(IGetUserFavoritesQueryParameters parameters);
        Task<IEnumerable<ITweet>> GetFavoriteTweets(IUserIdentifier user, IGetUserFavoritesParameters parameters);

        // Block User
        Task<bool> BlockUser(IUserIdentifier user);
        Task<bool> BlockUser(long userId);
        Task<bool> BlockUser(string userScreenName);

        // Unblock User
        Task<bool> UnBlockUser(IUserIdentifier user);
        Task<bool> UnBlockUser(long userId);
        Task<bool> UnBlockUser(string userScreenName);

        // Get Blocked Users
        Task<IEnumerable<long>> GetBlockedUserIds(int maxUserIds = Int32.MaxValue);

        Task<IEnumerable<IUser>> GetBlockedUsers(int maxUsers = 2147483647);

        // Stream Profile Image
        Stream GetProfileImageStream(IUser user, ImageSize imageSize = ImageSize.normal);
        Stream GetProfileImageStream(IUserDTO userDTO, ImageSize imageSize = ImageSize.normal);

        // Report Spam
        Task<bool> ReportUserForSpam(IUserIdentifier user);
        Task<bool> ReportUserForSpam(long userId);
        Task<bool> ReportUserForSpam(string userScreenName);
    }
}