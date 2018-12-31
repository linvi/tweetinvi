using System.Threading.Tasks;
using Tweetinvi.Models;

namespace Tweetinvi
{
    public static class RateLimitAsync
    {
        public static Task<ICredentialsRateLimits> GetCurrentCredentialsRateLimits(
            bool useRateLimitCache = false)
        {
            return Sync.ExecuteTaskAsync(() => RateLimit.GetCurrentCredentialsRateLimits(useRateLimitCache));
        }

        public static Task<ICredentialsRateLimits> GetCredentialsRateLimits(
            ITwitterCredentials credentials, bool useRateLimitCache = false)
        {
            return Sync.ExecuteTaskAsync(() => RateLimit.GetCredentialsRateLimits(credentials, useRateLimitCache));
        }
    }
}
