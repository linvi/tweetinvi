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
        Task<ITwitterResult<SavedSearchDTO>> CreateSavedSearchAsync(ICreateSavedSearchParameters parameters, ITwitterRequest request);
        Task<ITwitterResult<SavedSearchDTO>> GetSavedSearchAsync(IGetSavedSearchParameters parameters, ITwitterRequest request);
        Task<ITwitterResult<SavedSearchDTO[]>> ListSavedSearchesAsync(IListSavedSearchesParameters parameters, ITwitterRequest request);
        Task<ITwitterResult<SavedSearchDTO>> DestroySavedSearchAsync(IDestroySavedSearchParameters parameters, ITwitterRequest request);
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
                if (page?.Model?.SearchMetadata?.NextResults == null)
                {
                    return null;
                }

                return page.Model.TweetDTOs.Min(x => x.Id) - 1;
            }

            return new TwitterPageIterator<ITwitterResult<ISearchResultsDTO>, long?>(
                parameters.MaxId,
                cursor =>
                {
                    var cursoredParameters = new SearchTweetsParameters(parameters)
                    {
                        MaxId = cursor
                    };

                    return _searchQueryExecutor.SearchTweetsAsync(cursoredParameters, new TwitterRequest(request));
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

                    var page = await _searchQueryExecutor.SearchUsersAsync(cursoredParameters, new TwitterRequest(request)).ConfigureAwait(false);
                    return new FilteredTwitterResult<UserDTO[]>(page)
                    {
                        FilteredDTO = page.Model.Where(x => !previousResultIds.Contains(x.Id)).ToArray()
                    };
                },
                page =>
                {
                    if (page.Model.Length == 0)
                    {
                        return null;
                    }

                    return ++pageNumber;
                },
                page =>
                {
                    var requestUserIds = page.Model.Select(x => x.Id).ToArray();
                    var newItemIds = requestUserIds.Except(previousResultIds).ToArray();

                    foreach (var newItemId in newItemIds)
                    {
                        previousResultIds.Add(newItemId);
                    }

                    return newItemIds.Length == 0 || page.Model.Length == 0;
                });
        }

        public Task<ITwitterResult<SavedSearchDTO>> CreateSavedSearchAsync(ICreateSavedSearchParameters parameters, ITwitterRequest request)
        {
            return _searchQueryExecutor.CreateSavedSearchAsync(parameters, request);
        }

        public Task<ITwitterResult<SavedSearchDTO>> GetSavedSearchAsync(IGetSavedSearchParameters parameters, ITwitterRequest request)
        {
            return _searchQueryExecutor.GetSavedSearchAsync(parameters, request);
        }

        public Task<ITwitterResult<SavedSearchDTO[]>> ListSavedSearchesAsync(IListSavedSearchesParameters parameters, ITwitterRequest request)
        {
            return _searchQueryExecutor.ListSavedSearchesAsync(parameters, request);
        }

        public Task<ITwitterResult<SavedSearchDTO>> DestroySavedSearchAsync(IDestroySavedSearchParameters parameters, ITwitterRequest request)
        {
            return _searchQueryExecutor.DestroySavedSearchAsync(parameters, request);
        }
    }
}