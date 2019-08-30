using System.Threading.Tasks;
using Tweetinvi.Client.Requesters;
using Tweetinvi.Models;
using Tweetinvi.Parameters;

namespace Tweetinvi.Client
{
    public class UsersClient
    {
        private readonly IUsersRequester _usersRequester;

        public UsersClient(TwitterClient client)
        {
            _usersRequester = client.RequestExecutor.Users;
        }

        public async Task<IAuthenticatedUser> GetAuthenticatedUser()
        {
            var requestResult = await _usersRequester.GetAuthenticatedUser(null);
            return requestResult?.Result;
        }

        public async Task<IAuthenticatedUser> GetAuthenticatedUser(IGetAuthenticatedUserParameters parameters)
        {
            var requestResult = await _usersRequester.GetAuthenticatedUser(parameters);
            return requestResult?.Result;
        }
    }
}
