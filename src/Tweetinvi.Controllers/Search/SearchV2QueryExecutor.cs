using System.Threading.Tasks;
using Tweetinvi.Core.Web;
using Tweetinvi.Models;
using Tweetinvi.Models.V2.Responses;
using Tweetinvi.Parameters.V2;

namespace Tweetinvi.Controllers.Search
{
    public interface ISearchV2QueryExecutor
    {
        Task<ITwitterResult<SearchTweetsResponseDTO>> SearchTweetsAsync(ISearchTweetsV2Parameters parameters, ITwitterRequest request);
    }

    public class SearchV2QueryExecutor : ISearchV2QueryExecutor
    {
        private readonly ISearchV2QueryGenerator _searchQueryGenerator;
        private readonly ITwitterAccessor _twitterAccessor;

        public SearchV2QueryExecutor(
            ISearchV2QueryGenerator searchQueryGenerator,
            ITwitterAccessor twitterAccessor)
        {
            _searchQueryGenerator = searchQueryGenerator;
            _twitterAccessor = twitterAccessor;
        }

        public Task<ITwitterResult<SearchTweetsResponseDTO>> SearchTweetsAsync(ISearchTweetsV2Parameters parameters, ITwitterRequest request)
        {
            request.Query.Url = _searchQueryGenerator.GetSearchTweetsV2Query(parameters);
            return _twitterAccessor.ExecuteRequestAsync<SearchTweetsResponseDTO>(request);
        }
    }
}