using Tweetinvi.Parameters.Optionals;

namespace Tweetinvi.Parameters
{
    /// <summary>
    /// Parameters to get a user's blocked users
    /// </summary>
    public interface IGetBlockedUsersParameters : IGetCursorUsersOptionalParameters
    {
    }

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
