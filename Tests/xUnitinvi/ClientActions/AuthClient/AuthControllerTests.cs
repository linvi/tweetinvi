using System.Threading.Tasks;
using FakeItEasy;
using Tweetinvi.Controllers.Auth;
using Tweetinvi.Core.DTO;
using Tweetinvi.Core.Web;
using Tweetinvi.Models;
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
    }
}