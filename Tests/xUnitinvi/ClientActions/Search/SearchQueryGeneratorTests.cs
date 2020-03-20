using System;
using Tweetinvi;
using Tweetinvi.Controllers.Search;
using Tweetinvi.Models;
using Tweetinvi.Parameters;
using Tweetinvi.Parameters.Enum;
using Xunit;

namespace xUnitinvi.ClientActions.Search
{
    public class SearchQueryGeneratorTests
    {
        private ISearchQueryGenerator _searchQueryGenerator;

        public SearchQueryGeneratorTests()
        {
            var context = new TwitterClient(new TwitterCredentials()).CreateTwitterExecutionContext();
            _searchQueryGenerator = context.Container.Resolve<ISearchQueryGenerator>();
        }

        [Fact]
        public void SearchTweets_WithoutFilters_ReturnsExpectedQuery()
        {
            // arrange
            var parameters = new SearchTweetsParameters("plop")
            {
                Lang = LanguageFilter.French,
                Locale = "ja",
                Since = new DateTime(2000, 01, 11),
                Until = new DateTime(2020, 03, 19),
                GeoCode = new GeoCode(42, 43, 44, DistanceMeasure.Kilometers),
                MaxId = 1042,
                SinceId = 1043,
                SearchType = SearchResultType.Mixed,
                TweetMode = TweetMode.Extended,
                PageSize = 50,
                IncludeEntities = true,
                CustomQueryParameters = { new Tuple<string, string>("hello", "world") }
            };

            // act
            var url = _searchQueryGenerator.GetSearchTweetsQuery(parameters);

            // assert
            Assert.Equal(url, "https://api.twitter.com/1.1/search/tweets.json?q=plop&geocode=42%2C43%2C44km" +
                              "&lang=fr&locale=ja&result_type=mixed&count=50&since_id=1043&max_id=1042" +
                              "&since=2000-01-11&until=2020-03-19&include_entities=true&tweet_mode=extended&hello=world");
        }

        [Fact]
        public void SearchTweets_WithFilters_ReturnsExpectedQuery()
        {
            // arrange
            var parameters = new SearchTweetsParameters("plop")
            {
                Lang = LanguageFilter.French,
                Filters = TweetSearchFilters.Safe,
                Locale = "ja",
                Since = new DateTime(2000, 01, 11),
                Until = new DateTime(2020, 03, 19),
                GeoCode = new GeoCode(42, 43, 44, DistanceMeasure.Kilometers),
                MaxId = 1042,
                SinceId = 1043,
                SearchType = SearchResultType.Mixed,
                TweetMode = TweetMode.Extended,
                PageSize = 50,
                IncludeEntities = true,
                CustomQueryParameters = { new Tuple<string, string>("hello", "world") }
            };

            // act
            var url = _searchQueryGenerator.GetSearchTweetsQuery(parameters);

            // assert
            Assert.Equal(url, "https://api.twitter.com/1.1/search/tweets.json?q=plop%20filter%3Asafe&geocode=42%2C43%2C44km" +
                              "&lang=fr&locale=ja&result_type=mixed&count=50&since_id=1043&max_id=1042" +
                              "&since=2000-01-11&until=2020-03-19&include_entities=true&tweet_mode=extended&hello=world");
        }

        [Fact]
        public void SearchUsers_ReturnsExpectedQuery()
        {
            // arrange
            var parameters = new SearchUsersParameters("plop")
            {
                Page = 42,
                PageSize = 50,
                IncludeEntities = true,
                CustomQueryParameters = { new Tuple<string, string>("hello", "world") }
            };

            // act
            var url = _searchQueryGenerator.GetSearchUsersQuery(parameters);

            // assert
            Assert.Equal(url, "https://api.twitter.com/1.1/users/search.json?q=plop&page=42&count=50&include_entities=true&hello=world");
        }
    }
}