using Tweetinvi.Core.Core.Parameters;
using Tweetinvi.Core.Public.Parameters;
using Tweetinvi.Models;

namespace Tweetinvi.Logic.QueryParameters
{
    public class GetUserListMembershipsQueryParameters : IGetUserListMembershipsQueryParameters
    {
        public GetUserListMembershipsQueryParameters(IUserIdentifier userIdentifier)
        {
            UserIdentifier = userIdentifier;
            Parameters = new GetUserListMembershipsParameters();
        }

        public IUserIdentifier UserIdentifier { get; set; }
        public IGetUserListMembershipsParameters Parameters { get; set; }
    }
}
