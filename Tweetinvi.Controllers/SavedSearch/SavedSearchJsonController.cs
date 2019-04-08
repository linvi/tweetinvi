using System.Threading.Tasks;
using Tweetinvi.Core.Web;
using Tweetinvi.Models;

namespace Tweetinvi.Controllers.SavedSearch
{
    public interface ISavedSearchJsonController
    {
        Task<string> GetSavedSearches();
        Task<string> DestroySavedSearch(ISavedSearch savedSearch);
        Task<string> DestroySavedSearch(long searchId);
    }

    public class SavedSearchJsonController : ISavedSearchJsonController
    {
        private readonly ISavedSearchQueryGenerator _savedSearchQueryGenerator;
        private readonly ITwitterAccessor _twitterAccessor;

        public SavedSearchJsonController(
            ISavedSearchQueryGenerator savedSearchQueryGenerator,
            ITwitterAccessor twitterAccessor)
        {
            _savedSearchQueryGenerator = savedSearchQueryGenerator;
            _twitterAccessor = twitterAccessor;
        }

        public Task<string> GetSavedSearches()
        {
            string query = _savedSearchQueryGenerator.GetSavedSearchesQuery();
            return _twitterAccessor.ExecuteGETQueryReturningJson(query);
        }

        public Task<string> DestroySavedSearch(ISavedSearch savedSearch)
        {
            string query = _savedSearchQueryGenerator.GetDestroySavedSearchQuery(savedSearch);
            return _twitterAccessor.ExecutePOSTQueryReturningJson(query);
        }

        public Task<string> DestroySavedSearch(long searchId)
        {
            string query = _savedSearchQueryGenerator.GetDestroySavedSearchQuery(searchId);
            return _twitterAccessor.ExecutePOSTQueryReturningJson(query);
        }
    }
}