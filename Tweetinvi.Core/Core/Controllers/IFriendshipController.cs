using System.Collections.Generic;
using System.Threading.Tasks;
using Tweetinvi.Models;

namespace Tweetinvi.Core.Controllers
{
    public interface IFriendshipController
    {
        Task<IEnumerable<long>> GetUserIdsYouRequestedToFollow(
            int maximumUsersToRetrieve = TweetinviConsts.FRIENDSHIPS_OUTGOING_IDS_MAX_PER_REQ);
        Task<IEnumerable<IUser>> GetUsersYouRequestedToFollow(
            int maximumUsersToRetrieve = TweetinviConsts.FRIENDSHIPS_OUTGOING_USERS_MAX_PER_REQ);

        // Update Friendship Authorizations
        Task<bool> UpdateRelationshipAuthorizationsWith(IUserIdentifier user, bool retweetsEnabled, bool deviceNotificationEnabled);
        Task<bool> UpdateRelationshipAuthorizationsWith(long userId, bool retweetsEnabled, bool deviceNotificationEnabled);
        Task<bool> UpdateRelationshipAuthorizationsWith(string userScreenName, bool retweetsEnabled, bool deviceNotificationEnabled);


        // Retweet Muted Friends
        Task<long[]> GetUserIdsWhoseRetweetsAreMuted();
        Task<IEnumerable<IUser>> GetUsersWhoseRetweetsAreMuted();
    }
}