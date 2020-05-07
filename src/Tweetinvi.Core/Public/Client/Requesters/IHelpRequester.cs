using System.Threading.Tasks;
using Tweetinvi.Core.DTO;
using Tweetinvi.Core.Models;
using Tweetinvi.Core.Web;
using Tweetinvi.Models;
using Tweetinvi.Models.DTO;
using Tweetinvi.Parameters;

namespace Tweetinvi.Client.Requesters
{
    public interface IHelpRequester
    {
        /// <summary>
        /// Get the rate limits of the current client
        /// </summary>
        /// <para> Read more : https://developer.twitter.com/en/docs/developer-utilities/rate-limit-status/api-reference/get-application-rate_limit_status </para>
        /// <returns>The twitter response containing the client's rate limits</returns>
        Task<ITwitterResult<CredentialsRateLimitsDTO>> GetRateLimitsAsync(IGetRateLimitsParameters parameters);

        /// <summary>
        /// Get the Twitter API configuration
        /// </summary>
        /// <para> https://developer.twitter.com/en/docs/developer-utilities/configuration/api-reference/get-help-configuration </para>
        /// <returns>Twitter response containing the official configuration</returns>
        Task<ITwitterResult<ITwitterConfiguration>> GetTwitterConfigurationAsync(IGetTwitterConfigurationParameters parameters);

        Task<ITwitterResult<SupportedLanguage[]>> GetSupportedLanguagesAsync(IGetSupportedLanguagesParameters parameters);

        Task<ITwitterResult<IPlace>> GetPlaceAsync(IGetPlaceParameters parameters);
        Task<ITwitterResult<SearchGeoSearchResultDTO>> SearchGeoAsync(IGeoSearchParameters parameters);
        Task<ITwitterResult<SearchGeoSearchResultDTO>> SearchGeoReverseAsync(IGeoSearchReverseParameters parameters);
    }
}