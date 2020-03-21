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
        public async Task GetTwitterConfiguration()
        {
            if (!EndToEndTestConfig.ShouldRunEndToEndTests)
                return;

            var twitterConfiguration = await _tweetinviTestClient.Help.GetTwitterConfiguration();

            Assert.True(twitterConfiguration.PhotoSizeLimit > 0);
        }

        [Fact]
        public async Task GetSupportedLanguages()
        {
            if (!EndToEndTestConfig.ShouldRunEndToEndTests)
                return;

            var supportedLanguages = await _tweetinviTestClient.Help.GetSupportedLanguages();

            Assert.Contains(supportedLanguages, x => x.Name == "French");
        }

        [Fact]
        public async Task Geo()
        {
            if (!EndToEndTestConfig.ShouldRunEndToEndTests)
                return;

            var place = await _tweetinviTestClient.Help.GetPlace("df51dec6f4ee2b2c");
            var geoSearch = await _tweetinviTestClient.Help.SearchGeo(new GeoSearchParameters
            {
                Query = "Toronto"
            });

            var sanFranciscoCoordinates = new Coordinates(37.781157, -122.398720);
            var geoSearchReverse = await _tweetinviTestClient.Help.SearchGeoReverse(new GeoSearchReverseParameters(sanFranciscoCoordinates)
            {
                Granularity = Granularity.Neighborhood
            });

            Assert.Equal(place.CountryCode, "US");
            Assert.True(geoSearch.Length > 0);
            Assert.True(geoSearchReverse.Length > 0);
        }
    }
}