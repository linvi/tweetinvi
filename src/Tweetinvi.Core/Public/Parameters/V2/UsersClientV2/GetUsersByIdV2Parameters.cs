using Tweetinvi.Core.Parameters;

namespace Tweetinvi.Parameters.V2
{
    public interface IGetUsersByIdV2Parameters : IBaseUsersV2Parameters
    {
        long[] UserIds { get; set; }
    }

    public class GetUsersByIdV2Parameters : BaseUsersV2Parameters, IGetUsersByIdV2Parameters
    {
        public GetUsersByIdV2Parameters(params long[] userIds)
        {
            UserIds = userIds;
            this.WithAllFields();
;        }

        public long[] UserIds { get; set; }
    }
}