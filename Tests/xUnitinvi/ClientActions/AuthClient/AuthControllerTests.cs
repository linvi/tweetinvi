using System.Threading.Tasks;
using FakeItEasy;
using Tweetinvi.Controllers.Auth;
using Tweetinvi.Core.DTO;
using Tweetinvi.Core.Web;
using Tweetinvi.Credentials.Models;
using Tweetinvi.Exceptions;
using Tweetinvi.Models;
using Tweetinvi.Parameters;
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
        public async Task CreateBearerToken_ReturnsQueryExecutorResultAsync()
        {
            // Arrange
            var controller = CreateAuthController();
            var request = A.Fake<ITwitterRequest>();
            var expectedResult = A.Fake<ITwitterResult<CreateTokenResponseDTO>>();
            var parameters = A.Fake<ICreateBearerTokenParameters>();

            A.CallTo(() => _fakeAuthQueryExecutor.CreateBearerTokenAsync(parameters, request)).Returns(expectedResult);

            // Act
            var result = await controller.CreateBearerTokenAsync(parameters, request);

            // Assert
            Assert.Equal(result, expectedResult);
        }

        [Fact]
        public async Task RequestAuthUrl_PinCode_ReturnsFromRequestExecutorAsync()
        {
            // Arrange
            var controller = CreateAuthController();
            var request = A.Fake<ITwitterRequest>();
            var expectedResult = A.Fake<ITwitterResult>();
            var parameters = A.Fake<IRequestAuthUrlParameters>();
            var response = "oauth_token=MY_TOKEN&oauth_token_secret=MY_SECRET&oauth_callback_confirmed=true";

            A.CallTo(() => _fakeAuthQueryExecutor.RequestAuthUrlAsync(It.IsAny<RequestAuthUrlInternalParameters>(), request)).Returns(expectedResult);
            A.CallTo(() => expectedResult.RawResult).Returns(response);

            // Act
            var result = await controller.RequestAuthUrlAsync(parameters, request);

            // Assert
            Assert.Equal("MY_TOKEN", result.DataTransferObject.AuthorizationKey);
            Assert.Equal("MY_SECRET", result.DataTransferObject.AuthorizationSecret);
        }

        [Fact]
        public async Task RequestAuthUrl_WithRedirectUrl_ReturnsFromRequestExecutorAsync()
        {
            // Arrange
            var controller = CreateAuthController();
            var request = A.Fake<ITwitterRequest>();
            var expectedResult = A.Fake<ITwitterResult>();
            var parameters = new RequestUrlAuthUrlParameters("my_url");
            var response = "oauth_token=MY_TOKEN&oauth_token_secret=MY_SECRET&oauth_callback_confirmed=true";

            A.CallTo(() => _fakeAuthQueryExecutor.RequestAuthUrlAsync(A<RequestAuthUrlInternalParameters>.That.Matches(x => x.CallbackUrl == "my_url"), request))
                .Returns(expectedResult);
            A.CallTo(() => expectedResult.RawResult).Returns(response);

            // Act
            var result = await controller.RequestAuthUrlAsync(parameters, request);

            // Assert
            Assert.Equal("MY_TOKEN", result.DataTransferObject.AuthorizationKey);
            Assert.Equal("MY_SECRET", result.DataTransferObject.AuthorizationSecret);
        }

        [Fact]
        public async Task RequestAuthUrl_WithNonConfirmedCallback_ShouldThrowAsAbortedAsync()
        {
            // Arrange
            var controller = CreateAuthController();
            var request = A.Fake<ITwitterRequest>();
            var expectedResult = A.Fake<ITwitterResult>();
            var parameters = new RequestUrlAuthUrlParameters("my_url");
            var response = "oauth_token=MY_TOKEN&oauth_token_secret=MY_SECRET&oauth_callback_confirmed=false";

            A.CallTo(() => _fakeAuthQueryExecutor.RequestAuthUrlAsync(A<RequestAuthUrlInternalParameters>.That.Matches(x => x.CallbackUrl == "my_url"), request))
                .Returns(expectedResult);
            A.CallTo(() => expectedResult.RawResult).Returns(response);

            // Act
            await Assert.ThrowsAsync<TwitterAuthAbortedException>(() => controller.RequestAuthUrlAsync(parameters, request));
        }

        [Fact]
        public async Task RequestAuthUrl_WithoutResponse_ShouldThrowAuthExceptionAsync()
        {
            // Arrange
            var controller = CreateAuthController();
            var request = A.Fake<ITwitterRequest>();
            var expectedResult = A.Fake<ITwitterResult>();
            var parameters = new RequestUrlAuthUrlParameters("my_url");

            var response = "";

            A.CallTo(() => _fakeAuthQueryExecutor.RequestAuthUrlAsync(A<RequestAuthUrlInternalParameters>.That.Matches(x => x.CallbackUrl == "my_url"), request))
                .Returns(expectedResult);
            A.CallTo(() => expectedResult.RawResult).Returns(response);

            // Act
            await Assert.ThrowsAsync<TwitterAuthException>(() => controller.RequestAuthUrlAsync(parameters, request));
        }

        [Fact]
        public async Task RequestCredentials_ParsesTheTwitterResultAndReturnsCredentialsAsync()
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

            A.CallTo(() => _fakeAuthQueryExecutor.RequestCredentialsAsync(parameters, request)).Returns(expectedResult);
            A.CallTo(() => expectedResult.RawResult).Returns(response);

            // Act
            var result = await controller.RequestCredentialsAsync(parameters, request);

            // Assert
            Assert.Equal(result.DataTransferObject.AccessToken, $"access_token");
            Assert.Equal(result.DataTransferObject.AccessTokenSecret, $"access_secret");
            Assert.Equal(result.DataTransferObject.ConsumerKey, $"consumer_key");
            Assert.Equal(result.DataTransferObject.ConsumerSecret, $"consumer_secret");
        }

        [Fact]
        public async Task RequestCredentials_ThrowsWhenCredentialsAreMissingInResultAsync()
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

            A.CallTo(() => _fakeAuthQueryExecutor.RequestCredentialsAsync(parameters, request)).Returns(expectedResult);
            A.CallTo(() => expectedResult.RawResult).Returns(response);

            // Act
            await Assert.ThrowsAsync<TwitterAuthException>(() => controller.RequestCredentialsAsync(parameters, request));
        }

        [Fact]
        public async Task InvalidateBearerToken_ReturnsFromQueryExecutorAsync()
        {
            // Arrange
            var controller = CreateAuthController();
            var request = A.Fake<ITwitterRequest>();
            var expectedResult = A.Fake<ITwitterResult<InvalidateTokenResponse>>();
            var parameters = new InvalidateBearerTokenParameters();

            A.CallTo(() => _fakeAuthQueryExecutor.InvalidateBearerTokenAsync(parameters, request)).Returns(expectedResult);

            // Act
            var result = await controller.InvalidateBearerTokenAsync(parameters, request);

            // Assert
            Assert.Equal(result, expectedResult);
        }

        [Fact]
        public async Task InvalidateAccessToken_ReturnsFromQueryExecutorAsync()
        {
            // Arrange
            var controller = CreateAuthController();
            var request = A.Fake<ITwitterRequest>();
            var expectedResult = A.Fake<ITwitterResult<InvalidateTokenResponse>>();
            var parameters = new InvalidateAccessTokenParameters();

            A.CallTo(() => _fakeAuthQueryExecutor.InvalidateAccessTokenAsync(parameters, request)).Returns(expectedResult);

            // Act
            var result = await controller.InvalidateAccessTokenAsync(parameters, request);

            // Assert
            Assert.Equal(result, expectedResult);
        }
    }
}