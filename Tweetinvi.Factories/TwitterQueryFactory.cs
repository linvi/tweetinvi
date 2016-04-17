using System;
using Tweetinvi.Core;
using Tweetinvi.Core.Authentication;
using Tweetinvi.Core.Enum;
using Tweetinvi.Core.Injectinvi;
using Tweetinvi.Core.Interfaces.Credentials;
using Tweetinvi.Core.Interfaces.Models;

namespace Tweetinvi.Factories
{
    public class TwitterQueryFactory : ITwitterQueryFactory
    {
        private readonly IFactory<ITwitterQuery> _twitterQueryFactory;
        private readonly ITweetinviSettingsAccessor _tweetinviSettingsAccessor;
        private readonly ICredentialsAccessor _credentialsAccessor;

        public TwitterQueryFactory(
            IFactory<ITwitterQuery> twitterQueryFactory,
            ITweetinviSettingsAccessor tweetinviSettingsAccessor,
            ICredentialsAccessor credentialsAccessor)
        {
            _twitterQueryFactory = twitterQueryFactory;
            _tweetinviSettingsAccessor = tweetinviSettingsAccessor;
            _credentialsAccessor = credentialsAccessor;
        }

        public ITwitterQuery Create(string queryURL, HttpMethod httpMethod, bool withThreadCredentials = false)
        {
            var uri = new Uri(queryURL);
            var queryURLParameter = new ConstructorNamedParameter("queryURL", uri.AbsoluteUri);
            var httpMethodParameter = new ConstructorNamedParameter("httpMethod", httpMethod);

            var twitterQuery = _twitterQueryFactory.Create(queryURLParameter, httpMethodParameter);

            twitterQuery.Proxy = _tweetinviSettingsAccessor.ProxyURL;
            twitterQuery.Timeout = TimeSpan.FromMilliseconds(_tweetinviSettingsAccessor.HttpRequestTimeout);

            if (withThreadCredentials)
            {
                twitterQuery.TwitterCredentials = _credentialsAccessor.CurrentThreadCredentials;
            }

            return twitterQuery;
        }

        public ITwitterQuery Create(string queryURL, HttpMethod httpMethod, ITwitterCredentials twitterCredentials)
        {
            var twitterQuery = Create(queryURL, httpMethod);
            twitterQuery.TwitterCredentials = twitterCredentials;
            return twitterQuery;
        }
    }
}
