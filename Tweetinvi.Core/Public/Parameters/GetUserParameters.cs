using Tweetinvi.Models;

namespace Tweetinvi.Parameters
{
    public interface IGetUserParameters : ICustomRequestParameters
    {
        IUserIdentifier UserIdentifier { get; set; }
        bool? IncludeEntities { get; set; }
    }

    public class GetUserParameters : CustomRequestParameters, IGetUserParameters
    {
        public GetUserParameters(IUserIdentifier userIdentifier)
        {
            UserIdentifier = userIdentifier;
        }

        public IUserIdentifier UserIdentifier { get; set; }
        public bool? IncludeEntities { get; set; }
    }
}
