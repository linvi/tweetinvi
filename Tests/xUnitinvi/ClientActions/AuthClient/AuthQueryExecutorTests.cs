using System.Threading.Tasks;
using FakeItEasy;
using Tweetinvi.Controllers.Auth;
using Tweetinvi.Core.DTO;
using Tweetinvi.Core.Web;
using Tweetinvi.Credentials.AuthHttpHandlers;
using Tweetinvi.Models;
using Tweetinvi.Parameters;
using Tweetinvi.WebLogic;
using Xunit;
using xUnitinvi.TestHelpers;

namespace xUnitinvi.ClientActions.AuthClient
{
    public class AuthQueryExecutorTests
    {
        public AuthQueryExecutorTests()
        {
            _fakeBuilder = new FakeClassBuilder<AuthQueryExecutor>();
            _fakeAuthQueryGenerator = _fakeBuilder.GetFake<IAuthQueryGenerator>().FakedObject;
            _fakeOAuthWebRequestGeneratorFactory = _fakeBuilder.GetFake<IOAuthWebRequestGeneratorFactory>().FakedObject;
            _fakeTwitterAccessor = _fakeBuilder.GetFake<ITwitterAccessor>().FakedObject;
        }

        private readonly FakeClassBuilder<AuthQueryExecutor> _fakeBuilder;
        private readonly IAuthQueryGenerator _fakeAuthQueryGenerator;
        private readonly IOAuthWebRequestGeneratorFactory _fakeOAuthWebRequestGeneratorFactory;
        private readonly ITwitterAccessor _fakeTwitterAccessor;

        private AuthQueryExecutor CreateAuthQueryExecutor()
        {
            return _fakeBuilder.GenerateClass();
        }

        [Fact]
        public async Task CreateBearerToken_ReturnsBearerTokenAsync()
        {
            // Arrange
            var queryExecutor = CreateAuthQueryExecutor();

            var url = TestHelper.GenerateString();
            var request = A.Fake<ITwitterRequest>();
            var expectedResult = A.Fake<ITwitterResult<CreateTokenResponseDTO>>();
            var oAuthWebRequestGenerator = A.Fake<IOAuthWebRequestGenerator>();
            var parameters = A.Fake<ICreateBearerTokenParameters>();

            A.CallTo(() => _fakeAuthQueryGenerator.GetCreateBearerTokenQuery(parameters)).Returns(url);
            A.CallTo(() => _fakeTwitterAccessor.ExecuteRequestAsync<CreateTokenResponseDTO>(request)).Returns(expectedResult);
            A.CallTo(() => _fakeOAuthWebRequestGeneratorFactory.Create(request)).Returns(oAuthWebRequestGenerator);

            // Act
            var result = await queryExecutor.CreateBearerTokenAsync(parameters, request);

            // Assert
            Assert.Equal(result, expectedResult);
            Assert.Equal(request.Query.Url, url);
            Assert.Equal(HttpMethod.POST, request.Query.HttpMethod);
            Assert.True(request.TwitterClientHandler is BearerHttpHandler);
        }

        [Fact]
        public async Task RequestAuthUrl_ReturnsFromTwitterAccessorAsync()
        {
            // Arrange
            var queryExecutor = CreateAuthQueryExecutor();

            var url = TestHelper.GenerateString();
            var request = A.Fake<ITwitterRequest>();
            var expectedResult = A.Fake<ITwitterResult<CreateTokenResponseDTO>>();
            var oAuthQueryParams = A.Fake<IOAuthQueryParameter>();
            var parameters = A.Fake<RequestAuthUrlInternalParameters>();

            var oAuthWebRequestGenerator = A.Fake<IOAuthWebRequestGenerator>();
            A.CallTo(() => oAuthWebRequestGenerator.GenerateParameter("oauth_callback", It.IsAny<string>(), true, true, false)).Returns(oAuthQueryParams);

            A.CallTo(() => _fakeAuthQueryGenerator.GetRequestAuthUrlQuery(parameters)).Returns(url);
            A.CallTo(() => _fakeTwitterAccessor.ExecuteRequestAsync(request)).Returns(expectedResult);
            A.CallTo(() => _fakeOAuthWebRequestGeneratorFactory.Create(request)).Returns(oAuthWebRequestGenerator);

            // Act
            var result = await queryExecutor.RequestAuthUrlAsync(parameters, request);

            // Assert
            Assert.Equal(result, expectedResult);
            Assert.Equal(request.Query.Url, url);
            Assert.Equal(HttpMethod.POST, request.Query.HttpMethod);
            Assert.True(request.TwitterClientHandler is AuthHttpHandler);
        }

