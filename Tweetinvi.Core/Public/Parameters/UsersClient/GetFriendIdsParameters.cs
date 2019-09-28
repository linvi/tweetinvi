using Tweetinvi.Models;

namespace Tweetinvi.Parameters
{
    /// <summary>
    /// For more information visit : https://dev.twitter.com/en/docs/accounts-and-users/follow-search-get-users/api-reference/get-friends-ids
    /// </summary>
    /// <inheritdoc />
    public interface IGetFriendIdsParameters : ICursorQueryParameters
    {
        /// <summary>
        /// User for who you want to get the friends from.
        /// </summary>
        IUserIdentifier User { get; }
    }

    /// <inheritdoc />
    public class GetFriendIdsParameters : CursorQueryParameters, IGetFriendIdsParameters
    {
        private GetFriendIdsParameters()
        {
            PageSize = TwitterLimits.DEFAULTS.USERS_GET_FRIEND_IDS_PAGE_MAX_SIZE;
        }

        public GetFriendIdsParameters(IUserIdentifier userIdentifier) : this()
        {
            User = userIdentifier;
        }

        public GetFriendIdsParameters(string username) : this()
        {
            User = new UserIdentifier(username);
        }

        public GetFriendIdsParameters(long? userId) : this()
        {
            User = new UserIdentifier(userId);
        }
        
        public GetFriendIdsParameters(IGetFriendIdsParameters parameters) : base(parameters)
        {
            if (parameters == null)
            {
                PageSize = TwitterLimits.DEFAULTS.USERS_GET_FRIEND_IDS_PAGE_MAX_SIZE;
                return;
            }
            
            User = parameters.User;
        }

        /// <inheritdoc />
        public IUserIdentifier User { get; }
    }
}
