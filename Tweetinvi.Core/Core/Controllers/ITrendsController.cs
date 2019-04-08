using System.Collections.Generic;
using System.Threading.Tasks;
using Tweetinvi.Models;

namespace Tweetinvi.Core.Controllers
{
    public interface ITrendsController
    {
        Task<IPlaceTrends> GetPlaceTrendsAt(long woeid);
        Task<IPlaceTrends> GetPlaceTrendsAt(IWoeIdLocation woeIdLocation);
        Task<ITrendLocation[]> GetAvailableTrendLocations();
        Task<ITrendLocation[]> GetClosestTrendLocations(double latitude, double longitude);
        Task<ITrendLocation[]> GetClosestTrendLocations(ICoordinates coordinates);
    }
}