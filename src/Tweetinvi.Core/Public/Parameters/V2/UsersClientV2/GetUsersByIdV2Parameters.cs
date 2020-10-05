using System.Linq;
using Tweetinvi.Core.Parameters;

namespace Tweetinvi.Parameters.V2
{
    public interface IGetUsersByIdV2Parameters : IBaseUsersV2Parameters
    {
        string[] UserIds { get; set; }
    }

    public class GetUsersByIdV2Parameters : BaseUsersV2Parameters, IGetUsersByIdV2Parameters
    {
        public GetUsersByIdV2Parameters(params long[] userIds)
        {
            UserIds = userIds.Select(x => x.ToString()).ToArray();
            this.WithAllFields();
        }

        public GetUsersByIdV2Parameters(params string[] userIds)
        {
            UserIds = userIds;
            this.WithAllFields();
            ;
        }

        public string[] UserIds { get; set; }
    }
}