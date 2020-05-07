using System.Threading.Tasks;
using Tweetinvi.Controllers.Search;
using Tweetinvi.Core.Client.Validators;
using Tweetinvi.Core.DTO;
using Tweetinvi.Core.Events;
using Tweetinvi.Core.Iterators;
using Tweetinvi.Core.Web;
using Tweetinvi.Models.DTO;
using Tweetinvi.Parameters;

namespace Tweetinvi.Client.Requesters
{
    public class SearchRequester : BaseRequester, ISearchRequester
    {
        private readonly ISearchController _searchController;
        private readonly ISearchClientRequiredParametersValidator _validator;

        public SearchRequester(
        ISearchController searchController,
        ISearchClientRequiredParametersValidator validator,
        ITwitterClient client,
        ITwitterClientEvents twitterClientEvents)
        : base(client, twitterClientEvents)
        {
            _searchController = searchController;
            _validator = validator;
        }

        public ITwitterPageIterator<ITwitterResult<ISearchResultsDTO>, long?> GetSearchTweetsIterator(ISearchTweetsParameters parameters)
        {
            _validator.Validate(parameters);

            var request = TwitterClient.CreateRequest();
            return _searchController.GetSearchTweetsIterator(parameters, request);
        }

        public ITwitterPageIterator<IFilteredTwitterResult<UserDTO[]>, int?> GetSearchUsersIterator(ISearchUsersParameters parameters)
        {
            _validator.Validate(parameters);

            var request = TwitterClient.CreateRequest();
            return _searchController.GetSearchUsersIterator(parameters, request);
        }

        public Task<ITwitterResult<SavedSearchDTO>> CreateSavedSearchAsync(ICreateSavedSearchParameters parameters)
        {
            _validator.Validate(parameters);
            return ExecuteRequestAsync(request => _searchController.CreateSavedSearchAsync(parameters, request));
        }

        public Task<ITwitterResult<SavedSearchDTO>> GetSavedSearchAsync(IGetSavedSearchParameters parameters)
        {
            _validator.Validate(parameters);
            return ExecuteRequestAsync(request => _searchController.GetSavedSearchAsync(parameters, request));
        }

        public Task<ITwitterResult<SavedSearchDTO[]>> ListSavedSearchesAsync(IListSavedSearchesParameters parameters)
        {
            _validator.Validate(parameters);
            return ExecuteRequestAsync(request => _searchController.ListSavedSearchesAsync(parameters, request));
        }

        public Task<ITwitterResult<SavedSearchDTO>> DestroySavedSearchAsync(IDestroySavedSearchParameters parameters)
        {
            _validator.Validate(parameters);
            return ExecuteRequestAsync(request => _searchController.DestroySavedSearchAsync(parameters, request));
        }
    }
}