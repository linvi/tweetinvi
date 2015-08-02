using System;
using Tweetinvi.Core;
using Tweetinvi.Core.Credentials;
using Tweetinvi.Core.Enum;
using Tweetinvi.Core.Injectinvi;
using Tweetinvi.Core.Interfaces.Models;
using Tweetinvi.Core.Interfaces.WebLogic;

namespace Tweetinvi.Factories
{
    public class TwitterQueryFactory : ITwitterQueryFactory
    {
        private readonly IFactory<ITwitterQuery> _twitterQueryFactory;
        private readonly ITweetinviSettingsAccessor _tweetinviSettingsAccessor;

        public TwitterQueryFactory(
            IFactory<ITwitterQuery> twitterQueryFactory,
            ITweetinviSettingsAccessor tweetinviSettingsAccessor)
        {
            _twitterQueryFactory = twitterQueryFactory;
            _tweetinviSettingsAccessor = tweetinviSettingsAccessor;
        }

        public ITwitterQuery Create(string queryURL, HttpMethod httpMethod)
        {
            var queryURLParameter = new ConstructorNamedParameter("queryURL", queryURL);
            var httpMethodParameter = new ConstructorNamedParameter("httpMethod", httpMethod);

            var twitterQuery = _twitterQueryFactory.Create(queryURLParameter, httpMethodParameter);

            twitterQuery.Proxy = _tweetinviSettingsAccessor.ProxyURL;
            twitterQuery.Timeout = TimeSpan.FromMilliseconds(_tweetinviSettingsAccessor.WebRequestTimeout);

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
