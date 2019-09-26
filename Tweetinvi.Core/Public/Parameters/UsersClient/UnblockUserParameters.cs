using Tweetinvi.Models;

namespace Tweetinvi.Parameters
{
    /// <summary>
    /// For more information visit : https://dev.twitter.com/en/docs/accounts-and-users/mute-block-report-users/api-reference/post-blocks-destroy
    /// </summary>
    /// <inheritdoc />
    public interface IUnblockUserParameters : ICustomRequestParameters
    {
        /// <summary>
        /// User that you want to unblock
        /// </summary>  
        IUserIdentifier UserIdentifier { get; set; }
    }

    /// <inheritdoc />
    public class UnblockUserParameters : CustomRequestParameters, IUnblockUserParameters
    {
        public UnblockUserParameters(IUserIdentifier userIdentifier)
        {
            UserIdentifier = userIdentifier;
        }

        public UnblockUserParameters(string username) : this(new UserIdentifier(username))
        {
        }

        public UnblockUserParameters(long? userId) : this(new UserIdentifier(userId))
        {
        }

        public UnblockUserParameters(IUnblockUserParameters source) : base(source)
        {
            UserIdentifier = source?.UserIdentifier;
        }

        /// <inheritdoc />
        public IUserIdentifier UserIdentifier { get; set; }
    }
}
