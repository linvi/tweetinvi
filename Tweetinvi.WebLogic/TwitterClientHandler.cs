using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;
using Tweetinvi.Core.Extensions;
using Tweetinvi.Core.Injectinvi;
using Tweetinvi.Core.Web;
using Tweetinvi.Models;

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

        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            return SendAsync(_twitterQuery, request, cancellationToken);
        }

        protected virtual Task<HttpResponseMessage> SendAsync(ITwitterQuery twitterQuery, HttpRequestMessage request, CancellationToken cancellationToken)
        {
            if (_action != null)
            {
                _action(twitterQuery, request);
            }

            if (twitterQuery.AuthorizationHeader == null)
            {
                if (_func != null)
                {
                    twitterQuery.AuthorizationHeader = _func(twitterQuery, request);
                }
                else
                {
                    _webRequestGenerator.SetTwitterQueryAuthorizationHeader(twitterQuery);
                }
            }

            return SendAsync(request, cancellationToken, twitterQuery.AuthorizationHeader);
        }

        protected Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken, string authorizationHeader)
        {
            request.Headers.Add("User-Agent", "Tweetinvi/1.3.0.0");
            request.Headers.ExpectContinue = false;
            request.Headers.CacheControl = new CacheControlHeaderValue { NoCache = true };
            request.Headers.Add("Authorization", authorizationHeader);
            request.Version = new Version("1.1");

            _twitterQuery?.AcceptHeaders.ForEach(contentType =>
            {
                request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue(contentType));
            });

            return base.SendAsync(request, cancellationToken);
        }
    }
}