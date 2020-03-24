using System.Threading.Tasks;
using Tweetinvi.Core.Controllers;
using Tweetinvi.Core.Web;
using Tweetinvi.Models;
using Tweetinvi.Parameters.TrendsClient;

namespace Tweetinvi.Controllers.Trends
{
    public class TrendsController : ITrendsController
    {
        private readonly ITrendsQueryExecutor _trendsQueryExecutor;

        public TrendsController(ITrendsQueryExecutor trendsQueryExecutor)
        {
            _trendsQueryExecutor = trendsQueryExecutor;
        }


        public Task<ITwitterResult<IGetTrendsAtResult[]>> GetPlaceTrendsAt(IGetTrendsAtParameters parameters, ITwitterRequest request)
        {
            return _trendsQueryExecutor.GetPlaceTrendsAt(parameters, request);
        }

        public Task<ITwitterResult<ITrendLocation[]>> GetTrendLocations(IGetTrendsLocationParameters parameters, ITwitterRequest request)
        {
            return _trendsQueryExecutor.GetTrendLocations(parameters, request);
        }

        public Task<ITwitterResult<ITrendLocation[]>> GetTrendsLocationCloseTo(IGetTrendsLocationCloseToParameters parameters, ITwitterRequest request)
        {
            return _trendsQueryExecutor.GetTrendsLocationCloseTo(parameters, request);
        }
    }
}