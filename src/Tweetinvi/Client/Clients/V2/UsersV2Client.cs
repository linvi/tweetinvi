using System.Threading.Tasks;
using Tweetinvi.Client.Requesters.V2;
using Tweetinvi.Models.V2.Responses;
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

        Task<UserResponseDTO> IUsersV2Client.GetUserAsync(long userId)
        {
            return GetUserAsync(new GetUserByIdV2Parameters(userId));
        }

        public Task<UserResponseDTO> GetUserAsync(string username)
        {
            return GetUserAsync(new GetUserByUsernameV2Parameters(username));
        }

        public async Task<UserResponseDTO> GetUserAsync(IGetUserByIdV2Parameters parameters)
        {
            var twitterResponse = await _usersV2Requester.GetUserAsync(parameters).ConfigureAwait(false);
            return twitterResponse?.Model;
        }

        public async Task<UserResponseDTO> GetUserAsync(IGetUserByUsernameV2Parameters parameters)
        {
            var twitterResponse = await _usersV2Requester.GetUserAsync(parameters).ConfigureAwait(false);
            return twitterResponse?.Model;
        }

        public Task<UsersResponseDTO> GetUsersAsync(long[] userIds)
        {
            return GetUsersAsync(new GetUsersByIdV2Parameters(userIds));
        }

        public Task<UsersResponseDTO> GetUsersAsync(string[] usernames)
        {
            return GetUsersAsync(new GetUsersByUsernameV2Parameters(usernames));
        }

        public async Task<UsersResponseDTO> GetUsersAsync(IGetUsersByIdV2Parameters parameters)
        {
            var twitterResponse = await _usersV2Requester.GetUsersAsync(parameters).ConfigureAwait(false);
            return twitterResponse?.Model;
        }

        public async Task<UsersResponseDTO> GetUsersAsync(IGetUsersByUsernameV2Parameters parameters)
        {
            var twitterResponse = await _usersV2Requester.GetUsersAsync(parameters).ConfigureAwait(false);
            return twitterResponse?.Model;
        }
    }
}