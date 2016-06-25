using System.Collections.Generic;
using Tweetinvi.Models;

namespace Tweetinvi.Core.Controllers
{
    public interface ITrendsController
    {
        IPlaceTrends GetPlaceTrendsAt(long woeid);
        IPlaceTrends GetPlaceTrendsAt(IWoeIdLocation woeIdLocation);
        IEnumerable<ITrendLocation> GetAvailableTrendLocations();
        IEnumerable<ITrendLocation> GetClosestTrendLocations(double latitude, double longitude);
        IEnumerable<ITrendLocation> GetClosestTrendLocations(ICoordinates coordinates);
    }
}