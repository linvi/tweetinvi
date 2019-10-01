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

        // [Fact]
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
            var authenticatedUser = await Client.Account.GetAuthenticatedUser();
            var profile = File.ReadAllBytes("./tweetinvi-logo-purple.png");
            var banner = File.ReadAllBytes("./banner.jpg");
            await Client.AccountSettings.UpdateProfileImage(profile);
            await Client.AccountSettings.UpdateProfileBanner(banner);
            var userAfterAddingBanner = await Client.Users.GetUser(authenticatedUser);
            await Client.AccountSettings.RemoveProfileBanner();
            var userAfterRemovingBanner = await Client.Users.GetUser(authenticatedUser);

            // assert
            Assert.NotEqual(authenticatedUser.ProfileImageUrl, userAfterAddingBanner.ProfileImageUrl);
            Assert.NotEqual(authenticatedUser.ProfileBannerURL, userAfterAddingBanner.ProfileBannerURL);
            Assert.Null(userAfterRemovingBanner.ProfileBannerURL);
        }
    }
}