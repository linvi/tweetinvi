using System;
using Tweetinvi.Models;

namespace Tweetinvi.Events
{
    public class WaitingForRateLimitEventArgs : EventArgs
    {
        private readonly string _url;
        private readonly IEndpointRateLimit _queryRateLimit;
        private readonly IReadOnlyTwitterCredentials _twitterCredentials;

        public WaitingForRateLimitEventArgs(
            string url,
            IEndpointRateLimit queryRateLimit,
            IReadOnlyTwitterCredentials twitterCredentials)
        {
            _url = url;
            _queryRateLimit = queryRateLimit;
            _twitterCredentials = twitterCredentials;
        }

        public WaitingForRateLimitEventArgs(
            ITwitterRequest request,
            IEndpointRateLimit queryRateLimit)
        {
            Request = request;
            _queryRateLimit = queryRateLimit;
        }

        public string Url => Request?.Query?.Url ?? _url;
        public IReadOnlyTwitterCredentials Credentials => Request?.Query?.TwitterCredentials ?? _twitterCredentials;
        public IEndpointRateLimit QueryRateLimit => _queryRateLimit;
        public ITwitterRequest Request { get; }
        public DateTime ResetDateTime => _queryRateLimit.ResetDateTime;
        public int ResetInMilliseconds => (int)_queryRateLimit.ResetDateTimeInMilliseconds;
    }
}