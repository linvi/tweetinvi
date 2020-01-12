using System.Threading.Tasks;
using Tweetinvi;
using Tweetinvi.Models;
using Tweetinvi.Models.DTO;
using Xunit;
using Xunit.Abstractions;
using xUnitinvi.TestHelpers;

namespace xUnitinvi.EndToEnd
{
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

            await client.Execute.Request(request =>
            {
                request.Query.Url = "https://api.twitter.com/1.1/account/verify_credentials.json";
                request.Query.HttpMethod = HttpMethod.GET;
            });


            var userDTO = await client.Execute.Request<IUserDTO>(request =>
            {
                request.Query.Url = "https://api.twitter.com/1.1/account/verify_credentials.json";
                request.Query.HttpMethod = HttpMethod.GET;
            });

            // assert
            Assert.Equal(userDTO.DataTransferObject.ScreenName, EndToEndTestConfig.TweetinviTest.AccountId);
        }
    }
}