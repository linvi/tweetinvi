using System.Threading.Tasks;
using Tweetinvi.Core.Web;
using Tweetinvi.Models;

namespace Tweetinvi.Factories.SavedSearch
{
    public interface ISavedSearchQueryExecutor
    {
        Task<ISavedSearch> CreateSavedSearch(string searchQuery);
        Task<ISavedSearch> GetSavedSearch(long searchId);
    }

    public class SavedSearchFactoryQueryExecutor : ISavedSearchQueryExecutor
    {
        private readonly ISavedSearchQueryGenerator _savedSearchQueryGenerator;
        private readonly ITwitterAccessor _twitterAccessor;

        public SavedSearchFactoryQueryExecutor(
            ISavedSearchQueryGenerator savedSearchQueryGenerator,
            ITwitterAccessor twitterAccessor)
        {
            _savedSearchQueryGenerator = savedSearchQueryGenerator;
            _twitterAccessor = twitterAccessor;
        }

        public Task<ISavedSearch> CreateSavedSearch(string searchQuery)
        {
            if (searchQuery.Length > 100)
            {
                throw new System.ArgumentException("Saved search query should be bigger than 100 characters.");
            }

            var query = _savedSearchQueryGenerator.GetCreateSavedSearchQuery(searchQuery);
            return _twitterAccessor.ExecutePOSTQuery<ISavedSearch>(query);
        }

        public Task<ISavedSearch> GetSavedSearch(long searchId)
        {
            var query = _savedSearchQueryGenerator.GetSavedSearchQuery(searchId);
            return _twitterAccessor.ExecuteGETQuery<ISavedSearch>(query);
        }
    }
}