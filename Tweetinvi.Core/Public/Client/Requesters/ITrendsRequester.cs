using System.Threading.Tasks;
using Tweetinvi.Core.Web;
using Tweetinvi.Models;
using Tweetinvi.Parameters.TrendsClient;

namespace Tweetinvi.Client.Requesters
{
    public interface ITrendsRequester
    {
        Task<ITwitterResult<IGetTrendsAtResult[]>> GetPlaceTrendsAt(IGetTrendsAtParameters parameters);
        Task<ITwitterResult<ITrendLocation[]>> GetTrendLocations(IGetTrendsLocationParameters parameters);
        Task<ITwitterResult<ITrendLocation[]>> GetTrendsLocationCloseTo(IGetTrendsLocationCloseToParameters parameters);
    }
}