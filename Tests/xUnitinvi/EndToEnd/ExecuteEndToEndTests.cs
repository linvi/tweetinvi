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
        public async Task ExecuteRequestReturningTwitterResultAsync()
        {
            if (!EndToEndTestConfig.ShouldRunEndToEndTests)
                return;

            var twitterResult = await _tweetinviTestClient.Execute.RequestAsync(request =>
            {
                request.Url = "https://api.twitter.com/1.1/account/verify_credentials.json";
                request.HttpMethod = HttpMethod.GET;
            });

            var userFromJson = _tweetinviTestClient.Json.Deserialize<IUserDTO>(twitterResult.Content);

            var userTwitterResult = await _tweetinviTestClient.Execute.RequestAsync<IUserDTO>(request =>
            {
                request.Url = "https://api.twitter.com/1.1/account/verify_credentials.json";
                request.HttpMethod = HttpMethod.GET;
            });

            var user = _tweetinviTestClient.Factories.CreateUser(userTwitterResult.Model);

            // assert
            Assert.Equal(userFromJson.ScreenName, EndToEndTestConfig.TweetinviTest.AccountId);
            Assert.Equal(userTwitterResult.Model.ScreenName, EndToEndTestConfig.TweetinviTest.AccountId);
            Assert.Equal(user.ScreenName, EndToEndTestConfig.TweetinviTest.AccountId);
        }
    }
}