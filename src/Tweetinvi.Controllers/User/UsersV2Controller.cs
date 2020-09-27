using System.Threading.Tasks;
using Tweetinvi.Core.Controllers.V2;
using Tweetinvi.Core.Web;
using Tweetinvi.Models;
using Tweetinvi.Models.V2.Responses;
using Tweetinvi.Parameters.V2;

namespace Tweetinvi.Controllers.User
{
    public class UsersV2Controller : IUsersV2Controller
    {
        private readonly IUsersV2QueryExecutor _queryExecutor;

        public UsersV2Controller(IUsersV2QueryExecutor queryExecutor)
        {
            _queryExecutor = queryExecutor;
        }

        public Task<ITwitterResult<UserResponseDTO>> GetUserAsync(IGetUserV2Parameters parameters, ITwitterRequest request)
        {
            return _queryExecutor.GetUserAsync(parameters, request);
        }

        public Task<ITwitterResult<UsersResponseDTO>> GetUsersAsync(IGetUsersV2Parameters parameters, ITwitterRequest request)
        {
            return _queryExecutor.GetUsersAsync(parameters, request);
        }
    }
}