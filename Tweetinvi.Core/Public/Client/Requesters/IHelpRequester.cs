using System.Threading.Tasks;
using Tweetinvi.Core.Web;
using Tweetinvi.Models;
using Tweetinvi.Parameters.HelpClient;

namespace Tweetinvi.Client.Requesters
{
    public interface IHelpRequester
    {
        /// <summary>
        /// Get the rate limits of the current client
        /// </summary>
        /// <para> Read more : https://developer.twitter.com/en/docs/developer-utilities/rate-limit-status/api-reference/get-application-rate_limit_status </para>
        /// <returns>The twitter response containing the client's rate limits</returns>
        Task<ITwitterResult<ICredentialsRateLimits>> GetRateLimits(IGetRateLimitsParameters parameters);
    }
}