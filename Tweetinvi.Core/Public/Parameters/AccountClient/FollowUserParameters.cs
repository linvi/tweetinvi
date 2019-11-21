using Tweetinvi.Models;

namespace Tweetinvi.Parameters
{
    /// <summary>
    /// For more information visit : https://dev.twitter.com/en/docs/accounts-and-users/follow-search-get-users/api-reference/post-friendships-create
    /// </summary>
    /// <inheritdoc />
    public interface IFollowUserParameters : ICustomRequestParameters
    {
        /// <summary>
        /// User that want to follow
        /// </summary>
        IUserIdentifier User { get; set; }

        /// <summary>
        /// Enable notifications for the target user (twitter documentation name: follow)
        /// </summary>
        bool? EnableNotifications { get; set; }
    }

    /// <inheritdoc />
    public class FollowUserParameters : CustomRequestParameters, IFollowUserParameters
    {
        public FollowUserParameters(string username) : this(new UserIdentifier(username))
        {
        }

        public FollowUserParameters(long? userId) : this(new UserIdentifier(userId))
        {
        }

        public FollowUserParameters(IUserIdentifier userIdentifier)
        {
            User = userIdentifier;
        }

        public FollowUserParameters(IFollowUserParameters parameters) : base(parameters)
        {
            User = parameters?.User;
            EnableNotifications = parameters?.EnableNotifications;
        }

        /// <inheritdoc/>
        public IUserIdentifier User { get; set; }
        /// <inheritdoc/>
        public bool? EnableNotifications { get; set; }
    }
}
