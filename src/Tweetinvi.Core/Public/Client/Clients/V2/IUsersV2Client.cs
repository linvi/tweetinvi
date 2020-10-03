using System.Threading.Tasks;
using Tweetinvi.Models.V2.Responses;
using Tweetinvi.Parameters.V2;

namespace Tweetinvi.Client.V2
{
    public interface IUsersV2Client
    {
        Task<UserResponseDTO> GetUserAsync(long userId);
        Task<UserResponseDTO> GetUserAsync(string username);
        Task<UserResponseDTO> GetUserAsync(IGetUserByIdV2Parameters parameters);
        Task<UserResponseDTO> GetUserAsync(IGetUserByUsernameV2Parameters parameters);

        Task<UsersResponseDTO> GetUsersAsync(long[] userIds);
        Task<UsersResponseDTO> GetUsersAsync(string[] usernames);
        Task<UsersResponseDTO> GetUsersAsync(IGetUsersByIdV2Parameters parameters);
        Task<UsersResponseDTO> GetUsersAsync(IGetUsersByUsernameV2Parameters parameters);
    }
}