        [Fact]
        public async Task RequestCredentials_ReturnsFromTwitterAccessorAsync()
        {
            // Arrange
            var queryExecutor = CreateAuthQueryExecutor();

            var url = TestHelper.GenerateString();
            var request = A.Fake<ITwitterRequest>();
            var expectedResult = A.Fake<ITwitterResult<CreateTokenResponseDTO>>();
            var oAuthQueryParams = A.Fake<IOAuthQueryParameter>();
            var parameters = A.Fake<IRequestCredentialsParameters>();

            var oAuthWebRequestGenerator = A.Fake<IOAuthWebRequestGenerator>();
            A.CallTo(() => oAuthWebRequestGenerator.GenerateParameter("oauth_verifier", It.IsAny<string>(), true, true, false)).Returns(oAuthQueryParams);

            A.CallTo(() => _fakeAuthQueryGenerator.GetRequestCredentialsQuery(parameters)).Returns(url);
            A.CallTo(() => _fakeTwitterAccessor.ExecuteRequestAsync(request)).Returns(expectedResult);
            A.CallTo(() => _fakeOAuthWebRequestGeneratorFactory.Create(request)).Returns(oAuthWebRequestGenerator);

            // Act
            var result = await queryExecutor.RequestCredentialsAsync(parameters, request);

            // Assert
            Assert.Equal(result, expectedResult);
            Assert.Equal(request.Query.Url, url);
            Assert.Equal(HttpMethod.POST, request.Query.HttpMethod);
            Assert.True(request.TwitterClientHandler is AuthHttpHandler);
        }

        [Fact]
        public async Task InvalidateBearerToken_ReturnsFromTwitterAccessorAsync()
        {
            // Arrange
            var queryExecutor = CreateAuthQueryExecutor();

            var url = TestHelper.GenerateString();
            var request = A.Fake<ITwitterRequest>();
            var expectedResult = A.Fake<ITwitterResult<InvalidateTokenResponse>>();
            var parameters = A.Fake<IInvalidateBearerTokenParameters>();

            A.CallTo(() => _fakeAuthQueryGenerator.GetInvalidateBearerTokenQuery(parameters)).Returns(url);
            A.CallTo(() => _fakeTwitterAccessor.ExecuteRequestAsync<InvalidateTokenResponse>(request)).Returns(expectedResult);

            // Act
            var result = await queryExecutor.InvalidateBearerTokenAsync(parameters, request);

            // Assert
            Assert.Equal(result, expectedResult);
            Assert.Equal(request.Query.Url, url);
            Assert.Equal(HttpMethod.POST, request.Query.HttpMethod);
            Assert.True(request.TwitterClientHandler is InvalidateTokenHttpHandler);
        }

        [Fact]
        public async Task InvalidateAccessToken_ReturnsFromTwitterAccessorAsync()
        {
            // Arrange
            var queryExecutor = CreateAuthQueryExecutor();

            var url = TestHelper.GenerateString();
            var request = A.Fake<ITwitterRequest>();
            var expectedResult = A.Fake<ITwitterResult<InvalidateTokenResponse>>();
            var parameters = A.Fake<IInvalidateAccessTokenParameters>();

            A.CallTo(() => _fakeAuthQueryGenerator.GetInvalidateAccessTokenQuery(parameters)).Returns(url);
            A.CallTo(() => _fakeTwitterAccessor.ExecuteRequestAsync<InvalidateTokenResponse>(request)).Returns(expectedResult);

            // Act
            var result = await queryExecutor.InvalidateAccessTokenAsync(parameters, request);

            // Assert
            Assert.Equal(result, expectedResult);
            Assert.Equal(request.Query.Url, url);
            Assert.Equal(HttpMethod.POST, request.Query.HttpMethod);
        }
    }
}