using System.Net.Http;
using System.Threading.Tasks;
using Tweetinvi.Parameters;
using Xunit;
using Xunit.Abstractions;
using xUnitinvi.TestHelpers;

namespace xUnitinvi.EndToEnd
{
    [Collection("EndToEndTests")]
    public class UploadEndToEndTests : TweetinviTest
    {
        public UploadEndToEndTests(ITestOutputHelper logger) : base(logger)
        {
        }

        [Fact]
        public async Task UploadVideo()
        {
            if (!EndToEndTestConfig.ShouldRunEndToEndTests)
                return;

            var httpClient = new HttpClient();

            _logger.WriteLine("Downloading video...");
            var videoBinary = await httpClient.GetByteArrayAsync("https://github.com/linvi/tweetinvi.issues/raw/master/sample_video.mp4");
            _logger.WriteLine("Video downloaded");

            var uploadedVideo = await _tweetinviTestClient.Upload.UploadTweetVideo(videoBinary);
            await _tweetinviTestClient.Upload.WaitForMediaProcessingToGetAllMetadata(uploadedVideo);

            var tweet = await _tweetinviTestClient.Tweets.PublishTweet(new PublishTweetParameters("superb video...")
            {
                Medias = {uploadedVideo}
            });

            await tweet.Destroy();

            Assert.Equal(tweet.Media.Count, 1);
        }
    }
}