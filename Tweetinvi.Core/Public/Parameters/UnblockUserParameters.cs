using Tweetinvi.Models;

namespace Tweetinvi.Parameters
{
    public interface IUnblockUserParameters : ICustomRequestParameters
    {
        IUserIdentifier UserIdentifier { get; set; }
    }

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

        public IUserIdentifier UserIdentifier { get; set; }
    }
}
