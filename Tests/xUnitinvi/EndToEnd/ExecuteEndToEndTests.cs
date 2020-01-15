using System.Threading.Tasks;
using Tweetinvi;
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
            var testCreds = EndToEndTestConfig.TweetinviTest.Credentials;
            var client = new TwitterClient(testCreds);

            var twitterResult = await client.Execute.Request(request =>
            {
                request.Query.Url = "https://api.twitter.com/1.1/account/verify_credentials.json";
                request.Query.HttpMethod = HttpMethod.GET;
            });

            var userFromJson = client.Json.DeserializeObject<IUserDTO>(twitterResult.RawResult);

            var userTwitterResult = await client.Execute.Request<IUserDTO>(request =>
            {
                request.Query.Url = "https://api.twitter.com/1.1/account/verify_credentials.json";
                request.Query.HttpMethod = HttpMethod.GET;
            });

            var user = client.Factories.CreateUser(userTwitterResult.DataTransferObject);

            // assert
            Assert.Equal(userFromJson.ScreenName, EndToEndTestConfig.TweetinviTest.AccountId);
            Assert.Equal(userTwitterResult.DataTransferObject.ScreenName, EndToEndTestConfig.TweetinviTest.AccountId);
            Assert.Equal(user.ScreenName, EndToEndTestConfig.TweetinviTest.AccountId);
        }
    }
}