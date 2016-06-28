using System.Collections.Generic;
using System.Net;
using Tweetinvi.Core.Credentials;
using Tweetinvi.Core.Web;
using Tweetinvi.Exceptions;
using Tweetinvi.Models;

namespace Tweetinvi.WebLogic
{
    public class TwitterRequestGenerator : ITwitterRequestGenerator
    {
        private readonly IOAuthWebRequestGenerator _webRequestGenerator;
        private readonly ICredentialsAccessor _credentialsAccessor;

        public TwitterRequestGenerator(
            IOAuthWebRequestGenerator webRequestGenerator,
            ICredentialsAccessor credentialsAccessor)
        {
            _webRequestGenerator = webRequestGenerator;
            _credentialsAccessor = credentialsAccessor;
        }

        public HttpWebRequest GetQueryWebRequest(ITwitterQuery twitterQuery)
        {
            var url = twitterQuery.QueryURL;
            var httpMethod = twitterQuery.HttpMethod;
            var queryParameters = GetCredentialsQueryParameters(twitterQuery);

            return GetQueryWebRequestInternal(url, httpMethod, queryParameters);
        }

        public IEnumerable<IOAuthQueryParameter> GetCredentialsQueryParameters(ITwitterQuery twitterQuery)
        {
            var queryParameters = twitterQuery.QueryParameters;

            if (twitterQuery.TwitterCredentials == null)
            {
                if (_credentialsAccessor.CurrentThreadCredentials == null)
                {
                    throw new TwitterNullCredentialsException();
                }

                twitterQuery.TwitterCredentials = _credentialsAccessor.CurrentThreadCredentials;
            }

            if (queryParameters == null)
            {
                queryParameters = _webRequestGenerator.GenerateParameters(twitterQuery.TwitterCredentials);
            }

            return queryParameters;
        }

        private HttpWebRequest GetQueryWebRequestInternal(string url, HttpMethod httpMethod, IEnumerable<IOAuthQueryParameter> headers = null)
        {
            return _webRequestGenerator.GenerateWebRequest(url, httpMethod, headers);
        }
    }
}