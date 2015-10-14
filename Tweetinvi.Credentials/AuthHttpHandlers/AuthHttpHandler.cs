using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Tweetinvi.Core.Interfaces.Models;
using Tweetinvi.Core.Interfaces.WebLogic;
using Tweetinvi.WebLogic;
using Tweetinvi.WebLogic.Utils;

namespace Tweetinvi.Credentials.AuthHttpHandlers
{
    public class AuthHttpHandler : TwitterClientHandler
    {
        private readonly IOAuthQueryParameter _queryParameter;

        public AuthHttpHandler(IOAuthQueryParameter queryParameter)
        {
            _queryParameter = queryParameter;
        }

        protected override Task<HttpResponseMessage> SendAsync(ITwitterQuery twitterQuery, HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var headers = _webRequestGenerator.GenerateApplicationParameters(twitterQuery.TwitterCredentials, new[] { _queryParameter });
            var authorizatinHeader = _webRequestGenerator.GenerateAuthorizationHeader(request.RequestUri, request.Method.ToTweetinviHttpMethod(), headers);

            return base.SendAsync(request, cancellationToken, authorizatinHeader);
        }
    }
}