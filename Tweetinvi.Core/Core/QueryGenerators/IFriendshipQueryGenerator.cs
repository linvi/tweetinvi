using Tweetinvi.Models;

namespace Tweetinvi.Core.QueryGenerators
{
    public interface IFriendshipQueryGenerator
    {
        string GetUserIdsYouRequestedToFollowQuery();
        string GetUserIdsWhoseRetweetsAreMutedQuery();


        // Update Friendship Authorization
        string GetUpdateRelationshipAuthorizationsWithQuery(IUserIdentifier user, IFriendshipAuthorizations friendshipAuthorizations);
    }
}