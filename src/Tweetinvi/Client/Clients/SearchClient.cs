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

        public Task<IUser[]> SearchUsers(string query)
        {
            return SearchUsers(new SearchUsersParameters(query));
        }

        public async Task<IUser[]> SearchUsers(ISearchUsersParameters parameters)
        {
            var pageIterator = GetSearchUsersIterator(parameters);
            return (await pageIterator.MoveToNextPage().ConfigureAwait(false)).ToArray();
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

        public Task<ISavedSearch> CreateSavedSearch(string query)
        {
            return CreateSavedSearch(new CreateSavedSearchParameters(query));
        }

        public async Task<ISavedSearch> CreateSavedSearch(ICreateSavedSearchParameters parameters)
        {
            var twitterResult = await _client.Raw.Search.CreateSavedSearch(parameters).ConfigureAwait(false);
            return _client.Factories.CreateSavedSearch(twitterResult?.DataTransferObject);
        }

        public Task<ISavedSearch> GetSavedSearch(long savedSearchId)
        {
            return GetSavedSearch(new GetSavedSearchParameters(savedSearchId));
        }

        public async Task<ISavedSearch> GetSavedSearch(IGetSavedSearchParameters parameters)
        {
            var twitterResult = await _client.Raw.Search.GetSavedSearch(parameters).ConfigureAwait(false);
            return _client.Factories.CreateSavedSearch(twitterResult?.DataTransferObject);
        }

        public Task<ISavedSearch[]> ListSavedSearches()
        {
            return ListSavedSearches(new ListSavedSearchesParameters());
        }

        public async Task<ISavedSearch[]> ListSavedSearches(IListSavedSearchesParameters parameters)
        {
            var twitterResult = await _client.Raw.Search.ListSavedSearches(parameters).ConfigureAwait(false);
            return twitterResult?.DataTransferObject?.Select(_client.Factories.CreateSavedSearch).ToArray();
        }

        public Task<ISavedSearch> DestroySavedSearch(long savedSearchId)
        {
            return DestroySavedSearch(new DestroySavedSearchParameters(savedSearchId));
        }

        public Task<ISavedSearch> DestroySavedSearch(ISavedSearch savedSearch)
        {
            return DestroySavedSearch(new DestroySavedSearchParameters(savedSearch));
        }

        public async Task<ISavedSearch> DestroySavedSearch(IDestroySavedSearchParameters parameters)
        {
            var twitterResult = await _client.Raw.Search.DestroySavedSearch(parameters).ConfigureAwait(false);
            return _client.Factories.CreateSavedSearch(twitterResult?.DataTransferObject);
        }
    }
}