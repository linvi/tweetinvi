using System.Linq;
using System.Threading.Tasks;
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
            var twitterConfiguration = await _tweetinviTestClient.Help.GetTwitterConfiguration();

            Assert.True(twitterConfiguration.PhotoSizeLimit > 0);
        }

        [Fact]
        public async Task GetSupportedLanguages()
        {
            var supportedLanguages = await _tweetinviTestClient.Help.GetSupportedLanguages();

            Assert.Contains(supportedLanguages, x => x.Name == "French");
        }
    }
}