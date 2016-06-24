using Tweetinvi.Core.Web;
using Tweetinvi.Models;

namespace Tweetinvi.Controllers.Trends
{
    public interface ITrendsJsonController
    {
        string GetPlaceTrendsAt(long woeid);
        string GetPlaceTrendsAt(IWoeIdLocation woeIdLocation);
    }

    public class TrendsJsonController : ITrendsJsonController
    {
        private readonly ITrendsQueryGenerator _trendsQueryGenerator;
        private readonly ITwitterAccessor _twitterAccessor;

        public TrendsJsonController(
            ITrendsQueryGenerator trendsQueryGenerator,
            ITwitterAccessor twitterAccessor)
        {
            _trendsQueryGenerator = trendsQueryGenerator;
            _twitterAccessor = twitterAccessor;
        }

        public string GetPlaceTrendsAt(long woeid)
        {
            string query = _trendsQueryGenerator.GetPlaceTrendsAtQuery(woeid);
            return _twitterAccessor.ExecuteGETQueryReturningJson(query);
        }

        public string GetPlaceTrendsAt(IWoeIdLocation woeIdLocation)
        {
            string query = _trendsQueryGenerator.GetPlaceTrendsAtQuery(woeIdLocation);
            return _twitterAccessor.ExecuteGETQueryReturningJson(query);
        }
    }
}