using Tweetinvi.Parameters.Optionals;

namespace Tweetinvi.Parameters
{
    /// <summary>
    /// For more information visit : https://dev.twitter.com/en/docs/accounts-and-users/mute-block-report-users/api-reference/get-blocks-list
    /// </summary>
    /// <inheritdoc />
    public interface IGetBlockedUsersParameters : IGetCursorUsersOptionalParameters
    {
    }

    /// <inheritdoc />
    public class GetBlockedUsersParameters : GetCursorUsersOptionalParameters, IGetBlockedUsersParameters
    {
        public GetBlockedUsersParameters()
        {
            PageSize = 5000;
        }

        public GetBlockedUsersParameters(IGetBlockedUsersParameters source) : base(source)
        {
        }
    }
}
