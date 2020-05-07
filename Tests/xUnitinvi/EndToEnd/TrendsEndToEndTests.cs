using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;
using xUnitinvi.TestHelpers;

namespace xUnitinvi.EndToEnd
{
    [Collection("EndToEndTests")]
    public class TrendsEndToEndTests : TweetinviTest
    {
        public TrendsEndToEndTests(ITestOutputHelper logger) : base(logger)
        {
        }

        [Fact]
        public async Task TrendsAsync()
        {
            if (!EndToEndTestConfig.ShouldRunEndToEndTests)
                return;

            var trends = await _tweetinviClient.Trends.GetPlaceTrendsAtAsync(1);
            var availableLocations = await _tweetinviClient.Trends.GetTrendLocationsAsync();
            var locationsCloseTo = await _tweetinviClient.Trends.GetTrendsLocationCloseToAsync(37.781157, -122.400612831116);

            Assert.True(trends.Trends.Length > 0);
            Assert.True(availableLocations.Length > 0);
            Assert.True(locationsCloseTo.Length > 0);
        }
    }
}