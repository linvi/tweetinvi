using System;
using System.IO;
using System.Threading.Tasks;
using FakeItEasy;
using Tweetinvi.Controllers.User;
using Tweetinvi.Core.Web;
using Tweetinvi.Models;
using Tweetinvi.Models.DTO;
using Tweetinvi.Models.DTO.QueryDTO;
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
            _fakeUserQueryExecutor = _fakeBuilder.GetFake<IUserQueryExecutor>();
            _fakeTwitterResultFactory = _fakeBuilder.GetFake<ITwitterResultFactory>();
        }

        private readonly FakeClassBuilder<UserController> _fakeBuilder;
        private readonly Fake<IUserQueryExecutor> _fakeUserQueryExecutor;
        private readonly Fake<ITwitterResultFactory> _fakeTwitterResultFactory;

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
            var userDTOTwitterResult = A.Fake<ITwitterResult<IUserDTO>>();
            var expectedResult = A.Fake<ITwitterResult<IUserDTO, IUser>>();

            _fakeUserQueryExecutor.CallsTo(x => x.GetUser(parameters, A<ITwitterRequest>.Ignored)).Returns(userDTOTwitterResult);
            _fakeTwitterResultFactory.CallsTo(x => x.Create(userDTOTwitterResult, It.IsAny<Func<IUserDTO, IUser>>())).Returns(expectedResult);

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
            var userDTOTwitterResult = A.Fake<ITwitterResult<IUserDTO[]>>();
            var expectedResult = A.Fake<ITwitterResult<IUserDTO[], IUser[]>>();

            _fakeUserQueryExecutor.CallsTo(x => x.GetUsers(parameters, A<ITwitterRequest>.Ignored)).Returns(userDTOTwitterResult);
            _fakeTwitterResultFactory.CallsTo(x => x.Create(userDTOTwitterResult, It.IsAny<Func<IUserDTO[], IUser[]>>())).Returns(expectedResult);

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
            var expectedResult = A.Fake<ITwitterResult<IIdsCursorQueryResultDTO>>();

            _fakeUserQueryExecutor.CallsTo(x => x.GetFollowerIds(A<IGetFollowerIdsParameters>.Ignored, A<ITwitterRequest>.Ignored)).Returns(expectedResult);

            // Act
            var friendIdsIterator = controller.GetFollowerIds(parameters, A.Fake<ITwitterRequest>());

            A.CallTo(() => _fakeUserQueryExecutor.FakedObject.GetFollowerIds(A<IGetFollowerIdsParameters>.Ignored, A<ITwitterRequest>.Ignored)).MustNotHaveHappened();

            var page = await friendIdsIterator.MoveToNextPage();

            A.CallTo(() => _fakeUserQueryExecutor.FakedObject.GetFollowerIds(A<IGetFollowerIdsParameters>.Ignored, A<ITwitterRequest>.Ignored)).MustHaveHappenedOnceExactly();

            // Assert
            Assert.Equal(page.Content, expectedResult);
        }

        [Fact]
        public async Task GetFriendIds_ReturnsFromUserQueryExecutor()
        {
            // Arrange
            var controller = CreateUserController();

            var parameters = new GetFriendIdsParameters("username");
            var expectedResult = A.Fake<ITwitterResult<IIdsCursorQueryResultDTO>>();

            _fakeUserQueryExecutor.CallsTo(x => x.GetFriendIds(A<IGetFriendIdsParameters>.Ignored, A<ITwitterRequest>.Ignored)).Returns(expectedResult);

            // Act
            var friendIdsIterator = controller.GetFriendIds(parameters, A.Fake<ITwitterRequest>());

            A.CallTo(() => _fakeUserQueryExecutor.FakedObject.GetFriendIds(A<IGetFriendIdsParameters>.Ignored, A<ITwitterRequest>.Ignored)).MustNotHaveHappened();

            var page = await friendIdsIterator.MoveToNextPage();

            A.CallTo(() => _fakeUserQueryExecutor.FakedObject.GetFriendIds(A<IGetFriendIdsParameters>.Ignored, A<ITwitterRequest>.Ignored)).MustHaveHappenedOnceExactly();

            // Assert
            Assert.Equal(page.Content, expectedResult);
        }

        [Fact]
        public async Task GetProfileImageStream_ReturnsFromUserQueryExecutor()
        {
            // Arrange
            var controller = CreateUserController();
            var stream = A.Fake<Stream>();
            var parameters = A.Fake<IGetProfileImageParameters>();
            var request = A.Fake<ITwitterRequest>();

            _fakeUserQueryExecutor.CallsTo(x => x.GetProfileImageStream(parameters, It.IsAny<ITwitterRequest>())).Returns(stream);

            // Act
            var result = await controller.GetProfileImageStream(parameters, request);

            // Assert
            Assert.Equal(result, stream);
        }
    }
}