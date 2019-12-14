using System.Threading.Tasks;
using Tweetinvi.Models;
using Tweetinvi.Parameters.HelpClient;

namespace Tweetinvi.Client
{
    public interface IRateLimitsClient
    {
        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        Task InitializeRateLimitsManager();

        /// <inheritdoc cref="GetRateLimits(IGetRateLimitsParameters)" />
        Task<ICredentialsRateLimits> GetRateLimits();

        /// <inheritdoc cref="GetRateLimits(IGetRateLimitsParameters)" />
        Task<ICredentialsRateLimits> GetRateLimits(RateLimitsSource from);

        /// <summary>
        /// Get the rate limits of the current client
        /// </summary>
        /// <para> Read more : https://developer.twitter.com/en/docs/developer-utilities/rate-limit-status/api-reference/get-application-rate_limit_status </para>
        /// <returns>The client's rate limits</returns>
        Task<ICredentialsRateLimits> GetRateLimits(IGetRateLimitsParameters parameters);
    }
}