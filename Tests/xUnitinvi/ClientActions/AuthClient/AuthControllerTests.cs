using System.Threading.Tasks;
using FakeItEasy;
using Tweetinvi.Controllers.Auth;
using Tweetinvi.Core.DTO;
using Tweetinvi.Core.Web;
using Tweetinvi.Credentials.Models;
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
            var parameters = A.Fake<ICreateBearerTokenParameters>();

            A.CallTo(() => _fakeAuthQueryExecutor.CreateBearerToken(parameters, request)).Returns(expectedResult);

            // Act
            var result = await controller.CreateBearerToken(parameters, request);

            // Assert
            Assert.Equal(result, expectedResult);
        }

        [Fact]
        public async Task RequestAuthUrl_PinCode_ReturnsFromRequestExecutor()
        {
            // Arrange
            var controller = CreateAuthController();
            var request = A.Fake<ITwitterRequest>();
            var expectedResult = A.Fake<ITwitterResult>();
            var parameters = A.Fake<IRequestAuthUrlParameters>();
            var response = "oauth_token=MY_TOKEN&oauth_token_secret=MY_SECRET&oauth_callback_confirmed=true";

            A.CallTo(() => _fakeAuthQueryExecutor.RequestAuthUrl(It.IsAny<RequestAuthUrlInternalParameters>(), request)).Returns(expectedResult);
            A.CallTo(() => expectedResult.RawResult).Returns(response);

            // Act
            var result = await controller.RequestAuthUrl(parameters, request);

            // Assert
            Assert.Equal("MY_TOKEN", result.DataTransferObject.AuthorizationKey);
            Assert.Equal("MY_SECRET", result.DataTransferObject.AuthorizationSecret);
        }

        [Fact]
        public async Task RequestAuthUrl_WithRedirectUrl_ReturnsFromRequestExecutor()
        {
            // Arrange
            var controller = CreateAuthController();
            var request = A.Fake<ITwitterRequest>();
            var expectedResult = A.Fake<ITwitterResult>();
            var parameters = new RequestUrlAuthUrlParameters("my_url");
            var response = "oauth_token=MY_TOKEN&oauth_token_secret=MY_SECRET&oauth_callback_confirmed=true";

            A.CallTo(() => _fakeAuthQueryExecutor.RequestAuthUrl(A<RequestAuthUrlInternalParameters>.That.Matches(x => x.CallbackUrl == "my_url"), request))
                .Returns(expectedResult);
            A.CallTo(() => expectedResult.RawResult).Returns(response);

            // Act
            var result = await controller.RequestAuthUrl(parameters, request);

            // Assert
            Assert.Equal("MY_TOKEN", result.DataTransferObject.AuthorizationKey);
            Assert.Equal("MY_SECRET", result.DataTransferObject.AuthorizationSecret);
        }

        [Fact]
        public async Task RequestAuthUrl_WithNonConfirmedCallback_ShouldThrowAsAborted()
        {
            // Arrange
            var controller = CreateAuthController();
            var request = A.Fake<ITwitterRequest>();
            var expectedResult = A.Fake<ITwitterResult>();
            var parameters = new RequestUrlAuthUrlParameters("my_url");
            var response = "oauth_token=MY_TOKEN&oauth_token_secret=MY_SECRET&oauth_callback_confirmed=false";

            A.CallTo(() => _fakeAuthQueryExecutor.RequestAuthUrl(A<RequestAuthUrlInternalParameters>.That.Matches(x => x.CallbackUrl == "my_url"), request))
                .Returns(expectedResult);
            A.CallTo(() => expectedResult.RawResult).Returns(response);

            // Act
            await Assert.ThrowsAsync<TwitterAuthAbortedException>(() => controller.RequestAuthUrl(parameters, request));
        }

        [Fact]
        public async Task RequestAuthUrl_WithoutResponse_ShouldThrowAuthException()
        {
            // Arrange
            var controller = CreateAuthController();
            var request = A.Fake<ITwitterRequest>();
            var expectedResult = A.Fake<ITwitterResult>();
            var parameters = new RequestUrlAuthUrlParameters("my_url");

            var response = "";

            A.CallTo(() => _fakeAuthQueryExecutor.RequestAuthUrl(A<RequestAuthUrlInternalParameters>.That.Matches(x => x.CallbackUrl == "my_url"), request))
                .Returns(expectedResult);
            A.CallTo(() => expectedResult.RawResult).Returns(response);

            // Act
            await Assert.ThrowsAsync<TwitterAuthException>(() => controller.RequestAuthUrl(parameters, request));
        }

        [Fact]
        public async Task RequestCredentials_ParsesTheTwitterResultAndReturnsCredentials()
        {
            // Arrange
            var controller = CreateAuthController();
            var request = A.Fake<ITwitterRequest>();
            var expectedResult = A.Fake<ITwitterResult>();
            var parameters = new RequestCredentialsParameters("verifier_code", new AuthenticationRequest
            {
                ConsumerKey = "consumer_key",
                ConsumerSecret = "consumer_secret"
            });

            var response = "oauth_token=access_token&oauth_token_secret=access_secret";

            A.CallTo(() => _fakeAuthQueryExecutor.RequestCredentials(parameters, request)).Returns(expectedResult);
            A.CallTo(() => expectedResult.RawResult).Returns(response);

            // Act
            var result = await controller.RequestCredentials(parameters, request);

            // Assert
            Assert.Equal(result.DataTransferObject.AccessToken, $"access_token");
            Assert.Equal(result.DataTransferObject.AccessTokenSecret, $"access_secret");
            Assert.Equal(result.DataTransferObject.ConsumerKey, $"consumer_key");
            Assert.Equal(result.DataTransferObject.ConsumerSecret, $"consumer_secret");
        }

        [Fact]
        public async Task RequestCredentials_ThrowsWhenCredentialsAreMissingInResult()
        {
            // Arrange
            var controller = CreateAuthController();
            var request = A.Fake<ITwitterRequest>();
            var expectedResult = A.Fake<ITwitterResult>();
            var parameters = new RequestCredentialsParameters("verifier_code", new AuthenticationRequest
            {
                ConsumerKey = "consumer_key",
                ConsumerSecret = "consumer_secret"
            });

            var response = "oauth_token=access_token"; // missing secret

            A.CallTo(() => _fakeAuthQueryExecutor.RequestCredentials(parameters, request)).Returns(expectedResult);
            A.CallTo(() => expectedResult.RawResult).Returns(response);

            // Act
            await Assert.ThrowsAsync<TwitterAuthException>(() => controller.RequestCredentials(parameters, request));
        }

        [Fact]
        public async Task InvalidateBearerToken_ReturnsFromQueryExecutor()
        {
            // Arrange
            var controller = CreateAuthController();
            var request = A.Fake<ITwitterRequest>();
            var expectedResult = A.Fake<ITwitterResult>();
            var parameters = new InvalidateBearerTokenParameters();

            A.CallTo(() => _fakeAuthQueryExecutor.InvalidateBearerToken(parameters, request)).Returns(expectedResult);

            // Act
            var result = await controller.InvalidateBearerToken(parameters, request);

            // Assert
            Assert.Equal(result, expectedResult);
        }

        [Fact]
        public async Task InvalidateAccessToken_ReturnsFromQueryExecutor()
        {
            // Arrange
            var controller = CreateAuthController();
            var request = A.Fake<ITwitterRequest>();
            var expectedResult = A.Fake<ITwitterResult>();
            var parameters = new InvalidateAccessTokenParameters();

            A.CallTo(() => _fakeAuthQueryExecutor.InvalidateAccessToken(parameters, request)).Returns(expectedResult);

            // Act
            var result = await controller.InvalidateAccessToken(parameters, request);

            // Assert
            Assert.Equal(result, expectedResult);
        }
    }
}