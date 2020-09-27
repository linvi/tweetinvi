using System.Threading.Tasks;
using Tweetinvi.Core.Web;
using Tweetinvi.Models.V2.Responses;
using Tweetinvi.Parameters.V2;

namespace Tweetinvi.Client.Requesters.V2
{
    public interface IUsersV2Requester
    {
        Task<ITwitterResult<UserResponseDTO>> GetUser(IGetUserV2Parameters parameters);
        Task<ITwitterResult<UsersResponseDTO>> GetUsers(IGetUsersV2Parameters parameters);
    }
}