using System;
using System.Threading.Tasks;
using Tweetinvi;
using Tweetinvi.Models;
using Xunit;
using Xunit.Abstractions;

namespace xUnitinvi.IntegrationTests
{
    public class AuthIntegrationTests
    {
        private readonly ITestOutputHelper _logger;

        public AuthIntegrationTests(ITestOutputHelper logger)
        {
            _logger = logger;
            _logger.WriteLine(DateTime.Now.ToLongTimeString());

            TweetinviEvents.QueryBeforeExecute += (sender, args) => { _logger.WriteLine(args.Url); };
        }

        [Fact]
        public async Task RunAllAuthTests()
        {
            if (!IntegrationTestConfig.ShouldRunIntegrationTests)
                return;

            _logger.WriteLine($"Starting {nameof(BearerToken)}");
            await BearerToken().ConfigureAwait(false);
            _logger.WriteLine($"{nameof(BearerToken)} succeeded");
        }

        [Fact]
        public async Task BearerToken()
        {
            if (!IntegrationTestConfig.ShouldRunIntegrationTests)
                return;

            var testCreds = IntegrationTestConfig.TweetinviTest.Credentials;
            var appCreds = new TwitterCredentials(testCreds.ConsumerKey, testCreds.ConsumerSecret);

            var appClient = new TwitterClient(appCreds);
            await appClient.Auth.InitializeClientBearerToken();

            var tweet = await appClient.Tweets.GetTweet(979753598446948353);

            // assert
            Assert.Matches("Tweetinvi 3.0", tweet.Text);
        }
    }
}