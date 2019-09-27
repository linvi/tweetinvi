using System.Collections.Generic;
using Tweetinvi.Models;

namespace Tweetinvi.Core.QueryGenerators
{
    public interface IFriendshipQueryGenerator
    {
        string GetUserIdsYouRequestedToFollowQuery();
        string GetUserIdsWhoseRetweetsAreMutedQuery();

        // Lookup Relationship State
        string GetMultipleRelationshipsQuery(IEnumerable<IUserIdentifier> users);
        string GetMultipleRelationshipsQuery(IEnumerable<long> userIds);
        string GetMultipleRelationshipsQuery(IEnumerable<string> screenNames);

        // Update Friendship Authorization
        string GetUpdateRelationshipAuthorizationsWithQuery(IUserIdentifier user, IFriendshipAuthorizations friendshipAuthorizations);
    }
}