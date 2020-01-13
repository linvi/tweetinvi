using System.Threading.Tasks;
using Tweetinvi.Core.Factories;
using Tweetinvi.Models;

namespace Tweetinvi.Factories.SavedSearch
{
    public class SavedSearchFactory : ISavedSearchFactory
    {
        private readonly ISavedSearchQueryExecutor _savedSearchQueryExecutor;

        public SavedSearchFactory(ISavedSearchQueryExecutor savedSearchQueryExecutor)
        {
            _savedSearchQueryExecutor = savedSearchQueryExecutor;
        }

        public Task<ISavedSearch> CreateSavedSearch(string searchQuery)
        {
            return _savedSearchQueryExecutor.CreateSavedSearch(searchQuery);
        }

        public Task<ISavedSearch> GetSavedSearch(long searchId)
        {
            return _savedSearchQueryExecutor.GetSavedSearch(searchId);
        }
    }
}