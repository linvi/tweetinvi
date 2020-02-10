namespace Tweetinvi.Parameters
{
    /// <summary>
    /// For more information visit : https://developer.twitter.com/en/docs/accounts-and-users/create-manage-lists/api-reference/get-lists-memberships
    /// </summary>
    /// <inheritdoc />
    public interface IGetListsAccountIsMemberOfParameters : ICursorQueryParameters
    {
        /// <summary>
        /// When set to true the request will return only the lists the authenticating user owns,
        /// and the specified user is a member of.
        /// </summary>
        bool? OnlyRetrieveAccountLists { get; set; }
    }

    /// <inheritdoc />
    public class GetListsAccountIsMemberOfParameters : CursorQueryParameters, IGetListsAccountIsMemberOfParameters
    {
        public GetListsAccountIsMemberOfParameters()
        {
            PageSize = TwitterLimits.DEFAULTS.LISTS_GET_USER_MEMBERSHIPS_MAX_SIZE;
        }

        public GetListsAccountIsMemberOfParameters(IGetListsAccountIsMemberOfParameters parameters) : base(parameters)
        {
            if (parameters == null)
            {
                PageSize = TwitterLimits.DEFAULTS.LISTS_GET_USER_MEMBERSHIPS_MAX_SIZE;
            }

            OnlyRetrieveAccountLists = parameters?.OnlyRetrieveAccountLists;
        }

        /// <inheritdoc />
        public bool? OnlyRetrieveAccountLists { get; set; }
    }
}