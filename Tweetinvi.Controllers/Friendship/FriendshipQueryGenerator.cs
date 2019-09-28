using Tweetinvi.Controllers.Properties;
using Tweetinvi.Core.QueryGenerators;

namespace Tweetinvi.Controllers.Friendship
{
    public class FriendshipQueryGenerator : IFriendshipQueryGenerator
    {
        // Get Friendship
        public string GetUserIdsYouRequestedToFollowQuery()
        {
            return Resources.Friendship_GetOutgoingIds;
        }

        public string GetUserIdsWhoseRetweetsAreMutedQuery()
        {
            return Resources.Friendship_FriendIdsWithNoRetweets;
        }

    }
}