using System.Threading.Tasks;
using Tweetinvi.Core.DTO;
using Tweetinvi.Core.Web;
using Tweetinvi.Models;
using Tweetinvi.Models.DTO;
using Tweetinvi.Parameters;

namespace Tweetinvi.Controllers.Search
{
    public interface ISearchQueryExecutor
    {
        Task<ITwitterResult<ISearchResultsDTO>> SearchTweets(ISearchTweetsParameters parameters, ITwitterRequest request);
        Task<ITwitterResult<UserDTO[]>> SearchUsers(ISearchUsersParameters parameters, ITwitterRequest request);
        Task<ITwitterResult<SavedSearchDTO>> CreateSavedSearch(ICreateSavedSearchParameters parameters, ITwitterRequest request);
        Task<ITwitterResult<SavedSearchDTO>> GetSavedSearch(IGetSavedSearchParameters parameters, ITwitterRequest request);
        Task<ITwitterResult<SavedSearchDTO[]>> ListSavedSearches(IListSavedSearchesParameters parameters, ITwitterRequest request);
        Task<ITwitterResult<SavedSearchDTO>> DestroySavedSearch(IDestroySavedSearchParameters parameters, ITwitterRequest request);
    }

    public class SearchQueryExecutor : ISearchQueryExecutor
    {
        private readonly ISearchQueryGenerator _searchQueryGenerator;
        private readonly ITwitterAccessor _twitterAccessor;

        public SearchQueryExecutor(ISearchQueryGenerator searchQueryGenerator, ITwitterAccessor twitterAccessor)
        {
            _searchQueryGenerator = searchQueryGenerator;
            _twitterAccessor = twitterAccessor;
        }

        public Task<ITwitterResult<ISearchResultsDTO>> SearchTweets(ISearchTweetsParameters parameters, ITwitterRequest request)
        {
            request.Query.Url = _searchQueryGenerator.GetSearchTweetsQuery(parameters);
            request.Query.HttpMethod = HttpMethod.GET;
            return _twitterAccessor.ExecuteRequest<ISearchResultsDTO>(request);
        }

        public Task<ITwitterResult<UserDTO[]>> SearchUsers(ISearchUsersParameters parameters, ITwitterRequest request)
        {
            request.Query.Url = _searchQueryGenerator.GetSearchUsersQuery(parameters);
            request.Query.HttpMethod = HttpMethod.GET;
            return _twitterAccessor.ExecuteRequest<UserDTO[]>(request);
        }

        public Task<ITwitterResult<SavedSearchDTO>> CreateSavedSearch(ICreateSavedSearchParameters parameters, ITwitterRequest request)
        {
            request.Query.Url = _searchQueryGenerator.GetCreateSavedSearchQuery(parameters);
            request.Query.HttpMethod = HttpMethod.POST;
            return _twitterAccessor.ExecuteRequest<SavedSearchDTO>(request);
        }

        public Task<ITwitterResult<SavedSearchDTO>> GetSavedSearch(IGetSavedSearchParameters parameters, ITwitterRequest request)
        {
            request.Query.Url = _searchQueryGenerator.GetSavedSearchQuery(parameters);
            request.Query.HttpMethod = HttpMethod.GET;
            return _twitterAccessor.ExecuteRequest<SavedSearchDTO>(request);
        }

        public Task<ITwitterResult<SavedSearchDTO[]>> ListSavedSearches(IListSavedSearchesParameters parameters, ITwitterRequest request)
        {
            request.Query.Url = _searchQueryGenerator.GetListSavedSearchQuery(parameters);
            request.Query.HttpMethod = HttpMethod.GET;
            return _twitterAccessor.ExecuteRequest<SavedSearchDTO[]>(request);
        }

        public Task<ITwitterResult<SavedSearchDTO>> DestroySavedSearch(IDestroySavedSearchParameters parameters, ITwitterRequest request)
        {
            request.Query.Url = _searchQueryGenerator.GetDestroySavedSearchQuery(parameters);
            request.Query.HttpMethod = HttpMethod.POST;
            return _twitterAccessor.ExecuteRequest<SavedSearchDTO>(request);
        }
    }
}