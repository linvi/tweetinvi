using System.Threading.Tasks;
using Tweetinvi.Core.Interfaces.Credentials;
using Tweetinvi.Core.Interfaces.WebLogic;

namespace Tweetinvi
{
    public static class RateLimitAsync
    {
        public static async Task<ITokenRateLimits> GetCurrentCredentialsRateLimits()
        {
            return await Sync.ExecuteTaskAsync(() => RateLimit.GetCurrentCredentialsRateLimits());
        }

        public static async Task<ITokenRateLimits> GetCredentialsRateLimits(IOAuthCredentials credentials)
        {
            return await Sync.ExecuteTaskAsync(() => RateLimit.GetCredentialsRateLimits(credentials));
        }
    }
}
