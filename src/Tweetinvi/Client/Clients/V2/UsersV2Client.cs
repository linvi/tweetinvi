using System.Threading.Tasks;
using Tweetinvi.Client.Requesters.V2;
using Tweetinvi.Models.Responses;
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

        Task<UserV2Response> IUsersV2Client.GetUserAsync(long userId)
        {
            return GetUserAsync(new GetUserByIdV2Parameters(userId));
        }

        public Task<UserV2Response> GetUserAsync(string username)
        {
            return GetUserAsync(new GetUserByUsernameV2Parameters(username));
        }

        public async Task<UserV2Response> GetUserAsync(IGetUserByIdV2Parameters parameters)
        {
            var twitterResponse = await _usersV2Requester.GetUserAsync(parameters).ConfigureAwait(false);
            return twitterResponse?.Model;
        }

        public async Task<UserV2Response> GetUserAsync(IGetUserByUsernameV2Parameters parameters)
        {
            var twitterResponse = await _usersV2Requester.GetUserAsync(parameters).ConfigureAwait(false);
            return twitterResponse?.Model;
        }

        public Task<UsersV2Response> GetUsersAsync(long[] userIds)
        {
            return GetUsersAsync(new GetUsersByIdV2Parameters(userIds));
        }

        public Task<UsersV2Response> GetUsersAsync(string[] usernames)
        {
            return GetUsersAsync(new GetUsersByUsernameV2Parameters(usernames));
        }

        public async Task<UsersV2Response> GetUsersAsync(IGetUsersByIdV2Parameters parameters)
        {
            var twitterResponse = await _usersV2Requester.GetUsersAsync(parameters).ConfigureAwait(false);
            return twitterResponse?.Model;
        }

        public async Task<UsersV2Response> GetUsersAsync(IGetUsersByUsernameV2Parameters parameters)
        {
            var twitterResponse = await _usersV2Requester.GetUsersAsync(parameters).ConfigureAwait(false);
            return twitterResponse?.Model;
        }
    }
}