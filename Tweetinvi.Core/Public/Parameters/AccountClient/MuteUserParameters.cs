using Tweetinvi.Models;

namespace Tweetinvi.Parameters
{
    /// <summary>
    /// For more information visit : https://dev.twitter.com/en/docs/accounts-and-users/mute-block-report-users/api-reference/post-mutes-users-create
    /// </summary>
    public interface IMuteUserParameters : ICustomRequestParameters
    {
        /// <summary>
        /// User that you want to mute
        /// </summary>
        IUserIdentifier User { get; set; }
    }
    
    /// <inheritdoc/>
    public class MuteUserParameters : CustomRequestParameters, IMuteUserParameters
    {
        public MuteUserParameters(long? userId) : this(new UserIdentifier(userId))
        {
        }
        
        public MuteUserParameters(string username) : this(new UserIdentifier(username))
        {
        }
        
        public MuteUserParameters(IUserIdentifier user)
        {
            User = user;
        }

        public MuteUserParameters(IMuteUserParameters source) : base(source)
        {
            User = source?.User;
        }
        
        /// <inheritdoc/>
        public IUserIdentifier User { get; set; }
    }
}