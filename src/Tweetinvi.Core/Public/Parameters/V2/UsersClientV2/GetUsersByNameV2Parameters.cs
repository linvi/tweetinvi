using Tweetinvi.Core.Parameters;

namespace Tweetinvi.Parameters.V2
{
    public interface IGetUsersByNameV2Parameters : IBaseUsersV2GetByParameters
    {
        string[] Usernames { get; set; }
    }

    public class GetUsersByNameV2Parameters : BaseUsersV2GetByParameters, IGetUsersByNameV2Parameters
    {
        public GetUsersByNameV2Parameters(params string[] usernames) : base("username")
        {
            Usernames = usernames;
            this.WithAllFields();
        }

        public string[] Usernames { get; set; }
    }
}