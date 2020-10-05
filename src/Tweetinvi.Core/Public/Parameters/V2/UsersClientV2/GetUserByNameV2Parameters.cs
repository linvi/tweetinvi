using Tweetinvi.Core.Parameters;

namespace Tweetinvi.Parameters.V2
{
    public interface IGetUserByNameV2Parameters : IBaseUsersV2GetByParameters
    {
        string Username { get; set; }
    }

    public class GetUserByNameV2Parameters : BaseUsersV2GetByParameters, IGetUserByNameV2Parameters
    {
        public GetUserByNameV2Parameters(string username) : base("username")
        {
            Username = username;
            this.WithAllFields();
        }

        public string Username { get; set; }
    }
}