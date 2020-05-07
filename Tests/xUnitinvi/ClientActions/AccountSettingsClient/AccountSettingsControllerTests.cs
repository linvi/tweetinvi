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
        public async Task GetAccountSettings_ReturnsFromQueryExecutorAsync()
        {
            // Arrange
            var controller = CreateAccountSettingsController();
            var parameters = new GetAccountSettingsParameters();
            var request = A.Fake<ITwitterRequest>();
            var twitterResult = A.Fake<ITwitterResult<IAccountSettingsDTO>>();

            A.CallTo(() => _fakeAccountSettingsQueryExecutor.GetAccountSettingsAsync(parameters, request)).Returns(twitterResult);

            // Act
            var result = await controller.GetAccountSettingsAsync(parameters, request);

            // Assert
            Assert.Equal(result, twitterResult);
        }

        [Fact]
        public async Task UpdateAccountSettings_ReturnsFromQueryExecutorAsync()
        {
            // Arrange
            var controller = CreateAccountSettingsController();
            var parameters = new UpdateAccountSettingsParameters();
            var request = A.Fake<ITwitterRequest>();
            var twitterResult = A.Fake<ITwitterResult<IAccountSettingsDTO>>();

            A.CallTo(() => _fakeAccountSettingsQueryExecutor.UpdateAccountSettingsAsync(parameters, request)).Returns(twitterResult);

            // Act
            var result = await controller.UpdateAccountSettingsAsync(parameters, request);

            // Assert
            Assert.Equal(result, twitterResult);
        }

        [Fact]
        public async Task UpdateProfile_ReturnsFromQueryExecutorAsync()
        {
            // Arrange
            var controller = CreateAccountSettingsController();
            var parameters = new UpdateProfileParameters();
            var request = A.Fake<ITwitterRequest>();
            var twitterResult = A.Fake<ITwitterResult<IUserDTO>>();

            A.CallTo(() => _fakeAccountSettingsQueryExecutor.UpdateProfileAsync(parameters, request)).Returns(twitterResult);

            // Act
            var result = await controller.UpdateProfileAsync(parameters, request);

            // Assert
            Assert.Equal(result, twitterResult);
        }

        [Fact]
        public async Task UpdateProfileImage_ReturnsFromQueryExecutorAsync()
        {
            // Arrange
            var controller = CreateAccountSettingsController();
            var parameters = new UpdateProfileImageParameters(null);
            var request = A.Fake<ITwitterRequest>();
            var twitterResult = A.Fake<ITwitterResult<IUserDTO>>();

            A.CallTo(() => _fakeAccountSettingsQueryExecutor.UpdateProfileImageAsync(parameters, request)).Returns(twitterResult);

            // Act
            var result = await controller.UpdateProfileImageAsync(parameters, request);

            // Assert
            Assert.Equal(result, twitterResult);
        }

        [Fact]
        public async Task UpdateProfileBanner_ReturnsFromQueryExecutorAsync()
        {
            // Arrange
            var controller = CreateAccountSettingsController();
            var parameters = new UpdateProfileBannerParameters(null);
            var request = A.Fake<ITwitterRequest>();
            var twitterResult = A.Fake<ITwitterResult>();

            A.CallTo(() => _fakeAccountSettingsQueryExecutor.UpdateProfileBannerAsync(parameters, request)).Returns(twitterResult);

            // Act
            var result = await controller.UpdateProfileBannerAsync(parameters, request);

            // Assert
            Assert.Equal(result, twitterResult);
        }

        [Fact]
        public async Task RemoveProfileBanner_ReturnsFromQueryExecutorAsync()
        {
            // Arrange
            var controller = CreateAccountSettingsController();
            var parameters = new RemoveProfileBannerParameters();
            var request = A.Fake<ITwitterRequest>();
            var twitterResult = A.Fake<ITwitterResult>();

            A.CallTo(() => _fakeAccountSettingsQueryExecutor.RemoveProfileBannerAsync(parameters, request)).Returns(twitterResult);

            // Act
            var result = await controller.RemoveProfileBannerAsync(parameters, request);

            // Assert
            Assert.Equal(result, twitterResult);
        }
    }
}