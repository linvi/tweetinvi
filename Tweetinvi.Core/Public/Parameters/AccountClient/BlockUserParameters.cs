using Tweetinvi.Models;

namespace Tweetinvi.Parameters
{
    /// <summary>
    /// For more information visit : https://dev.twitter.com/en/docs/accounts-and-users/mute-block-report-users/api-reference/post-blocks-create
    /// </summary>
    /// <inheritdoc />
    public interface IBlockUserParameters : ICustomRequestParameters
    {
        /// <summary>
        /// The user that you wish to block
        /// </summary>   
        IUserIdentifier User { get; set; }
    }

    /// <inheritdoc cref="IBlockUserParameters" />
    public class BlockUserParameters : CustomRequestParameters, IBlockUserParameters
    {
        public BlockUserParameters(IUserIdentifier userIdentifier)
        {
            User = userIdentifier;
        }
        
        public BlockUserParameters(string username) : this(new UserIdentifier(username))
        {
        }

        public BlockUserParameters(long? userId) : this(new UserIdentifier(userId))
        {
        }
        
        public BlockUserParameters(IBlockUserParameters source) : base(source)
        {
            User = source?.User;
        }
        
        /// <inheritdoc/>
        public IUserIdentifier User { get; set; }
    }
}