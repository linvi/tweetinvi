namespace Tweetinvi.Parameters.ListsClient
{
    /// <summary>
    /// For more information visit : https://developer.twitter.com/en/docs/accounts-and-users/create-manage-lists/api-reference/get-lists-ownerships
    /// </summary>
    /// <inheritdoc />
    public interface IGetListsOwnedParameters : ICursorQueryParameters
    {
    }

    /// <inheritdoc />
    public class GetListsOwnedParameters : CursorQueryParameters, IGetListsOwnedParameters
    {
        public GetListsOwnedParameters()
        {
            PageSize = TwitterLimits.DEFAULTS.LISTS_GET_USER_OWNED_LISTS_MAX_SIZE;
        }

        public GetListsOwnedParameters(IGetListsOwnedParameters parameters) : base(parameters)
        {
            if (parameters == null)
            {
                PageSize = TwitterLimits.DEFAULTS.LISTS_GET_USER_OWNED_LISTS_MAX_SIZE;
            }
        }
    }
}