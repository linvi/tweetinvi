using Tweetinvi.Core.Interfaces.Credentials;
using Tweetinvi.Core.Interfaces.WebLogic;

namespace Tweetinvi.Core.Interfaces.RateLimit
{
    public interface IRateLimitCacheManager
    {
        ITokenRateLimit GetQueryRateLimit(string query, IOAuthCredentials credentials);
        ITokenRateLimits GetTokenRateLimits(IOAuthCredentials credentials);

        void UpdateTokenRateLimits(IOAuthCredentials credentials, ITokenRateLimits tokenRateLimits);
    }
}