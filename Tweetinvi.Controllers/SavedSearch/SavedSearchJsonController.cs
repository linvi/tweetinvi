using Tweetinvi.Core.Interfaces.Credentials;
using Tweetinvi.Core.Interfaces.Models;

namespace Tweetinvi.Controllers.SavedSearch
{
    public interface ISavedSearchJsonController
    {
        string GetSavedSearches();

        string DestroySavedSearch(ISavedSearch savedSearch);
        string DestroySavedSearch(long searchId);
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

        public string GetSavedSearches()
        {
            string query = _savedSearchQueryGenerator.GetSavedSearchesQuery();
            return _twitterAccessor.ExecuteJsonGETQuery(query);
        }

        public string DestroySavedSearch(ISavedSearch savedSearch)
        {
            string query = _savedSearchQueryGenerator.GetDestroySavedSearchQuery(savedSearch);
            return _twitterAccessor.ExecuteJsonPOSTQuery(query);
        }

        public string DestroySavedSearch(long searchId)
        {
            string query = _savedSearchQueryGenerator.GetDestroySavedSearchQuery(searchId);
            return _twitterAccessor.ExecuteJsonPOSTQuery(query);
        }
    }
}