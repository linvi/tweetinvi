using Tweetinvi.Core.Interfaces.Controllers;
using Tweetinvi.Core.Interfaces.Models;

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
    }
}