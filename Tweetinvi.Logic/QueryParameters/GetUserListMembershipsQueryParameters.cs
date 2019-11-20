using Tweetinvi.Core.Parameters;
using Tweetinvi.Models;
using Tweetinvi.Parameters;

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
