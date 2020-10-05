using System.Threading.Tasks;
using Tweetinvi.Core.Web;
using Tweetinvi.Models.V2;
using Tweetinvi.Parameters.V2;

namespace Tweetinvi.Client.Requesters.V2
{
    public interface IUsersV2Requester
    {
        Task<ITwitterResult<UserV2Response>> GetUserAsync(IGetUserByIdV2Parameters parameters);
        Task<ITwitterResult<UserV2Response>> GetUserAsync(IGetUserByNameV2Parameters parameters);

        Task<ITwitterResult<UsersV2Response>> GetUsersAsync(IGetUsersByIdV2Parameters parameters);
        Task<ITwitterResult<UsersV2Response>> GetUsersAsync(IGetUsersByNameV2Parameters parameters);
    }
}