using System.Threading.Tasks;
using Tweetinvi.Client.Requesters;
using Tweetinvi.Models;

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
    }
}