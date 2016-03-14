using System.Collections.Generic;
using Tweetinvi.Core.Interfaces.Controllers;
using Tweetinvi.Core.Interfaces.Models;
using Tweetinvi.Core.Parameters;

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

        public IEnumerable<ITrendLocation> GetClosestTrendLocations(double longitude, double latitude)
        {
            return GetClosestTrendLocations(new Coordinates(longitude, latitude));
        }

        public IEnumerable<ITrendLocation> GetClosestTrendLocations(ICoordinates coordinates)
        {
            return _trendsQueryExecutor.GetClosestTrendLocations(coordinates);
        }
    }
}