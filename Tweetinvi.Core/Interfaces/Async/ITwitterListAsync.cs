using System.Collections.Generic;
using System.Threading.Tasks;
using Tweetinvi.Core.Enum;
using Tweetinvi.Core.Interfaces.Models;
using Tweetinvi.Core.Parameters;

namespace Tweetinvi.Core.Interfaces.Async
{
    public interface ITwitterListAsync
    {
        Task<bool> UpdateAsync(ITwitterListUpdateParameters parameters);
        
        Task<bool> DestroyAsync();

        Task<IEnumerable<ITweet>> GetTweetsAsync(IGetTweetsFromListParameters getTweetsFromListParameters = null);
        
        // Membership
        Task<IEnumerable<IUser>> GetMembersAsync(int maxNumberOfUsersToRetrieve = 100);

        Task<bool> AddMemberAsync(IUserIdentifier user);

        Task<MultiRequestsResult> AddMultipleMembersAsync(IEnumerable<IUserIdentifier> users);

        Task<bool> RemoveMemberAsync(IUserIdentifier user);

        Task<MultiRequestsResult> RemoveMultipleMembersAsync(IEnumerable<IUserIdentifier> users);

        Task<bool> CheckUserMembershipAsync(IUserIdentifier user);

        // Subscription
        Task<IEnumerable<IUser>> GetSubscribersAsync(int maximumNumberOfUsersToRetrieve = 100);

        Task<bool> SubscribeLoggedUserToListAsync(ILoggedUser loggedUser = null);
        
        Task<bool> UnSubscribeLoggedUserFromListAsync(ILoggedUser loggedUser = null);
        
        Task<bool> CheckUserSubscriptionAsync(IUserIdentifier user);
    }
}
