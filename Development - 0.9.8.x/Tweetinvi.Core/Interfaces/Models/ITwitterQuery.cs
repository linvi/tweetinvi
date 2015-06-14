using System.Collections.Generic;
using Tweetinvi.Core.Enum;
using Tweetinvi.Core.Interfaces.Credentials;
using Tweetinvi.Core.Interfaces.WebLogic;

namespace Tweetinvi.Core.Interfaces.Models
{
    public interface ITwitterQueryFactory
    {
        ITwitterQuery Create(string queryURL, HttpMethod httpMethod = HttpMethod.GET);
        ITwitterQuery Create(string queryURL, HttpMethod httpMethod, IOAuthCredentials oAuthCredentials);
        ITwitterQuery Create(string queryURL, HttpMethod httpMethod, ITemporaryCredentials temporaryCredentials);
    }

    public interface ITwitterQuery
    {
        string QueryURL { get; set; }
        IOAuthCredentials OAuthCredentials { get; set; }
        ITemporaryCredentials TemporaryCredentials { get; set; }
        HttpMethod HttpMethod { get; set; }
        IEnumerable<IOAuthQueryParameter> QueryParameters { get; set; }

        ITwitterQuery Clone();
    }
}