using System.Threading.Tasks;
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

        public Task<IAuthenticationContext> StartAuthProcess()
        {
            return StartAuthProcess(new StartPinAuthProcessParameters());
        }

        public Task<IAuthenticationContext> StartAuthProcess(string redirectUrl)
        {
            return StartAuthProcess(new StartUrlAuthProcessParameters(redirectUrl));
        }

        public async Task<IAuthenticationContext> StartAuthProcess(IStartAuthProcessParameters parameters)
        {
            var twitterResult = await _authRequester.StartAuthProcess(parameters).ConfigureAwait(false);
            return twitterResult?.DataTransferObject;
        }
    }
}