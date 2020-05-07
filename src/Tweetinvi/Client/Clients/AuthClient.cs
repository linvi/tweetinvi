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

        public Task<string> CreateBearerTokenAsync()
        {
            return CreateBearerTokenAsync(new CreateBearerTokenParameters());
        }

        public async Task<string> CreateBearerTokenAsync(ICreateBearerTokenParameters parameters)
        {
            var twitterResult = await _authRequester.CreateBearerTokenAsync(parameters).ConfigureAwait(false);
            return twitterResult?.DataTransferObject.AccessToken;
        }

        public async Task InitializeClientBearerTokenAsync()
        {
            var bearerToken = await CreateBearerTokenAsync().ConfigureAwait(false);

            _client.Credentials = new TwitterCredentials(_client.Credentials)
            {
                BearerToken = bearerToken
            };
        }

        public Task<IAuthenticationRequest> RequestAuthenticationUrlAsync()
        {
            return RequestAuthenticationUrlAsync(new RequestPinAuthUrlParameters());
        }

        public Task<IAuthenticationRequest> RequestAuthenticationUrlAsync(string callbackUrl)
        {
            return RequestAuthenticationUrlAsync(new RequestUrlAuthUrlParameters(callbackUrl));
        }

        public Task<IAuthenticationRequest> RequestAuthenticationUrlAsync(Uri callbackUri)
        {
            return RequestAuthenticationUrlAsync(new RequestUrlAuthUrlParameters(callbackUri));
        }

        public async Task<IAuthenticationRequest> RequestAuthenticationUrlAsync(IRequestAuthUrlParameters parameters)
        {
            var twitterResult = await _authRequester.RequestAuthUrlAsync(parameters).ConfigureAwait(false);
            return twitterResult?.DataTransferObject;
        }

        public async Task<ITwitterCredentials> RequestCredentialsAsync(IRequestCredentialsParameters parameters)
        {
            var twitterResult = await _authRequester.RequestCredentialsAsync(parameters).ConfigureAwait(false);
            return twitterResult?.DataTransferObject;
        }

        public Task<ITwitterCredentials> RequestCredentialsFromVerifierCodeAsync(string verifierCode, IAuthenticationRequest authenticationRequest)
        {
            return RequestCredentialsAsync(new RequestCredentialsParameters(verifierCode, authenticationRequest));
        }

        public Task<ITwitterCredentials> RequestCredentialsFromCallbackUrlAsync(string callbackUrl, IAuthenticationRequest authenticationRequest)
        {
            return RequestCredentialsAsync(RequestCredentialsParameters.FromCallbackUrl(callbackUrl, authenticationRequest));
        }

        public Task<ITwitterCredentials> RequestCredentialsFromCallbackUrlAsync(Uri callbackUri, IAuthenticationRequest authenticationRequest)
        {
            return RequestCredentialsAsync(RequestCredentialsParameters.FromCallbackUrl(callbackUri, authenticationRequest));
        }

        public Task<InvalidateTokenResponse> InvalidateBearerTokenAsync()
        {
            return InvalidateBearerTokenAsync(new InvalidateBearerTokenParameters());
        }

        public async Task<InvalidateTokenResponse> InvalidateBearerTokenAsync(IInvalidateBearerTokenParameters parameters)
        {
            var twitterResult = await _authRequester.InvalidateBearerTokenAsync(parameters).ConfigureAwait(false);
            return twitterResult?.DataTransferObject;
        }

        public Task<InvalidateTokenResponse> InvalidateAccessTokenAsync()
        {
            return InvalidateAccessTokenAsync(new InvalidateAccessTokenParameters());
        }

        public async Task<InvalidateTokenResponse> InvalidateAccessTokenAsync(IInvalidateAccessTokenParameters parameters)
        {
            var twitterResult = await _authRequester.InvalidateAccessTokenAsync(parameters).ConfigureAwait(false);
            return twitterResult?.DataTransferObject;
        }
    }
}