using System.Threading.Tasks;
using Tweetinvi.Client.Requesters.V2;
using Tweetinvi.Models.V2;
using Tweetinvi.Parameters.V2;

namespace Tweetinvi.Client.V2
{
    public class UsersV2Client : IUsersV2Client
    {
        private readonly IUsersV2Requester _usersV2Requester;

        public UsersV2Client(IUsersV2Requester usersV2Requester)
        {
            _usersV2Requester = usersV2Requester;
        }

        public Task<UserV2Response> GetUserByIdAsync(long userId)
        {
            return GetUserByIdAsync(new GetUserByIdV2Parameters(userId));
        }

        public Task<UserV2Response> GetUserByIdAsync(string userId)
        {
            return GetUserByIdAsync(new GetUserByIdV2Parameters(userId));
        }

        public Task<UserV2Response> GetUserByNameAsync(string username)
        {
            return GetUserByNameAsync(new GetUserByNameV2Parameters(username));
        }

        public async Task<UserV2Response> GetUserByIdAsync(IGetUserByIdV2Parameters parameters)
        {
            var twitterResponse = await _usersV2Requester.GetUserAsync(parameters).ConfigureAwait(false);
            return twitterResponse?.Model;
        }

        public async Task<UserV2Response> GetUserByNameAsync(IGetUserByNameV2Parameters parameters)
        {
            var twitterResponse = await _usersV2Requester.GetUserAsync(parameters).ConfigureAwait(false);
            return twitterResponse?.Model;
        }

        public Task<UsersV2Response> GetUsersByIdAsync(long[] userIds)
        {
            return GetUsersByIdAsync(new GetUsersByIdV2Parameters(userIds));
        }

        public Task<UsersV2Response> GetUsersByIdAsync(string[] userIds)
        {
            return GetUsersByIdAsync(new GetUsersByIdV2Parameters(userIds));
        }

        public Task<UsersV2Response> GetUsersByNameAsync(string[] usernames)
        {
            return GetUsersByNameAsync(new GetUsersByNameV2Parameters(usernames));
        }

        public async Task<UsersV2Response> GetUsersByIdAsync(IGetUsersByIdV2Parameters parameters)
        {
            var twitterResponse = await _usersV2Requester.GetUsersAsync(parameters).ConfigureAwait(false);
            return twitterResponse?.Model;
        }

        public async Task<UsersV2Response> GetUsersByNameAsync(IGetUsersByNameV2Parameters parameters)
        {
            var twitterResponse = await _usersV2Requester.GetUsersAsync(parameters).ConfigureAwait(false);
            return twitterResponse?.Model;
        }
    }
}