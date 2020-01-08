using System.IO;
using System.Threading.Tasks;
using FakeItEasy;
using Tweetinvi.Controllers.User;
using Tweetinvi.Core.Web;
using Tweetinvi.Models;
using Tweetinvi.Models.DTO;
using Tweetinvi.Parameters;
using Xunit;
using xUnitinvi.TestHelpers;

namespace xUnitinvi.ClientActions.UsersClient
{
    public class UserControllerTests
    {
        public UserControllerTests()
        {
            _fakeBuilder = new FakeClassBuilder<UserController>();
            _fakeUserQueryExecutor = _fakeBuilder.GetFake<IUserQueryExecutor>().FakedObject;
        }

        private readonly FakeClassBuilder<UserController> _fakeBuilder;
        private readonly IUserQueryExecutor _fakeUserQueryExecutor;

        private UserController CreateUserController()
        {
            return _fakeBuilder.GenerateClass();
        }

        [Fact]
        public async Task GetUser_ReturnsFromUserQueryExecutor()
        {
            // Arrange
            var controller = CreateUserController();

            var parameters = new GetUserParameters("username");
            var expectedResult = A.Fake<ITwitterResult<IUserDTO>>();

            A.CallTo(() => _fakeUserQueryExecutor.GetUser(parameters, A<ITwitterRequest>.Ignored)).Returns(expectedResult);

            // Act
            var twitterResultUser = await controller.GetUser(parameters, A.Fake<ITwitterRequest>());

            // Assert
            Assert.Same(twitterResultUser, expectedResult);
        }
        
        [Fact]
        public async Task GetUsers_ReturnsFromUserQueryExecutor()
        {
            // Arrange
            var controller = CreateUserController();

            var parameters = new GetUsersParameters(new [] { "username" });
            var expectedResult = A.Fake<ITwitterResult<IUserDTO[]>>();

            A.CallTo(() => _fakeUserQueryExecutor.GetUsers(parameters, A<ITwitterRequest>.Ignored)).Returns(expectedResult);

            // Act
            var twitterResultUser = await controller.GetUsers(parameters, A.Fake<ITwitterRequest>());

            // Assert
            Assert.Same(twitterResultUser, expectedResult);
        }
        
        [Fact]
        public async Task GetFollowerIds_ReturnsFromUserQueryExecutor()
        {
            // Arrange
            var controller = CreateUserController();
            var parameters = new GetFollowerIdsParameters("username");

            var iterator = controller.GetFollowerIdsIterator(parameters, A.Fake<ITwitterRequest>());
            var iteratorTestRunner = new TwitterIdsIteratorTestRunner(iterator);
            
            iteratorTestRunner.Arrange(A.CallTo(() => _fakeUserQueryExecutor.GetFollowerIds(A<IGetFollowerIdsParameters>.Ignored, A<ITwitterRequest>.Ignored)));
            await iteratorTestRunner.Act();
            await iteratorTestRunner.Assert();
        }

        [Fact]
        public async Task GetFriendIds_ReturnsFromUserQueryExecutor()
        {
            // Arrange
            var controller = CreateUserController();

            var parameters = new GetFriendIdsParameters("username");

            var iterator = controller.GetFriendIdsIterator(parameters, A.Fake<ITwitterRequest>());
            var iteratorTestRunner = new TwitterIdsIteratorTestRunner(iterator);

            iteratorTestRunner.Arrange(A.CallTo(() => _fakeUserQueryExecutor.GetFriendIds(A<IGetFriendIdsParameters>.Ignored, A<ITwitterRequest>.Ignored)));
            await iteratorTestRunner.Act();
            await iteratorTestRunner.Assert();
        }

        [Fact]
        public async Task GetRelationshipBetween_ReturnsFromUserQueryExecutor()
        {
            // Arrange
            var controller = CreateUserController();
            var parameters = new GetRelationshipBetweenParameters(42, 43);
            var request = A.Fake<ITwitterRequest>();
            var expectedResult = A.Fake<ITwitterResult<IRelationshipDetailsDTO>>();

            A.CallTo(() => _fakeUserQueryExecutor.GetRelationshipBetween(parameters, request)).Returns(expectedResult);

            // Act
            var result = await controller.GetRelationshipBetween(parameters, request);

            // Assert
            Assert.Equal(result, expectedResult);
        }

        [Fact]
        public async Task GetProfileImageStream_ReturnsFromUserQueryExecutor()
        {
            // Arrange
            var controller = CreateUserController();
            var stream = A.Fake<Stream>();
            var parameters = A.Fake<IGetProfileImageParameters>();
            var request = A.Fake<ITwitterRequest>();

            A.CallTo(() => _fakeUserQueryExecutor.GetProfileImageStream(parameters, It.IsAny<ITwitterRequest>())).Returns(stream);

            // Act
            var result = await controller.GetProfileImageStream(parameters, request);

            // Assert
            Assert.Equal(result, stream);
        }
    }
}