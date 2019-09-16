using Tweetinvi.Models;

namespace Tweetinvi.Parameters
{
    /// <summary>
    /// https://developer.twitter.com/en/docs/accounts-and-users/follow-search-get-users/api-reference/post-friendships-destroy
    /// </summary>
    public interface IUnFollowUserParameters : ICustomRequestParameters
    {
        /// <summary>
        /// User that you want to follow
        /// </summary>
        IUserIdentifier UserIdentifier { get; set; }
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
            UserIdentifier = userIdentifier;
        }

        public UnFollowUserParameters(IUnFollowUserParameters parameters) : base(parameters)
        {
            UserIdentifier = parameters?.UserIdentifier;
        }

        public IUserIdentifier UserIdentifier { get; set; }
    }
}
