using System.Threading.Tasks;
using Tweetinvi.Core.Web;
using Tweetinvi.Models;
using Tweetinvi.Models.V2.Responses;
using Tweetinvi.Parameters.V2;

namespace Tweetinvi.Core.Controllers.V2
{
    public interface IUsersV2Controller
    {
        Task<ITwitterResult<UserResponseDTO>> GetUserAsync(IGetUserV2Parameters parameters, ITwitterRequest request);
        Task<ITwitterResult<UsersResponseDTO>> GetUsersAsync(IGetUsersV2Parameters parameters, ITwitterRequest request);
    }
}