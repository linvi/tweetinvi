using System.Threading.Tasks;
using Tweetinvi.Core.Web;
using Tweetinvi.Models;
using Tweetinvi.Parameters.TrendsClient;

namespace Tweetinvi.Controllers.Trends
{
    public interface ITrendsQueryExecutor
    {
        Task<ITwitterResult<IGetTrendsAtResult[]>> GetPlaceTrendsAtAsync(IGetTrendsAtParameters parameters, ITwitterRequest request);
        Task<ITwitterResult<ITrendLocation[]>> GetTrendLocationsAsync(IGetTrendsLocationParameters parameters, ITwitterRequest request);
        Task<ITwitterResult<ITrendLocation[]>> GetTrendsLocationCloseToAsync(IGetTrendsLocationCloseToParameters parameters, ITwitterRequest request);
    }

    public class TrendsQueryExecutor : ITrendsQueryExecutor
    {
        private readonly ITrendsQueryGenerator _trendsQueryGenerator;
        private readonly ITwitterAccessor _twitterAccessor;

        public TrendsQueryExecutor(
            ITrendsQueryGenerator trendsQueryGenerator,
            ITwitterAccessor twitterAccessor)
        {
            _trendsQueryGenerator = trendsQueryGenerator;
            _twitterAccessor = twitterAccessor;
        }

        public Task<ITwitterResult<IGetTrendsAtResult[]>> GetPlaceTrendsAtAsync(IGetTrendsAtParameters parameters, ITwitterRequest request)
        {
            request.Query.Url = _trendsQueryGenerator.GetTrendsAtQuery(parameters);
            request.Query.HttpMethod = HttpMethod.GET;
            return _twitterAccessor.ExecuteRequestAsync<IGetTrendsAtResult[]>(request);
        }

        public Task<ITwitterResult<ITrendLocation[]>> GetTrendLocationsAsync(IGetTrendsLocationParameters parameters, ITwitterRequest request)
        {
            request.Query.Url = _trendsQueryGenerator.GetTrendsLocationQuery(parameters);
            request.Query.HttpMethod = HttpMethod.GET;
            return _twitterAccessor.ExecuteRequestAsync<ITrendLocation[]>(request);
        }

        public Task<ITwitterResult<ITrendLocation[]>> GetTrendsLocationCloseToAsync(IGetTrendsLocationCloseToParameters parameters, ITwitterRequest request)
        {
            request.Query.Url = _trendsQueryGenerator.GetTrendsLocationCloseToQuery(parameters);
            request.Query.HttpMethod = HttpMethod.GET;
            return _twitterAccessor.ExecuteRequestAsync<ITrendLocation[]>(request);
        }
    }
}