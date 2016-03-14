using System.Collections.Generic;
using Tweetinvi.Core.Interfaces.Models;

namespace Tweetinvi.Core.Interfaces.Controllers
{
    public interface ITrendsController
    {
        IPlaceTrends GetPlaceTrendsAt(long woeid);
        IPlaceTrends GetPlaceTrendsAt(IWoeIdLocation woeIdLocation);
        IEnumerable<ITrendLocation> GetAvailableTrendLocations();
        IEnumerable<ITrendLocation> GetClosestTrendLocations(double longitude, double latitude);
        IEnumerable<ITrendLocation> GetClosestTrendLocations(ICoordinates coordinates);
    }
}