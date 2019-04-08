using System.Collections.Generic;
using System.Threading.Tasks;
using Tweetinvi.Core.Web;
using Tweetinvi.Models;
using Tweetinvi.Models.DTO;

namespace Tweetinvi.Controllers.SavedSearch
{
    public interface ISavedSearchQueryExecutor
    {
        Task<IEnumerable<ISavedSearchDTO>> GetSavedSearches();

        Task<bool> DestroySavedSearch(ISavedSearch savedSearch);
        Task<bool> DestroySavedSearch(long searchId);
    }

    public class SavedSearchQueryExecutor : ISavedSearchQueryExecutor
    {
        private readonly ISavedSearchQueryGenerator _savedSearchQueryGenerator;
        private readonly ITwitterAccessor _twitterAccessor;

        public SavedSearchQueryExecutor(
            ISavedSearchQueryGenerator savedSearchQueryGenerator,
            ITwitterAccessor twitterAccessor)
        {
            _savedSearchQueryGenerator = savedSearchQueryGenerator;
            _twitterAccessor = twitterAccessor;
        }

        public Task<IEnumerable<ISavedSearchDTO>> GetSavedSearches()
        {
            string query = _savedSearchQueryGenerator.GetSavedSearchesQuery();
            return _twitterAccessor.ExecuteGETQuery<IEnumerable<ISavedSearchDTO>>(query);
        }

        public async Task<bool> DestroySavedSearch(ISavedSearch savedSearch)
        {
            string query = _savedSearchQueryGenerator.GetDestroySavedSearchQuery(savedSearch);
            var asyncOperation = await _twitterAccessor.TryExecutePOSTQuery(query);

            return asyncOperation.Success;
        }

        public async Task<bool> DestroySavedSearch(long searchId)
        {
            string query = _savedSearchQueryGenerator.GetDestroySavedSearchQuery(searchId);
            var asyncOperation = await _twitterAccessor.TryExecutePOSTQuery(query);

            return asyncOperation.Success;
        }
    }
}