namespace Tweetinvi.Parameters
{
    /// <summary>
    /// For more information visit : https://developer.twitter.com/en/docs/accounts-and-users/create-manage-lists/api-reference/get-lists-subscriptions
    /// </summary>
    public interface IGetAccountListSubscriptionsParameters : ICursorQueryParameters
    {
    }

    public class GetAccountListSubscriptionsParameters : CursorQueryParameters, IGetAccountListSubscriptionsParameters
    {
        public GetAccountListSubscriptionsParameters()
        {
            PageSize = TwitterLimits.DEFAULTS.LISTS_GET_USER_SUBSCRIPTIONS_MAX_PAGE_SIZE;
        }

        public GetAccountListSubscriptionsParameters(IGetAccountListSubscriptionsParameters parameters) : base(parameters)
        {
            if (parameters == null)
            {
                PageSize = TwitterLimits.DEFAULTS.LISTS_GET_USER_SUBSCRIPTIONS_MAX_PAGE_SIZE;
            }
        }
    }
}