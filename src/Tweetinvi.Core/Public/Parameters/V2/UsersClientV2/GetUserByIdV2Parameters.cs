using Tweetinvi.Core.Parameters;

namespace Tweetinvi.Parameters.V2
{
    public interface IGetUserByIdV2Parameters : IBaseUsersV2Parameters
    {
        string UserId { get; set; }
    }

    public class GetUserByIdV2Parameters : BaseUsersV2Parameters, IGetUserByIdV2Parameters
    {
        public GetUserByIdV2Parameters(long userId)
        {
            UserId = userId.ToString();
            this.WithAllFields();
        }

        public GetUserByIdV2Parameters(string userId)
        {
            UserId = userId;
            this.WithAllFields();
        }

        public string UserId { get; set; }
    }
}