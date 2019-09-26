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
        IUserIdentifier UserIdentifier { get; }
    }

    /// <inheritdoc />
    public class GetFriendIdsParameters : CursorQueryParameters, IGetFriendIdsParameters
    {
        private GetFriendIdsParameters()
        {
            PageSize = 5000;
        }

        public GetFriendIdsParameters(IUserIdentifier userIdentifier) : this()
        {
            UserIdentifier = userIdentifier;
        }

        public GetFriendIdsParameters(string username) : this()
        {
            UserIdentifier = new UserIdentifier(username);
        }

        public GetFriendIdsParameters(long? userId) : this()
        {
            UserIdentifier = new UserIdentifier(userId);
        }
        
        public GetFriendIdsParameters(IGetFriendIdsParameters parameters) : base(parameters)
        {
            if (parameters == null) { return; }
            
            UserIdentifier = parameters.UserIdentifier;
        }

        /// <inheritdoc />
        public IUserIdentifier UserIdentifier { get; }
    }
}
