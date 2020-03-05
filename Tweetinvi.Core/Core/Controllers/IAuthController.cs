using System.Threading.Tasks;
using Tweetinvi.Core.DTO;
using Tweetinvi.Core.Web;
using Tweetinvi.Models;
using Tweetinvi.Parameters;

namespace Tweetinvi.Core.Controllers
{
    public interface IAuthController
    {
        Task<ITwitterResult<CreateTokenResponseDTO>> CreateBearerToken(ICreateBearerTokenParameters parameters, ITwitterRequest request);
        Task<ITwitterResult<IAuthenticationRequest>> RequestAuthUrl(IRequestAuthUrlParameters parameters, ITwitterRequest request);
        Task<ITwitterResult<ITwitterCredentials>> RequestCredentials(IRequestCredentialsParameters parameters, ITwitterRequest request);
        Task<ITwitterResult> InvalidateAccessToken(IInvalidateAccessTokenParameters parameters, ITwitterRequest request);
        Task<ITwitterResult> InvalidateBearerToken(IInvalidateBearerTokenParameters parameters, ITwitterRequest request);
    }
}