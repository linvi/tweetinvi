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

        /// <inheritdoc cref="GetPlaceTrendsAt(IGetTrendsAtParameters)"/>
        Task<IGetTrendsAtResult> GetPlaceTrendsAt(long woeid);

        /// <summary>
        /// Returns the top 50 trending topics for a specific WOEID
        /// </summary>
        /// <para>Read more : https://developer.twitter.com/en/docs/trends/trends-for-location/api-reference/get-trends-place </para>
        /// <returns>Trending topics</returns>
        Task<IGetTrendsAtResult> GetPlaceTrendsAt(IGetTrendsAtParameters parameters);

        /// <inheritdoc cref="GetTrendLocations(IGetTrendsLocationParameters)"/>
        Task<ITrendLocation[]> GetTrendLocations();

        /// <summary>
        /// Returns the locations that Twitter has trending topic information for.
        /// </summary>
        /// <para>Read more : https://developer.twitter.com/en/docs/trends/locations-with-trending-topics/api-reference/get-trends-available </para>
        /// <returns>Trending locations</returns>
        Task<ITrendLocation[]> GetTrendLocations(IGetTrendsLocationParameters parameters);

        /// <inheritdoc cref="GetTrendsLocationCloseTo(IGetTrendsLocationCloseToParameters)"/>
        Task<ITrendLocation[]> GetTrendsLocationCloseTo(double latitude, double longitude);
        /// <inheritdoc cref="GetTrendsLocationCloseTo(IGetTrendsLocationCloseToParameters)"/>
        Task<ITrendLocation[]> GetTrendsLocationCloseTo(ICoordinates coordinates);

        /// <summary>
        /// Returns the locations that Twitter has trending topic information for, closest to a specified location.
        /// </summary>
        /// <para>Read more : https://developer.twitter.com/en/docs/trends/locations-with-trending-topics/api-reference/get-trends-closest </para>
        /// <returns>Trending locations</returns>
        Task<ITrendLocation[]> GetTrendsLocationCloseTo(IGetTrendsLocationCloseToParameters parameters);
    }
}