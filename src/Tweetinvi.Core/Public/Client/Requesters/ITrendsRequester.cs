using System.Threading.Tasks;
using Tweetinvi.Core.Web;
using Tweetinvi.Models;
using Tweetinvi.Parameters.TrendsClient;

namespace Tweetinvi.Client.Requesters
{
    public interface ITrendsRequester
    {
        /// <summary>
        /// Returns the top 50 trending topics for a specific WOEID
        /// </summary>
        /// <para>Read more : https://developer.twitter.com/en/docs/trends/trends-for-location/api-reference/get-trends-place </para>
        /// <returns>Twitter result containing the place trends</returns>
        Task<ITwitterResult<IGetTrendsAtResult[]>> GetPlaceTrendsAtAsync(IGetTrendsAtParameters parameters);

        /// <summary>
        /// Returns the locations that Twitter has trending topic information for.
        /// </summary>
        /// <para>Read more : https://developer.twitter.com/en/docs/trends/locations-with-trending-topics/api-reference/get-trends-available </para>
        /// <returns>Twitter result containing the trending locations</returns>
        Task<ITwitterResult<ITrendLocation[]>> GetTrendLocationsAsync(IGetTrendsLocationParameters parameters);

        /// <summary>
        /// Returns the locations that Twitter has trending topic information for, closest to a specified location.
        /// </summary>
        /// <para>Read more : https://developer.twitter.com/en/docs/trends/locations-with-trending-topics/api-reference/get-trends-closest </para>
        /// <returns>Twitter result containing the trending locations</returns>
        Task<ITwitterResult<ITrendLocation[]>> GetTrendsLocationCloseToAsync(IGetTrendsLocationCloseToParameters parameters);
    }
}