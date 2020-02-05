namespace Tweetinvi.Parameters.ListsClient
{
    /// <summary>
    /// For more information visit : https://developer.twitter.com/en/docs/accounts-and-users/create-manage-lists/api-reference/get-lists-ownerships
    /// </summary>
    /// <inheritdoc />
    public interface IGetListsOwnedByAccountParameters : ICursorQueryParameters
    {
    }

    /// <inheritdoc />
    public class GetListsOwnedByAccountParameters : CursorQueryParameters, IGetListsOwnedByAccountParameters
    {
        public GetListsOwnedByAccountParameters()
        {
            PageSize = TwitterLimits.DEFAULTS.LISTS_GET_USER_OWNED_LISTS_MAX_SIZE;
        }

        public GetListsOwnedByAccountParameters(IGetListsOwnedByAccountParameters parameters) : base(parameters)
        {
            if (parameters == null)
            {
                PageSize = TwitterLimits.DEFAULTS.LISTS_GET_USER_OWNED_LISTS_MAX_SIZE;
            }
        }
    }
}