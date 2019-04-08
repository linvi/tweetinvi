using System.Collections.Generic;
using System.Threading.Tasks;
using Tweetinvi.Core.Controllers;
using Tweetinvi.Models;

namespace Tweetinvi.Controllers.Trends
{
    public class TrendsController : ITrendsController
    {
        private readonly ITrendsQueryExecutor _trendsQueryExecutor;

        public TrendsController(ITrendsQueryExecutor trendsQueryExecutor)
        {
            _trendsQueryExecutor = trendsQueryExecutor;
        }

        public Task<IPlaceTrends> GetPlaceTrendsAt(long woeid)
        {
            return _trendsQueryExecutor.GetPlaceTrendsAt(woeid);
        }

        public Task<IPlaceTrends> GetPlaceTrendsAt(IWoeIdLocation woeIdLocation)
        {
            return _trendsQueryExecutor.GetPlaceTrendsAt(woeIdLocation);
        }

        public Task<ITrendLocation[]> GetAvailableTrendLocations()
        {
            return _trendsQueryExecutor.GetAvailableTrendLocations();
        }

        public Task<ITrendLocation[]> GetClosestTrendLocations(double latitude, double longitude)
        {
            return GetClosestTrendLocations(new Coordinates(latitude, longitude));
        }

        public Task<ITrendLocation[]> GetClosestTrendLocations(ICoordinates coordinates)
        {
            return _trendsQueryExecutor.GetClosestTrendLocations(coordinates);
        }
    }
}