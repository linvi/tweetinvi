using System.Runtime.CompilerServices;
using Tweetinvi.Models;

namespace Tweetinvi
{
    public static class RateLimitAsync
    {
        public static ConfiguredTaskAwaitable<ICredentialsRateLimits> GetCurrentCredentialsRateLimits(
            bool useRateLimitCache = false)
        {
            return Sync.ExecuteTaskAsync(() => RateLimit.GetCurrentCredentialsRateLimits(useRateLimitCache));
        }

        public static ConfiguredTaskAwaitable<ICredentialsRateLimits> GetCredentialsRateLimits(
            ITwitterCredentials credentials, bool useRateLimitCache = false)
        {
            return Sync.ExecuteTaskAsync(() => RateLimit.GetCredentialsRateLimits(credentials, useRateLimitCache));
        }
    }
}
