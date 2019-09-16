using System.Collections.Generic;
using System.Threading.Tasks;
using Tweetinvi.Core.Factories;
using Tweetinvi.Core.QueryGenerators;
using Tweetinvi.Core.Web;
using Tweetinvi.Models;
using Tweetinvi.Models.DTO.QueryDTO;

namespace Tweetinvi.Controllers.Friendship
{
    public interface IFriendshipJsonController
    {
        Task<IEnumerable<string>> GetUserIdsRequestingFriendship();
        Task<IEnumerable<string>> GetUserIdsYouRequestedToFollow();

        // Update Friendship Authorizations
        Task<string> UpdateRelationshipAuthorizationsWith(IUserIdentifier user, bool retweetsEnabled, bool deviceNotifictionEnabled);
        Task<string> UpdateRelationshipAuthorizationsWith(long userId, bool retweetsEnabled, bool deviceNotifictionEnabled);
        Task<string> UpdateRelationshipAuthorizationsWith(string userScreenName, bool retweetsEnabled, bool deviceNotifictionEnabled);
    }

    public class FriendshipJsonController : IFriendshipJsonController
    {
        private readonly IFriendshipQueryGenerator _friendshipQueryGenerator;
        private readonly IFriendshipFactory _friendshipFactory;
        private readonly ITwitterAccessor _twitterAccessor;

        public FriendshipJsonController(
            IFriendshipQueryGenerator friendshipQueryGenerator,
            IFriendshipFactory friendshipFactory,
            ITwitterAccessor twitterAccessor)
        {
            _friendshipQueryGenerator = friendshipQueryGenerator;
            _friendshipFactory = friendshipFactory;
            _twitterAccessor = twitterAccessor;
        }

        public Task<IEnumerable<string>> GetUserIdsRequestingFriendship()
        {
            string query = _friendshipQueryGenerator.GetUserIdsRequestingFriendshipQuery();
            return _twitterAccessor.ExecuteJsonCursorGETQuery<IIdsCursorQueryResultDTO>(query);
        }

        public Task<IEnumerable<string>> GetUserIdsYouRequestedToFollow()
        {
            string query = _friendshipQueryGenerator.GetUserIdsYouRequestedToFollowQuery();
            return _twitterAccessor.ExecuteJsonCursorGETQuery<IIdsCursorQueryResultDTO>(query);
        }

        public Task<string> UpdateRelationshipAuthorizationsWith(IUserIdentifier user, bool retweetsEnabled, bool deviceNotifictionEnabled)
        {
            var friendshipAuthorizations = _friendshipFactory.GenerateFriendshipAuthorizations(retweetsEnabled, deviceNotifictionEnabled);
            string query = _friendshipQueryGenerator.GetUpdateRelationshipAuthorizationsWithQuery(user, friendshipAuthorizations);
            return _twitterAccessor.ExecutePOSTQueryReturningJson(query);
        }

        public Task<string> UpdateRelationshipAuthorizationsWith(long userId, bool retweetsEnabled, bool deviceNotifictionEnabled)
        {
            var friendshipAuthorizations = _friendshipFactory.GenerateFriendshipAuthorizations(retweetsEnabled, deviceNotifictionEnabled);
            string query = _friendshipQueryGenerator.GetUpdateRelationshipAuthorizationsWithQuery(new UserIdentifier(userId), friendshipAuthorizations);
            return _twitterAccessor.ExecutePOSTQueryReturningJson(query);
        }

        public Task<string> UpdateRelationshipAuthorizationsWith(string userScreenName, bool retweetsEnabled, bool deviceNotifictionEnabled)
        {
            var friendshipAuthorizations = _friendshipFactory.GenerateFriendshipAuthorizations(retweetsEnabled, deviceNotifictionEnabled);
            string query = _friendshipQueryGenerator.GetUpdateRelationshipAuthorizationsWithQuery(new UserIdentifier(userScreenName), friendshipAuthorizations);
            return _twitterAccessor.ExecutePOSTQueryReturningJson(query);
        }
    }
}