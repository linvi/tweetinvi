using System.Threading.Tasks;
using FakeItEasy;
using Tweetinvi.Controllers.Auth;
using Tweetinvi.Core.DTO;
using Tweetinvi.Core.Web;
using Tweetinvi.Credentials.AuthHttpHandlers;
using Tweetinvi.Models;
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
        public async Task CreateBearerToken_ReturnsBearerToken()
        {
            // Arrange
            var queryExecutor = CreateAuthQueryExecutor();

            var url = TestHelper.GenerateString();
            var request = A.Fake<ITwitterRequest>();
            var expectedResult = A.Fake<ITwitterResult<CreateTokenResponseDTO>>();
            var oAuthWebRequestGenerator = A.Fake<IOAuthWebRequestGenerator>();

            A.CallTo(() => _fakeAuthQueryGenerator.GetCreateBearerTokenQuery()).Returns(url);
            A.CallTo(() => _fakeTwitterAccessor.ExecuteRequest<CreateTokenResponseDTO>(A<ITwitterRequest>.Ignored)).Returns(expectedResult);
            A.CallTo(() => _fakeOAuthWebRequestGeneratorFactory.Create(request)).Returns(oAuthWebRequestGenerator);

            // Act
            var result = await queryExecutor.CreateBearerToken(request);

            // Assert
            Assert.Equal(result, expectedResult);
            Assert.Equal(request.Query.Url, url);
            Assert.Equal(HttpMethod.POST, request.Query.HttpMethod);
            Assert.True(request.TwitterClientHandler is BearerHttpHandler);
        }
    }
}