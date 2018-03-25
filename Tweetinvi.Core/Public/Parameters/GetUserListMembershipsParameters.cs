namespace Tweetinvi.Core.Public.Parameters
{
    /// <summary>
    /// For more description visit : https://developer.twitter.com/en/docs/accounts-and-users/create-manage-lists/api-reference/get-lists-memberships
    /// </summary>
    public interface IGetUserListMembershipsParameters : ICursorQueryParameters
    {
        /// <summary>
        /// Maximum number of TwitterLists to retrieve.
        /// </summary>
        new int MaximumNumberOfResults { get; set; }

        /// <summary>
        /// Only return Lists the user owns or is a member of
        /// </summary>
        bool? FilterToOwnLists { get; set; }
    }

    public class GetUserListMembershipsParameters : CursorQueryParameters, IGetUserListMembershipsParameters
    {
        public GetUserListMembershipsParameters()
        {
            FilterToOwnLists = false;
            MaximumNumberOfResults = 20;
        }

        public bool? FilterToOwnLists { get; set; }
    }
}
