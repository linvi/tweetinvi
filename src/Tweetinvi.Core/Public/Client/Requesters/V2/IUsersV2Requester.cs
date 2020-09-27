using System.Threading.Tasks;
using Tweetinvi.Core.Web;
using Tweetinvi.Models.V2.Responses;
using Tweetinvi.Parameters.V2;

namespace Tweetinvi.Client.Requesters.V2
{
    public interface IUsersV2Requester
    {
        Task<ITwitterResult<UserResponseDTO>> GetUser(IGetUserByIdV2Parameters parameters);
        Task<ITwitterResult<UserResponseDTO>> GetUser(IGetUserByUsernameV2Parameters parameters);

        Task<ITwitterResult<UsersResponseDTO>> GetUsers(IGetUsersByIdV2Parameters parameters);
        Task<ITwitterResult<UsersResponseDTO>> GetUsers(IGetUsersByUsernameV2Parameters parameters);
    }
}