using System.Threading.Tasks;
using Tweetinvi.Core.Web;

namespace Tweetinvi.Factories.SavedSearch
{
    public interface ISavedSearchJsonFactory
    {
        Task<string> CreateSavedSearch(string searchQuery);
        Task<string> GetSavedSearch(long searchId);
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

        public Task<string> CreateSavedSearch(string searchQuery)
        {
            string query = _savedSearchQueryGenerator.GetCreateSavedSearchQuery(searchQuery);
            return _twitterAccessor.ExecutePOSTQueryReturningJson(query);
        }

        public Task<string> GetSavedSearch(long searchId)
        {
            string query = _savedSearchQueryGenerator.GetSavedSearchQuery(searchId);
            return _twitterAccessor.ExecuteGETQueryReturningJson(query);
        }
    }
}