using System.Threading.Tasks;
using Tweetinvi.Core.Credentials;
using Tweetinvi.Core.Interfaces.Credentials;

namespace Tweetinvi
{
    public static class RateLimitAsync
    {
        public static async Task<ITokenRateLimits> GetCurrentCredentialsRateLimits()
        {
            return await Sync.ExecuteTaskAsync(() => RateLimit.GetCurrentCredentialsRateLimits());
        }

        public static async Task<ITokenRateLimits> GetCredentialsRateLimits(ITwitterCredentials credentials)
        {
            return await Sync.ExecuteTaskAsync(() => RateLimit.GetCredentialsRateLimits(credentials));
        }
    }
}
