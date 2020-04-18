using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Tweetinvi.Core.DTO;
using Tweetinvi.Core.Iterators;
using Tweetinvi.Core.Web;
using Tweetinvi.Models;
using Tweetinvi.Models.DTO;
using Tweetinvi.Parameters;

namespace Tweetinvi.Controllers.Search
{
    public interface ISearchController
    {
        ITwitterPageIterator<ITwitterResult<ISearchResultsDTO>, long?> GetSearchTweetsIterator(ISearchTweetsParameters parameters, ITwitterRequest request);
        ITwitterPageIterator<IFilteredTwitterResult<UserDTO[]>, int?> GetSearchUsersIterator(ISearchUsersParameters parameters, ITwitterRequest request);
        Task<ITwitterResult<SavedSearchDTO>> CreateSavedSearch(ICreateSavedSearchParameters parameters, ITwitterRequest request);
        Task<ITwitterResult<SavedSearchDTO>> GetSavedSearch(IGetSavedSearchParameters parameters, ITwitterRequest request);
        Task<ITwitterResult<SavedSearchDTO[]>> ListSavedSearches(IListSavedSearchesParameters parameters, ITwitterRequest request);
        Task<ITwitterResult<SavedSearchDTO>> DestroySavedSearch(IDestroySavedSearchParameters parameters, ITwitterRequest request);
    }

    public class SearchController : ISearchController
    {
        private readonly ISearchQueryExecutor _searchQueryExecutor;

        public SearchController(ISearchQueryExecutor searchQueryExecutor)
        {
            _searchQueryExecutor = searchQueryExecutor;
        }

        public ITwitterPageIterator<ITwitterResult<ISearchResultsDTO>, long?> GetSearchTweetsIterator(ISearchTweetsParameters parameters, ITwitterRequest request)
        {
            long lastCursor = -1;

            long? getNextCursor(ITwitterResult<ISearchResultsDTO> page)
            {
                if (page?.DataTransferObject?.SearchMetadata?.NextResults == null)
                {
                    return null;
                }

                return page.DataTransferObject.TweetDTOs.Min(x => x.Id) - 1;
            }

            return new TwitterPageIterator<ITwitterResult<ISearchResultsDTO>, long?>(
                parameters.MaxId,
                cursor =>
                {
                    var cursoredParameters = new SearchTweetsParameters(parameters)
                    {
                        MaxId = cursor
                    };

                    return _searchQueryExecutor.SearchTweets(cursoredParameters, new TwitterRequest(request));
                },
                getNextCursor,
                page =>
                {
                    var nextCursor = getNextCursor(page);
                    if (nextCursor == null)
                    {
                        return true;
                    }

                    if (lastCursor == nextCursor)
                    {
                        return true;
                    }

                    lastCursor = nextCursor.Value;
                    return false;
                });
        }

        public ITwitterPageIterator<IFilteredTwitterResult<UserDTO[]>, int?> GetSearchUsersIterator(ISearchUsersParameters parameters, ITwitterRequest request)
        {
            var pageNumber = parameters.Page ?? 1;
            var previousResultIds = new HashSet<long>();
            return new TwitterPageIterator<IFilteredTwitterResult<UserDTO[]>, int?>(
                parameters.Page,
                async cursor =>
                {
                    var cursoredParameters = new SearchUsersParameters(parameters)
                    {
                        Page = cursor
                    };

                    var page = await _searchQueryExecutor.SearchUsers(cursoredParameters, new TwitterRequest(request)).ConfigureAwait(false);
                    return new FilteredTwitterResult<UserDTO[]>(page)
                    {
                        FilteredDTO = page.DataTransferObject.Where(x => !previousResultIds.Contains(x.Id)).ToArray()
                    };
                },
                page =>
                {
                    if (page.DataTransferObject.Length == 0)
                    {
                        return null;
                    }

                    return ++pageNumber;
                },
                page =>
                {
                    var requestUserIds = page.DataTransferObject.Select(x => x.Id).ToArray();
                    var newItemIds = requestUserIds.Except(previousResultIds).ToArray();

                    foreach (var newItemId in newItemIds)
                    {
                        previousResultIds.Add(newItemId);
                    }

                    return newItemIds.Length == 0 || page.DataTransferObject.Length == 0;
                });
        }

        public Task<ITwitterResult<SavedSearchDTO>> CreateSavedSearch(ICreateSavedSearchParameters parameters, ITwitterRequest request)
        {
            return _searchQueryExecutor.CreateSavedSearch(parameters, request);
        }

        public Task<ITwitterResult<SavedSearchDTO>> GetSavedSearch(IGetSavedSearchParameters parameters, ITwitterRequest request)
        {
            return _searchQueryExecutor.GetSavedSearch(parameters, request);
        }

        public Task<ITwitterResult<SavedSearchDTO[]>> ListSavedSearches(IListSavedSearchesParameters parameters, ITwitterRequest request)
        {
            return _searchQueryExecutor.ListSavedSearches(parameters, request);
        }

        public Task<ITwitterResult<SavedSearchDTO>> DestroySavedSearch(IDestroySavedSearchParameters parameters, ITwitterRequest request)
        {
            return _searchQueryExecutor.DestroySavedSearch(parameters, request);
        }
    }
}