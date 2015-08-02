using Tweetinvi.Core.Credentials;
using Tweetinvi.Core.Interfaces.WebLogic;

namespace Tweetinvi.Core.Interfaces.RateLimit
{
    public interface IRateLimitUpdater
    {
        void QueryExecuted(string query, int numberOfRequests = 1);
        void QueryExecuted(string query, ITwitterCredentials credentials, int numberOfRequests = 1);
        void ClearRateLimitsForQuery(string query);
    }
}