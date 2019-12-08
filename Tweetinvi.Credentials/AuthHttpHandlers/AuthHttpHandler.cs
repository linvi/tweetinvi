using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Tweetinvi.Core.Web;
using Tweetinvi.Models;
using Tweetinvi.WebLogic;
using Tweetinvi.WebLogic.Utils;

namespace Tweetinvi.Credentials.AuthHttpHandlers
{
    public class AuthHttpHandler : TwitterClientHandler
    {
        private readonly IOAuthQueryParameter _queryParameter;
        private readonly IAuthenticationRequest _authRequest;

        public AuthHttpHandler(IOAuthQueryParameter queryParameter, IAuthenticationRequest authRequest, IOAuthWebRequestGenerator oAuthWebRequestGenerator) : base(oAuthWebRequestGenerator)
        {
            _queryParameter = queryParameter;
            _authRequest = authRequest;
        }

        protected override Task<HttpResponseMessage> SendAsync(ITwitterQuery twitterQuery, HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var headers = WebRequestGenerator.GenerateApplicationParameters(twitterQuery.TwitterCredentials, _authRequest, new[] { _queryParameter });
            twitterQuery.AuthorizationHeader = WebRequestGenerator.GenerateAuthorizationHeader(request.RequestUri, request.Method.ToTweetinviHttpMethod(), headers);

            return base.SendAsync(request, cancellationToken, twitterQuery.AuthorizationHeader);
        }
    }
}