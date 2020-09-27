using Tweetinvi.Core.Parameters;

namespace Tweetinvi.Parameters.V2
{
    public interface IGetUsersV2Parameters : IBaseUsersV2Parameters
    {
        long[] UserIds { get; set; }
    }

    public class GetUsersV2V2Parameters : BaseUsersV2Parameters, IGetUsersV2Parameters
    {
        public GetUsersV2V2Parameters(long[] userIds)
        {
            UserIds = userIds;
        }

        public long[] UserIds { get; set; }
    }
}