using Tweetinvi.Models;

namespace Tweetinvi.Parameters
{
    /// <summary>
    /// For more information visit : https://dev.twitter.com/en/docs/accounts-and-users/follow-search-get-users/api-reference/post-friendships-destroy
    /// </summary>
    /// <inheritdoc />
    public interface IUnfollowUserParameters : ICustomRequestParameters
    {
        /// <summary>
        /// The user that you want to stop following
        /// </summary>
        IUserIdentifier User { get; set; }
    }

    public class UnfollowUserParameters : CustomRequestParameters, IUnfollowUserParameters
    {
        public UnfollowUserParameters(string username) : this(new UserIdentifier(username))
        {
        }

        public UnfollowUserParameters(long userId) : this(new UserIdentifier(userId))
        {
        }

        public UnfollowUserParameters(IUserIdentifier user)
        {
            User = user;
        }

        public UnfollowUserParameters(IUnfollowUserParameters parameters) : base(parameters)
        {
            User = parameters?.User;
        }

        public IUserIdentifier User { get; set; }
    }
}
