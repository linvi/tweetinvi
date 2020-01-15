using System;
using Tweetinvi.Controllers.Help;
using Tweetinvi.Parameters;
using Xunit;
using xUnitinvi.TestHelpers;

namespace xUnitinvi.ClientActions.HelpClient
{
    public class HelpQueryGeneratorTests
    {
        public HelpQueryGeneratorTests()
        {
            _fakeBuilder = new FakeClassBuilder<HelpQueryGenerator>();
        }

        private readonly FakeClassBuilder<HelpQueryGenerator> _fakeBuilder;

        private HelpQueryGenerator CreateHelpQueryGenerator()
        {
            return _fakeBuilder.GenerateClass();
        }

        [Fact]
        public void GetRateLimitsQuery_ReturnsExpectedQuery()
        {
            // arrange
            var queryGenerator = CreateHelpQueryGenerator();
            var parameters = new GetRateLimitsParameters
            {
                CustomQueryParameters = { new Tuple<string, string>("hello", "world") }
            };

            // act
            var result = queryGenerator.GetRateLimitsQuery(parameters);

            // assert
            Assert.Equal(result, $"https://api.twitter.com/1.1/application/rate_limit_status.json?hello=world");
        }

        [Fact]
        public void GetTwitterConfigurationQuery_ReturnsExpectedQuery()
        {
            // arrange
            var queryGenerator = CreateHelpQueryGenerator();
            var parameters = new GetTwitterConfigurationParameters
            {
                CustomQueryParameters = { new Tuple<string, string>("hello", "world") }
            };

            // act
            var result = queryGenerator.GetTwitterConfigurationQuery(parameters);

            // assert
            Assert.Equal(result, $"https://api.twitter.com/1.1/help/configuration.json?hello=world");
        }

        [Fact]
        public void GetSupportedLanguagesQuery_ReturnsExpectedQuery()
        {
            // arrange
            var queryGenerator = CreateHelpQueryGenerator();
            var parameters = new GetSupportedLanguagesParameters
            {
                CustomQueryParameters = { new Tuple<string, string>("hello", "world") }
            };

            // act
            var result = queryGenerator.GetSupportedLanguagesQuery(parameters);

            // assert
            Assert.Equal(result, $"https://api.twitter.com/1.1/help/languages.json?hello=world");
        }
    }
}