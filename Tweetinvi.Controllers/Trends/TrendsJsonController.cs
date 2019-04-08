using System.Threading.Tasks;
using Tweetinvi.Core.Web;
using Tweetinvi.Models;

namespace Tweetinvi.Controllers.Trends
{
    public interface ITrendsJsonController
    {
        Task<string> GetPlaceTrendsAt(long woeid);
        Task<string> GetPlaceTrendsAt(IWoeIdLocation woeIdLocation);
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

        public Task<string> GetPlaceTrendsAt(long woeid)
        {
            string query = _trendsQueryGenerator.GetPlaceTrendsAtQuery(woeid);
            return _twitterAccessor.ExecuteGETQueryReturningJson(query);
        }

        public Task<string> GetPlaceTrendsAt(IWoeIdLocation woeIdLocation)
        {
            string query = _trendsQueryGenerator.GetPlaceTrendsAtQuery(woeIdLocation);
            return _twitterAccessor.ExecuteGETQueryReturningJson(query);
        }
    }
}