using System.Threading.Tasks;
using Tweetinvi.Core.Web;
using Tweetinvi.Models;
using Tweetinvi.Models.V2;
using Tweetinvi.Parameters.V2;

namespace Tweetinvi.Core.Controllers.V2
{
    public interface IUsersV2Controller
    {
        Task<ITwitterResult<UserV2Response>> GetUserAsync(IGetUserByIdV2Parameters parameters, ITwitterRequest request);
        Task<ITwitterResult<UserV2Response>> GetUserAsync(IGetUserByUsernameV2Parameters parameters, ITwitterRequest request);

        Task<ITwitterResult<UsersV2Response>> GetUsersAsync(IGetUsersByIdV2Parameters parameters, ITwitterRequest request);
        Task<ITwitterResult<UsersV2Response>> GetUsersAsync(IGetUsersByUsernameV2Parameters parameters, ITwitterRequest request);
    }
}