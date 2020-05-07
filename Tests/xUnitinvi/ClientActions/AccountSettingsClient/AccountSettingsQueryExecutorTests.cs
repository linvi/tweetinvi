using System.Threading.Tasks;
using FakeItEasy;
using Tweetinvi;
using Tweetinvi.Controllers.AccountSettings;
using Tweetinvi.Core.Web;
using Tweetinvi.Models;
using Tweetinvi.Models.DTO;
using Tweetinvi.Parameters;
using Xunit;
using xUnitinvi.TestHelpers;

namespace xUnitinvi.ClientActions.AccountSettingsClient
{
    public class AccountSettingsQueryExecutorTests
    {
        public AccountSettingsQueryExecutorTests()
        {
            _fakeBuilder = new FakeClassBuilder<AccountSettingsQueryExecutor>();
            _fakeAccountSettingsQueryGenerator = _fakeBuilder.GetFake<IAccountSettingsQueryGenerator>().FakedObject;
            _fakeTwitterAccessor = _fakeBuilder.GetFake<ITwitterAccessor>().FakedObject;
        }

        private readonly FakeClassBuilder<AccountSettingsQueryExecutor> _fakeBuilder;
        private readonly IAccountSettingsQueryGenerator _fakeAccountSettingsQueryGenerator;
        private readonly ITwitterAccessor _fakeTwitterAccessor;

        private AccountSettingsQueryExecutor CreateAccountSettingsQueryExecutor()
        {
            return _fakeBuilder.GenerateClass();
        }

        [Fact]
        public async Task GetAccountSettings_ReturnsAccountUserDTOAsync()
        {
            // Arrange
            var queryExecutor = CreateAccountSettingsQueryExecutor();
            var parameters = new GetAccountSettingsParameters();

            var url = TestHelper.GenerateString();
            var request = A.Fake<ITwitterRequest>();
            var expectedResult = A.Fake<ITwitterResult<IAccountSettingsDTO>>();

            A.CallTo(() => _fakeAccountSettingsQueryGenerator.GetAccountSettingsQuery(parameters)).Returns(url);
            A.CallTo(() => _fakeTwitterAccessor.ExecuteRequestAsync<IAccountSettingsDTO>(request)).Returns(expectedResult);

            // Act
            var result = await queryExecutor.GetAccountSettingsAsync(parameters, request);

            // Assert
            Assert.Equal(result, expectedResult);
            Assert.Equal(request.Query.Url, url);
            Assert.Equal(HttpMethod.GET, request.Query.HttpMethod);
        }

        [Fact]
        public async Task UpdateAccountSettings_ReturnsAccountUserDTOAsync()
        {
            // Arrange
            var queryExecutor = CreateAccountSettingsQueryExecutor();
            var parameters = new UpdateAccountSettingsParameters();

            var url = TestHelper.GenerateString();
            var request = A.Fake<ITwitterRequest>();
            var expectedResult = A.Fake<ITwitterResult<IAccountSettingsDTO>>();

            A.CallTo(() => _fakeAccountSettingsQueryGenerator.GetUpdateAccountSettingsQuery(parameters)).Returns(url);
            A.CallTo(() => _fakeTwitterAccessor.ExecuteRequestAsync<IAccountSettingsDTO>(request)).Returns(expectedResult);

            // Act
            var result = await queryExecutor.UpdateAccountSettingsAsync(parameters, request);

            // Assert
            Assert.Equal(result, expectedResult);
            Assert.Equal(request.Query.Url, url);
            Assert.Equal(HttpMethod.POST, request.Query.HttpMethod);
        }

