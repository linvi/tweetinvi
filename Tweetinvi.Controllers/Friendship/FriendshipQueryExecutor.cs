using System.Collections.Generic;
using System.Threading.Tasks;
using Tweetinvi.Core.QueryGenerators;
using Tweetinvi.Core.QueryValidators;
using Tweetinvi.Core.Web;
using Tweetinvi.Models;
using Tweetinvi.Models.DTO.QueryDTO;

namespace Tweetinvi.Controllers.Friendship
{
    public interface IFriendshipQueryExecutor
    {
        Task<IEnumerable<long>> GetUserIdsYouRequestedToFollow(int maximumUserIdsToRetrieve);
        Task<long[]> GetUserIdsWhoseRetweetsAreMuted();

        // Update Friendship Authorization
        Task<bool> UpdateRelationshipAuthorizationsWith(IUserIdentifier user, IFriendshipAuthorizations friendshipAuthorizations);
    }

    public class FriendshipQueryExecutor : IFriendshipQueryExecutor
    {

        private readonly IFriendshipQueryGenerator _friendshipQueryGenerator;
        private readonly IUserQueryValidator _userQueryValidator;
        private readonly ITwitterAccessor _twitterAccessor;

        public FriendshipQueryExecutor(
            IFriendshipQueryGenerator friendshipQueryGenerator,
            IUserQueryValidator userQueryValidator,
            ITwitterAccessor twitterAccessor)
        {
            _twitterAccessor = twitterAccessor;
            _friendshipQueryGenerator = friendshipQueryGenerator;
            _userQueryValidator = userQueryValidator;
        }


        public Task<IEnumerable<long>> GetUserIdsYouRequestedToFollow(int maximumUserIdsToRetrieve)
        {
            string query = _friendshipQueryGenerator.GetUserIdsYouRequestedToFollowQuery();
            return _twitterAccessor.ExecuteCursorGETQuery<long, IIdsCursorQueryResultDTO>(query, maximumUserIdsToRetrieve);
        }

        public Task<long[]> GetUserIdsWhoseRetweetsAreMuted()
        {
            string query = _friendshipQueryGenerator.GetUserIdsWhoseRetweetsAreMutedQuery();
            return _twitterAccessor.ExecuteGETQuery<long[]>(query);
        }

        // Update Friendship Authorizations
        public async Task<bool> UpdateRelationshipAuthorizationsWith(IUserIdentifier user, IFriendshipAuthorizations friendshipAuthorizations)
        {
            if (!_userQueryValidator.CanUserBeIdentified(user))
            {
                return false;
            }

            string query = _friendshipQueryGenerator.GetUpdateRelationshipAuthorizationsWithQuery(user, friendshipAuthorizations);
            var asyncOeration = await _twitterAccessor.TryExecutePOSTQuery(query);

            return asyncOeration.Success;
        }
    }
}