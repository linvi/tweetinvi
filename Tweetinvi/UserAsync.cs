using System.Collections.Generic;
using System.Threading.Tasks;
using Tweetinvi.Core.Authentication;
using Tweetinvi.Core.Enum;
using Tweetinvi.Core.Interfaces;
using Tweetinvi.Core.Interfaces.DTO;
using Tweetinvi.Core.Interfaces.Models;
using Tweetinvi.Core.Parameters;

namespace Tweetinvi
{
    public static class UserAsync
    {
        // User Factory
        public static async Task<IAuthenticatedUser> GetAuthenticatedUser()
        {
            return await Sync.ExecuteTaskAsync(() => User.GetAuthenticatedUser());
        }

        public static async Task<IAuthenticatedUser> GetAuthenticatedUser(ITwitterCredentials credentials)
        {
            return await Sync.ExecuteTaskAsync(() => User.GetAuthenticatedUser(credentials));
        }

        public static async Task<IUser> GetUserFromId(long userId)
        {
            return await Sync.ExecuteTaskAsync(() => User.GetUserFromId(userId));
        }

        public static async Task<IUser> GetUserFromScreenName(string userName)
        {
            return await Sync.ExecuteTaskAsync(() => User.GetUserFromScreenName(userName));
        }

        public static async Task<IUser> GenerateUserFromDTO(IUserDTO userDTO)
        {
            return await Sync.ExecuteTaskAsync(() => User.GenerateUserFromDTO(userDTO));
        }

        public static async Task<IEnumerable<IUser>> GenerateUsersFromDTO(IEnumerable<IUserDTO> usersDTO)
        {
            return await Sync.ExecuteTaskAsync(() => User.GenerateUsersFromDTO(usersDTO));
        }

        // User Controller
        // Friend Ids
        public static async Task<IEnumerable<long>> GetFriendIds(IUser user, int maxFriendsToRetrieve = 5000)
        {
            return await Sync.ExecuteTaskAsync(() => User.GetFriendIds(user, maxFriendsToRetrieve));
        }

        public static async Task<IEnumerable<long>> GetFriendIds(IUserIdentifier userDTO, int maxFriendsToRetrieve = 5000)
        {
            return await Sync.ExecuteTaskAsync(() => User.GetFriendIds(userDTO, maxFriendsToRetrieve));
        }

        public static async Task<IEnumerable<long>> GetFriendIds(long userId, int maxFriendsToRetrieve = 5000)
        {
            return await Sync.ExecuteTaskAsync(() => User.GetFriendIds(userId, maxFriendsToRetrieve));
        }

        public static async Task<IEnumerable<long>> GetFriendIds(string userScreenName, int maxFriendsToRetrieve = 5000)
        {
            return await Sync.ExecuteTaskAsync(() => User.GetFriendIds(userScreenName, maxFriendsToRetrieve));
        }

        // Friends
        public static async Task<IEnumerable<IUser>> GetFriends(IUser user, int maxFriendsToRetrieve = 250)
        {
            return await Sync.ExecuteTaskAsync(() => User.GetFriends(user, maxFriendsToRetrieve));
        }

        public static async Task<IEnumerable<IUser>> GetFriends(IUserIdentifier userDTO, int maxFriendsToRetrieve = 250)
        {
            return await Sync.ExecuteTaskAsync(() => User.GetFriends(userDTO, maxFriendsToRetrieve));
        }

        public static async Task<IEnumerable<IUser>> GetFriends(long userId, int maxFriendsToRetrieve = 250)
        {
            return await Sync.ExecuteTaskAsync(() => User.GetFriends(userId, maxFriendsToRetrieve));
        }

        public static async Task<IEnumerable<IUser>> GetFriends(string userScreenName, int maxFriendsToRetrieve = 250)
        {
            return await Sync.ExecuteTaskAsync(() => User.GetFriends(userScreenName, maxFriendsToRetrieve));
        }

        // Follower Ids
        public static async Task<IEnumerable<long>> GetFollowerIds(IUser user, int maxFollowersToRetrieve = 5000)
        {
            return await Sync.ExecuteTaskAsync(() => User.GetFollowerIds(user, maxFollowersToRetrieve));
        }

