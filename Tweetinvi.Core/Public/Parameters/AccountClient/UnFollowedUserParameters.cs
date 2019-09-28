using Tweetinvi.Models;

namespace Tweetinvi.Parameters
{
    /// <summary>
    /// For more information visit : https://dev.twitter.com/en/docs/accounts-and-users/follow-search-get-users/api-reference/post-friendships-destroy
    /// </summary>
    /// <inheritdoc />
    public interface IUnFollowUserParameters : ICustomRequestParameters
    {
        /// <summary>
        /// The user that you want to stop following
        /// </summary>
        IUserIdentifier User { get; set; }
    }

    public class UnFollowUserParameters : CustomRequestParameters, IUnFollowUserParameters
    {
        public UnFollowUserParameters(string username) : this(new UserIdentifier(username))
        {
        }

        public UnFollowUserParameters(long userId) : this(new UserIdentifier(userId))
        {
        }

        public UnFollowUserParameters(IUserIdentifier userIdentifier)
        {
            User = userIdentifier;
        }

        public UnFollowUserParameters(IUnFollowUserParameters parameters) : base(parameters)
        {
            User = parameters?.User;
        }

        public IUserIdentifier User { get; set; }
    }
}