        [Fact]
        public async Task UpdateProfile_ReturnsAccountUserDTOAsync()
        {
            // Arrange
            var queryExecutor = CreateAccountSettingsQueryExecutor();
            var parameters = new UpdateProfileParameters();

            var url = TestHelper.GenerateString();
            var request = A.Fake<ITwitterRequest>();
            var expectedResult = A.Fake<ITwitterResult<IUserDTO>>();

            A.CallTo(() => _fakeAccountSettingsQueryGenerator.GetUpdateProfileQuery(parameters)).Returns(url);
            A.CallTo(() => _fakeTwitterAccessor.ExecuteRequestAsync<IUserDTO>(request)).Returns(expectedResult);

            // Act
            var result = await queryExecutor.UpdateProfileAsync(parameters, request);

            // Assert
            Assert.Equal(result, expectedResult);
            Assert.Equal(request.Query.Url, url);
            Assert.Equal(HttpMethod.POST, request.Query.HttpMethod);
        }

        [Fact]
        public async Task UpdateProfileImage_ReturnsAccountUserDTOAsync()
        {
            // Arrange
            var queryExecutor = CreateAccountSettingsQueryExecutor();
            var parameters = new UpdateProfileImageParameters(null);

            var url = TestHelper.GenerateString();
            var request = A.Fake<ITwitterRequest>();
            var expectedResult = A.Fake<ITwitterResult<IUserDTO>>();

            A.CallTo(() => _fakeAccountSettingsQueryGenerator.GetUpdateProfileImageQuery(parameters)).Returns(url);
            A.CallTo(() => _fakeTwitterAccessor.ExecuteRequestAsync<IUserDTO>(A<ITwitterRequest>
                    .That.Matches(x => x.Query is IMultipartTwitterQuery && x.Query.Url == url)))
                .Returns(expectedResult);

            // Act
            var result = await queryExecutor.UpdateProfileImageAsync(parameters, request);

            // Assert
            Assert.Equal(result, expectedResult);
            Assert.Equal(request.Query.Url, url);
            Assert.Equal(HttpMethod.POST, request.Query.HttpMethod);
        }

        [Fact]
        public async Task UpdateProfileBanner_ReturnsAccountUserDTOAsync()
        {
            // Arrange
            var queryExecutor = CreateAccountSettingsQueryExecutor();
            var parameters = new UpdateProfileBannerParameters(new byte[2]);

            var url = TestHelper.GenerateString();
            var request = A.Fake<ITwitterRequest>();
            var expectedResult = A.Fake<ITwitterResult>();

            A.CallTo(() => _fakeAccountSettingsQueryGenerator.GetUpdateProfileBannerQuery(parameters)).Returns(url);
            A.CallTo(() => _fakeTwitterAccessor.ExecuteRequestAsync(A<ITwitterRequest>.Ignored)).Returns(expectedResult);

            // Act
            var result = await queryExecutor.UpdateProfileBannerAsync(parameters, request);

            // Assert
            Assert.Equal(result, expectedResult);
            Assert.True(request.Query.IsHttpContentPartOfQueryParams);
            Assert.Equal(request.Query.Url, url);
            Assert.Equal(HttpMethod.POST, request.Query.HttpMethod);
        }

        [Fact]
        public async Task RemoveProfileBanner_ReturnsAccountUserDTOAsync()
        {
            // Arrange
            var queryExecutor = CreateAccountSettingsQueryExecutor();
            var parameters = new RemoveProfileBannerParameters();

            var url = TestHelper.GenerateString();
            var request = A.Fake<ITwitterRequest>();
            var expectedResult = A.Fake<ITwitterResult>();

            A.CallTo(() => _fakeAccountSettingsQueryGenerator.GetRemoveProfileBannerQuery(parameters)).Returns(url);
            A.CallTo(() => _fakeTwitterAccessor.ExecuteRequestAsync(A<ITwitterRequest>.Ignored)).Returns(expectedResult);

            // Act
            var result = await queryExecutor.RemoveProfileBannerAsync(parameters, request);

            // Assert
            Assert.Equal(result, expectedResult);
            Assert.Equal(request.Query.Url, url);
            Assert.Equal(HttpMethod.POST, request.Query.HttpMethod);
        }
    }
}