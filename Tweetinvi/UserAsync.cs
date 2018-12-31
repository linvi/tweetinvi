using System.Collections.Generic;
using System.Threading.Tasks;
using Tweetinvi.Models;
using Tweetinvi.Models.DTO;
using Tweetinvi.Parameters;

namespace Tweetinvi
{
    public static class UserAsync
    {
        // User Factory
        public static Task<IAuthenticatedUser> GetAuthenticatedUser()
        {
            return Sync.ExecuteTaskAsync(() => User.GetAuthenticatedUser());
        }

        public static Task<IAuthenticatedUser> GetAuthenticatedUser(ITwitterCredentials credentials)
        {
            return Sync.ExecuteTaskAsync(() => User.GetAuthenticatedUser(credentials));
        }

        public static Task<IUser> GetUserFromId(long userId)
        {
            return Sync.ExecuteTaskAsync(() => User.GetUserFromId(userId));
        }

        public static Task<IUser> GetUserFromScreenName(string userName)
        {
            return Sync.ExecuteTaskAsync(() => User.GetUserFromScreenName(userName));
        }

        public static Task<IUser> GenerateUserFromDTO(IUserDTO userDTO)
        {
            return Sync.ExecuteTaskAsync(() => User.GenerateUserFromDTO(userDTO));
        }

        public static Task<IEnumerable<IUser>> GenerateUsersFromDTO(IEnumerable<IUserDTO> usersDTO)
        {
            return Sync.ExecuteTaskAsync(() => User.GenerateUsersFromDTO(usersDTO));
        }

        // User Controller
        // Friend Ids
        public static Task<IEnumerable<long>> GetFriendIds(IUserIdentifier user, int maxFriendsToRetrieve = 5000)
        {
            return Sync.ExecuteTaskAsync(() => User.GetFriendIds(user, maxFriendsToRetrieve));
        }

        public static Task<IEnumerable<long>> GetFriendIds(long userId, int maxFriendsToRetrieve = 5000)
        {
            return Sync.ExecuteTaskAsync(() => User.GetFriendIds(userId, maxFriendsToRetrieve));
        }

        public static Task<IEnumerable<long>> GetFriendIds(string userScreenName, int maxFriendsToRetrieve = 5000)
        {
            return Sync.ExecuteTaskAsync(() => User.GetFriendIds(userScreenName, maxFriendsToRetrieve));
        }

        // Friends
        public static Task<IEnumerable<IUser>> GetFriends(IUserIdentifier user, int maxFriendsToRetrieve = 250)
        {
            return Sync.ExecuteTaskAsync(() => User.GetFriends(user, maxFriendsToRetrieve));
        }

        public static Task<IEnumerable<IUser>> GetFriends(long userId, int maxFriendsToRetrieve = 250)
        {
            return Sync.ExecuteTaskAsync(() => User.GetFriends(userId, maxFriendsToRetrieve));
        }

        public static Task<IEnumerable<IUser>> GetFriends(string userScreenName, int maxFriendsToRetrieve = 250)
        {
            return Sync.ExecuteTaskAsync(() => User.GetFriends(userScreenName, maxFriendsToRetrieve));
        }

        // Follower Ids
        public static Task<IEnumerable<long>> GetFollowerIds(IUserIdentifier user, int maxFollowersToRetrieve = 5000)
        {
            return Sync.ExecuteTaskAsync(() => User.GetFollowerIds(user, maxFollowersToRetrieve));
        }

        public static Task<IEnumerable<long>> GetFollowerIds(long userId, int maxFollowersToRetrieve = 5000)
        {
            return Sync.ExecuteTaskAsync(() => User.GetFollowerIds(userId, maxFollowersToRetrieve));
        }

        public static Task<IEnumerable<long>> GetFollowerIds(string userScreenName, int maxFollowersToRetrieve = 5000)
        {
            return Sync.ExecuteTaskAsync(() => User.GetFollowerIds(userScreenName, maxFollowersToRetrieve));
        }

        // Followers
        public static Task<IEnumerable<IUser>> GetFollowers(IUserIdentifier user, int maxFollowersToRetrieve = 250)
        {
            return Sync.ExecuteTaskAsync(() => User.GetFollowers(user, maxFollowersToRetrieve));
        }

        public static Task<IEnumerable<IUser>> GetFollowers(long userId, int maxFollowersToRetrieve = 250)
        {
            return Sync.ExecuteTaskAsync(() => User.GetFollowers(userId, maxFollowersToRetrieve));
        }

        public static Task<IEnumerable<IUser>> GetFollowers(string userScreenName, int maxFollowersToRetrieve = 250)
        {
            return Sync.ExecuteTaskAsync(() => User.GetFollowers(userScreenName, maxFollowersToRetrieve));
        }

        // Favourites

        public static Task<IEnumerable<ITweet>> GetFavoriteTweets(IUserIdentifier user, IGetUserFavoritesParameters parameters = null)
        {
            return Sync.ExecuteTaskAsync(() => User.GetFavoriteTweets(user, parameters));
        }

        public static Task<IEnumerable<ITweet>> GetFavoriteTweets(long userId, IGetUserFavoritesParameters parameters = null)
        {
            return Sync.ExecuteTaskAsync(() => User.GetFavoriteTweets(userId, parameters));
        }

        public static Task<IEnumerable<ITweet>> GetFavoriteTweets(string userScreenName, IGetUserFavoritesParameters parameters = null)
        {
            return Sync.ExecuteTaskAsync(() => User.GetFavoriteTweets(userScreenName, parameters));
        }

        // Block User
        public static Task<bool> BlockUser(IUserIdentifier user)
        {
            return Sync.ExecuteTaskAsync(() => User.BlockUser(user));
        }

        public static Task<bool> BlockUser(long userId)
        {
            return Sync.ExecuteTaskAsync(() => User.BlockUser(userId));
        }

        public static Task<bool> BlockUser(string userScreenName)
        {
            return Sync.ExecuteTaskAsync(() => User.BlockUser(userScreenName));
        }

        // Stream Profile Image 
        public static Task<System.IO.Stream> GetProfileImageStream(IUser user, ImageSize imageSize = ImageSize.normal)
        {
            return Sync.ExecuteTaskAsync(() => User.GetProfileImageStream(user, imageSize));
        }

        public static Task<System.IO.Stream> GetProfileImageStream(IUserDTO userDTO, ImageSize imageSize = ImageSize.normal)
        {
            return Sync.ExecuteTaskAsync(() => User.GetProfileImageStream(userDTO, imageSize));
        }
    }
}
