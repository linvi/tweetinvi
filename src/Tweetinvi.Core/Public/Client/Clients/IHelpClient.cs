using System.Threading.Tasks;
using Tweetinvi.Core.Client.Validators;
using Tweetinvi.Core.Models;
using Tweetinvi.Models;
using Tweetinvi.Models.DTO;
using Tweetinvi.Parameters;

namespace Tweetinvi.Client
{
    public interface IHelpClient
    {
        /// <summary>
        /// Validate all the Help client parameters
        /// </summary>
        IHelpClientParametersValidator ParametersValidator { get; }

        /// <inheritdoc cref="IHelpClient.GetTwitterConfigurationAsync(IGetTwitterConfigurationParameters)"/>
        Task<ITwitterConfiguration> GetTwitterConfigurationAsync();

        /// <summary>
        /// Get the Twitter API configuration
        /// </summary>
        /// <para> https://developer.twitter.com/en/docs/developer-utilities/configuration/api-reference/get-help-configuration </para>
        /// <returns>Twitter official configuration</returns>
        Task<ITwitterConfiguration> GetTwitterConfigurationAsync(IGetTwitterConfigurationParameters parameters);

        /// <inheritdoc cref="IHelpClient.GetTwitterConfigurationAsync(IGetTwitterConfigurationParameters)"/>
        Task<SupportedLanguage[]> GetSupportedLanguagesAsync();

        /// <summary>
        /// Get Twitter supported languages
        /// </summary>
        /// <para> https://developer.twitter.com/en/docs/developer-utilities/supported-languages/api-reference/get-help-languages </para>
        /// <returns>Twitter supported languages</returns>
        Task<SupportedLanguage[]> GetSupportedLanguagesAsync(IGetSupportedLanguagesParameters parameters);

        /// <inheritdoc cref="IHelpClient.GetPlaceAsync(IGetPlaceParameters)"/>
        Task<IPlace> GetPlaceAsync(string placeId);

        /// <summary>
        /// Get a place information from place identifier.
        /// </summary>
        /// <para> https://developer.twitter.com/en/docs/geo/place-information/api-reference/get-geo-id-place_id </para>
        /// <returns>Requested Place</returns>
        Task<IPlace> GetPlaceAsync(IGetPlaceParameters parameters);

        /// <summary>
        /// Search for places that can be attached to a statuses/update. Given a latitude and a longitude pair, an IP address, or a name.
        /// </summary>
        /// <para> https://developer.twitter.com/en/docs/geo/places-near-location/api-reference/get-geo-search </para>
        /// <returns>Places matching search</returns>
        Task<IPlace[]> SearchGeoAsync(IGeoSearchParameters parameters);

        /// <inheritdoc cref="IHelpClient.SearchGeoReverseAsync(IGeoSearchReverseParameters)"/>
        Task<IPlace[]> SearchGeoReverseAsync(ICoordinates coordinates);

        /// <summary>
        /// Given a latitude and a longitude, searches for up to 20 places.
        /// </summary>
        /// <para> https://developer.twitter.com/en/docs/geo/places-near-location/api-reference/get-geo-reverse_geocode </para>
        /// <returns>Places matching the search</returns>
        Task<IPlace[]> SearchGeoReverseAsync(IGeoSearchReverseParameters parameters);
    }
}