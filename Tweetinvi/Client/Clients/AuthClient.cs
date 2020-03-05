using System.Threading.Tasks;
using Tweetinvi.Client.Requesters;
using Tweetinvi.Models;
using Tweetinvi.Parameters;

namespace Tweetinvi.Client
{
    public class AuthClient : IAuthClient
    {
        private readonly TwitterClient _client;
        private readonly IAuthRequester _authRequester;

        public AuthClient(TwitterClient client)
        {
            _client = client;
            _authRequester = client.RequestExecutor.Auth;
        }

        public Task<string> CreateBearerToken()
        {
            return CreateBearerToken(new CreateBearerTokenParameters());
        }

        public async Task<string> CreateBearerToken(ICreateBearerTokenParameters parameters)
        {
            var twitterResult = await _authRequester.CreateBearerToken(parameters).ConfigureAwait(false);
            return twitterResult?.DataTransferObject.AccessToken;
        }

        public async Task InitializeClientBearerToken()
        {
            var bearerToken = await CreateBearerToken().ConfigureAwait(false);

            _client.Credentials = new TwitterCredentials(_client.Credentials)
            {
                BearerToken = bearerToken
            };
        }

        public Task<IAuthenticationRequest> RequestAuthenticationUrl()
        {
            return RequestAuthenticationUrl(new RequestPinAuthUrlParameters());
        }

        public Task<IAuthenticationRequest> RequestAuthenticationUrl(string callbackUrl)
        {
            return RequestAuthenticationUrl(new RequestUrlAuthUrlParameters(callbackUrl));
        }

        public async Task<IAuthenticationRequest> RequestAuthenticationUrl(IRequestAuthUrlParameters parameters)
        {
            var twitterResult = await _authRequester.RequestAuthUrl(parameters).ConfigureAwait(false);
            return twitterResult?.DataTransferObject;
        }

        public async Task<ITwitterCredentials> RequestCredentials(IRequestCredentialsParameters parameters)
        {
            var twitterResult = await _authRequester.RequestAuthUrl(parameters).ConfigureAwait(false);
            return twitterResult?.DataTransferObject;
        }

        public Task<ITwitterCredentials> RequestCredentialsFromVerifierCode(string verifierCode, IAuthenticationRequest authenticationRequest)
        {
            return RequestCredentials(new RequestCredentialsParameters(verifierCode, authenticationRequest));
        }

        public Task<ITwitterCredentials> RequestCredentialsFromCallbackUrl(string callbackUrl, IAuthenticationRequest authenticationRequest)
        {
            return RequestCredentials(RequestCredentialsParameters.FromCallbackUrl(callbackUrl, authenticationRequest));
        }

        public Task InvalidateBearerToken()
        {
            return InvalidateBearerToken(new InvalidateBearerTokenParameters());
        }

        public Task InvalidateBearerToken(IInvalidateBearerTokenParameters parameters)
        {
            return _authRequester.InvalidateBearerToken(parameters);
        }

        public Task InvalidateAccessToken()
        {
            return InvalidateAccessToken(new InvalidateAccessTokenParameters());
        }

        public Task InvalidateAccessToken(IInvalidateAccessTokenParameters parameters)
        {
            return _authRequester.InvalidateAccessToken(parameters);
        }
    }
}