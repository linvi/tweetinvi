using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Tweetinvi.Core.Iterators;
using Tweetinvi.Core.Web;
using Tweetinvi.Iterators;
using Tweetinvi.Logic;
using Tweetinvi.Models;
using Tweetinvi.Models.DTO;
using Tweetinvi.Parameters;
using Tweetinvi.Parameters.Enum;

namespace Tweetinvi.Client
{
    public class SearchClient : ISearchClient
    {
        private readonly ITwitterClient _client;

        public SearchClient(ITwitterClient client)
        {
            _client = client;
        }

        public Task<ITweet[]> SearchTweets(string query)
        {
            return SearchTweets(new SearchTweetsParameters(query));
        }

        public Task<ITweet[]> SearchTweets(IGeoCode geoCode)
        {
            return SearchTweets(new SearchTweetsParameters(geoCode));
        }

        public async Task<ITweet[]> SearchTweets(ISearchTweetsParameters parameters)
        {
            var iterator = GetSearchTweetsIterator(parameters);
            return (await iterator.MoveToNextPage().ConfigureAwait(false)).ToArray();
        }

        public Task<ISearchResults> SearchTweetsWithMetadata(string query)
        {
            return SearchTweetsWithMetadata(new SearchTweetsParameters(query));
        }

        public async Task<ISearchResults> SearchTweetsWithMetadata(ISearchTweetsParameters parameters)
        {
            var pageIterator = _client.Raw.Search.GetSearchTweetsIterator(parameters);
            var page = await pageIterator.MoveToNextPage().ConfigureAwait(false);
            return _client.Factories.CreateSearchResult(page?.Content?.DataTransferObject);
        }

        public ITwitterIterator<ITweet, long?> GetSearchTweetsIterator(string query)
        {
            return GetSearchTweetsIterator(new SearchTweetsParameters(query));
        }

        public ITwitterIterator<ITweet, long?> GetSearchTweetsIterator(ISearchTweetsParameters parameters)
        {
            var pageIterator = _client.Raw.Search.GetSearchTweetsIterator(parameters);
            return new TwitterIteratorProxy<ITwitterResult<ISearchResultsDTO>, ITweet, long?>(pageIterator,
                twitterResult => _client.Factories.CreateTweets(twitterResult?.DataTransferObject?.TweetDTOs));
        }

        public ITweet[] FilterTweets(ITweet[] tweets, OnlyGetTweetsThatAre? filter, bool tweetsMustContainGeoInformation)
        {
            IEnumerable<ITweet> matchingTweets = tweets;

            if (filter == OnlyGetTweetsThatAre.OriginalTweets)
            {
                matchingTweets = matchingTweets.Where(x => x.RetweetedTweet == null).ToArray();
            }

            if (filter == OnlyGetTweetsThatAre.Retweets)
            {
                matchingTweets = matchingTweets.Where(x => x.RetweetedTweet != null).ToArray();
            }

            if (matchingTweets != null && tweetsMustContainGeoInformation)
            {
                matchingTweets = matchingTweets.Where(x => x.Coordinates != null || x.Place != null);
            }

            return matchingTweets?.ToArray();
        }
    }
}