using System.IO;
using System.Threading.Tasks;
using Tweetinvi;
using Xunit;

namespace xUnitinvi.IntegrationTests
{
    public class AccountSettingsIntegrationTests
    {
        private readonly ITwitterClient Client;
        
        public AccountSettingsIntegrationTests()
        {
            Client = new TwitterClient(IntegrationTestCredentials.ProtectedUserCredentials);
        }

//        [Fact]
        [Fact(Skip = "Integration Tests")]
        public async Task RunAllAccountSettings()
        {
            await ChangeImages();
        }
        
        [Fact(Skip = "Integration Tests")]
        public async Task ChangeImages()
        {
            // act
            var user = await Client.Account.GetAuthenticatedUser();
            var image = File.ReadAllBytes("./tweetinvi-logo-purple.png");
            await Client.AccountSettings.UpdateProfileImage(image);
            var userAfter = await Client.Account.GetAuthenticatedUser();
            
            // assert
            Assert.NotEqual(user.ProfileImageUrl, userAfter.ProfileImageUrl);
        }
    }
}