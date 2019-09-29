using System;
using System.Collections.Generic;
using System.Linq;
using Tweetinvi.Core.Web;
using Tweetinvi.Models;
using HttpMethod = Tweetinvi.Models.HttpMethod;

namespace Tweetinvi
{
    public class TwitterQuery : TwitterRequestParameters, ITwitterQuery
    {
        public TwitterQuery()
        {
            _timeout = TimeSpan.FromSeconds(10);

            AcceptHeaders = new List<string>
            {
                "image/jpeg",
                "application/json"
            };

            HttpMethod = HttpMethod.GET;
            CustomHeaders = new Dictionary<string, string>();
        }

        public TwitterQuery(string queryURL, HttpMethod httpMethod) : this()
        {
            Url = queryURL;
            HttpMethod = httpMethod;
        }

        public TwitterQuery(ITwitterQuery source) : base(source)
        {
            if (source == null)
            {
                return;
            }

            ProxyConfig = source.ProxyConfig;
            Timeout = source.Timeout;
            QueryParameters = source.QueryParameters?.ToArray();
            TwitterCredentials = source.TwitterCredentials;
            CredentialsRateLimits = source.CredentialsRateLimits;
            QueryRateLimit = source.QueryRateLimit;
            DateWhenCredentialsWillHaveTheRequiredRateLimits = source.DateWhenCredentialsWillHaveTheRequiredRateLimits;
        }

        public IProxyConfig ProxyConfig { get; set; }
        private TimeSpan _timeout;
        public TimeSpan Timeout
        {
            get => _timeout;
            set
            {
                if ((int)value.TotalMilliseconds == 0) // Default
                {
                    _timeout = TimeSpan.FromSeconds(10);
                    return;
                }

                if (value.TotalMilliseconds < 0) // Infinite
                {
                    _timeout = TimeSpan.FromMilliseconds(System.Threading.Timeout.Infinite);
                    return;
                }

                _timeout = value;
            }
        }
        public ITwitterCredentials TwitterCredentials { get; set; }
        public IOAuthQueryParameter[] QueryParameters { get; set; }
        public IEndpointRateLimit QueryRateLimit { get; set; }
        public ICredentialsRateLimits CredentialsRateLimits { get; set; }
        public DateTime? DateWhenCredentialsWillHaveTheRequiredRateLimits { get; set; }
        public int? TimeToWaitBeforeExecutingTheQueryInMilliSeconds
        {
            get
            {
                if (DateWhenCredentialsWillHaveTheRequiredRateLimits == null)
                {
                    return null;
                }

                var timeToWait = DateWhenCredentialsWillHaveTheRequiredRateLimits.Value.Subtract(DateTime.Now).TotalMilliseconds;
                return (int)Math.Max(0, timeToWait);
            }
        }
        public override string ToString()
        {
            return Url;
        }
    }
}