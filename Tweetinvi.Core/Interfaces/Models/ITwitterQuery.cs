using System;
using System.Collections.Generic;
using Tweetinvi.Core.Credentials;
using Tweetinvi.Core.Enum;
using Tweetinvi.Core.Interfaces.WebLogic;

namespace Tweetinvi.Core.Interfaces.Models
{
    public interface ITwitterQueryFactory
    {
        ITwitterQuery Create(string queryURL, HttpMethod httpMethod = HttpMethod.GET, bool withThreadCredentials = false);
        ITwitterQuery Create(string queryURL, HttpMethod httpMethod, ITwitterCredentials twitterCredentials);
    }

    public interface ITwitterQuery
    {
        string QueryURL { get; set; }
        HttpMethod HttpMethod { get; set; }
        
        string Proxy { get; set; }
        TimeSpan Timeout { get; set; }

        ITwitterCredentials TwitterCredentials { get; set; }
        IEnumerable<IOAuthQueryParameter> QueryParameters { get; set; }

        DateTime? DateWhenCredentialsWillHaveRequiredRateLimits { get; set; }
        int? TimeToWaitBeforeExecutingTheQueryInMilliSeconds { get; }

        ITwitterQuery Clone();
    }
}