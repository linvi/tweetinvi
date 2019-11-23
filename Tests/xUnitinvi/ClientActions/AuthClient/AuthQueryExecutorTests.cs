using System.Threading.Tasks;
using FakeItEasy;
using Tweetinvi;
using Tweetinvi.Controllers.Auth;
using Tweetinvi.Core.DTO;
using Tweetinvi.Core.Web;
using Tweetinvi.Models;
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
            _fakeTwitterAccessor = _fakeBuilder.GetFake<ITwitterAccessor>().FakedObject;
        }

        private readonly FakeClassBuilder<AuthQueryExecutor> _fakeBuilder;
        private readonly IAuthQueryGenerator _fakeAuthQueryGenerator;
        private readonly ITwitterAccessor _fakeTwitterAccessor;

        private AuthQueryExecutor CreateAuthQueryExecutor()
        {
            return _fakeBuilder.GenerateClass();
        }

        [Fact]
        public async Task CreateBearerToken_ReturnsBearerToken()
        {
            // Arrange
            // TODO : remove in upcoming iteration when TwitterClient handlers do not
            // use dependency injector to crate the OAuthWebRequestGenerator
            TweetinviContainer.Resolve<IOAuthWebRequestGenerator>();
            var queryExecutor = CreateAuthQueryExecutor();

            var url = TestHelper.GenerateString();
            var request = A.Fake<ITwitterRequest>();
            var expectedResult = A.Fake<ITwitterResult<CreateTokenResponseDTO>>();

            A.CallTo(() => _fakeAuthQueryGenerator.GetCreateBearerTokenQuery()).Returns(url);
            A.CallTo(() => _fakeTwitterAccessor.ExecuteRequest<CreateTokenResponseDTO>(A<ITwitterRequest>.Ignored)).Returns(expectedResult);

            // Act
            var result = await queryExecutor.CreateBearerToken(request);

            // Assert
            Assert.Equal(result, expectedResult);
            Assert.Equal(request.Query.Url, url);
            Assert.Equal(HttpMethod.POST, request.Query.HttpMethod);
        }
    }
}