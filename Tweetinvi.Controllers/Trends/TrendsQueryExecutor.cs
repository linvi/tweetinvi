using Tweetinvi.Core.Interfaces.Credentials;
using Tweetinvi.Core.Interfaces.Models;

namespace Tweetinvi.Controllers.Trends
{
    public interface ITrendsQueryExecutor
    {
        IPlaceTrends GetPlaceTrendsAt(long woeid);
        IPlaceTrends GetPlaceTrendsAt(IWoeIdLocation woeIdLocation);
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
    }
}