using System.Collections.Generic;
using System.Threading.Tasks;
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
        // Favorites
        Task<string> GetFavoriteTweets(IGetUserFavoritesQueryParameters parameters);
        Task<string> GetFavoriteTweets(IUserIdentifier user, IGetUserFavoritesParameters parameters);

        // Block User
        Task<string> BlockUser(IUserIdentifier user);
        Task<string> BlockUser(long userId);
        Task<string> BlockUser(string userScreenName);
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

        // Favourites
        public Task<string> GetFavoriteTweets(IGetUserFavoritesQueryParameters parameters)
        {
            var query = _userQueryGenerator.GetFavoriteTweetsQuery(parameters);
            return _twitterAccessor.ExecuteGETQueryReturningJson(query);
        }

        public Task<string> GetFavoriteTweets(IUserIdentifier user, IGetUserFavoritesParameters parameters)
        {
            var favoritesParameters = new GetUserFavoritesQueryParameters(user, parameters);
            return GetFavoriteTweets(favoritesParameters);
        }

        // Block User
        public Task<string> BlockUser(IUserIdentifier user)
        {
            string query = _userQueryGenerator.GetBlockUserQuery(user);
            return _twitterAccessor.ExecutePOSTQueryReturningJson(query);
        }

        public Task<string> BlockUser(long userId)
        {
            string query = _userQueryGenerator.GetBlockUserQuery(new UserIdentifier(userId));
            return _twitterAccessor.ExecutePOSTQueryReturningJson(query);
        }

        public Task<string> BlockUser(string userScreenName)
        {
            string query = _userQueryGenerator.GetBlockUserQuery(new UserIdentifier(userScreenName));
            return _twitterAccessor.ExecutePOSTQueryReturningJson(query);
        }
    }
}