using System.Collections.Generic;
using Tweetinvi.Core.Web;
using Tweetinvi.Models;
using Tweetinvi.Models.DTO;

namespace Tweetinvi.Controllers.SavedSearch
{
    public interface ISavedSearchQueryExecutor
    {
        IEnumerable<ISavedSearchDTO> GetSavedSearches();

        bool DestroySavedSearch(ISavedSearch savedSearch);
        bool DestroySavedSearch(long searchId);
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

        public IEnumerable<ISavedSearchDTO> GetSavedSearches()
        {
            string query = _savedSearchQueryGenerator.GetSavedSearchesQuery();
            return _twitterAccessor.ExecuteGETQuery<IEnumerable<ISavedSearchDTO>>(query);
        }

        public bool DestroySavedSearch(ISavedSearch savedSearch)
        {
            string query = _savedSearchQueryGenerator.GetDestroySavedSearchQuery(savedSearch);
            return _twitterAccessor.TryExecutePOSTQuery(query);
        }

        public bool DestroySavedSearch(long searchId)
        {
            string query = _savedSearchQueryGenerator.GetDestroySavedSearchQuery(searchId);
            return _twitterAccessor.TryExecutePOSTQuery(query);
        }
    }
}