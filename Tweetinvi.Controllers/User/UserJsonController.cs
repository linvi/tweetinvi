using System.Collections.Generic;
using Tweetinvi.Core.Parameters;
using Tweetinvi.Core.QueryGenerators;
using Tweetinvi.Core.Web;
using Tweetinvi.Logic.QueryParameters;
using Tweetinvi.Models;
using Tweetinvi.Models.DTO.QueryDTO;
using Tweetinvi.Parameters;

namespace Tweetinvi.Controllers.User
{
    public interface IUserJsonController
    {
        // Friends
        IEnumerable<string> GetFriendIds(IUserIdentifier user, int maxFriendsToRetrieve = 5000);
        IEnumerable<string> GetFriendIds(long userId, int maxFriendsToRetrieve = 5000);
        IEnumerable<string> GetFriendIds(string userScreenName, int maxFriendsToRetrieve = 5000);

        // Followers
        IEnumerable<string> GetFollowerIds(IUserIdentifier user, int maxFollowersToRetrieve = 5000);
        IEnumerable<string> GetFollowerIds(long userId, int maxFollowersToRetrieve = 5000);
        IEnumerable<string> GetFollowerIds(string userScreenName, int maxFollowersToRetrieve = 5000);

        // Favorites
        string GetFavoriteTweets(IGetUserFavoritesQueryParameters parameters);
        string GetFavoriteTweets(IUserIdentifier user, IGetUserFavoritesParameters parameters);

        // Block User
        string BlockUser(IUserIdentifier user);
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
        public IEnumerable<string> GetFriendIds(IUserIdentifier user, int maxFriendsToRetrieve = 5000)
        {
            string query = _userQueryGenerator.GetFriendIdsQuery(user, maxFriendsToRetrieve);
            return _twitterAccessor.ExecuteJsonCursorGETQuery<IIdsCursorQueryResultDTO>(query);
        }

        public IEnumerable<string> GetFriendIds(long userId, int maxFriendsToRetrieve = 5000)
        {
            string query = _userQueryGenerator.GetFriendIdsQuery(new UserIdentifier(userId), maxFriendsToRetrieve);
            return _twitterAccessor.ExecuteJsonCursorGETQuery<IIdsCursorQueryResultDTO>(query);
        }

        public IEnumerable<string> GetFriendIds(string userScreenName, int maxFriendsToRetrieve = 5000)
        {
            string query = _userQueryGenerator.GetFriendIdsQuery(new UserIdentifier(userScreenName), maxFriendsToRetrieve);
            return _twitterAccessor.ExecuteJsonCursorGETQuery<IIdsCursorQueryResultDTO>(query);
        }

        // Followers
        public IEnumerable<string> GetFollowerIds(IUserIdentifier user, int maxFollowersToRetrieve = 5000)
        {
            string query = _userQueryGenerator.GetFollowerIdsQuery(user, maxFollowersToRetrieve);
            return _twitterAccessor.ExecuteJsonCursorGETQuery<IIdsCursorQueryResultDTO>(query);
        }

        public IEnumerable<string> GetFollowerIds(long userId, int maxFollowersToRetrieve = 5000)
        {
            string query = _userQueryGenerator.GetFollowerIdsQuery(new UserIdentifier(userId), maxFollowersToRetrieve);
            return _twitterAccessor.ExecuteJsonCursorGETQuery<IIdsCursorQueryResultDTO>(query);
        }

        public IEnumerable<string> GetFollowerIds(string userScreenName, int maxFollowersToRetrieve = 5000)
        {
            string query = _userQueryGenerator.GetFollowerIdsQuery(new UserIdentifier(userScreenName), maxFollowersToRetrieve);
            return _twitterAccessor.ExecuteJsonCursorGETQuery<IIdsCursorQueryResultDTO>(query);
        }

        // Favourites
        public string GetFavoriteTweets(IGetUserFavoritesQueryParameters parameters)
        {
            var query = _userQueryGenerator.GetFavoriteTweetsQuery(parameters);
            return _twitterAccessor.ExecuteGETQueryReturningJson(query);
        }

        public string GetFavoriteTweets(IUserIdentifier user, IGetUserFavoritesParameters parameters)
        {
            var favoritesParameters = new GetUserFavoritesQueryParameters(user, parameters);
            return GetFavoriteTweets(favoritesParameters);
        }

        // Block User
        public string BlockUser(IUserIdentifier user)
        {
            string query = _userQueryGenerator.GetBlockUserQuery(user);
            return _twitterAccessor.ExecutePOSTQueryReturningJson(query);
        }

        public string BlockUser(long userId)
        {
            string query = _userQueryGenerator.GetBlockUserQuery(new UserIdentifier(userId));
            return _twitterAccessor.ExecutePOSTQueryReturningJson(query);
        }

        public string BlockUser(string userScreenName)
        {
            string query = _userQueryGenerator.GetBlockUserQuery(new UserIdentifier(userScreenName));
            return _twitterAccessor.ExecutePOSTQueryReturningJson(query);
        }
    }
}