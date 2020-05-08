using System.Threading.Tasks;
using Tweetinvi.Core.Client.Validators;
using Tweetinvi.Models;
using Tweetinvi.Parameters.TrendsClient;

namespace Tweetinvi.Client
{
    public interface ITrendsClient
    {
        /// <summary>
        /// Validate all the TrendsClient parameters
        /// </summary>
        ITrendsClientParametersValidator ParametersValidator { get; }

        /// <inheritdoc cref="ITrendsClient.GetPlaceTrendsAtAsync(IGetTrendsAtParameters)"/>
        Task<IGetTrendsAtResult> GetPlaceTrendsAtAsync(long woeid);

        /// <summary>
        /// Returns the top 50 trending topics for a specific WOEID
        /// </summary>
        /// <para>Read more : https://developer.twitter.com/en/docs/trends/trends-for-location/api-reference/get-trends-place </para>
        /// <returns>Trending topics</returns>
        Task<IGetTrendsAtResult> GetPlaceTrendsAtAsync(IGetTrendsAtParameters parameters);

        /// <inheritdoc cref="ITrendsClient.GetTrendLocationsAsync(IGetTrendsLocationParameters)"/>
        Task<ITrendLocation[]> GetTrendLocationsAsync();

        /// <summary>
        /// Returns the locations that Twitter has trending topic information for.
        /// </summary>
        /// <para>Read more : https://developer.twitter.com/en/docs/trends/locations-with-trending-topics/api-reference/get-trends-available </para>
        /// <returns>Trending locations</returns>
        Task<ITrendLocation[]> GetTrendLocationsAsync(IGetTrendsLocationParameters parameters);

        /// <inheritdoc cref="ITrendsClient.GetTrendsLocationCloseToAsync(IGetTrendsLocationCloseToParameters)"/>
        Task<ITrendLocation[]> GetTrendsLocationCloseToAsync(double latitude, double longitude);
        /// <inheritdoc cref="ITrendsClient.GetTrendsLocationCloseToAsync(IGetTrendsLocationCloseToParameters)"/>
        Task<ITrendLocation[]> GetTrendsLocationCloseToAsync(ICoordinates coordinates);

        /// <summary>
        /// Returns the locations that Twitter has trending topic information for, closest to a specified location.
        /// </summary>
        /// <para>Read more : https://developer.twitter.com/en/docs/trends/locations-with-trending-topics/api-reference/get-trends-closest </para>
        /// <returns>Trending locations</returns>
        Task<ITrendLocation[]> GetTrendsLocationCloseToAsync(IGetTrendsLocationCloseToParameters parameters);
    }
}