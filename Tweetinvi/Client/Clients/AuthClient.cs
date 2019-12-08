using System.Threading.Tasks;
using Tweetinvi.Auth;
using Tweetinvi.Client.Requesters;
using Tweetinvi.Models;
using Tweetinvi.Parameters.Auth;

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

        public async Task<string> CreateBearerToken()
        {
            var twitterResult = await _authRequester.CreateBearerToken().ConfigureAwait(false);
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

        public Task<IAuthenticationContext> RequestAuthenticationUrl()
        {
            return RequestAuthenticationUrl(new RequestPinAuthUrlParameters());
        }

        public Task<IAuthenticationContext> RequestAuthenticationUrl(string redirectUrl)
        {
            return RequestAuthenticationUrl(new RequestUrlAuthUrlParameters(redirectUrl));
        }

        public Task<IAuthenticationContext> RequestAuthenticationUrl(string redirectUrl, IAuthenticationTokenProvider authenticationTokenProvider)
        {
            return RequestAuthenticationUrl(new RequestUrlAuthUrlParameters(redirectUrl, authenticationTokenProvider));
        }

        public async Task<IAuthenticationContext> RequestAuthenticationUrl(IRequestAuthUrlParameters parameters)
        {
            var twitterResult = await _authRequester.RequestAuthUrl(parameters).ConfigureAwait(false);
            return twitterResult?.DataTransferObject;
        }

        public async Task<ITwitterCredentials> RequestCredentialsFromCallbackUrl(string callbackUrl, IAuthenticationTokenProvider authenticationTokenProvider)
        {
            var requestParameters = await RequestCredentialsParameters.FromCallbackUrl(callbackUrl, authenticationTokenProvider).ConfigureAwait(false);
            return await RequestCredentials(requestParameters).ConfigureAwait(false);
        }

        public Task<ITwitterCredentials> RequestCredentialsFromCallbackUrl(string callbackUrl, IAuthenticationContext authenticationContext)
        {
            return RequestCredentials(RequestCredentialsParameters.FromCallbackUrl(callbackUrl, authenticationContext));
        }

        public Task<ITwitterCredentials> RequestCredentialsFromCallbackUrl(string callbackUrl, IAuthenticationToken authenticationToken)
        {
            return RequestCredentials(RequestCredentialsParameters.FromCallbackUrl(callbackUrl, authenticationToken));
        }

        public Task<ITwitterCredentials> RequestCredentials(string verifierCode, IAuthenticationContext authenticationContext)
        {
            return RequestCredentials(new RequestCredentialsParameters(verifierCode, authenticationContext));
        }

        public Task<ITwitterCredentials> RequestCredentials(string verifierCode, IAuthenticationToken authenticationToken)
        {
            return RequestCredentials(new RequestCredentialsParameters(verifierCode, authenticationToken));
        }

        public async Task<ITwitterCredentials> RequestCredentials(IRequestCredentialsParameters parameters)
        {
            var twitterResult = await _authRequester.RequestCredentialsFromPinCode(parameters).ConfigureAwait(false);
            return twitterResult?.DataTransferObject;
        }


    }
}