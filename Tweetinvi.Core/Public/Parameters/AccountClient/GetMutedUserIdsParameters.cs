namespace Tweetinvi.Parameters
{
    /// <summary>
    /// For more information visit : https://dev.twitter.com/en/docs/accounts-and-users/mute-block-report-users/api-reference/get-mutes-users-ids
    /// </summary>
    public interface IGetMutedUserIdsParameters : ICursorQueryParameters
    {
    }
    
    public class GetMutedUserIdsParameters : CursorQueryParameters, IGetMutedUserIdsParameters
    {
        public GetMutedUserIdsParameters()
        {
            PageSize = TwitterLimits.DEFAULTS.ACCOUNT_GET_MUTED_USER_IDS_MAX_PAGE_SIZE;
        }

        public GetMutedUserIdsParameters(IGetMutedUserIdsParameters source) : base(source)
        {
            if (source == null)
            {
                PageSize = TwitterLimits.DEFAULTS.ACCOUNT_GET_MUTED_USER_IDS_MAX_PAGE_SIZE;
            }
        }
    }
}