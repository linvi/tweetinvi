using System.Collections.Generic;
using System.Threading.Tasks;
using Tweetinvi.Models;

namespace Tweetinvi.Core.Controllers
{
    public interface IFriendshipController
    {
        // Update Friendship Authorizations
        Task<bool> UpdateRelationshipAuthorizationsWith(IUserIdentifier user, bool retweetsEnabled, bool deviceNotificationEnabled);
        Task<bool> UpdateRelationshipAuthorizationsWith(long userId, bool retweetsEnabled, bool deviceNotificationEnabled);
        Task<bool> UpdateRelationshipAuthorizationsWith(string userScreenName, bool retweetsEnabled, bool deviceNotificationEnabled);


        // Retweet Muted Friends
        Task<long[]> GetUserIdsWhoseRetweetsAreMuted();
        Task<IEnumerable<IUser>> GetUsersWhoseRetweetsAreMuted();
    }
}