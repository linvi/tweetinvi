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
    }
}