using System.Collections.Generic;
using Tweetinvi.Models;

namespace Tweetinvi.Core.QueryGenerators
{
    public interface IFriendshipQueryGenerator
    {
        string GetUserIdsRequestingFriendshipQuery();
        string GetUserIdsYouRequestedToFollowQuery();
        string GetUserIdsWhoseRetweetsAreMutedQuery();

        // Get Existing Friendship
        string GetRelationshipDetailsQuery(IUserIdentifier sourceUserIdentifier, IUserIdentifier targetUserIdentifier);

        // Lookup Relationship State
        string GetMultipleRelationshipsQuery(IEnumerable<IUserIdentifier> userIdentifiers);
        string GetMultipleRelationshipsQuery(IEnumerable<long> userIds);
        string GetMultipleRelationshipsQuery(IEnumerable<string> screenNames);

        // Create Friendship
        string GetCreateFriendshipWithQuery(IUserIdentifier userIdentifier);

        // Destroy Friendship
        string GetDestroyFriendshipWithQuery(IUserIdentifier userIdentifier);

        // Update Friendship Authorization
        string GetUpdateRelationshipAuthorizationsWithQuery(IUserIdentifier userIdentifier, IFriendshipAuthorizations friendshipAuthorizations);
    }
}