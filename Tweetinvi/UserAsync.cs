using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Tweetinvi.Models;
using Tweetinvi.Models.DTO;
using Tweetinvi.Parameters;

namespace Tweetinvi
{
    public static class UserAsync
    {
        // User Factory
        public static ConfiguredTaskAwaitable<IAuthenticatedUser> GetAuthenticatedUser()
        {
            return Sync.ExecuteTaskAsync(() => User.GetAuthenticatedUser());
        }

        public static ConfiguredTaskAwaitable<IAuthenticatedUser> GetAuthenticatedUser(ITwitterCredentials credentials)
        {
            return Sync.ExecuteTaskAsync(() => User.GetAuthenticatedUser(credentials));
        }

        public static ConfiguredTaskAwaitable<IUser> GetUserFromId(long userId)
        {
            return Sync.ExecuteTaskAsync(() => User.GetUserFromId(userId));
        }

        public static ConfiguredTaskAwaitable<IUser> GetUserFromScreenName(string userName)
        {
            return Sync.ExecuteTaskAsync(() => User.GetUserFromScreenName(userName));
        }

        public static ConfiguredTaskAwaitable<IUser> GenerateUserFromDTO(IUserDTO userDTO)
        {
            return Sync.ExecuteTaskAsync(() => User.GenerateUserFromDTO(userDTO));
        }

        public static ConfiguredTaskAwaitable<IEnumerable<IUser>> GenerateUsersFromDTO(IEnumerable<IUserDTO> usersDTO)
        {
            return Sync.ExecuteTaskAsync(() => User.GenerateUsersFromDTO(usersDTO));
        }

        // User Controller
        // Friend Ids
        public static ConfiguredTaskAwaitable<IEnumerable<long>> GetFriendIds(IUserIdentifier user, int maxFriendsToRetrieve = 5000)
        {
            return Sync.ExecuteTaskAsync(() => User.GetFriendIds(user, maxFriendsToRetrieve));
        }

        public static ConfiguredTaskAwaitable<IEnumerable<long>> GetFriendIds(long userId, int maxFriendsToRetrieve = 5000)
        {
            return Sync.ExecuteTaskAsync(() => User.GetFriendIds(userId, maxFriendsToRetrieve));
        }

        public static ConfiguredTaskAwaitable<IEnumerable<long>> GetFriendIds(string userScreenName, int maxFriendsToRetrieve = 5000)
        {
            return Sync.ExecuteTaskAsync(() => User.GetFriendIds(userScreenName, maxFriendsToRetrieve));
        }

        // Friends
        public static ConfiguredTaskAwaitable<IEnumerable<IUser>> GetFriends(IUserIdentifier user, int maxFriendsToRetrieve = 250)
        {
            return Sync.ExecuteTaskAsync(() => User.GetFriends(user, maxFriendsToRetrieve));
        }

        public static ConfiguredTaskAwaitable<IEnumerable<IUser>> GetFriends(long userId, int maxFriendsToRetrieve = 250)
        {
            return Sync.ExecuteTaskAsync(() => User.GetFriends(userId, maxFriendsToRetrieve));
        }

        public static ConfiguredTaskAwaitable<IEnumerable<IUser>> GetFriends(string userScreenName, int maxFriendsToRetrieve = 250)
        {
            return Sync.ExecuteTaskAsync(() => User.GetFriends(userScreenName, maxFriendsToRetrieve));
        }

        // Follower Ids
        public static ConfiguredTaskAwaitable<IEnumerable<long>> GetFollowerIds(IUserIdentifier user, int maxFollowersToRetrieve = 5000)
        {
            return Sync.ExecuteTaskAsync(() => User.GetFollowerIds(user, maxFollowersToRetrieve));
        }

        public static ConfiguredTaskAwaitable<IEnumerable<long>> GetFollowerIds(long userId, int maxFollowersToRetrieve = 5000)
        {
            return Sync.ExecuteTaskAsync(() => User.GetFollowerIds(userId, maxFollowersToRetrieve));
        }

        public static ConfiguredTaskAwaitable<IEnumerable<long>> GetFollowerIds(string userScreenName, int maxFollowersToRetrieve = 5000)
        {
            return Sync.ExecuteTaskAsync(() => User.GetFollowerIds(userScreenName, maxFollowersToRetrieve));
        }

        // Followers
        public static ConfiguredTaskAwaitable<IEnumerable<IUser>> GetFollowers(IUserIdentifier user, int maxFollowersToRetrieve = 250)
        {
            return Sync.ExecuteTaskAsync(() => User.GetFollowers(user, maxFollowersToRetrieve));
        }

        public static ConfiguredTaskAwaitable<IEnumerable<IUser>> GetFollowers(long userId, int maxFollowersToRetrieve = 250)
        {
            return Sync.ExecuteTaskAsync(() => User.GetFollowers(userId, maxFollowersToRetrieve));
        }

        public static ConfiguredTaskAwaitable<IEnumerable<IUser>> GetFollowers(string userScreenName, int maxFollowersToRetrieve = 250)
        {
            return Sync.ExecuteTaskAsync(() => User.GetFollowers(userScreenName, maxFollowersToRetrieve));
        }

        // Favourites

        public static ConfiguredTaskAwaitable<IEnumerable<ITweet>> GetFavoriteTweets(IUserIdentifier user, IGetUserFavoritesParameters parameters = null)
        {
            return Sync.ExecuteTaskAsync(() => User.GetFavoriteTweets(user, parameters));
        }

        public static ConfiguredTaskAwaitable<IEnumerable<ITweet>> GetFavoriteTweets(long userId, IGetUserFavoritesParameters parameters = null)
        {
            return Sync.ExecuteTaskAsync(() => User.GetFavoriteTweets(userId, parameters));
        }

        public static ConfiguredTaskAwaitable<IEnumerable<ITweet>> GetFavoriteTweets(string userScreenName, IGetUserFavoritesParameters parameters = null)
        {
            return Sync.ExecuteTaskAsync(() => User.GetFavoriteTweets(userScreenName, parameters));
        }

        // Block User
        public static ConfiguredTaskAwaitable<bool> BlockUser(IUserIdentifier user)
        {
            return Sync.ExecuteTaskAsync(() => User.BlockUser(user));
        }

        public static ConfiguredTaskAwaitable<bool> BlockUser(long userId)
        {
            return Sync.ExecuteTaskAsync(() => User.BlockUser(userId));
        }

        public static ConfiguredTaskAwaitable<bool> BlockUser(string userScreenName)
        {
            return Sync.ExecuteTaskAsync(() => User.BlockUser(userScreenName));
        }

        // Stream Profile Image 
        public static ConfiguredTaskAwaitable<System.IO.Stream> GetProfileImageStream(IUser user, ImageSize imageSize = ImageSize.normal)
        {
            return Sync.ExecuteTaskAsync(() => User.GetProfileImageStream(user, imageSize));
        }

        public static ConfiguredTaskAwaitable<System.IO.Stream> GetProfileImageStream(IUserDTO userDTO, ImageSize imageSize = ImageSize.normal)
        {
            return Sync.ExecuteTaskAsync(() => User.GetProfileImageStream(userDTO, imageSize));
        }
    }
}
