using System.Collections.Generic;
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

        public IPlaceTrends GetPlaceTrendsAt(long woeid)
        {
            return _trendsQueryExecutor.GetPlaceTrendsAt(woeid);
        }

        public IPlaceTrends GetPlaceTrendsAt(IWoeIdLocation woeIdLocation)
        {
            return _trendsQueryExecutor.GetPlaceTrendsAt(woeIdLocation);
        }

        public IEnumerable<ITrendLocation> GetAvailableTrendLocations()
        {
            return _trendsQueryExecutor.GetAvailableTrendLocations();
        }

        public IEnumerable<ITrendLocation> GetClosestTrendLocations(double latitude, double longitude)
        {
            return GetClosestTrendLocations(new Coordinates(latitude, longitude));
        }

        public IEnumerable<ITrendLocation> GetClosestTrendLocations(ICoordinates coordinates)
        {
            return _trendsQueryExecutor.GetClosestTrendLocations(coordinates);
        }
    }
}