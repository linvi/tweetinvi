using System;
using System.Collections.Generic;
using Tweetinvi.Core.Interfaces;
using Tweetinvi.Core.Interfaces.Credentials;
using Tweetinvi.Core.Interfaces.DTO.QueryDTO;
using Tweetinvi.Core.Interfaces.Models;
using Tweetinvi.Core.Interfaces.QueryGenerators;

namespace Tweetinvi.Controllers.User
{
    public interface IUserJsonController
    {
        // Friends
        IEnumerable<string> GetFriendIds(IUser user, int maxFriendsToRetrieve = 5000);
        IEnumerable<string> GetFriendIds(IUserIdentifier userDTO, int maxFriendsToRetrieve = 5000);
        IEnumerable<string> GetFriendIds(long userId, int maxFriendsToRetrieve = 5000);
        IEnumerable<string> GetFriendIds(string userScreenName, int maxFriendsToRetrieve = 5000);

        // Followers
        IEnumerable<string> GetFollowerIds(IUser user, int maxFollowersToRetrieve = 5000);
        IEnumerable<string> GetFollowerIds(IUserIdentifier userDTO, int maxFollowersToRetrieve = 5000);
        IEnumerable<string> GetFollowerIds(long userId, int maxFollowersToRetrieve = 5000);
        IEnumerable<string> GetFollowerIds(string userScreenName, int maxFollowersToRetrieve = 5000);

        // Favorites
        string GetFavouriteTweets(IUser user, int maxFavouritesToRetrieve = 40);
        string GetFavouriteTweets(IUserIdentifier userDTO, int maxFavouritesToRetrieve = 40);
        string GetFavouriteTweets(long userId, int maxFavouritesToRetrieve = 40);
        string GetFavouriteTweets(string userScreenName, int maxFavouritesToRetrieve = 40);

        // Block User
        string BlockUser(IUser user);
        string BlockUser(IUserIdentifier userDTO);
        string BlockUser(long userId);
        string BlockUser(string userScreenName);
    }

    public class UserJsonController : IUserJsonController
    {
        private readonly IUserQueryGenerator _userQueryGenerator;
        private readonly ITwitterAccessor _twitterAccessor;

        public UserJsonController(
            IUserQueryGenerator userQueryGenerator,
            ITwitterAccessor twitterAccessor)
        {
            _userQueryGenerator = userQueryGenerator;
            _twitterAccessor = twitterAccessor;
        }

        // Friend ids
        public IEnumerable<string> GetFriendIds(IUser user, int maxFriendsToRetrieve = 5000)
        {
            if (user == null)
            {
                return null;
            }

            return GetFriendIds(user.UserDTO, maxFriendsToRetrieve);
        }

        public IEnumerable<string> GetFriendIds(IUserIdentifier userDTO, int maxFriendsToRetrieve = 5000)
        {
            string query = _userQueryGenerator.GetFriendIdsQuery(userDTO, maxFriendsToRetrieve);
            return _twitterAccessor.ExecuteJsonCursorGETQuery<IIdsCursorQueryResultDTO>(query);
        }

        public IEnumerable<string> GetFriendIds(long userId, int maxFriendsToRetrieve = 5000)
        {
            string query = _userQueryGenerator.GetFriendIdsQuery(userId, maxFriendsToRetrieve);
            return _twitterAccessor.ExecuteJsonCursorGETQuery<IIdsCursorQueryResultDTO>(query);
        }

        public IEnumerable<string> GetFriendIds(string userScreenName, int maxFriendsToRetrieve = 5000)
        {
            string query = _userQueryGenerator.GetFriendIdsQuery(userScreenName, maxFriendsToRetrieve);
            return _twitterAccessor.ExecuteJsonCursorGETQuery<IIdsCursorQueryResultDTO>(query);
        }

        // Followers
        public IEnumerable<string> GetFollowerIds(IUser user, int maxFollowersToRetrieve = 5000)
        {
            if (user == null)
            {
                throw new ArgumentException("User cannot be null");
            }

            return GetFollowerIds(user.UserDTO, maxFollowersToRetrieve);
        }

        public IEnumerable<string> GetFollowerIds(IUserIdentifier userDTO, int maxFollowersToRetrieve = 5000)
        {
            string query = _userQueryGenerator.GetFollowerIdsQuery(userDTO, maxFollowersToRetrieve);
            return _twitterAccessor.ExecuteJsonCursorGETQuery<IIdsCursorQueryResultDTO>(query);
        }

        public IEnumerable<string> GetFollowerIds(long userId, int maxFollowersToRetrieve = 5000)
        {
            string query = _userQueryGenerator.GetFollowerIdsQuery(userId, maxFollowersToRetrieve);
            return _twitterAccessor.ExecuteJsonCursorGETQuery<IIdsCursorQueryResultDTO>(query);
        }

        public IEnumerable<string> GetFollowerIds(string userScreenName, int maxFollowersToRetrieve = 5000)
        {
            string query = _userQueryGenerator.GetFollowerIdsQuery(userScreenName, maxFollowersToRetrieve);
            return _twitterAccessor.ExecuteJsonCursorGETQuery<IIdsCursorQueryResultDTO>(query);
        }

        // Favourites
        public string GetFavouriteTweets(IUser user, int maxFavouritesToRetrieve = 40)
        {
            if (user == null)
            {
                throw new ArgumentException("User cannot be null");
            }

            return GetFavouriteTweets(user.UserDTO, maxFavouritesToRetrieve);
        }

        public string GetFavouriteTweets(IUserIdentifier userDTO, int maxFavouritesToRetrieve = 40)
        {
            string query = _userQueryGenerator.GetFavouriteTweetsQuery(userDTO, maxFavouritesToRetrieve);
            return _twitterAccessor.ExecuteJsonGETQuery(query);
        }

        public string GetFavouriteTweets(long userId, int maxFavouritesToRetrieve = 40)
        {
            string query = _userQueryGenerator.GetFavouriteTweetsQuery(userId, maxFavouritesToRetrieve);
            return _twitterAccessor.ExecuteJsonGETQuery(query);
        }

        public string GetFavouriteTweets(string userScreenName, int maxFavouritesToRetrieve = 40)
        {
            string query = _userQueryGenerator.GetFavouriteTweetsQuery(userScreenName, maxFavouritesToRetrieve);
            return _twitterAccessor.ExecuteJsonGETQuery(query);
        }

        // Block User
        public string BlockUser(IUser user)
        {
            if (user == null)
            {
                throw new ArgumentException("User cannot be null");
            }

            return BlockUser(user.UserDTO);
        }

        public string BlockUser(IUserIdentifier userDTO)
        {
            string query = _userQueryGenerator.GetBlockUserQuery(userDTO);
            return _twitterAccessor.ExecuteJsonPOSTQuery(query);
        }

        public string BlockUser(long userId)
        {
            string query = _userQueryGenerator.GetBlockUserQuery(userId);
            return _twitterAccessor.ExecuteJsonPOSTQuery(query);
        }

        public string BlockUser(string userScreenName)
        {
            string query = _userQueryGenerator.GetBlockUserQuery(userScreenName);
            return _twitterAccessor.ExecuteJsonPOSTQuery(query);
        }
    }
}