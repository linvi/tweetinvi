using System;
using System.Threading.Tasks;
using Tweetinvi.Core.Iterators;
using Tweetinvi.Core.Web;
using Tweetinvi.Models;
using Tweetinvi.Models.V2.Responses;
using Tweetinvi.Parameters.V2;

namespace Tweetinvi.Controllers.Search
{
    public interface ISearchV2Controller
    {
        Task<ITwitterResult<SearchTweetsResponseDTO>> SearchTweetsAsync(ISearchTweetsV2Parameters parameters, ITwitterRequest request);
        ITwitterPageIterator<ITwitterResult<SearchTweetsResponseDTO>, string> GetSearchTweetsV2Iterator(ISearchTweetsV2Parameters parameters, ITwitterRequest request);
    }

    public class SearchV2Controller : ISearchV2Controller
    {
        private readonly ISearchV2QueryExecutor _searchQueryExecutor;

        public SearchV2Controller(ISearchV2QueryExecutor searchQueryExecutor)
        {
            _searchQueryExecutor = searchQueryExecutor;
        }

        public Task<ITwitterResult<SearchTweetsResponseDTO>> SearchTweetsAsync(ISearchTweetsV2Parameters parameters, ITwitterRequest request)
        {
            return _searchQueryExecutor.SearchTweetsAsync(parameters, request);
        }

        public ITwitterPageIterator<ITwitterResult<SearchTweetsResponseDTO>, string> GetSearchTweetsV2Iterator(ISearchTweetsV2Parameters parameters, ITwitterRequest request)
        {
            Func<string, Task<ITwitterResult<SearchTweetsResponseDTO>>> getNext = nextToken =>
            {
                var cursoredParameters = new SearchTweetsV2Parameters(parameters)
                {
                    NextToken = nextToken
                };

                return _searchQueryExecutor.SearchTweetsAsync(cursoredParameters, new TwitterRequest(request));
            };

            var twitterCursorResult = new TwitterPageIterator<ITwitterResult<SearchTweetsResponseDTO>, string>(
                parameters.NextToken,
                getNext,
                page =>
                {
                    if (page.Model.data.Length == 0)
                    {
                        return null;
                    }

                    return page.Model.meta.next_token;
                },
                page =>
                {
                    if (page.Model.data.Length == 0)
                    {
                        return true;
                    }

                    return page.Model.meta.next_token == null;
                });

            return twitterCursorResult;
        }
    }
}