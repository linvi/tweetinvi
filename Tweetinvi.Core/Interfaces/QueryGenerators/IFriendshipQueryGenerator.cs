using System.Collections.Generic;
using Tweetinvi.Core.Interfaces.Models;

namespace Tweetinvi.Core.Interfaces.QueryGenerators
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
        string GetCreateFriendshipWithQuery(IUserIdentifier userDTO);
        string GetCreateFriendshipWithQuery(long userId);
        string GetCreateFriendshipWithQuery(string screenName);

        // Destroy Friendship
        string GetDestroyFriendshipWithQuery(IUserIdentifier userDTO);
        string GetDestroyFriendshipWithQuery(long userId);
        string GetDestroyFriendshipWithQuery(string screenName);

        // Update Friendship Authorization
        string GetUpdateRelationshipAuthorizationsWithQuery(IUserIdentifier userDTO, IFriendshipAuthorizations friendshipAuthorizations);
        string GetUpdateRelationshipAuthorizationsWithQuery(long userId, IFriendshipAuthorizations friendshipAuthorizations);
        string GetUpdateRelationshipAuthorizationsWithQuery(string screenName, IFriendshipAuthorizations friendshipAuthorizations);
    }
}
