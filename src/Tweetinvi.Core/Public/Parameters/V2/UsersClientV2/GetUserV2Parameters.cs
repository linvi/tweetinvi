using Tweetinvi.Core.Parameters;

namespace Tweetinvi.Parameters.V2
{
    public interface IGetUserV2Parameters : IBaseUsersV2Parameters
    {
        long UserId { get; set; }
    }

    public class GetUserV2V2Parameters : BaseUsersV2Parameters, IGetUserV2Parameters
    {
        public GetUserV2V2Parameters(long userId)
        {
            UserId = userId;
            this.WithAllFields();
        }

        public long UserId { get; set; }
    }
}