using System.Threading.Tasks;
using Tweetinvi.Core.DTO;
using Tweetinvi.Core.Web;
using Tweetinvi.Models;
using Tweetinvi.Parameters;

namespace Tweetinvi.Core.Controllers
{
    public interface IAuthController
    {
        Task<ITwitterResult<CreateTokenResponseDTO>> CreateBearerTokenAsync(ICreateBearerTokenParameters parameters, ITwitterRequest request);
        Task<ITwitterResult<IAuthenticationRequest>> RequestAuthUrlAsync(IRequestAuthUrlParameters parameters, ITwitterRequest request);
        Task<ITwitterResult<ITwitterCredentials>> RequestCredentialsAsync(IRequestCredentialsParameters parameters, ITwitterRequest request);
        Task<ITwitterResult<InvalidateTokenResponse>> InvalidateAccessTokenAsync(IInvalidateAccessTokenParameters parameters, ITwitterRequest request);
        Task<ITwitterResult<InvalidateTokenResponse>> InvalidateBearerTokenAsync(IInvalidateBearerTokenParameters parameters, ITwitterRequest request);
    }
}