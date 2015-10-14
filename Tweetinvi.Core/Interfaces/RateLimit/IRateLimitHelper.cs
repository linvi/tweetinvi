using Tweetinvi.Core.Interfaces.Credentials;

namespace Tweetinvi.Core.Interfaces.RateLimit
{
    public interface IRateLimitHelper
    {
        bool IsQueryAssociatedWithTokenRateLimit(string query, ITokenRateLimits rateLimits);
        ITokenRateLimit GetTokenRateLimitFromQuery(string query, ITokenRateLimits rateLimits);
    }
}