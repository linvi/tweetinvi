using Tweetinvi.Models;

namespace Tweetinvi.Parameters
{
    /// <summary>
    /// For more information visit : https://dev.twitter.com/en/docs/accounts-and-users/mute-block-report-users/api-reference/post-mutes-users-destroy
    /// </summary>
    public interface IUnmuteUserParameters : ICustomRequestParameters
    {
        /// <summary>
        /// User that you no longer want to mute
        /// </summary>
        IUserIdentifier User { get; set; }
    }

    /// <inheritdoc/>
    public class UnmuteUserParameters : CustomRequestParameters, IUnmuteUserParameters
    {
        public UnmuteUserParameters(long userId) : this(new UserIdentifier(userId))
        {
        }

        public UnmuteUserParameters(string username) : this(new UserIdentifier(username))
        {
        }

        public UnmuteUserParameters(IUserIdentifier user)
        {
            User = user;
        }

        public UnmuteUserParameters(IMuteUserParameters source) : base(source)
        {
            User = source?.User;
        }

        /// <inheritdoc/>
        public IUserIdentifier User { get; set; }
    }
}