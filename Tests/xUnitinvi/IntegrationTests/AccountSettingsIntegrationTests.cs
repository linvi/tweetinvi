using System;
using System.IO;
using System.Threading.Tasks;
using Tweetinvi;
using Xunit;
using Xunit.Abstractions;

namespace xUnitinvi.IntegrationTests
{
    public class AccountSettingsIntegrationTests
    {
        private readonly ITestOutputHelper _logger;
        private readonly ITwitterClient Client;

        public AccountSettingsIntegrationTests(ITestOutputHelper logger)
        {
            _logger = logger;
            _logger.WriteLine(DateTime.Now.ToLongTimeString());
            Client = new TwitterClient(IntegrationTestCredentials.ProtectedUserCredentials);
        }

        //        [Fact]
        [Fact(Skip = "Integration Tests")]
        public async Task RunAllAccountSettings()
        {
            TweetinviEvents.QueryBeforeExecute += (sender, args) => { _logger.WriteLine(args.Url); };

            await ChangeImages();
        }

        [Fact(Skip = "Integration Tests")]
        public async Task ChangeImages()
        {
            // act
            var user = await Client.Account.GetAuthenticatedUser();
            var profile = File.ReadAllBytes("./tweetinvi-logo-purple.png");
            var banner = File.ReadAllBytes("./banner.jpg");
            await Client.AccountSettings.UpdateProfileImage(profile);
            var success = await Client.AccountSettings.UpdateProfileBanner(banner);
            var userAfter = await Client.Account.GetAuthenticatedUser();

            // assert
            Assert.NotEqual(user.ProfileImageUrl, userAfter.ProfileImageUrl);
            Assert.NotEqual(user.ProfileBannerURL, userAfter.ProfileBannerURL);
        }
    }
}