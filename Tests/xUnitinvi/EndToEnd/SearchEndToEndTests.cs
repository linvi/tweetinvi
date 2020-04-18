using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Tweetinvi.Core.Extensions;
using Tweetinvi.Models;
using Tweetinvi.Parameters;
using Tweetinvi.Parameters.Enum;
using Xunit;
using Xunit.Abstractions;
using xUnitinvi.TestHelpers;

namespace xUnitinvi.EndToEnd
{
    [Collection("EndToEndTests")]
    public class SearchEndToEndTests : TweetinviTest
    {
        public SearchEndToEndTests(ITestOutputHelper logger) : base(logger)
        {
        }

        [Fact]
        public async Task SearchTweets()
        {
            if (!EndToEndTestConfig.ShouldRunEndToEndTests)
                return;

            var tweets = await _tweetinviClient.Search.SearchTweets("hello");
            var searchTweetsIterator = _tweetinviClient.Search.GetSearchTweetsIterator(new SearchTweetsParameters("hello")
            {
                PageSize = 50,
            });

            await searchTweetsIterator.NextPage();
            var result2 = (await searchTweetsIterator.NextPage()).ToArray();

            var geoSearchTweets = await _tweetinviClient.Search.SearchTweets(new GeoCode
            {
                Coordinates = new Coordinates(37.781157, -122.398720),
                Radius = 100,
                DistanceMeasure = DistanceMeasure.Kilometers
            });

            // assert
            Assert.True(tweets.Length > 0);
            Assert.True(geoSearchTweets.Length > 0);
            Assert.Contains(tweets.Select(x => x.Id), x => result2.Any(tweet => tweet.Id == x));
        }

        [Fact]
        public async Task SearchWithFilters()
        {
            if (!EndToEndTestConfig.ShouldRunEndToEndTests)
                return;

            var searchWithMetadata = await _tweetinviClient.Search.SearchTweetsWithMetadata(new SearchTweetsParameters("hello")
            {
                Filters = TweetSearchFilters.Safe
            });

            // assert
            Assert.True(searchWithMetadata.Tweets.Length > 0);
        }

        [Fact]
        public async Task SearchTweetsWithMetadata()
        {
            if (!EndToEndTestConfig.ShouldRunEndToEndTests)
                return;

            var searchWithMetadata = await _tweetinviClient.Search.SearchTweetsWithMetadata("hello");

            // assert
            Assert.True(searchWithMetadata.Tweets.Length > 0);
        }

        [Fact]
        public async Task SearchUsers()
        {
            if (!EndToEndTestConfig.ShouldRunEndToEndTests)
                return;

            var users = await _tweetinviClient.Search.SearchUsers("bob");
            var searchUsersIterator = _tweetinviClient.Search.GetSearchUsersIterator(new SearchUsersParameters("bob")
            {
                PageSize = 10
            });

            var result1 = (await searchUsersIterator.NextPage()).ToArray();
            var result2 = (await searchUsersIterator.NextPage()).ToArray();

            // assert
            Assert.True(users.Length > 0);
            Assert.Equal(searchUsersIterator.NextCursor, 2);
            Assert.Contains(users.Select(x => x.Id), x => result1.Any(tweet => tweet.Id == x));
            Assert.Contains(users.Select(x => x.Id), x => result2.Any(tweet => tweet.Id == x));
        }

        [Fact]
        public async Task SavedSearch()
        {
            if (!EndToEndTestConfig.ShouldRunEndToEndTests)
                return;

            var savedSearchesBefore = await _tweetinviTestClient.Search.ListSavedSearches();
            var createdSavedSearch = await _tweetinviTestClient.Search.CreateSavedSearch("tweetinvi");
            var savedSearch = await _tweetinviTestClient.Search.GetSavedSearch(createdSavedSearch.Id);
            var savedSearchesDuring = await _tweetinviTestClient.Search.ListSavedSearches();
            var deletedSavedSearch = await _tweetinviTestClient.Search.DestroySavedSearch(savedSearch);
            await Task.Delay(1000);
            var savedSearchesAfter = await _tweetinviTestClient.Search.ListSavedSearches();

            // assert
            Assert.Equal(savedSearchesDuring.Length, savedSearchesBefore.Length + 1);
            Assert.Equal(savedSearchesAfter.Length, savedSearchesBefore.Length);
            Assert.Equal(createdSavedSearch.Query, savedSearch.Query);
            Assert.Equal(createdSavedSearch.Query, deletedSavedSearch.Query);
        }
    }
}