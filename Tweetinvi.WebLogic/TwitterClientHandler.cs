using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;
using Tweetinvi.Core;
using Tweetinvi.Core.Interfaces.Models;
using Tweetinvi.Core.Interfaces.WebLogic;
using Tweetinvi.Core.Web;

namespace Tweetinvi.WebLogic
{
    public class TwitterClientHandler : HttpClientHandler, ITwitterClientHandler
    {
        private readonly Action<ITwitterQuery, HttpRequestMessage> _action;
        private readonly Func<ITwitterQuery, HttpRequestMessage, string> _func;
        private readonly IWebProxyFactory _webProxyFactory;
        protected readonly IOAuthWebRequestGenerator _webRequestGenerator;

        private ITwitterQuery _twitterQuery;

        public TwitterClientHandler()
        {
            UseCookies = false;
            UseDefaultCredentials = false;
            

            _webRequestGenerator = TweetinviCoreModule.TweetinviContainer.Resolve<IOAuthWebRequestGenerator>();
            _webProxyFactory = TweetinviCoreModule.TweetinviContainer.Resolve<IWebProxyFactory>();
        }

        public TwitterClientHandler(Action<ITwitterQuery, HttpRequestMessage> action)
            : this()
        {
            _action = action;
        }

        public TwitterClientHandler(Func<ITwitterQuery, HttpRequestMessage, string> func)
            : this()
        {
            _func = func;
        }

        public ITwitterQuery TwitterQuery
        {
            get { return _twitterQuery; }
            set
            {
                _twitterQuery = value;

                if (value != null)
                {
                    Proxy = _webProxyFactory.GetProxy(value.Proxy);

                    if (Proxy != null)
                    {
                        UseProxy = true;
                    }
                }
                else
                {
                    Proxy = null;
                    UseProxy = false;
                }
            }
        }

        protected override sealed Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            return SendAsync(_twitterQuery, request, cancellationToken);
        }

        protected virtual Task<HttpResponseMessage> SendAsync(ITwitterQuery twitterQuery, HttpRequestMessage request, CancellationToken cancellationToken)
        {
            if (_action != null)
            {
                _action(twitterQuery, request);
            }

            string authorizationHeader;

            if (_func != null)
            {
                authorizationHeader = _func(twitterQuery, request);
            }
            else
            {
                var credentials = twitterQuery.TwitterCredentials;

                if (!string.IsNullOrEmpty(credentials.AccessToken) &&
                    !string.IsNullOrEmpty(credentials.AccessTokenSecret))
                {
                    var uri = new Uri(twitterQuery.QueryURL);
                    var credentialsParameters = _webRequestGenerator.GenerateParameters(twitterQuery.TwitterCredentials);
                    authorizationHeader = _webRequestGenerator.GenerateAuthorizationHeader(uri, twitterQuery.HttpMethod, credentialsParameters);
                }
                else
                {
                    authorizationHeader = string.Format("Bearer {0}", credentials.ApplicationOnlyBearerToken);
                }
            }

            return SendAsync(request, cancellationToken, authorizationHeader);
        }

        protected Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken, string authorizationHeader)
        {
            request.Headers.Add("User-Agent", "Tweetinvi/0.9.12.2");
            request.Headers.ExpectContinue = false;
            request.Headers.CacheControl = new CacheControlHeaderValue { NoCache = true };
            request.Headers.Add("Authorization", authorizationHeader);
            request.Version = new Version("1.1");

            return base.SendAsync(request, cancellationToken);
        }
    }
}