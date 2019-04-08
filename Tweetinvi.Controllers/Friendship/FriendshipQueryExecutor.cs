using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Tweetinvi.Core.Extensions;
using Tweetinvi.Core.QueryGenerators;
using Tweetinvi.Core.QueryValidators;
using Tweetinvi.Core.Web;
using Tweetinvi.Models;
using Tweetinvi.Models.DTO;
using Tweetinvi.Models.DTO.QueryDTO;

namespace Tweetinvi.Controllers.Friendship
{
    public interface IFriendshipQueryExecutor
    {
        Task<IEnumerable<long>> GetUserIdsRequestingFriendship(int maximumUserIdsToRetrieve);
        Task<IEnumerable<long>> GetUserIdsYouRequestedToFollow(int maximumUserIdsToRetrieve);
        Task<long[]> GetUserIdsWhoseRetweetsAreMuted();

        // Create Friendship
        Task<bool> CreateFriendshipWith(IUserIdentifier user);

        // Destroy Friendship
        Task<bool> DestroyFriendshipWith(IUserIdentifier user);

        // Update Friendship Authorization
        Task<bool> UpdateRelationshipAuthorizationsWith(IUserIdentifier user, IFriendshipAuthorizations friendshipAuthorizations);

        // Get Existing Relationship
        Task<IRelationshipDetailsDTO> GetRelationshipBetween(IUserIdentifier sourceUserIdentifier, IUserIdentifier targetUserIdentifier);

        // Get Multiple Relationships
        Task<IEnumerable<IRelationshipStateDTO>> GetMultipleRelationshipsQuery(IEnumerable<IUserIdentifier> targetUserIdentifiers);
        Task<IEnumerable<IRelationshipStateDTO>> GetMultipleRelationshipsQuery(IEnumerable<long> targetUserIds);
        Task<IEnumerable<IRelationshipStateDTO>> GetMultipleRelationshipsQuery(IEnumerable<string> targetUsersScreenName);
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

        public Task<IEnumerable<long>> GetUserIdsRequestingFriendship(int maximumUserIdsToRetrieve)
        {
            string query = _friendshipQueryGenerator.GetUserIdsRequestingFriendshipQuery();
            return _twitterAccessor.ExecuteCursorGETQuery<long, IIdsCursorQueryResultDTO>(query, maximumUserIdsToRetrieve);
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

        // Create Friendship
        public async Task<bool> CreateFriendshipWith(IUserIdentifier user)
        {
            string query = _friendshipQueryGenerator.GetCreateFriendshipWithQuery(user);
            var asyncOperation = await _twitterAccessor.TryExecutePOSTQuery(query);

            return asyncOperation.Success;
        }

        // Destroy Friendship
        public async Task<bool> DestroyFriendshipWith(IUserIdentifier user)
        {
            if (!_userQueryValidator.CanUserBeIdentified(user))
            {
                return false;
            }

            string query = _friendshipQueryGenerator.GetDestroyFriendshipWithQuery(user);
            var asyncOperation = await _twitterAccessor.TryExecutePOSTQuery(query);

            return asyncOperation.Success;
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

        // Relationship Between
        public Task<IRelationshipDetailsDTO> GetRelationshipBetween(IUserIdentifier sourceUserIdentifier, IUserIdentifier targetUserIdentifier)
        {
            var query = _friendshipQueryGenerator.GetRelationshipDetailsQuery(sourceUserIdentifier, targetUserIdentifier);
            return _twitterAccessor.ExecuteGETQuery<IRelationshipDetailsDTO>(query);
        }

        // Get Relationship with
        public async Task<IEnumerable<IRelationshipStateDTO>> GetMultipleRelationshipsQuery(IEnumerable<IUserIdentifier> targetUserIdentifiers)
        {
            var targetUserIdentifiersArray = IEnumerableExtension.GetDistinctUserIdentifiers(targetUserIdentifiers);

            var distinctRelationships = new List<IRelationshipStateDTO>();

            for (int i = 0; i < targetUserIdentifiersArray.Length; i += TweetinviConsts.FRIENDSHIP_MAX_NUMBER_OF_FRIENDSHIP_TO_GET_IN_A_SINGLE_QUERY)
            {
                var usersToAnalyze = targetUserIdentifiersArray.Skip(i).Take(TweetinviConsts.FRIENDSHIP_MAX_NUMBER_OF_FRIENDSHIP_TO_GET_IN_A_SINGLE_QUERY).ToArray();
                var query = _friendshipQueryGenerator.GetMultipleRelationshipsQuery(usersToAnalyze);
                var relationshipStateDtos = await _twitterAccessor.ExecuteGETQuery<IEnumerable<IRelationshipStateDTO>>(query);

                // As soon as we cannot retrieve additional objects, we stop the query
                if (relationshipStateDtos == null)
                {
                    break;
                }

                distinctRelationships.AddRange(relationshipStateDtos);
            }

            return distinctRelationships;
        }

        public async Task<IEnumerable<IRelationshipStateDTO>> GetMultipleRelationshipsQuery(IEnumerable<long> targetUserIds)
        {
            var targetUserIdsArray = targetUserIds.Distinct().ToArray();
            var distinctRelationships = new List<IRelationshipStateDTO>();

            for (int i = 0; i < targetUserIdsArray.Length; i += TweetinviConsts.FRIENDSHIP_MAX_NUMBER_OF_FRIENDSHIP_TO_GET_IN_A_SINGLE_QUERY)
            {
                var userIdsToAnalyze = targetUserIdsArray.Skip(i).Take(TweetinviConsts.FRIENDSHIP_MAX_NUMBER_OF_FRIENDSHIP_TO_GET_IN_A_SINGLE_QUERY).ToArray();
                var query = _friendshipQueryGenerator.GetMultipleRelationshipsQuery(userIdsToAnalyze);
                var relationshipStateDtos = await _twitterAccessor.ExecuteGETQuery<IEnumerable<IRelationshipStateDTO>>(query);

                // As soon as we cannot retrieve additional objects, we stop the query
                if (relationshipStateDtos == null)
                {
                    break;
                }

                distinctRelationships.AddRange(relationshipStateDtos);
            }

            return distinctRelationships;
        }

        public async Task<IEnumerable<IRelationshipStateDTO>> GetMultipleRelationshipsQuery(IEnumerable<string> targetUsersScreenName)
        {
            var targetUserScreenNamesArray = targetUsersScreenName.Distinct().ToArray();
            var distinctRelationships = new List<IRelationshipStateDTO>();

            for (int i = 0; i < targetUserScreenNamesArray.Length; i += TweetinviConsts.FRIENDSHIP_MAX_NUMBER_OF_FRIENDSHIP_TO_GET_IN_A_SINGLE_QUERY)
            {
                var userScreenNamesToAnalyze = targetUserScreenNamesArray.Skip(i).Take(TweetinviConsts.FRIENDSHIP_MAX_NUMBER_OF_FRIENDSHIP_TO_GET_IN_A_SINGLE_QUERY).ToArray();
                var query = _friendshipQueryGenerator.GetMultipleRelationshipsQuery(userScreenNamesToAnalyze);
                var relationshipStateDtos = await _twitterAccessor.ExecuteGETQuery<IEnumerable<IRelationshipStateDTO>>(query);

                // As soon as we cannot retrieve additional objects, we stop the query
                if (relationshipStateDtos == null)
                {
                    break;
                }

                distinctRelationships.AddRange(relationshipStateDtos);
            }

            return distinctRelationships;
        }
    }
}