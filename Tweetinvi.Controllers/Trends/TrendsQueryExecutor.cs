using System.Threading.Tasks;
using Tweetinvi.Core.Web;
using Tweetinvi.Models;

namespace Tweetinvi.Controllers.Trends
{
    public interface ITrendsQueryExecutor
    {
        Task<IPlaceTrends> GetPlaceTrendsAt(long woeid);
        Task<IPlaceTrends> GetPlaceTrendsAt(IWoeIdLocation woeIdLocation);
        Task<ITrendLocation[]> GetAvailableTrendLocations();
        Task<ITrendLocation[]> GetClosestTrendLocations(ICoordinates coordinates);
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

        public async Task<IPlaceTrends> GetPlaceTrendsAt(long woeid)
        {
            string query = _trendsQueryGenerator.GetPlaceTrendsAtQuery(woeid);
            var placeTrends = await _twitterAccessor.ExecuteGETQuery<IPlaceTrends[]>(query);

            return placeTrends?[0];
        }

        public async Task<IPlaceTrends> GetPlaceTrendsAt(IWoeIdLocation woeIdLocation)
        {
            string query = _trendsQueryGenerator.GetPlaceTrendsAtQuery(woeIdLocation);
            var placeTrends = await _twitterAccessor.ExecuteGETQuery<IPlaceTrends[]>(query);

            return placeTrends?[0];
        }

        public Task<ITrendLocation[]> GetAvailableTrendLocations()
        {
            var query = _trendsQueryGenerator.GetAvailableTrendLocationsQuery();
            return _twitterAccessor.ExecuteGETQuery<ITrendLocation[]>(query);
        }

        public Task<ITrendLocation[]> GetClosestTrendLocations(ICoordinates coordinates)
        {
            var query = _trendsQueryGenerator.GetClosestTrendLocationsQuery(coordinates);
            return _twitterAccessor.ExecuteGETQuery<ITrendLocation[]>(query);
        }
    }
}