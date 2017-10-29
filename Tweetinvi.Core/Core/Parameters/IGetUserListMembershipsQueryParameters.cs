using Tweetinvi.Core.Public.Parameters;
using Tweetinvi.Models;

namespace Tweetinvi.Core.Core.Parameters
{
    /// <summary>
    /// https://developer.twitter.com/en/docs/accounts-and-users/create-manage-lists/api-reference/get-lists-memberships
    /// </summary>
    public interface IGetUserListMembershipsQueryParameters
    {
        /// <summary>
        /// User identifier
        /// </summary>
        IUserIdentifier UserIdentifier { get; set; }

        /// <summary>
        /// Optional Parameters
        /// </summary>
        IGetUserListMembershipsParameters Parameters { get; set; }
    }
}
