using System;
using System.Collections.Generic;
using System.IO;
using Tweetinvi.Core.Enum;
using Tweetinvi.Core.Helpers;
using Tweetinvi.Core.Interfaces.Credentials;
using Tweetinvi.Core.Interfaces.DTO;
using Tweetinvi.Core.Interfaces.DTO.QueryDTO;
using Tweetinvi.Core.Interfaces.Models;
using Tweetinvi.Core.Interfaces.QueryGenerators;
using Tweetinvi.Core.Parameters.QueryParameters;

namespace Tweetinvi.Controllers.User
{
    public interface IUserQueryExecutor
    {
        // Friend Ids
        IEnumerable<long> GetFriendIds(IUserIdentifier userDTO, int maxFriendsToRetrieve);
        IEnumerable<long> GetFriendIds(long userId, int maxFriendsToRetrieve);
        IEnumerable<long> GetFriendIds(string userScreenName, int maxFriendsToRetrieve);

        // Followers Ids
        IEnumerable<long> GetFollowerIds(IUserIdentifier userDTO, int maxFollowersToRetrieve);
        IEnumerable<long> GetFollowerIds(long userId, int maxFollowersToRetrieve);
        IEnumerable<long> GetFollowerIds(string userScreenName, int maxFollowersToRetrieve);

        // Favourites
        IEnumerable<ITweetDTO> GetFavoriteTweets(IGetUserFavoritesQueryParameters parameters);

        // Block User
        bool BlockUser(IUserIdentifier userDTO);
        bool BlockUser(long userId);
        bool BlockUser(string userScreenName);

        // UnBlock User
        bool UnBlockUser(IUserIdentifier userDTO);
        bool UnBlockUser(long userId);
        bool UnBlockUser(string userScreenName);

        // Get blocked users
        IEnumerable<long> GetBlockedUserIds(int maxUserIds = Int32.MaxValue);
        IEnumerable<IUserDTO> GetBlockedUsers(int maxUsers = Int32.MaxValue);

        // Stream Profile Image
        Stream GetProfileImageStream(IUserDTO userDTO, ImageSize imageSize = ImageSize.normal);

        // Spam
        bool ReportUserForSpam(IUserIdentifier userDTO);
        bool ReportUserForSpam(long userId);
        bool ReportUserForSpam(string userScreenName);
    }

    public class UserQueryExecutor : IUserQueryExecutor
    {
        private readonly IUserQueryGenerator _userQueryGenerator;
        private readonly ITwitterAccessor _twitterAccessor;
        private readonly IWebHelper _webHelper;

        public UserQueryExecutor(
            IUserQueryGenerator userQueryGenerator,
            ITwitterAccessor twitterAccessor,
            IWebHelper webHelper)
        {
            _userQueryGenerator = userQueryGenerator;
            _twitterAccessor = twitterAccessor;
            _webHelper = webHelper;
        }

        // Friend ids
        public IEnumerable<long> GetFriendIds(IUserIdentifier userDTO, int maxFriendsToRetrieve)
        {
            string query = _userQueryGenerator.GetFriendIdsQuery(userDTO, maxFriendsToRetrieve);
            return _twitterAccessor.ExecuteCursorGETQuery<long, IIdsCursorQueryResultDTO>(query, maxFriendsToRetrieve);
        }

        public IEnumerable<long> GetFriendIds(long userId, int maxFriendsToRetrieve)
        {
            string query = _userQueryGenerator.GetFriendIdsQuery(userId, maxFriendsToRetrieve);
            return _twitterAccessor.ExecuteCursorGETQuery<long, IIdsCursorQueryResultDTO>(query, maxFriendsToRetrieve);
        }

        public IEnumerable<long> GetFriendIds(string userScreenName, int maxFriendsToRetrieve)
        {
            string query = _userQueryGenerator.GetFriendIdsQuery(userScreenName, maxFriendsToRetrieve);
            return _twitterAccessor.ExecuteCursorGETQuery<long, IIdsCursorQueryResultDTO>(query, maxFriendsToRetrieve);
        }

        // Followers
        public IEnumerable<long> GetFollowerIds(IUserIdentifier userDTO, int maxFollowersToRetrieve)
        {
            string query = _userQueryGenerator.GetFollowerIdsQuery(userDTO, maxFollowersToRetrieve);
            return _twitterAccessor.ExecuteCursorGETQuery<long, IIdsCursorQueryResultDTO>(query, maxFollowersToRetrieve);
        }

