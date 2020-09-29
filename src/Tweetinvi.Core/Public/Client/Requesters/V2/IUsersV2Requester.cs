using System.Threading.Tasks;
using Tweetinvi.Core.Web;
using Tweetinvi.Models.V2.Responses;
using Tweetinvi.Parameters.V2;

namespace Tweetinvi.Client.Requesters.V2
{
    public interface IUsersV2Requester
    {
        Task<ITwitterResult<UserResponseDTO>> GetUserAsync(IGetUserByIdV2Parameters parameters);
        Task<ITwitterResult<UserResponseDTO>> GetUserAsync(IGetUserByUsernameV2Parameters parameters);

        Task<ITwitterResult<UsersResponseDTO>> GetUsersAsync(IGetUsersByIdV2Parameters parameters);
        Task<ITwitterResult<UsersResponseDTO>> GetUsersAsync(IGetUsersByUsernameV2Parameters parameters);
    }
}