using Tweetinvi.Core.Public.Parameters;

namespace Tweetinvi.Parameters
{
    /// <summary>
    /// Parameters to get a user's list of friends
    /// </summary>
    public interface IGetBlockedUserIdsParameters : ICursorQueryParameters
    {
    }

    public class GetBlockedUserIdsParameters : CursorQueryParameters, IGetBlockedUserIdsParameters
    {
        public GetBlockedUserIdsParameters()
        {
            MaximumNumberOfResults = 5000;
        }

        public GetBlockedUserIdsParameters(IGetBlockedUserIdsParameters source) : base(source)
        {
        }
    }
}
