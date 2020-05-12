using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Tweetinvi.Core.Client.Validators;
using Tweetinvi.Core.DTO;
using Tweetinvi.Core.Iterators;
using Tweetinvi.Core.Web;
using Tweetinvi.Iterators;
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

        public ISearchClientParametersValidator ParametersValidator => _client.ParametersValidator;

        public Task<ITweet[]> SearchTweetsAsync(string query)
        {
            return SearchTweetsAsync(new SearchTweetsParameters(query));
        }

        public Task<ITweet[]> SearchTweetsAsync(IGeoCode geoCode)
        {
            return SearchTweetsAsync(new SearchTweetsParameters(geoCode));
        }

        public async Task<ITweet[]> SearchTweetsAsync(ISearchTweetsParameters parameters)
        {
            var iterator = GetSearchTweetsIterator(parameters);
            return (await iterator.NextPageAsync().ConfigureAwait(false)).ToArray();
        }

        public Task<ISearchResults> SearchTweetsWithMetadataAsync(string query)
        {
            return SearchTweetsWithMetadataAsync(new SearchTweetsParameters(query));
        }

        public async Task<ISearchResults> SearchTweetsWithMetadataAsync(ISearchTweetsParameters parameters)
        {
            var pageIterator = _client.Raw.Search.GetSearchTweetsIterator(parameters);
            var page = await pageIterator.NextPageAsync().ConfigureAwait(false);
            return _client.Factories.CreateSearchResult(page?.Content?.Model);
        }

        public ITwitterIterator<ITweet, long?> GetSearchTweetsIterator(string query)
        {
            return GetSearchTweetsIterator(new SearchTweetsParameters(query));
        }

        public ITwitterIterator<ITweet, long?> GetSearchTweetsIterator(ISearchTweetsParameters parameters)
        {
            var pageIterator = _client.Raw.Search.GetSearchTweetsIterator(parameters);
            return new TwitterIteratorProxy<ITwitterResult<ISearchResultsDTO>, ITweet, long?>(pageIterator,
                twitterResult => _client.Factories.CreateTweets(twitterResult?.Model?.TweetDTOs));
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

        public Task<IUser[]> SearchUsersAsync(string query)
        {
            return SearchUsersAsync(new SearchUsersParameters(query));
        }

        public async Task<IUser[]> SearchUsersAsync(ISearchUsersParameters parameters)
        {
            var pageIterator = GetSearchUsersIterator(parameters);
            return (await pageIterator.NextPageAsync().ConfigureAwait(false)).ToArray();
        }

        public ITwitterIterator<IUser, int?> GetSearchUsersIterator(string query)
        {
            return GetSearchUsersIterator(new SearchUsersParameters(query));
        }

        public ITwitterIterator<IUser, int?> GetSearchUsersIterator(ISearchUsersParameters parameters)
        {
            var pageIterator = _client.Raw.Search.GetSearchUsersIterator(parameters);
            return new TwitterIteratorProxy<IFilteredTwitterResult<UserDTO[]>, IUser, int?>(pageIterator,
                twitterResult => _client.Factories.CreateUsers(twitterResult?.FilteredDTO));
        }

        public Task<ISavedSearch> CreateSavedSearchAsync(string query)
        {
            return CreateSavedSearchAsync(new CreateSavedSearchParameters(query));
        }

        public async Task<ISavedSearch> CreateSavedSearchAsync(ICreateSavedSearchParameters parameters)
        {
            var twitterResult = await _client.Raw.Search.CreateSavedSearchAsync(parameters).ConfigureAwait(false);
            return _client.Factories.CreateSavedSearch(twitterResult?.Model);
        }

        public Task<ISavedSearch> GetSavedSearchAsync(long savedSearchId)
        {
            return GetSavedSearchAsync(new GetSavedSearchParameters(savedSearchId));
        }

        public async Task<ISavedSearch> GetSavedSearchAsync(IGetSavedSearchParameters parameters)
        {
            var twitterResult = await _client.Raw.Search.GetSavedSearchAsync(parameters).ConfigureAwait(false);
            return _client.Factories.CreateSavedSearch(twitterResult?.Model);
        }

        public Task<ISavedSearch[]> ListSavedSearchesAsync()
        {
            return ListSavedSearchesAsync(new ListSavedSearchesParameters());
        }

        public async Task<ISavedSearch[]> ListSavedSearchesAsync(IListSavedSearchesParameters parameters)
        {
            var twitterResult = await _client.Raw.Search.ListSavedSearchesAsync(parameters).ConfigureAwait(false);
            return twitterResult?.Model?.Select(_client.Factories.CreateSavedSearch).ToArray();
        }

        public Task<ISavedSearch> DestroySavedSearchAsync(long savedSearchId)
        {
            return DestroySavedSearchAsync(new DestroySavedSearchParameters(savedSearchId));
        }

        public Task<ISavedSearch> DestroySavedSearchAsync(ISavedSearch savedSearch)
        {
            return DestroySavedSearchAsync(new DestroySavedSearchParameters(savedSearch));
        }

        public async Task<ISavedSearch> DestroySavedSearchAsync(IDestroySavedSearchParameters parameters)
        {
            var twitterResult = await _client.Raw.Search.DestroySavedSearchAsync(parameters).ConfigureAwait(false);
            return _client.Factories.CreateSavedSearch(twitterResult?.Model);
        }
    }
}