        public static async Task<IEnumerable<long>> GetFollowerIds(IUserIdentifier userDTO, int maxFollowersToRetrieve = 5000)
        {
            return await Sync.ExecuteTaskAsync(() => User.GetFollowerIds(userDTO, maxFollowersToRetrieve));
        }

        public static async Task<IEnumerable<long>> GetFollowerIds(long userId, int maxFollowersToRetrieve = 5000)
        {
            return await Sync.ExecuteTaskAsync(() => User.GetFollowerIds(userId, maxFollowersToRetrieve));
        }

        public static async Task<IEnumerable<long>> GetFollowerIds(string userScreenName, int maxFollowersToRetrieve = 5000)
        {
            return await Sync.ExecuteTaskAsync(() => User.GetFollowerIds(userScreenName, maxFollowersToRetrieve));
        }

        // Followers
        public static async Task<IEnumerable<IUser>> GetFollowers(IUser user, int maxFollowersToRetrieve = 250)
        {
            return await Sync.ExecuteTaskAsync(() => User.GetFollowers(user, maxFollowersToRetrieve));
        }

        public static async Task<IEnumerable<IUser>> GetFollowers(IUserIdentifier userDTO, int maxFollowersToRetrieve = 250)
        {
            return await Sync.ExecuteTaskAsync(() => User.GetFollowers(userDTO, maxFollowersToRetrieve));
        }

        public static async Task<IEnumerable<IUser>> GetFollowers(long userId, int maxFollowersToRetrieve = 250)
        {
            return await Sync.ExecuteTaskAsync(() => User.GetFollowers(userId, maxFollowersToRetrieve));
        }

        public static async Task<IEnumerable<IUser>> GetFollowers(string userScreenName, int maxFollowersToRetrieve = 250)
        {
            return await Sync.ExecuteTaskAsync(() => User.GetFollowers(userScreenName, maxFollowersToRetrieve));
        }

        // Favourites

        public static async Task<IEnumerable<ITweet>> GetFavoriteTweets(IUserIdentifier userDTO, IGetUserFavoritesParameters parameters = null)
        {
            return await Sync.ExecuteTaskAsync(() => User.GetFavoriteTweets(userDTO, parameters));
        }

        public static async Task<IEnumerable<ITweet>> GetFavoriteTweets(long userId, IGetUserFavoritesParameters parameters = null)
        {
            return await Sync.ExecuteTaskAsync(() => User.GetFavoriteTweets(userId, parameters));
        }

        public static async Task<IEnumerable<ITweet>> GetFavoriteTweets(string userScreenName, IGetUserFavoritesParameters parameters = null)
        {
            return await Sync.ExecuteTaskAsync(() => User.GetFavoriteTweets(userScreenName, parameters));
        }

        // Block User
        public static async Task<bool> BlockUser(IUser user)
        {
            return await Sync.ExecuteTaskAsync(() => User.BlockUser(user));
        }

        public static async Task<bool> BlockUser(IUserIdentifier userDTO)
        {
            return await Sync.ExecuteTaskAsync(() => User.BlockUser(userDTO));
        }

        public static async Task<bool> BlockUser(long userId)
        {
            return await Sync.ExecuteTaskAsync(() => User.BlockUser(userId));
        }

        public static async Task<bool> BlockUser(string userScreenName)
        {
            return await Sync.ExecuteTaskAsync(() => User.BlockUser(userScreenName));
        }

        // Stream Profile Image 
        public static async Task<System.IO.Stream> GetProfileImageStream(IUser user, ImageSize imageSize = ImageSize.normal)
        {
            return await Sync.ExecuteTaskAsync(() => User.GetProfileImageStream(user, imageSize));
        }

        public static async Task<System.IO.Stream> GetProfileImageStream(IUserDTO userDTO, ImageSize imageSize = ImageSize.normal)
        {
            return await Sync.ExecuteTaskAsync(() => User.GetProfileImageStream(userDTO, imageSize));
        }
    }
}
