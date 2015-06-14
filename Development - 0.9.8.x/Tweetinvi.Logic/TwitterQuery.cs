using System.Collections.Generic;
using Tweetinvi.Core.Enum;
using Tweetinvi.Core.Interfaces.Credentials;
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
        public IEnumerable<IOAuthQueryParameter> QueryParameters { get; set; }
        public IOAuthCredentials OAuthCredentials { get; set; }
        public ITemporaryCredentials TemporaryCredentials { get; set; }

        public ITwitterQuery Clone()
        {
            var clone = new TwitterQuery(QueryURL, HttpMethod)
            {
                QueryParameters = QueryParameters, 
                OAuthCredentials = OAuthCredentials, 
                TemporaryCredentials = TemporaryCredentials
            };

            return clone;
        }
    }
}