using System.Linq;
using System.Threading.Tasks;
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

            await searchTweetsIterator.MoveToNextPage();
            var result2 = (await searchTweetsIterator.MoveToNextPage()).ToArray();

            var geoSearchTweets = await _tweetinviClient.Search.SearchTweets(new GeoCode
            {
                Coordinates = new Coordinates(37.781157, -122.398720),
                Radius = 100,
                DistanceMeasure = DistanceMeasure.Kilometers
            });

            var searchWithMetadata = await _tweetinviClient.Search.SearchTweetsWithMetadata("hello");


            // assert

            Assert.True(tweets.Length > 0);
            Assert.True(geoSearchTweets.Length > 0);
            Assert.Contains(tweets.Select(x => x.Id), x => result2.Any(tweet => tweet.Id == x));
        }

        [Fact]
        public async Task SearchWithFilters()
        {
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
            var searchWithMetadata = await _tweetinviClient.Search.SearchTweetsWithMetadata("hello");

            // assert
            Assert.True(searchWithMetadata.Tweets.Length > 0);
        }
    }
}