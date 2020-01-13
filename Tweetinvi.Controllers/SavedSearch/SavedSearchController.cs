using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Tweetinvi.Client.Tools;
using Tweetinvi.Core.Controllers;
using Tweetinvi.Models;

namespace Tweetinvi.Controllers.SavedSearch
{
    public class SavedSearchController : ISavedSearchController
    {
        private readonly ISavedSearchQueryExecutor _savedSearchQueryExecutor;
        private readonly ITwitterClientFactories _factories;

        public SavedSearchController(
            ISavedSearchQueryExecutor savedSearchQueryExecutor,
            ITwitterClientFactories factories)
        {
            _savedSearchQueryExecutor = savedSearchQueryExecutor;
            _factories = factories;
        }

        public async Task<IEnumerable<ISavedSearch>> GetSavedSearches()
        {
            var savedSearchesDTO = await _savedSearchQueryExecutor.GetSavedSearches();
            return savedSearchesDTO?.Select(_factories.CreateSavedSearch);
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