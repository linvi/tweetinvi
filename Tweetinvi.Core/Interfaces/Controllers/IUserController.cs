using System;
using System.Collections.Generic;
using System.IO;
using Tweetinvi.Core.Enum;
using Tweetinvi.Core.Interfaces.DTO;
using Tweetinvi.Core.Interfaces.Models;
using Tweetinvi.Core.Parameters;
using Tweetinvi.Core.Parameters.QueryParameters;

namespace Tweetinvi.Core.Interfaces.Controllers
{
    public interface IUserController
    {
        // Friends
        IEnumerable<long> GetFriendIds(IUser user, int maxFriendsToRetrieve = 5000);
        IEnumerable<long> GetFriendIds(IUserIdentifier userDTO, int maxFriendsToRetrieve = 5000);
        IEnumerable<long> GetFriendIds(long userId, int maxFriendsToRetrieve = 5000);
        IEnumerable<long> GetFriendIds(string userScreenName, int maxFriendsToRetrieve = 5000);

        IEnumerable<IUser> GetFriends(IUser user, int maxFriendsToRetrieve = 250);
        IEnumerable<IUser> GetFriends(IUserIdentifier userDTO, int maxFriendsToRetrieve = 250);
        IEnumerable<IUser> GetFriends(long userId, int maxFriendsToRetrieve = 250);
        IEnumerable<IUser> GetFriends(string userScreenName, int maxFriendsToRetrieve = 250);

        // Followers
        IEnumerable<long> GetFollowerIds(IUser user, int maxFollowersToRetrieve = 5000);
        IEnumerable<long> GetFollowerIds(IUserIdentifier userDTO, int maxFollowersToRetrieve = 5000);
        IEnumerable<long> GetFollowerIds(long userId, int maxFollowersToRetrieve = 5000);
        IEnumerable<long> GetFollowerIds(string userScreenName, int maxFollowersToRetrieve = 5000);

        IEnumerable<IUser> GetFollowers(IUser user, int maxFollowersToRetrieve = 250);
        IEnumerable<IUser> GetFollowers(IUserIdentifier userDTO, int maxFollowersToRetrieve = 250);
        IEnumerable<IUser> GetFollowers(long userId, int maxFollowersToRetrieve = 250);
        IEnumerable<IUser> GetFollowers(string userScreenName, int maxFollowersToRetrieve = 250);

        // Favourites
        IEnumerable<ITweet> GetFavoriteTweets(IGetUserFavoritesQueryParameters parameters);
        IEnumerable<ITweet> GetFavoriteTweets(IUserIdentifier userIdentifier, IGetUserFavoritesParameters parameters);

        // Block User
        bool BlockUser(IUserIdentifier userIdentifier);
        bool BlockUser(long userId);
        bool BlockUser(string userScreenName);

        // Unblock User
        bool UnBlockUser(IUserIdentifier userIdentifier);
        bool UnBlockUser(long userId);
        bool UnBlockUser(string userScreenName);

        // Get Blocked Users
        IEnumerable<long> GetBlockedUserIds(int maxUserIds = Int32.MaxValue);

        IEnumerable<IUser> GetBlockedUsers(int maxUsers = Int32.MaxValue);

        // Stream Profile Image
        Stream GetProfileImageStream(IUser user, ImageSize imageSize = ImageSize.normal);
        Stream GetProfileImageStream(IUserDTO userDTO, ImageSize imageSize = ImageSize.normal);

        // Report Spam
        bool ReportUserForSpam(IUser user);
        bool ReportUserForSpam(IUserIdentifier userDTO);
        bool ReportUserForSpam(long userId);
        bool ReportUserForSpam(string userScreenName);
    }
}