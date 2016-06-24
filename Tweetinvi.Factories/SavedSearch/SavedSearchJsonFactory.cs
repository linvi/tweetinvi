using Tweetinvi.Core.Web;

namespace Tweetinvi.Factories.SavedSearch
{
    public interface ISavedSearchJsonFactory
    {
        string CreateSavedSearch(string searchQuery);
        string GetSavedSearch(long searchId);
    }

    public class SavedSearchJsonFactory : ISavedSearchJsonFactory
    {
        private readonly ISavedSearchQueryGenerator _savedSearchQueryGenerator;
        private readonly ITwitterAccessor _twitterAccessor;

        public SavedSearchJsonFactory(
            ISavedSearchQueryGenerator savedSearchQueryGenerator,
            ITwitterAccessor twitterAccessor)
        {
            _savedSearchQueryGenerator = savedSearchQueryGenerator;
            _twitterAccessor = twitterAccessor;
        }

        public string CreateSavedSearch(string searchQuery)
        {
            string query = _savedSearchQueryGenerator.GetCreateSavedSearchQuery(searchQuery);
            return _twitterAccessor.ExecutePOSTQueryReturningJson(query);
        }

        public string GetSavedSearch(long searchId)
        {
            string query = _savedSearchQueryGenerator.GetSavedSearchQuery(searchId);
            return _twitterAccessor.ExecuteGETQueryReturningJson(query);
        }
    }
}