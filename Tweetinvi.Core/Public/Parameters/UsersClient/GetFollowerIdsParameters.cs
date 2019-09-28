using Tweetinvi.Models;

namespace Tweetinvi.Parameters
{
    /// <summary>
    /// For more information visit : https://dev.twitter.com/en/docs/accounts-and-users/follow-search-get-users/api-reference/get-followers-ids
    /// </summary>
    /// <inheritdoc />
    public interface IGetFollowerIdsParameters : ICursorQueryParameters
    {
        /// <summary>
        /// A unique identifier of a user
        /// </summary>
        IUserIdentifier User { get; }
    }

    /// <inheritdoc />
    public class GetFollowerIdsParameters : CursorQueryParameters, IGetFollowerIdsParameters
    {
        private GetFollowerIdsParameters()
        {
            PageSize = TwitterLimits.DEFAULTS.USERS_GET_FOLLOWER_IDS_PAGE_MAX_SIZE;
        }

        public GetFollowerIdsParameters(IUserIdentifier userIdentifier) : this()
        {
            User = userIdentifier;
        }

        public GetFollowerIdsParameters(string username) : this()
        {
            User = new UserIdentifier(username);
        }

        public GetFollowerIdsParameters(long? userId) : this()
        {
            User = new UserIdentifier(userId);
        }

        public GetFollowerIdsParameters(IGetFollowerIdsParameters parameters) : base(parameters)
        {
            if (parameters == null)
            {
                PageSize = TwitterLimits.DEFAULTS.USERS_GET_FOLLOWER_IDS_PAGE_MAX_SIZE;
                return;
            }

            User = parameters.User;
        }

        /// <inheritdoc />
        public IUserIdentifier User { get; }
    }
}