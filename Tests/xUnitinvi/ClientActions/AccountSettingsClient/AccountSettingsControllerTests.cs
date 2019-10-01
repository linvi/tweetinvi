using System.Threading.Tasks;
using FakeItEasy;
using Tweetinvi.Controllers.AccountSettings;
using Tweetinvi.Core.Web;
using Tweetinvi.Models;
using Tweetinvi.Models.DTO;
using Tweetinvi.Parameters;
using Xunit;
using xUnitinvi.TestHelpers;

namespace xUnitinvi.ClientActions.AccountSettingsClient
{
    public class AccountSettingsControllerTests
    {
        public AccountSettingsControllerTests()
        {
            _fakeBuilder = new FakeClassBuilder<AccountSettingsController>();
            _fakeAccountSettingsQueryExecutor = _fakeBuilder.GetFake<IAccountSettingsQueryExecutor>().FakedObject;
        }

        private readonly FakeClassBuilder<AccountSettingsController> _fakeBuilder;
        private readonly IAccountSettingsQueryExecutor _fakeAccountSettingsQueryExecutor;

        private AccountSettingsController CreateAccountSettingsController()
        {
            return _fakeBuilder.GenerateClass();
        }

        [Fact]
        public async Task UpdateProfileImage_ReturnsFromQueryExecutor()
        {
            // Arrange
            var controller = CreateAccountSettingsController();
            var parameters = new UpdateProfileImageParameters(null);
            var request = A.Fake<ITwitterRequest>();
            var twitterResult = A.Fake<ITwitterResult<IUserDTO>>();

            A.CallTo(() => _fakeAccountSettingsQueryExecutor.UpdateProfileImage(parameters, request)).Returns(twitterResult);

            // Act
            var result = await controller.UpdateProfileImage(parameters, request);

            // Assert
            Assert.Equal(result, twitterResult);
        }
        
        [Fact]
        public async Task UpdateProfileBanner_ReturnsFromQueryExecutor()
        {
            // Arrange
            var controller = CreateAccountSettingsController();
            var parameters = new UpdateProfileBannerParameters(null);
            var request = A.Fake<ITwitterRequest>();
            var twitterResult = A.Fake<ITwitterResult>();

            A.CallTo(() => _fakeAccountSettingsQueryExecutor.UpdateProfileBanner(parameters, request)).Returns(twitterResult);

            // Act
            var result = await controller.UpdateProfileBanner(parameters, request);

            // Assert
            Assert.Equal(result, twitterResult);
        }
        
        [Fact]
        public async Task RemoveProfileBanner_ReturnsFromQueryExecutor()
        {
            // Arrange
            var controller = CreateAccountSettingsController();
            var parameters = new RemoveProfileBannerParameters();
            var request = A.Fake<ITwitterRequest>();
            var twitterResult = A.Fake<ITwitterResult>();

            A.CallTo(() => _fakeAccountSettingsQueryExecutor.RemoveProfileBanner(parameters, request)).Returns(twitterResult);

            // Act
            var result = await controller.RemoveProfileBanner(parameters, request);

            // Assert
            Assert.Equal(result, twitterResult);
        }
    }
}