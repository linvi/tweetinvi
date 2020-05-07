using System.Threading.Tasks;
using Tweetinvi.Models;
using Tweetinvi.Parameters;
using Xunit;
using Xunit.Abstractions;
using xUnitinvi.TestHelpers;

namespace xUnitinvi.EndToEnd
{
    [Collection("EndToEndTests")]
    public class HelpEndToEndTests : TweetinviTest
    {
        public HelpEndToEndTests(ITestOutputHelper logger) : base(logger)
        {
        }

        [Fact]
        public async Task GetTwitterConfigurationAsync()
        {
            if (!EndToEndTestConfig.ShouldRunEndToEndTests)
                return;

            var twitterConfiguration = await _tweetinviTestClient.Help.GetTwitterConfigurationAsync();

            Assert.True(twitterConfiguration.PhotoSizeLimit > 0);
        }

        [Fact]
        public async Task GetSupportedLanguagesAsync()
        {
            if (!EndToEndTestConfig.ShouldRunEndToEndTests)
                return;

            var supportedLanguages = await _tweetinviTestClient.Help.GetSupportedLanguagesAsync();

            Assert.Contains(supportedLanguages, x => x.Name == "French");
        }

        [Fact]
        public async Task GeoAsync()
        {
            if (!EndToEndTestConfig.ShouldRunEndToEndTests)
                return;

            var place = await _tweetinviTestClient.Help.GetPlaceAsync("df51dec6f4ee2b2c");
            var geoSearch = await _tweetinviTestClient.Help.SearchGeoAsync(new GeoSearchParameters
            {
                Query = "Toronto"
            });

            var sanFranciscoCoordinates = new Coordinates(37.781157, -122.398720);
            var geoSearchReverse = await _tweetinviTestClient.Help.SearchGeoReverseAsync(new GeoSearchReverseParameters(sanFranciscoCoordinates)
            {
                Granularity = Granularity.Neighborhood
            });

            Assert.Equal(place.CountryCode, "US");
            Assert.True(geoSearch.Length > 0);
            Assert.True(geoSearchReverse.Length > 0);
        }
    }
}