        public IEnumerable<long> GetFollowerIds(long userId, int maxFollowersToRetrieve)
        {
            string query = _userQueryGenerator.GetFollowerIdsQuery(userId, maxFollowersToRetrieve);
            return _twitterAccessor.ExecuteCursorGETQuery<long, IIdsCursorQueryResultDTO>(query, maxFollowersToRetrieve);
        }

        public IEnumerable<long> GetFollowerIds(string userScreenName, int maxFollowersToRetrieve)
        {
            string query = _userQueryGenerator.GetFollowerIdsQuery(userScreenName, maxFollowersToRetrieve);
            return _twitterAccessor.ExecuteCursorGETQuery<long, IIdsCursorQueryResultDTO>(query, maxFollowersToRetrieve);
        }

        // Favourites
        public IEnumerable<ITweetDTO> GetFavoriteTweets(IGetUserFavoritesQueryParameters parameters)
        {
            var query = _userQueryGenerator.GetFavoriteTweetsQuery(parameters);
            return _twitterAccessor.ExecuteGETQuery<IEnumerable<ITweetDTO>>(query);
        }

        // Block
        public bool BlockUser(IUserIdentifier userDTO)
        {
            string query = _userQueryGenerator.GetBlockUserQuery(userDTO);
            return _twitterAccessor.TryExecutePOSTQuery(query);
        }

        public bool BlockUser(long userId)
        {
            string query = _userQueryGenerator.GetBlockUserQuery(userId);
            return _twitterAccessor.TryExecutePOSTQuery(query);
        }

        public bool BlockUser(string userScreenName)
        {
            string query = _userQueryGenerator.GetBlockUserQuery(userScreenName);
            return _twitterAccessor.TryExecutePOSTQuery(query);
        }

        // UnBlock User
        public bool UnBlockUser(IUserIdentifier userDTO)
        {
            string query = _userQueryGenerator.GetUnBlockUserQuery(userDTO);
            return _twitterAccessor.TryExecutePOSTQuery(query);
        }

        public bool UnBlockUser(long userId)
        {
            string query = _userQueryGenerator.GetUnBlockUserQuery(userId);
            return _twitterAccessor.TryExecutePOSTQuery(query);
        }

        public bool UnBlockUser(string userScreenName)
        {
            string query = _userQueryGenerator.GetUnBlockUserQuery(userScreenName);
            return _twitterAccessor.TryExecutePOSTQuery(query);
        }

        // Get Block List
        public IEnumerable<long> GetBlockedUserIds(int maxUserIds = Int32.MaxValue)
        {
            string query = _userQueryGenerator.GetBlockedUserIdsQuery();
            return _twitterAccessor.ExecuteCursorGETQuery<long, IIdsCursorQueryResultDTO>(query, maxUserIds);
        }

        public IEnumerable<IUserDTO> GetBlockedUsers(int maxUsers = Int32.MaxValue)
        {
            string query = _userQueryGenerator.GetBlockedUsersQuery();
            return _twitterAccessor.ExecuteCursorGETQuery<IUserDTO, IUserCursorQueryResultDTO>(query, maxUsers);
        }

        // Stream Profile Image
        public Stream GetProfileImageStream(IUserDTO userDTO, ImageSize imageSize = ImageSize.normal)
        {
            var url = _userQueryGenerator.DownloadProfileImageURL(userDTO, imageSize);
            return _webHelper.GetResponseStream(url);
        }

        // Report Spam
        public bool ReportUserForSpam(IUserIdentifier userDTO)
        {
            string query = _userQueryGenerator.GetReportUserForSpamQuery(userDTO);
            return _twitterAccessor.TryExecutePOSTQuery(query);
        }

        public bool ReportUserForSpam(long userId)
        {
            string query = _userQueryGenerator.GetReportUserForSpamQuery(userId);
            return _twitterAccessor.TryExecutePOSTQuery(query);
        }

        public bool ReportUserForSpam(string userScreenName)
        {
            string query = _userQueryGenerator.GetReportUserForSpamQuery(userScreenName);
            return _twitterAccessor.TryExecutePOSTQuery(query);
        }
    }
}