using System.Threading.Tasks;
using Tweetinvi.Core.Controllers;
using Tweetinvi.Core.DTO;
using Tweetinvi.Core.Web;
using Tweetinvi.Models;

namespace Tweetinvi.Controllers.Auth
{
    public class AuthController : IAuthController
    {
        private readonly IAuthQueryExecutor _authQueryExecutor;

        public AuthController(IAuthQueryExecutor authQueryExecutor)
        {
            _authQueryExecutor = authQueryExecutor;
        }

        public Task<ITwitterResult<CreateTokenResponseDTO>> CreateBearerToken(ITwitterRequest request)
        {
            return _authQueryExecutor.CreateBearerToken(request);
        }
    }
}