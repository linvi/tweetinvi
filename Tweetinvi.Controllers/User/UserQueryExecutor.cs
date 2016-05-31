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
        IEnumerable<long> GetFriendIds(IUserIdentifier userIdentifier, int maxFriendsToRetrieve);

        // Followers Ids
        IEnumerable<long> GetFollowerIds(IUserIdentifier userIdentifier, int maxFollowersToRetrieve);

        // Favourites
        IEnumerable<ITweetDTO> GetFavoriteTweets(IGetUserFavoritesQueryParameters parameters);

        // Block User
        bool BlockUser(IUserIdentifier userIdentifier);

        // UnBlock User
        bool UnBlockUser(IUserIdentifier userIdentifier);

        // Get blocked users
        IEnumerable<long> GetBlockedUserIds(int maxUserIds = Int32.MaxValue);
        IEnumerable<IUserDTO> GetBlockedUsers(int maxUsers = Int32.MaxValue);

        // Stream Profile Image
        Stream GetProfileImageStream(IUserDTO userDTO, ImageSize imageSize = ImageSize.normal);

        // Spam
        bool ReportUserForSpam(IUserIdentifier userDTO);
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
        public IEnumerable<long> GetFriendIds(IUserIdentifier userIdentifier, int maxFriendsToRetrieve)
        {
            string query = _userQueryGenerator.GetFriendIdsQuery(userIdentifier, maxFriendsToRetrieve);
            return _twitterAccessor.ExecuteCursorGETQuery<long, IIdsCursorQueryResultDTO>(query, maxFriendsToRetrieve);
        }

        // Followers
        public IEnumerable<long> GetFollowerIds(IUserIdentifier userIdentifier, int maxFollowersToRetrieve)
        {
            string query = _userQueryGenerator.GetFollowerIdsQuery(userIdentifier, maxFollowersToRetrieve);
            return _twitterAccessor.ExecuteCursorGETQuery<long, IIdsCursorQueryResultDTO>(query, maxFollowersToRetrieve);
        }

        // Favourites
        public IEnumerable<ITweetDTO> GetFavoriteTweets(IGetUserFavoritesQueryParameters parameters)
        {
            var query = _userQueryGenerator.GetFavoriteTweetsQuery(parameters);
            return _twitterAccessor.ExecuteGETQuery<IEnumerable<ITweetDTO>>(query);
        }

        // Block
        public bool BlockUser(IUserIdentifier userIdentifier)
        {
            string query = _userQueryGenerator.GetBlockUserQuery(userIdentifier);
            return _twitterAccessor.TryExecutePOSTQuery(query);
        }

        // UnBlock User
        public bool UnBlockUser(IUserIdentifier userIdentifier)
        {
            string query = _userQueryGenerator.GetUnBlockUserQuery(userIdentifier);
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
    }
}