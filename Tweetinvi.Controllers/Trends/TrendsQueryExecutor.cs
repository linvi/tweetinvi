using System.Collections.Generic;
using Tweetinvi.Core.Interfaces.Credentials;
using Tweetinvi.Core.Interfaces.Models;

namespace Tweetinvi.Controllers.Trends
{
    public interface ITrendsQueryExecutor
    {
        IPlaceTrends GetPlaceTrendsAt(long woeid);
        IPlaceTrends GetPlaceTrendsAt(IWoeIdLocation woeIdLocation);
        IEnumerable<ITrendLocation> GetAvailableTrendLocations();
        IEnumerable<ITrendLocation> GetClosestTrendLocations(ICoordinates coordinates);
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

        public IPlaceTrends GetPlaceTrendsAt(long woeid)
        {
            string query = _trendsQueryGenerator.GetPlaceTrendsAtQuery(woeid);
            return _twitterAccessor.ExecuteGETQuery<IPlaceTrends[]>(query)[0];
        }

        public IPlaceTrends GetPlaceTrendsAt(IWoeIdLocation woeIdLocation)
        {
            string query = _trendsQueryGenerator.GetPlaceTrendsAtQuery(woeIdLocation);
            return _twitterAccessor.ExecuteGETQuery<IPlaceTrends[]>(query)[0];
        }

        public IEnumerable<ITrendLocation> GetAvailableTrendLocations()
        {
            var query = _trendsQueryGenerator.GetAvailableTrendLocationsQuery();
            return _twitterAccessor.ExecuteGETQuery<ITrendLocation[]>(query);
        }

        public IEnumerable<ITrendLocation> GetClosestTrendLocations(ICoordinates coordinates)
        {
            var query = _trendsQueryGenerator.GetClosestTrendLocationsQuery(coordinates);
            return _twitterAccessor.ExecuteGETQuery<ITrendLocation[]>(query);
        }
    }
}