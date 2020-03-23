using System;
using Tweetinvi;
using Tweetinvi.Controllers.Trends;
using Tweetinvi.Parameters;
using Tweetinvi.Parameters.TrendsClient;
using Xunit;
using xUnitinvi.TestHelpers;

namespace xUnitinvi.ClientActions.TrendsClient
{
    public class TrendsQueryGeneratorTests
    {
        public TrendsQueryGeneratorTests()
        {
            _fakeBuilder = new FakeClassBuilder<TrendsQueryGenerator>();
        }

        private readonly FakeClassBuilder<TrendsQueryGenerator> _fakeBuilder;

        private TrendsQueryGenerator CreateTrendsQueryGenerator()
        {
            return _fakeBuilder.GenerateClass();
        }

        [Fact]
        public void GetTrendsAtQuery_ReturnsExpectedQuery()
        {
            // Arrange
            var queryGenerator = CreateTrendsQueryGenerator();

            var parameters = new GetTrendsAtParameters(42)
            {
                Exclude = GetTrendsExclude.Hashtags,
                CustomQueryParameters = { new Tuple<string, string>("hello", "world") }
            };

            // Act
            var result = queryGenerator.GetTrendsAtQuery(parameters);

            // Assert
            Assert.Equal(result, "https://api.twitter.com/1.1/trends/place.json?id=42&exclude=hashtags&hello=world");
        }

        [Fact]
        public void GetTrendsLocationQuery_ReturnsExpectedQuery()
        {
            // Arrange
            var queryGenerator = CreateTrendsQueryGenerator();

            var parameters = new GetTrendsLocationParameters
            {
                CustomQueryParameters = { new Tuple<string, string>("hello", "world") }
            };

            // Act
            var result = queryGenerator.GetTrendsLocationQuery(parameters);

            // Assert
            Assert.Equal(result, "https://api.twitter.com/1.1/trends/available.json?hello=world");
        }

        [Fact]
        public void GetTrendsLocationCloseToQuery_ReturnsExpectedQuery()
        {
            // Arrange
            var queryGenerator = CreateTrendsQueryGenerator();

            var parameters = new GetTrendsLocationCloseToParameters(42,43)
            {
                CustomQueryParameters = { new Tuple<string, string>("hello", "world") }
            };

            // Act
            var result = queryGenerator.GetTrendsLocationCloseToQuery(parameters);

            // Assert
            Assert.Equal(result, "https://api.twitter.com/1.1/trends/closest.json?lat=42&long=43&hello=world");
        }
    }
}