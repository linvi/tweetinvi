using System.Threading.Tasks;
using Tweetinvi.Core.Credentials;
using Tweetinvi.Core.DTO;
using Tweetinvi.Core.Web;
using Tweetinvi.Models;
using Tweetinvi.Parameters.Auth;

namespace Tweetinvi.Core.Controllers
{
    public interface IAuthController
    {
        Task<ITwitterResult<CreateTokenResponseDTO>> CreateBearerToken(ITwitterRequest request);
        Task<ITwitterResult<IAuthenticationContext>> RequestAuthUrl(IRequestAuthUrlParameters parameters, ITwitterRequest request);
        Task<ITwitterResult<ITwitterCredentials>> RequestCredentials(IRequestCredentialsParameters parameters, ITwitterRequest request);
    }
}