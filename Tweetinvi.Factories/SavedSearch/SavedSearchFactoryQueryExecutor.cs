using Tweetinvi.Core.Interfaces.Credentials;
using Tweetinvi.Core.Interfaces.Models;

namespace Tweetinvi.Factories.SavedSearch
{
    public interface ISavedSearchQueryExecutor
    {
        ISavedSearch CreateSavedSearch(string searchQuery);
        ISavedSearch GetSavedSearch(long searchId);
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

        public ISavedSearch CreateSavedSearch(string searchQuery)
        {
            if (searchQuery.Length > 100)
            {
                throw new System.ArgumentException("Saved search query should be bigger than 100 characters.");
            }

            var query = _savedSearchQueryGenerator.GetCreateSavedSearchQuery(searchQuery);
            return _twitterAccessor.ExecutePOSTQuery<ISavedSearch>(query);
        }

        public ISavedSearch GetSavedSearch(long searchId)
        {
            var query = _savedSearchQueryGenerator.GetSavedSearchQuery(searchId);
            return _twitterAccessor.ExecuteGETQuery<ISavedSearch>(query);
        }
    }
}