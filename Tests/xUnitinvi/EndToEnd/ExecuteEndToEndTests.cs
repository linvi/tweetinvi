using System.Threading.Tasks;
using Tweetinvi.Models;
using Tweetinvi.Models.DTO;
using Xunit;
using Xunit.Abstractions;
using xUnitinvi.TestHelpers;

namespace xUnitinvi.EndToEnd
{
    [Collection("EndToEndTests")]
    public class ExecuteEndToEndTests : TweetinviTest
    {
        public ExecuteEndToEndTests(ITestOutputHelper logger) : base(logger)
        {
        }

        [Fact]
        public async Task ExecuteRequestReturningTwitterResult()
        {
            if (!EndToEndTestConfig.ShouldRunEndToEndTests)
                return;

            var twitterResult = await _tweetinviTestClient.Execute.Request(request =>
            {
                request.Query.Url = "https://api.twitter.com/1.1/account/verify_credentials.json";
                request.Query.HttpMethod = HttpMethod.GET;
            });

            var userFromJson = _tweetinviTestClient.Json.Deserialize<IUserDTO>(twitterResult.RawResult);

            var userTwitterResult = await _tweetinviTestClient.Execute.Request<IUserDTO>(request =>
            {
                request.Query.Url = "https://api.twitter.com/1.1/account/verify_credentials.json";
                request.Query.HttpMethod = HttpMethod.GET;
            });

            var user = _tweetinviTestClient.Factories.CreateUser(userTwitterResult.DataTransferObject);

            // assert
            Assert.Equal(userFromJson.ScreenName, EndToEndTestConfig.TweetinviTest.AccountId);
            Assert.Equal(userTwitterResult.DataTransferObject.ScreenName, EndToEndTestConfig.TweetinviTest.AccountId);
            Assert.Equal(user.ScreenName, EndToEndTestConfig.TweetinviTest.AccountId);
        }
    }
}