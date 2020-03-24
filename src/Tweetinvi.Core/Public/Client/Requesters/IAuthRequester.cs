using System.Threading.Tasks;
using Tweetinvi.Core.DTO;
using Tweetinvi.Core.Web;
using Tweetinvi.Models;
using Tweetinvi.Parameters;

namespace Tweetinvi.Client.Requesters
{
    public interface IAuthRequester
    {
        /// <summary>
        /// Allows a registered application to obtain an OAuth 2 Bearer Token.
        /// Bearer token allows to make API requests on an application's own behalf, without a user context.
        /// This is called Application-only authentication.
        /// </summary>
        /// <param name="parameters"></param>
        /// <para> https://developer.twitter.com/en/docs/basics/authentication/api-reference/token </para>
        /// <returns>The bearer token to use for application only authentication</returns>
        Task<ITwitterResult<CreateTokenResponseDTO>> CreateBearerToken(ICreateBearerTokenParameters parameters);

        /// <summary>
        /// Initiates the authentication process for a user.
        /// The AuthenticationContext returned contains a url to authenticate on twitter.
        /// </summary>
        /// <para> https://developer.twitter.com/en/docs/basics/authentication/api-reference/request_token </para>
        /// <returns>An AuthenticationContext containing both the url to redirect to and an AuthenticationToken</returns>
        Task<ITwitterResult<IAuthenticationRequest>> RequestAuthUrl(IRequestAuthUrlParameters parameters);

        /// <summary>
        /// Request credentials with a verifierCode
        /// </summary>
        /// <para> https://developer.twitter.com/en/docs/basics/authentication/api-reference/token </para>
        /// <returns>The requested user credentials</returns>
        Task<ITwitterResult<ITwitterCredentials>> RequestAuthUrl(IRequestCredentialsParameters parameters);

        /// <summary>
        /// Invalidate bearer token
        /// </summary>
        /// <para> https://developer.twitter.com/en/docs/basics/authentication/api-reference/invalidate_bearer_token </para>
        /// <returns>Request result</returns>
        Task<ITwitterResult> InvalidateBearerToken(IInvalidateBearerTokenParameters parameters);

        /// <summary>
        /// Invalidate access token
        /// </summary>
        /// <para> https://developer.twitter.com/en/docs/basics/authentication/api-reference/invalidate_access_token </para>
        /// <returns>Request result</returns>
        Task<ITwitterResult> InvalidateAccessToken(IInvalidateAccessTokenParameters parameters);
    }
}