using Tweetinvi.Models;

namespace Tweetinvi.Parameters
{
    public interface IGetUsersParameters : ICustomRequestParameters
    {
        IUserIdentifier[] UserIdentifiers { get; set; }
        bool? IncludeEntities { get; set; }
    }

    public class GetUsersParameters : CustomRequestParameters, IGetUsersParameters
    {
        public GetUsersParameters(IUserIdentifier[] userIdentifiers)
        {
            UserIdentifiers = userIdentifiers;
        }

        public GetUsersParameters(IGetUsersParameters source) : base(source)
        {
            if (source == null) { return; }

            UserIdentifiers = source.UserIdentifiers;
            IncludeEntities = source.IncludeEntities;
        }

        public IUserIdentifier[] UserIdentifiers { get; set; }
        public bool? IncludeEntities { get; set; }
    }
}
