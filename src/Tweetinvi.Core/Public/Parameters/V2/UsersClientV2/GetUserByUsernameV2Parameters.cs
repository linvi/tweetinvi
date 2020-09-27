using Tweetinvi.Core.Parameters;

namespace Tweetinvi.Parameters.V2
{
    public interface IGetUserByUsernameV2Parameters : IBaseUsersV2GetByParameters
    {
        string Username { get; set; }
    }

    public class GetUserByUsernameV2Parameters : BaseUsersV2GetByParameters, IGetUserByUsernameV2Parameters
    {
        public GetUserByUsernameV2Parameters(string username) : base("username")
        {
            Username = username;
            this.WithAllFields();
        }

        public string Username { get; set; }
    }
}