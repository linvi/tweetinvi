using System.Threading.Tasks;
using Tweetinvi.Core.Web;
using Tweetinvi.Models;
using Tweetinvi.Parameters.TrendsClient;

namespace Tweetinvi.Core.Controllers
{
    public interface ITrendsController
    {
        Task<ITwitterResult<IGetTrendsAtResult[]>> GetPlaceTrendsAt(IGetTrendsAtParameters parameters, ITwitterRequest request);
        Task<ITwitterResult<ITrendLocation[]>> GetTrendLocations(IGetTrendsLocationParameters parameters, ITwitterRequest request);
        Task<ITwitterResult<ITrendLocation[]>> GetTrendsLocationCloseTo(IGetTrendsLocationCloseToParameters parameters, ITwitterRequest request);
    }
}