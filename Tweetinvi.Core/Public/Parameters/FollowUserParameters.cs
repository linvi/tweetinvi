using Tweetinvi.Models;

namespace Tweetinvi.Parameters
{
    /// <summary>
    /// https://developer.twitter.com/en/docs/accounts-and-users/follow-search-get-users/api-reference/post-friendships-create
    /// </summary>
    public interface IFollowUserParameters : ICustomRequestParameters
    {
        /// <summary>
        /// User that you want to follow
        /// </summary>
        IUserIdentifier UserIdentifier { get; set; }

        /// <summary>
        /// Enable notifications for the target user (twitter documentation name: follow)
        /// </summary>
        bool? EnableNotifications { get; set; }
    }

    public class FollowUserParameters : CustomRequestParameters, IFollowUserParameters
    {
        public FollowUserParameters()
        {
            
        }

        public FollowUserParameters(string username) : this(new UserIdentifier(username))
        {
        }

        public FollowUserParameters(long userId) : this(new UserIdentifier(userId))
        {
        }

        public FollowUserParameters(IUserIdentifier userIdentifier)
        {
            UserIdentifier = userIdentifier;
        }

        public FollowUserParameters(IFollowUserParameters parameters) : base(parameters)
        {
            UserIdentifier = parameters?.UserIdentifier;
            EnableNotifications = parameters?.EnableNotifications;
        }

        public IUserIdentifier UserIdentifier { get; set; }
        public bool? EnableNotifications { get; set; }
    }
}
