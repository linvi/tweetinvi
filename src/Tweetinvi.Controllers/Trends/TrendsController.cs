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


        public Task<ITwitterResult<IGetTrendsAtResult[]>> GetPlaceTrendsAtAsync(IGetTrendsAtParameters parameters, ITwitterRequest request)
        {
            return _trendsQueryExecutor.GetPlaceTrendsAtAsync(parameters, request);
        }

        public Task<ITwitterResult<ITrendLocation[]>> GetTrendLocationsAsync(IGetTrendsLocationParameters parameters, ITwitterRequest request)
        {
            return _trendsQueryExecutor.GetTrendLocationsAsync(parameters, request);
        }

        public Task<ITwitterResult<ITrendLocation[]>> GetTrendsLocationCloseToAsync(IGetTrendsLocationCloseToParameters parameters, ITwitterRequest request)
        {
            return _trendsQueryExecutor.GetTrendsLocationCloseToAsync(parameters, request);
        }
    }
}