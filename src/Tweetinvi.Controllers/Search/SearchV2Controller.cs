using System;
using System.Threading.Tasks;
using Tweetinvi.Core.Iterators;
using Tweetinvi.Core.Web;
using Tweetinvi.Models;
using Tweetinvi.Models.Responses;
using Tweetinvi.Parameters.V2;

namespace Tweetinvi.Controllers.Search
{
    public interface ISearchV2Controller
    {
        Task<ITwitterResult<SearchTweetsV2Response>> SearchTweetsAsync(ISearchTweetsV2Parameters parameters, ITwitterRequest request);
        ITwitterPageIterator<ITwitterResult<SearchTweetsV2Response>, string> GetSearchTweetsV2Iterator(ISearchTweetsV2Parameters parameters, ITwitterRequest request);
    }

    public class SearchV2Controller : ISearchV2Controller
    {
        private readonly ISearchV2QueryExecutor _searchQueryExecutor;

        public SearchV2Controller(ISearchV2QueryExecutor searchQueryExecutor)
        {
            _searchQueryExecutor = searchQueryExecutor;
        }

        public Task<ITwitterResult<SearchTweetsV2Response>> SearchTweetsAsync(ISearchTweetsV2Parameters parameters, ITwitterRequest request)
        {
            return _searchQueryExecutor.SearchTweetsAsync(parameters, request);
        }

        public ITwitterPageIterator<ITwitterResult<SearchTweetsV2Response>, string> GetSearchTweetsV2Iterator(ISearchTweetsV2Parameters parameters, ITwitterRequest request)
        {
            Func<string, Task<ITwitterResult<SearchTweetsV2Response>>> getNext = nextToken =>
            {
                var cursoredParameters = new SearchTweetsV2Parameters(parameters)
                {
                    NextToken = nextToken
                };

                return _searchQueryExecutor.SearchTweetsAsync(cursoredParameters, new TwitterRequest(request));
            };

            var twitterCursorResult = new TwitterPageIterator<ITwitterResult<SearchTweetsV2Response>, string>(
                parameters.NextToken,
                getNext,
                page =>
                {
                    if (page.Model.Tweets.Length == 0)
                    {
                        return null;
                    }

                    return page.Model.SearchMetadata.NextToken;
                },
                page =>
                {
                    if (page.Model.Tweets.Length == 0)
                    {
                        return true;
                    }

                    return page.Model.SearchMetadata.NextToken == null;
                });

            return twitterCursorResult;
        }
    }
}