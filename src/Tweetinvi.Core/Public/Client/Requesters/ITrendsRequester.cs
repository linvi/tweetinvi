using System.Threading.Tasks;
using Tweetinvi.Core.Web;
using Tweetinvi.Models;
using Tweetinvi.Parameters.TrendsClient;

namespace Tweetinvi.Client.Requesters
{
    public interface ITrendsRequester
    {
        Task<ITwitterResult<IGetTrendsAtResult[]>> GetPlaceTrendsAtAsync(IGetTrendsAtParameters parameters);
        Task<ITwitterResult<ITrendLocation[]>> GetTrendLocationsAsync(IGetTrendsLocationParameters parameters);
        Task<ITwitterResult<ITrendLocation[]>> GetTrendsLocationCloseToAsync(IGetTrendsLocationCloseToParameters parameters);
    }
}