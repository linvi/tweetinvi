using Tweetinvi.Core.Enum;
using Tweetinvi.Core.Injectinvi;
using Tweetinvi.Core.Interfaces.Credentials;
using Tweetinvi.Core.Interfaces.Models;
using Tweetinvi.Core.Interfaces.WebLogic;

namespace Tweetinvi.Factories
{
    public class TwitterQueryFactory : ITwitterQueryFactory
    {
        private readonly IFactory<ITwitterQuery> _twitterQueryFactory;

        public TwitterQueryFactory(IFactory<ITwitterQuery> twitterQueryFactory)
        {
            _twitterQueryFactory = twitterQueryFactory;
        }

        public ITwitterQuery Create(string queryURL, HttpMethod httpMethod)
        {
            var queryURLParameter = new ConstructorNamedParameter("queryURL", queryURL);
            var httpMethodParameter = new ConstructorNamedParameter("httpMethod", httpMethod);

            return _twitterQueryFactory.Create(queryURLParameter, httpMethodParameter);
        }

        public ITwitterQuery Create(string queryURL, HttpMethod httpMethod, IOAuthCredentials oAuthCredentials)
        {
            var twitterQuery = Create(queryURL, httpMethod);
            twitterQuery.OAuthCredentials = oAuthCredentials;
            return twitterQuery;
        }

        public ITwitterQuery Create(string queryURL, HttpMethod httpMethod, ITemporaryCredentials temporaryCredentials)
        {
            var twitterQuery = Create(queryURL, httpMethod);
            twitterQuery.TemporaryCredentials = temporaryCredentials;
            return twitterQuery;
        }
    }
}
