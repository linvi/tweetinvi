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

        /// <summary>
        /// Get Twitter supported languages
        /// </summary>
        /// <para> https://developer.twitter.com/en/docs/developer-utilities/supported-languages/api-reference/get-help-languages </para>
        /// <returns>Twitter result containing the supported languages</returns>
        Task<ITwitterResult<SupportedLanguage[]>> GetSupportedLanguagesAsync(IGetSupportedLanguagesParameters parameters);

        /// <summary>
        /// Get a place information from place identifier.
        /// </summary>
        /// <para> https://developer.twitter.com/en/docs/geo/place-information/api-reference/get-geo-id-place_id </para>
        /// <returns>Twitter result containing the requested Place</returns>
        Task<ITwitterResult<IPlace>> GetPlaceAsync(IGetPlaceParameters parameters);

        /// <summary>
        /// Search for places that can be attached to a statuses/update. Given a latitude and a longitude pair, an IP address, or a name.
        /// </summary>
        /// <para> https://developer.twitter.com/en/docs/geo/places-near-location/api-reference/get-geo-search </para>
        /// <returns>Twitter result containing the places matching search</returns>
        Task<ITwitterResult<SearchGeoSearchResultDTO>> SearchGeoAsync(IGeoSearchParameters parameters);

        /// <summary>
        /// Given a latitude and a longitude, searches for up to 20 places.
        /// </summary>
        /// <para> https://developer.twitter.com/en/docs/geo/places-near-location/api-reference/get-geo-reverse_geocode </para>
        /// <returns>Twitter result containing the matching the search</returns>
        Task<ITwitterResult<SearchGeoSearchResultDTO>> SearchGeoReverseAsync(IGeoSearchReverseParameters parameters);
    }
}