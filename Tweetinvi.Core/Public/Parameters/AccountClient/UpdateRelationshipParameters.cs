using Tweetinvi.Models;

namespace Tweetinvi.Parameters
{
    /// <summary>
    /// https://dev.twitter.com/en/docs/accounts-and-users/follow-search-get-users/api-reference/post-friendships-update
    /// </summary>
    public interface IUpdateRelationshipParameters : ICustomRequestParameters
    {
        /// <summary>
        /// User with whom you want to change the friendship
        /// </summary>
        IUserIdentifier User { get; set; }
        
        /// <summary>
        /// Enable/disable device notifications from the user. 	
        /// </summary>
        bool? EnableRetweets { get; set; }
        
        /// <summary>
        /// Enable/disable Retweets from the user.
        /// </summary>
        bool? EnableDeviceNotifications { get; set; }
    }
    
    /// <inheritdoc />
    public class UpdateRelationshipParameters : CustomRequestParameters, IUpdateRelationshipParameters
    {
        public UpdateRelationshipParameters(long? userId) : this(new UserIdentifier(userId))
        {
        }

        public UpdateRelationshipParameters(string username) : this(new UserIdentifier(username))
        {
        }

        public UpdateRelationshipParameters(IUserIdentifier user)
        {
            User = user;
        }
        
        public UpdateRelationshipParameters(IUpdateRelationshipParameters source) : base(source)
        {
            User = source?.User;
            EnableRetweets = source?.EnableRetweets;
            EnableDeviceNotifications = source?.EnableDeviceNotifications;
        }
        
        /// <inheritdoc />
        public IUserIdentifier User { get; set; }
        /// <inheritdoc />
        public bool? EnableRetweets { get; set; }
        /// <inheritdoc />
        public bool? EnableDeviceNotifications { get; set; }
    }
}