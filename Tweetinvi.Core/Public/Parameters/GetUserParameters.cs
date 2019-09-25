using Tweetinvi.Models;
using Tweetinvi.Parameters.Optionals;

namespace Tweetinvi.Parameters
{
    public interface IGetUserParameters : IGetUsersOptionalParameters
    {
        IUserIdentifier UserIdentifier { get; set; }
    }

    public class GetUserParameters : GetUsersOptionalParameters, IGetUserParameters
    {
        public GetUserParameters(IUserIdentifier userIdentifier)
        {
            UserIdentifier = userIdentifier;
        }

        public GetUserParameters(IGetUserParameters source) : base(source)
        {
            UserIdentifier = source?.UserIdentifier;
        }

        public IUserIdentifier UserIdentifier { get; set; }
    }
}