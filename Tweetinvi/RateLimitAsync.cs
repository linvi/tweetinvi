using System.Threading.Tasks;
using Tweetinvi.Models;

namespace Tweetinvi
{
    public static class RateLimitAsync
    {
        public static async Task<ICredentialsRateLimits> GetCurrentCredentialsRateLimits()
        {
            return await Sync.ExecuteTaskAsync(() => RateLimit.GetCurrentCredentialsRateLimits());
        }

        public static async Task<ICredentialsRateLimits> GetCredentialsRateLimits(ITwitterCredentials credentials)
        {
            return await Sync.ExecuteTaskAsync(() => RateLimit.GetCredentialsRateLimits(credentials));
        }
    }
}
