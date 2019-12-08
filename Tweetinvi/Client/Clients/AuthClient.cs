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

        public Task<IAuthenticationRequestToken> RequestAuthenticationUrl()
        {
            return RequestAuthenticationUrl(new RequestPinAuthUrlParameters());
        }

        public Task<IAuthenticationRequestToken> RequestAuthenticationUrl(string redirectUrl)
        {
            return RequestAuthenticationUrl(new RequestUrlAuthUrlParameters(redirectUrl));
        }

        public Task<IAuthenticationRequestToken> RequestAuthenticationUrl(string redirectUrl, IAuthenticationTokenProvider authenticationTokenProvider)
        {
            return RequestAuthenticationUrl(new RequestUrlAuthUrlParameters(redirectUrl, authenticationTokenProvider));
        }

        public async Task<IAuthenticationRequestToken> RequestAuthenticationUrl(IRequestAuthUrlParameters parameters)
        {
            var twitterResult = await _authRequester.RequestAuthUrl(parameters).ConfigureAwait(false);
            return twitterResult?.DataTransferObject;
        }

        public async Task<ITwitterCredentials> RequestCredentialsFromCallbackUrl(string callbackUrl, IAuthenticationTokenProvider authenticationTokenProvider)
        {
            var requestParameters = await RequestCredentialsParameters.FromCallbackUrl(callbackUrl, authenticationTokenProvider).ConfigureAwait(false);
            return await RequestCredentials(requestParameters).ConfigureAwait(false);
        }

        public Task<ITwitterCredentials> RequestCredentialsFromCallbackUrl(string callbackUrl, IAuthenticationRequestToken authenticationRequestToken)
        {
            return RequestCredentials(RequestCredentialsParameters.FromCallbackUrl(callbackUrl, authenticationRequestToken));
        }

        public Task<ITwitterCredentials> RequestCredentials(string verifierCode, IAuthenticationRequestToken authenticationRequestToken)
        {
            return RequestCredentials(new RequestCredentialsParameters(verifierCode, authenticationRequestToken));
        }

        public async Task<ITwitterCredentials> RequestCredentials(IRequestCredentialsParameters parameters)
        {
            var twitterResult = await _authRequester.RequestCredentialsFromPinCode(parameters).ConfigureAwait(false);
            return twitterResult?.DataTransferObject;
        }


    }
}