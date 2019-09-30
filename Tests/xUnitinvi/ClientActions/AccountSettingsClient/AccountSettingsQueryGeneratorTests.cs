using System;
using Tweetinvi.Controllers.AccountSettings;
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
    }
}