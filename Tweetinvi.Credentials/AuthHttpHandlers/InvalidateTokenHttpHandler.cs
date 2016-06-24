using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Tweetinvi.Models;
using Tweetinvi.WebLogic;

namespace Tweetinvi.Credentials.AuthHttpHandlers
{
    public class InvalidateTokenHttpHandler : TwitterClientHandler
    {
        protected override Task<HttpResponseMessage> SendAsync(ITwitterQuery twitterQuery, HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var credentials = twitterQuery.TwitterCredentials;
            var accessToken = credentials.AccessToken ?? credentials.ApplicationOnlyBearerToken;
            request.Content = new StringContent("access_token=" + accessToken, Encoding.UTF8, "application/x-www-form-urlencoded");

            if (credentials.AccessToken != null)
            {
                return base.SendAsync(twitterQuery, request, cancellationToken);
            }
            
            var authorizationHeader = BearerHttpHandler.GetBearerTokenAuthorizationHeader(credentials);
            return base.SendAsync(request, cancellationToken, authorizationHeader);
        }
    }
}