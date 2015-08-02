using System;
using System.Collections.Generic;
using Tweetinvi.Core.Credentials;
using Tweetinvi.Core.Enum;
using Tweetinvi.Core.Interfaces.Models;
using Tweetinvi.Core.Interfaces.WebLogic;

namespace Tweetinvi.Logic
{
    public class TwitterQuery : ITwitterQuery
    {
        public TwitterQuery(string queryURL, HttpMethod httpMethod)
        {
            QueryURL = queryURL;
            HttpMethod = httpMethod;
        }

        public string QueryURL { get; set; }
        public HttpMethod HttpMethod { get; set; }

        public string Proxy { get; set; }

        private TimeSpan? _timeout;

        public TimeSpan? Timeout
        {
            get
            {
                return _timeout;
            }
            set
            {
                _timeout = value;
            }
        }

        public ITwitterCredentials TwitterCredentials { get; set; }
        public IEnumerable<IOAuthQueryParameter> QueryParameters { get; set; }

        public ITwitterQuery Clone()
        {
            var clone = new TwitterQuery(QueryURL, HttpMethod)
            {
                TwitterCredentials = TwitterCredentials, 
                QueryParameters = QueryParameters
            };

            return clone;
        }
    }
}