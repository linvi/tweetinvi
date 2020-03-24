namespace Tweetinvi.Parameters
{
    /// <summary>
    /// For more information visit : 
    /// </summary>
    /// <inheritdoc />
    public interface IGetUserIdsYouRequestedToFollowParameters : ICursorQueryParameters
    {
    
    }
    
    /// <inheritdoc />
    public class GetUserIdsYouRequestedToFollowParameters : CursorQueryParameters, IGetUserIdsYouRequestedToFollowParameters
    {
        public GetUserIdsYouRequestedToFollowParameters()
        {
            PageSize = TwitterLimits.DEFAULTS.ACCOUNT_GET_REQUESTED_USER_IDS_TO_FOLLOW_MAX_PAGE_SIZE;
        }

        public GetUserIdsYouRequestedToFollowParameters(IGetUserIdsYouRequestedToFollowParameters source) : base(source)
        {
            if (source == null)
            {
                PageSize = TwitterLimits.DEFAULTS.ACCOUNT_GET_REQUESTED_USER_IDS_TO_FOLLOW_MAX_PAGE_SIZE;
                return;
            }

            PageSize = source.PageSize;
        }
    }
}