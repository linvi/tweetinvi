using System.Threading.Tasks;
using FakeItEasy;
using Tweetinvi.Controllers.Auth;
using Tweetinvi.Core.DTO;
using Tweetinvi.Core.Web;
using Tweetinvi.Exceptions;
using Tweetinvi.Models;
using Tweetinvi.Parameters.Auth;
using Xunit;
using xUnitinvi.TestHelpers;

namespace xUnitinvi.ClientActions.AuthClient
{
    public class AuthControllerTests
    {
        public AuthControllerTests()
        {
            _fakeBuilder = new FakeClassBuilder<AuthController>();
            _fakeAuthQueryExecutor = _fakeBuilder.GetFake<IAuthQueryExecutor>().FakedObject;
        }

        private readonly FakeClassBuilder<AuthController> _fakeBuilder;
        private readonly IAuthQueryExecutor _fakeAuthQueryExecutor;

        private AuthController CreateAuthController()
        {
            return _fakeBuilder.GenerateClass();
        }

        [Fact]
        public async Task CreateBearerToken_ReturnsQueryExecutorResult()
        {
            // Arrange
            var controller = CreateAuthController();
            var request = A.Fake<ITwitterRequest>();
            var expectedResult = A.Fake<ITwitterResult<CreateTokenResponseDTO>>();

            A.CallTo(() => _fakeAuthQueryExecutor.CreateBearerToken(request)).Returns(expectedResult);

            // Act
            var result = await controller.CreateBearerToken(request);

            // Assert
            Assert.Equal(result, expectedResult);
        }

        [Fact]
        public async Task StartAuthProcess_PinCode_ReturnsFromRequestExecutor()
        {
            // Arrange
            var controller = CreateAuthController();
            var request = A.Fake<ITwitterRequest>();
            var expectedResult = A.Fake<ITwitterResult>();
            var parameters = A.Fake<IRequestAuthUrlParameters>();
            var response = "oauth_token=MY_TOKEN&oauth_token_secret=MY_SECRET&oauth_callback_confirmed=true";

            A.CallTo(() => _fakeAuthQueryExecutor.RequestAuthUrl(It.IsAny<RequestAuthUrlInternalParameters>(), request)).Returns(expectedResult);
            A.CallTo(() => expectedResult.Json).Returns(response);

            // Act
            var result = await controller.RequestAuthUrl(parameters, request);

            // Assert
            Assert.Equal("MY_TOKEN", result.DataTransferObject.Token.AuthorizationKey);
            Assert.Equal("MY_SECRET", result.DataTransferObject.Token.AuthorizationSecret);
        }

        [Fact]
        public async Task StartAuthProcess_WithRedirectUrl_ReturnsFromRequestExecutor()
        {
            // Arrange
            var controller = CreateAuthController();
            var request = A.Fake<ITwitterRequest>();
            var expectedResult = A.Fake<ITwitterResult>();
            var parameters = new RequestUrlAuthUrlParameters("my_url")
            {
                RequestId = null
            };
            var response = "oauth_token=MY_TOKEN&oauth_token_secret=MY_SECRET&oauth_callback_confirmed=true";

            A.CallTo(() => _fakeAuthQueryExecutor.RequestAuthUrl(A<RequestAuthUrlInternalParameters>.That.Matches(x => x.CallbackUrl == "my_url"), request))
                .Returns(expectedResult);
            A.CallTo(() => expectedResult.Json).Returns(response);

            // Act
            var result = await controller.RequestAuthUrl(parameters, request);

            // Assert
            Assert.Equal("MY_TOKEN", result.DataTransferObject.Token.AuthorizationKey);
            Assert.Equal("MY_SECRET", result.DataTransferObject.Token.AuthorizationSecret);
        }

        [Fact]
        public async Task StartAuthProcess_WithRedirectUrlAndAuthId_ReturnsFromRequestExecutor()
        {
            // Arrange
            var controller = CreateAuthController();
            var request = A.Fake<ITwitterRequest>();
            var expectedResult = A.Fake<ITwitterResult>();
            var parameters = new RequestUrlAuthUrlParameters("my_url")
            {
                RequestId = "my_auth_id"
            };
            var response = "oauth_token=MY_TOKEN&oauth_token_secret=MY_SECRET&oauth_callback_confirmed=true";

            A.CallTo(() => _fakeAuthQueryExecutor.RequestAuthUrl(A<RequestAuthUrlInternalParameters>.That.Matches(x => x.CallbackUrl == "my_url?authorization_id=my_auth_id"), request))
                .Returns(expectedResult);
            A.CallTo(() => expectedResult.Json).Returns(response);

            // Act
            var result = await controller.RequestAuthUrl(parameters, request);

            // Assert
            Assert.Equal("MY_TOKEN", result.DataTransferObject.Token.AuthorizationKey);
            Assert.Equal("MY_SECRET", result.DataTransferObject.Token.AuthorizationSecret);
        }

        [Fact]
        public async Task StartAuthProcess_WithNonConfirmedCallback_ShouldThrowAsAborted()
        {
            // Arrange
            var controller = CreateAuthController();
            var request = A.Fake<ITwitterRequest>();
            var expectedResult = A.Fake<ITwitterResult>();
            var parameters = new RequestUrlAuthUrlParameters("my_url")
            {
                RequestId = "my_auth_id"
            };
            var response = "oauth_token=MY_TOKEN&oauth_token_secret=MY_SECRET&oauth_callback_confirmed=false";

            A.CallTo(() => _fakeAuthQueryExecutor.RequestAuthUrl(A<RequestAuthUrlInternalParameters>.That.Matches(x => x.CallbackUrl == "my_url?authorization_id=my_auth_id"), request))
                .Returns(expectedResult);
            A.CallTo(() => expectedResult.Json).Returns(response);

            // Act
            await Assert.ThrowsAsync<TwitterAuthAbortedException>(() => controller.RequestAuthUrl(parameters, request));
        }

        [Fact]
        public async Task StartAuthProcess_WithoutResponse_ShouldThrowAuthException()
        {
            // Arrange
            var controller = CreateAuthController();
            var request = A.Fake<ITwitterRequest>();
            var expectedResult = A.Fake<ITwitterResult>();
            var parameters = new RequestUrlAuthUrlParameters("my_url")
            {
                RequestId = "my_auth_id"
            };

            var response = "";

            A.CallTo(() => _fakeAuthQueryExecutor.RequestAuthUrl(A<RequestAuthUrlInternalParameters>.That.Matches(x => x.CallbackUrl == "my_url?authorization_id=my_auth_id"), request))
                .Returns(expectedResult);
            A.CallTo(() => expectedResult.Json).Returns(response);

            // Act
            await Assert.ThrowsAsync<TwitterAuthException>(() => controller.RequestAuthUrl(parameters, request));
        }
    }
}