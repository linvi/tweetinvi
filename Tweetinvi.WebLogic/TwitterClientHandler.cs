using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;
using Tweetinvi.Core.Extensions;
using Tweetinvi.Core.Web;
using Tweetinvi.Models;

namespace Tweetinvi.WebLogic
{
    public class TwitterClientHandler : HttpClientHandler, ITwitterClientHandler
    {
        private readonly Action<ITwitterQuery, HttpRequestMessage> _action;
        private readonly Func<ITwitterQuery, HttpRequestMessage, string> _func;
        protected IOAuthWebRequestGenerator WebRequestGenerator { get; }

        private ITwitterQuery _twitterQuery;

        public TwitterClientHandler(IOAuthWebRequestGenerator oAuthWebRequestGenerator)
        {
            UseCookies = false;
            UseDefaultCredentials = false;

            WebRequestGenerator = oAuthWebRequestGenerator;
        }

        public TwitterClientHandler(IOAuthWebRequestGenerator oAuthWebRequestGenerator, Action<ITwitterQuery, HttpRequestMessage> action) : this(oAuthWebRequestGenerator)
        {
            _action = action;
        }

        public TwitterClientHandler(IOAuthWebRequestGenerator oAuthWebRequestGenerator, Func<ITwitterQuery, HttpRequestMessage, string> func) : this(oAuthWebRequestGenerator)
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
                    Proxy = value.ProxyConfig;

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

        protected virtual async Task<HttpResponseMessage> SendAsync(ITwitterQuery twitterQuery, HttpRequestMessage request, CancellationToken cancellationToken)
        {
            _action?.Invoke(twitterQuery, request);

            if (twitterQuery.AuthorizationHeader == null)
            {
                if (_func != null)
                {
                    twitterQuery.AuthorizationHeader = _func(twitterQuery, request);
                }
                else
                {
                    await WebRequestGenerator.SetTwitterQueryAuthorizationHeader(twitterQuery).ConfigureAwait(false);
                }
            }

            return await SendAsync(request, cancellationToken, twitterQuery.AuthorizationHeader).ConfigureAwait(false);
        }

        protected Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken, string authorizationHeader)
        {
            request.Headers.Add("User-Agent", "Tweetinvi/4.0.2");
            request.Headers.ExpectContinue = false;
            request.Headers.CacheControl = new CacheControlHeaderValue { NoCache = true };
            request.Headers.Add("Authorization", authorizationHeader);
            request.Version = new Version("1.1");

            _twitterQuery?.AcceptHeaders.ForEach(contentType =>
            {
                request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue(contentType));
            });

            _twitterQuery?.CustomHeaders.ForEach(header =>
            {
                request.Headers.Add(header.Key, header.Value);
            });

            return base.SendAsync(request, cancellationToken);
        }
    }
}