using System;
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
            _authRequester = client.Raw.Auth;
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

        public Task<IAuthenticationRequest> RequestAuthenticationUrl(Uri callbackUri)
        {
            return RequestAuthenticationUrl(new RequestUrlAuthUrlParameters(callbackUri));
        }

        public async Task<IAuthenticationRequest> RequestAuthenticationUrl(IRequestAuthUrlParameters parameters)
        {
            var twitterResult = await _authRequester.RequestAuthUrl(parameters).ConfigureAwait(false);
            return twitterResult?.DataTransferObject;
        }

        public async Task<ITwitterCredentials> RequestCredentials(IRequestCredentialsParameters parameters)
        {
            var twitterResult = await _authRequester.RequestCredentials(parameters).ConfigureAwait(false);
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

        public Task<ITwitterCredentials> RequestCredentialsFromCallbackUrl(Uri callbackUri, IAuthenticationRequest authenticationRequest)
        {
            return RequestCredentials(RequestCredentialsParameters.FromCallbackUrl(callbackUri, authenticationRequest));
        }

        public Task<InvalidateTokenResponse> InvalidateBearerToken()
        {
            return InvalidateBearerToken(new InvalidateBearerTokenParameters());
        }

        public async Task<InvalidateTokenResponse> InvalidateBearerToken(IInvalidateBearerTokenParameters parameters)
        {
            var twitterResult = await _authRequester.InvalidateBearerToken(parameters).ConfigureAwait(false);
            return twitterResult?.DataTransferObject;
        }

        public Task<InvalidateTokenResponse> InvalidateAccessToken()
        {
            return InvalidateAccessToken(new InvalidateAccessTokenParameters());
        }

        public async Task<InvalidateTokenResponse> InvalidateAccessToken(IInvalidateAccessTokenParameters parameters)
        {
            var twitterResult = await _authRequester.InvalidateAccessToken(parameters).ConfigureAwait(false);
            return twitterResult?.DataTransferObject;
        }
    }
}