using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Tweetinvi;
using Tweetinvi.Models;
using Tweetinvi.Parameters;
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
            TweetinviEvents.QueryBeforeExecute += (sender, args) => { _logger.WriteLine($"{args.TwitterQuery.HttpMethod} : {args.Url}"); };

            await ChangeImagesTests().ConfigureAwait(false);
            await AccountSettingsTests().ConfigureAwait(false);
            await AccountProfileTests().ConfigureAwait(false);
        }

        [Fact(Skip = "Integration Tests")]
        public async Task ChangeImagesTests()
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

        [Fact(Skip = "Integration Tests")]
        public async Task AccountProfileTests()
        {
            var initialProfile = await Client.Account.GetAuthenticatedUser();

            // act
            var updatedProfileParameters = new UpdateProfileParameters
            {
                Name = $"{initialProfile.Name}_42",
                Description = "new_desc",
                Location = "new_loc",
                WebsiteUrl = "https://www.twitter.com/artwolkt",
                ProfileLinkColor = "F542B9"
            };

            var newProfile = await Client.AccountSettings.UpdateProfile(updatedProfileParameters);
            
            var restoredProfileParameters = new UpdateProfileParameters
            {
                Name = initialProfile.Name,
                Description = initialProfile.Description,
                Location = initialProfile.Location,
                WebsiteUrl = initialProfile.Url,
                ProfileLinkColor = initialProfile.ProfileLinkColor
            };

            var restoredProfile = await Client.AccountSettings.UpdateProfile(restoredProfileParameters);
            
            // assert
            Assert.Equal($"{initialProfile.Name}_42", newProfile.Name);
            Assert.NotEqual(initialProfile.Name, newProfile.Name);
            Assert.Equal(initialProfile.Name, restoredProfile.Name);
            
            Assert.Equal("new_desc", newProfile.Description);
            Assert.NotEqual(initialProfile.Description, updatedProfileParameters.Description);
            Assert.Equal(initialProfile.Description, newProfile.Description);
            
            Assert.Equal("new_loc", newProfile.Location);
            Assert.NotEqual(initialProfile.Location, newProfile.Location);
            Assert.Equal(initialProfile.Location, restoredProfile.Location);
            
            Assert.Equal("new_url", newProfile.Url);
            Assert.NotEqual(initialProfile.Url, newProfile.Url);
            Assert.Equal(initialProfile.Url, restoredProfile.Url);
            
            Assert.Equal("blue", newProfile.ProfileLinkColor);
            Assert.NotEqual(initialProfile.ProfileLinkColor, newProfile.ProfileLinkColor);
            Assert.Equal(initialProfile.ProfileLinkColor, restoredProfile.ProfileLinkColor);
        }
        
        [Fact(Skip = "Integration Tests")]
        public async Task AccountSettingsTests()
        {
            var initialSettings = await Client.AccountSettings.GetAccountSettings();

            var newSettings = new UpdateAccountSettingsParameters
            {
                DisplayLanguage = DisplayLanguages.Spanish,
                SleepTimeEnabled = !initialSettings.SleepTimeEnabled,
                StartSleepHour = 23,
                EndSleepHour = 7,
                TimeZone = TimeZoneFromTwitter.Bangkok.ToTZinfo(),
                TrendLocationWoeid = 580778
            };

            var updatedSettings = await Client.AccountSettings.UpdateAccountSettings(newSettings);

            var recoveredSettings = await Client.AccountSettings.UpdateAccountSettings(new UpdateAccountSettingsParameters
            {
                DisplayLanguage = initialSettings.Language,
                TimeZone = initialSettings.TimeZone.TzinfoName,
                SleepTimeEnabled = initialSettings.SleepTimeEnabled,
                StartSleepHour = initialSettings.StartSleepHour,
                EndSleepHour = initialSettings.EndSleepHour,
                TrendLocationWoeid = initialSettings.TrendLocations.FirstOrDefault()?.WoeId ?? 1
            });

            // assert
            Assert.Equal(Language.Spanish, updatedSettings.Language);
            Assert.NotEqual(initialSettings.Language, updatedSettings.Language);
            Assert.Equal(initialSettings.Language, recoveredSettings.Language);

            Assert.Equal(TimeZoneFromTwitter.Bangkok.ToTZinfo(), updatedSettings.TimeZone.Name);
            Assert.NotEqual(initialSettings.TimeZone.Name, updatedSettings.TimeZone.Name);
            Assert.Equal(initialSettings.TimeZone.Name, recoveredSettings.TimeZone.Name);

            Assert.Equal(updatedSettings.SleepTimeEnabled, !initialSettings.SleepTimeEnabled);
            Assert.Equal(initialSettings.SleepTimeEnabled, recoveredSettings.SleepTimeEnabled);

            Assert.Equal(23, updatedSettings.StartSleepHour);
            Assert.NotEqual(initialSettings.StartSleepHour, updatedSettings.StartSleepHour);
            Assert.Equal(initialSettings.StartSleepHour, recoveredSettings.StartSleepHour);

            Assert.Equal(7, updatedSettings.EndSleepHour);
            Assert.NotEqual(initialSettings.StartSleepHour, updatedSettings.StartSleepHour);
            Assert.Equal(initialSettings.EndSleepHour, recoveredSettings.EndSleepHour);

            Assert.Equal(580778, updatedSettings.TrendLocations[0].WoeId);
            Assert.NotEqual(initialSettings.TrendLocations?.FirstOrDefault()?.WoeId, updatedSettings.TrendLocations[0].WoeId);

            if (initialSettings.TrendLocations != null)
            {
                Assert.Equal(initialSettings.TrendLocations[0].WoeId, recoveredSettings.TrendLocations[0].WoeId);
            }
        }
    }
}