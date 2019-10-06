using System;
using Tweetinvi.Controllers.AccountSettings;
using Tweetinvi.Models;
using Tweetinvi.Parameters;
using Xunit;
using xUnitinvi.TestHelpers;

namespace xUnitinvi.ClientActions.AccountSettingsClient
{
    public class AccountSettingsQueryGeneratorTests
    {
        public AccountSettingsQueryGeneratorTests()
        {
            _fakeBuilder = new FakeClassBuilder<AccountSettingsQueryGenerator>();
        }

        private readonly FakeClassBuilder<AccountSettingsQueryGenerator> _fakeBuilder;

        private AccountSettingsQueryGenerator CreateAccountSettingsQueryGenerator()
        {
            return _fakeBuilder.GenerateClass();
        }

        [Fact]
        public void GetAccountSettingsQuery_ReturnsExpectedQuery()
        {
            // arrange
            var queryGenerator = CreateAccountSettingsQueryGenerator();
            var parameters = new GetAccountSettingsParameters
            {
                CustomQueryParameters = { new Tuple<string, string>("hello", "world") }
            };

            // act
            var result = queryGenerator.GetAccountSettingsQuery(parameters);

            // assert
            Assert.Equal(result, $"https://api.twitter.com/1.1/account/settings.json?hello=world");
        }

        [Fact]
        public void GetUpdateAccountSettingsQuery_ReturnsExpectedQuery()
        {
            // arrange
            var queryGenerator = CreateAccountSettingsQueryGenerator();
            var parameters = new UpdateAccountSettingsParameters
            {
                DisplayLanguage = DisplayLanguages.Spanish,
                SleepTimeEnabled = true,
                StartSleepHour = 23,
                EndSleepHour = 7,
                TimeZone = TimeZoneFromTwitter.Bangkok.ToTZinfo(),
                TrendLocationWoeid = 580778,
                CustomQueryParameters = { new Tuple<string, string>("hello", "world") }
            };

            // act
            var result = queryGenerator.GetUpdateAccountSettingsQuery(parameters);

            // assert
            Assert.Equal(result, $"https://api.twitter.com/1.1/account/settings.json?lang=es&time_zone=Asia%2FBangkok&sleep_time_enabled=true&start_sleep_time=23&end_sleep_time=07&trend_location_woeid=580778&hello=world");
        }
        
        [Fact]
        public void GetUpdateProfileQuery_ReturnsExpectedQuery()
        {
            // arrange
            var queryGenerator = CreateAccountSettingsQueryGenerator();
            var parameters = new UpdateProfileParameters
            {
                Name = "name",
                WebsiteUrl = "url",
                Location = "loc",
                Description = "desc",
                IncludeEntities = true,
                SkipStatus = true,
                ProfileLinkColor = "blue",
                CustomQueryParameters = { new Tuple<string, string>("hello", "world") }
            };

            // act
            var result = queryGenerator.GetUpdateProfileQuery(parameters);

            // assert
            Assert.Equal(result, $"https://api.twitter.com/1.1/account/update_profile.json?name=name&url=url&location=loc&description=desc&profile_link_color=blue&include_entities=true&skip_status=true&hello=world");
        }

        [Fact]
        public void GetUpdateProfileImageQuery_ReturnsExpectedQuery()
        {
            // arrange
            var queryGenerator = CreateAccountSettingsQueryGenerator();
            var parameters = new UpdateProfileImageParameters(null)
            {
                IncludeEntities = true,
                SkipStatus = false,
                CustomQueryParameters = { new Tuple<string, string>("hello", "world") }
            };

            // act
            var result = queryGenerator.GetUpdateProfileImageQuery(parameters);

            // assert
            Assert.Equal(result, $"https://api.twitter.com/1.1/account/update_profile_image.json?include_entities=true&skip_status=false&hello=world");
        }

        [Fact]
        public void GetUpdateProfileBannerQuery_ReturnsExpectedQuery()
        {
            // arrange
            var queryGenerator = CreateAccountSettingsQueryGenerator();
            var parameters = new UpdateProfileBannerParameters(null)
            {
                Height = 42,
                Width = 43,
                OffsetLeft = 44,
                OffsetTop = 45,
                CustomQueryParameters = { new Tuple<string, string>("hello", "world") }
            };

            // act
            var result = queryGenerator.GetUpdateProfileBannerQuery(parameters);

            // assert
            Assert.Equal(result, $"https://api.twitter.com/1.1/account/update_profile_banner.json?width=43&height=42&offset_left=44&offset_top=45&hello=world");
        }

        [Fact]
        public void GetRemoveProfileBannerQuery_ReturnsExpectedQuery()
        {
            // arrange
            var queryGenerator = CreateAccountSettingsQueryGenerator();
            var parameters = new RemoveProfileBannerParameters
            {
                CustomQueryParameters = { new Tuple<string, string>("hello", "world") }
            };

            // act
            var result = queryGenerator.GetRemoveProfileBannerQuery(parameters);

            // assert
            Assert.Equal(result, $"https://api.twitter.com/1.1/account/remove_profile_banner.json?hello=world");
        }
    }
}