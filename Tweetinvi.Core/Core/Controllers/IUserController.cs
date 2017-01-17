using System;
using System.Collections.Generic;
using System.IO;
using Tweetinvi.Core.Parameters;
using Tweetinvi.Models;
using Tweetinvi.Models.DTO;
using Tweetinvi.Parameters;

namespace Tweetinvi.Core.Controllers
{
    public interface IUserController
    {
        // Friends
        IEnumerable<long> GetFriendIds(IUserIdentifier user, int maxFriendsToRetrieve = 5000);
        IEnumerable<long> GetFriendIds(long userId, int maxFriendsToRetrieve = 5000);
        IEnumerable<long> GetFriendIds(string userScreenName, int maxFriendsToRetrieve = 5000);

        IEnumerable<IUser> GetFriends(IUserIdentifier user, int maxFriendsToRetrieve = 250);
        IEnumerable<IUser> GetFriends(long userId, int maxFriendsToRetrieve = 250);
        IEnumerable<IUser> GetFriends(string userScreenName, int maxFriendsToRetrieve = 250);

        // Followers
        IEnumerable<long> GetFollowerIds(IUserIdentifier user, int maxFollowersToRetrieve = 5000);
        IEnumerable<long> GetFollowerIds(long userId, int maxFollowersToRetrieve = 5000);
        IEnumerable<long> GetFollowerIds(string userScreenName, int maxFollowersToRetrieve = 5000);

        IEnumerable<IUser> GetFollowers(IUserIdentifier user, int maxFollowersToRetrieve = 250);
        IEnumerable<IUser> GetFollowers(long userId, int maxFollowersToRetrieve = 250);
        IEnumerable<IUser> GetFollowers(string userScreenName, int maxFollowersToRetrieve = 250);

        // Favourites
        IEnumerable<ITweet> GetFavoriteTweets(IGetUserFavoritesQueryParameters parameters);
        IEnumerable<ITweet> GetFavoriteTweets(IUserIdentifier user, IGetUserFavoritesParameters parameters);

        // Block User
        bool BlockUser(IUserIdentifier user);
        bool BlockUser(long userId);
        bool BlockUser(string userScreenName);

        // Unblock User
        bool UnBlockUser(IUserIdentifier user);
        bool UnBlockUser(long userId);
        bool UnBlockUser(string userScreenName);

        // Get Blocked Users
        IEnumerable<long> GetBlockedUserIds(int maxUserIds = Int32.MaxValue);

        IEnumerable<IUser> GetBlockedUsers(int maxUsers = Int32.MaxValue);

        // Stream Profile Image
        Stream GetProfileImageStream(IUser user, ImageSize imageSize = ImageSize.normal);
        Stream GetProfileImageStream(IUserDTO userDTO, ImageSize imageSize = ImageSize.normal);

        // Report Spam
        bool ReportUserForSpam(IUserIdentifier user);
        bool ReportUserForSpam(long userId);
        bool ReportUserForSpam(string userScreenName);
    }
}