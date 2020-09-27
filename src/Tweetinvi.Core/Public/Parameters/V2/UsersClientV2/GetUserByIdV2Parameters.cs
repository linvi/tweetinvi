using Tweetinvi.Core.Parameters;

namespace Tweetinvi.Parameters.V2
{
    public interface IGetUserByIdV2Parameters : IBaseUsersV2Parameters
    {
        long UserId { get; set; }
    }

    public class GetUserByIdV2Parameters : BaseUsersV2Parameters, IGetUserByIdV2Parameters
    {
        public GetUserByIdV2Parameters(long userId)
        {
            UserId = userId;
            this.WithAllFields();
        }

        public long UserId { get; set; }
    }
}