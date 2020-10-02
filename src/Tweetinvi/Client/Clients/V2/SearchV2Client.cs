using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Xml;
using Tweetinvi.Client.Requesters.V2;
using Tweetinvi.Core.Iterators;
using Tweetinvi.Core.Web;
using Tweetinvi.Iterators;
using Tweetinvi.Models;
using Tweetinvi.Models.DTO;
using Tweetinvi.Models.V2.Responses;
using Tweetinvi.Parameters.V2;

namespace Tweetinvi.Client.V2
{
    public class SearchIteratorPage : ITwitterIteratorPage<SearchTweetsResponseDTO, string>
    {
        public string NextCursor { get; }
        public bool IsLastPage { get; }
        public SearchTweetsResponseDTO Response { get; }
    }

    public class SearchV2Client : ISearchV2Client
    {
        private readonly ISearchV2Requester _searchV2Requester;

        public SearchV2Client(ISearchV2Requester searchV2Requester)
        {
            _searchV2Requester = searchV2Requester;
        }

        public async Task<SearchTweetsResponseDTO> SearchTweetsAsync(ISearchTweetsV2Parameters parameters)
        {
            var iterator = GetSearchTweetsV2Iterator(parameters);
            return (await iterator.NextPageAsync().ConfigureAwait(false)).ToArray();
        }

        public async Task Plop()
        {
            var responses = new List<SearchTweetsResponseDTO>();
            var iterator = GetSearchTweetsV2Iterator(null);

            while (!iterator.Completed)
            {
                var response = await iterator.NextPageAsync();
                responses.Add(response.Tweets);
            }
        }

        public ITwitterSimpleIterator<SearchTweetsIteratorResponseDTO, string> GetSearchTweetsV2Iterator(ISearchTweetsV2Parameters parameters)
        {
            var pageIterator = _searchV2Requester.GetSearchTweetsV2Iterator(parameters);
            var p = new List<SearchTweetsResponseDTO>();

            while (!pageIterator.Completed)
            {
                var result = await
            }
        }

        public class TwitterSimpleIterator<TItem, TCursor> : ITwitterSimpleIterator<TItem, TCursor> where TItem: ITwitterSimpleIteratorResult<TCursor>
        {
            public TwitterSimpleIterator(ITwitterPageIterator<ITwitterResult<SearchTweetsResponseDTO>, string>)
            {

            }

            public TCursor NextCursor { get; }
            public bool Completed { get; }
            public Task<TItem> NextPageAsync()
            {

            }
        }
    }
}