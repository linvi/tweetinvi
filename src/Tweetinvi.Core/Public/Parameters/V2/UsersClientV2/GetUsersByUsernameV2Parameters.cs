using Tweetinvi.Core.Parameters;

namespace Tweetinvi.Parameters.V2
{
    public interface IGetUsersByUsernameV2Parameters : IBaseUsersV2GetByParameters
    {
        string[] Usernames { get; set; }
    }

    public class GetUsersByUsernameV2Parameters : BaseUsersV2GetByParameters, IGetUsersByUsernameV2Parameters
    {
        public GetUsersByUsernameV2Parameters(params string[] usernames) : base("username")
        {
            Usernames = usernames;
            this.WithAllFields();
        }

        public string[] Usernames { get; set; }
    }
}