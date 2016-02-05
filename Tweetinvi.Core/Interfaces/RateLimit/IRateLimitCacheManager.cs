using Tweetinvi.Core.Credentials;
using Tweetinvi.Core.Interfaces.Credentials;

namespace Tweetinvi.Core.Interfaces.RateLimit
{
    public interface IRateLimitCacheManager
    {
        ITokenRateLimit GetQueryRateLimit(string query, ITwitterCredentials credentials);
        ITokenRateLimits GetTokenRateLimits(ITwitterCredentials credentials);

        void UpdateTokenRateLimits(ITwitterCredentials credentials, ITokenRateLimits tokenRateLimits);
    }
}