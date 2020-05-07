using System.Threading.Tasks;
using Tweetinvi.Core.Web;
using Tweetinvi.Models;
using Tweetinvi.Parameters.TrendsClient;

namespace Tweetinvi.Core.Controllers
{
    public interface ITrendsController
    {
        Task<ITwitterResult<IGetTrendsAtResult[]>> GetPlaceTrendsAtAsync(IGetTrendsAtParameters parameters, ITwitterRequest request);
        Task<ITwitterResult<ITrendLocation[]>> GetTrendLocationsAsync(IGetTrendsLocationParameters parameters, ITwitterRequest request);
        Task<ITwitterResult<ITrendLocation[]>> GetTrendsLocationCloseToAsync(IGetTrendsLocationCloseToParameters parameters, ITwitterRequest request);
    }
}