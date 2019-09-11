using Tweetinvi.Models;

namespace Tweetinvi.Parameters
{
    public interface IUnblockUserParameters : ICustomRequestParameters
    {
        IUserIdentifier UserIdentifier { get; set; }
        
        /// <summary>
        /// Include user entities.
        /// </summary>
        bool? IncludeEntities { get; set; }
        bool? SkipStatus { get; set; }
    }

    public class UnblockUserParameters : CustomRequestParameters, IUnblockUserParameters
    {
        public UnblockUserParameters(IUserIdentifier userIdentifier)
        {
            UserIdentifier = userIdentifier;
            SkipStatus = true;
        }

        public UnblockUserParameters(string username) : this(new UserIdentifier(username))
        {
        }

        public UnblockUserParameters(long? userId) : this(new UserIdentifier(userId))
        {
        }

        public UnblockUserParameters(IUnblockUserParameters source) : base(source)
        {
            UserIdentifier = source.UserIdentifier;
            IncludeEntities = source.IncludeEntities;
            SkipStatus = source.SkipStatus;
        }

        public IUserIdentifier UserIdentifier { get; set; }
        public bool? IncludeEntities { get; set; }
        public bool? SkipStatus { get; set; }
    }
}
