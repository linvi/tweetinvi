using System.Collections.Generic;
using System.Threading.Tasks;
using Tweetinvi.Core.Controllers;
using Tweetinvi.Core.Factories;
using Tweetinvi.Models;

namespace Tweetinvi.Controllers.SavedSearch
{
    public class SavedSearchController : ISavedSearchController
    {
        private readonly ISavedSearchQueryExecutor _savedSearchQueryExecutor;
        private readonly ISavedSearchFactory _savedSearchFactory;

        public SavedSearchController(
            ISavedSearchQueryExecutor savedSearchQueryExecutor,
            ISavedSearchFactory savedSearchFactory)
        {
            _savedSearchQueryExecutor = savedSearchQueryExecutor;
            _savedSearchFactory = savedSearchFactory;
        }

        public async Task<IEnumerable<ISavedSearch>> GetSavedSearches()
        {
            var savedSearchesDTO = await _savedSearchQueryExecutor.GetSavedSearches();
            return _savedSearchFactory.GenerateSavedSearchesFromDTOs(savedSearchesDTO);
        }

        public Task<bool> DestroySavedSearch(ISavedSearch savedSearch)
        {
            return _savedSearchQueryExecutor.DestroySavedSearch(savedSearch);
        }

        public Task<bool> DestroySavedSearch(long searchId)
        {
            return _savedSearchQueryExecutor.DestroySavedSearch(searchId);
        }
    }
}