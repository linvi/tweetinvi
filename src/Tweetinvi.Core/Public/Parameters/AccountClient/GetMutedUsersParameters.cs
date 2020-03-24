using Tweetinvi.Parameters.Optionals;

namespace Tweetinvi.Parameters
{
    /// <summary>
    /// For more information visit : https://dev.twitter.com/en/docs/accounts-and-users/mute-block-report-users/api-reference/get-mutes-users-list
    /// </summary>
    /// <inheritdoc />
    public interface IGetMutedUsersParameters : IGetCursorUsersOptionalParameters
    {
    }
    
    /// <inheritdoc/>
    public class GetMutedUsersParameters : GetCursorUsersOptionalParameters, IGetMutedUsersParameters
    {
        public GetMutedUsersParameters()
        {
            PageSize = TwitterLimits.DEFAULTS.ACCOUNT_GET_MUTED_USER_IDS_MAX_PAGE_SIZE;
        }

        public GetMutedUsersParameters(IGetMutedUsersParameters source) : base(source)
        {
            if (source == null)
            {
                PageSize = TwitterLimits.DEFAULTS.ACCOUNT_GET_MUTED_USER_IDS_MAX_PAGE_SIZE;
                return;
            }

            PageSize = source.PageSize;
        }
    }
}