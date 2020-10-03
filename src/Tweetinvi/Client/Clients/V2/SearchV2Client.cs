using System.Threading.Tasks;
using Tweetinvi.Client.Requesters.V2;
using Tweetinvi.Core.Iterators;
using Tweetinvi.Core.Web;
using Tweetinvi.Iterators;
using Tweetinvi.Models.V2;
using Tweetinvi.Parameters.V2;

namespace Tweetinvi.Client.V2
{
    public class SearchV2Client : ISearchV2Client
    {
        private readonly ISearchV2Requester _searchV2Requester;

        public SearchV2Client(ISearchV2Requester searchV2Requester)
        {
            _searchV2Requester = searchV2Requester;
        }

        public Task<SearchTweetsV2Response> SearchTweetsAsync(string query)
        {
            return SearchTweetsAsync(new SearchTweetsV2Parameters(query));
        }

        public async Task<SearchTweetsV2Response> SearchTweetsAsync(ISearchTweetsV2Parameters parameters)
        {
            var iterator = _searchV2Requester.GetSearchTweetsV2Iterator(parameters);
            var firstResponse = await iterator.NextPageAsync().ConfigureAwait(false);
            return firstResponse?.Content?.Model;
        }

        public ITwitterRequestIterator<SearchTweetsV2Response, string> GetSearchTweetsV2Iterator(string query)
        {
            return GetSearchTweetsV2Iterator(new SearchTweetsV2Parameters(query));
        }

        public ITwitterRequestIterator<SearchTweetsV2Response, string> GetSearchTweetsV2Iterator(ISearchTweetsV2Parameters parameters)
        {
            var iterator = _searchV2Requester.GetSearchTweetsV2Iterator(parameters);
            return new IteratorPageProxy<ITwitterResult<SearchTweetsV2Response>, SearchTweetsV2Response, string>(iterator, input => input.Model);
        }
    }
}