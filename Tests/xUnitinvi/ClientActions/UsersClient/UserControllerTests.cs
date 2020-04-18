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

            var parameters = new GetUsersParameters(new[] { "username" });
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

        [Fact]
        public async Task GetAuthenticatedUser_ReturnsFromUserQueryExecutor()
        {
            // Arrange
            var controller = CreateUserController();
            var parameters = new GetAuthenticatedUserParameters();
            var request = A.Fake<ITwitterRequest>();
            var twitterResult = A.Fake<ITwitterResult<IUserDTO>>();

            A.CallTo(() => _fakeUserQueryExecutor.GetAuthenticatedUser(parameters, request)).Returns(twitterResult);

            // Act
            var result = await controller.GetAuthenticatedUser(parameters, request);

            // Assert
            Assert.Equal(result, twitterResult);
        }

        // BLOCK
        [Fact]
        public async Task GetBlockedUserIds_ReturnsAllPages()
        {
            // arrange
            var accountController = CreateUserController();
            var parameters = new GetBlockedUserIdsParameters();

            var iterator = accountController.GetBlockedUserIdsIterator(parameters, A.Fake<ITwitterRequest>());
            var iteratorTestRunner = new TwitterIdsIteratorTestRunner(iterator);

            iteratorTestRunner.Arrange(A.CallTo(() => _fakeUserQueryExecutor.GetBlockedUserIds(It.IsAny<IGetBlockedUserIdsParameters>(), It.IsAny<ITwitterRequest>())));

            // act
            await iteratorTestRunner.Act();

            // assert
            await iteratorTestRunner.Assert();
        }

        [Fact]
        public async Task GetBlockedUsers_ReturnsAllPages()
        {
            // arrange
            var accountController = CreateUserController();
            var parameters = new GetBlockedUsersParameters();

            var iterator = accountController.GetBlockedUsersIterator(parameters, A.Fake<ITwitterRequest>());
            var iteratorTestRunner = new TwitterUsersIteratorTestRunner(iterator);

            iteratorTestRunner.Arrange(A.CallTo(() => _fakeUserQueryExecutor.GetBlockedUsers(It.IsAny<IGetBlockedUsersParameters>(), It.IsAny<ITwitterRequest>())));

            // act
            await iteratorTestRunner.Act();

            // assert
            await iteratorTestRunner.Assert();
        }

        [Fact]
        public async Task BlockUser_ReturnsFromUserQueryExecutor()
        {
            // Arrange
            var controller = CreateUserController();
            var userDTO = A.Fake<IUserDTO>();

            var blockUserParameters = new BlockUserParameters(userDTO);
            var request = A.Fake<ITwitterRequest>();
            var expectedResult = A.Fake<ITwitterResult<IUserDTO>>();

            A.CallTo(() => _fakeUserQueryExecutor.BlockUser(blockUserParameters, request)).Returns(expectedResult);

            // Act
            var result = await controller.BlockUser(blockUserParameters, request);

            // Assert
            Assert.Equal(result, expectedResult);
        }

        [Fact]
        public async Task UnblockUser_ReturnsFromUserQueryExecutor()
        {
            // Arrange
            var controller = CreateUserController();
            var userDTO = A.Fake<IUserDTO>();

            var parameters = new UnblockUserParameters(userDTO);
            var request = A.Fake<ITwitterRequest>();
            var expectedResult = A.Fake<ITwitterResult<IUserDTO>>();

            A.CallTo(() => _fakeUserQueryExecutor.UnblockUser(parameters, request)).Returns(expectedResult);

            // Act
            var result = await controller.UnblockUser(parameters, request);

            // Assert
            Assert.Equal(result, expectedResult);
        }

        [Fact]
        public async Task ReportUserFromSpam_ReturnsFromUserQueryExecutor()
        {
            // Arrange
            var controller = CreateUserController();
            var userDTO = A.Fake<IUserDTO>();

            var parameters = new ReportUserForSpamParameters(userDTO);
            var request = A.Fake<ITwitterRequest>();
            var expectedResult = A.Fake<ITwitterResult<IUserDTO>>();

            A.CallTo(() => _fakeUserQueryExecutor.ReportUserForSpam(parameters, request)).Returns(expectedResult);

            // Act
            var result = await controller.ReportUserForSpam(parameters, request);

            // Assert
            Assert.Equal(result, expectedResult);
        }

        // FOLLOWERS

        [Fact]
        public async Task FollowUser_ReturnsFromUserQueryExecutor()
        {
            // Arrange
            var controller = CreateUserController();
            var userDTO = A.Fake<IUserDTO>();

            var followUserParameters = new FollowUserParameters(userDTO);
            var request = A.Fake<ITwitterRequest>();
            var expectedResult = A.Fake<ITwitterResult<IUserDTO>>();

            A.CallTo(() => _fakeUserQueryExecutor.FollowUser(followUserParameters, request)).Returns(expectedResult);

            // Act
            var result = await controller.FollowUser(followUserParameters, request);

            // Assert
            Assert.Equal(result, expectedResult);
        }

        [Fact]
        public async Task UnfollowUser_ReturnsFromUserQueryExecutor()
        {
            // Arrange
            var controller = CreateUserController();
            var userDTO = A.Fake<IUserDTO>();

            var followUserParameters = new UnfollowUserParameters(userDTO);
            var request = A.Fake<ITwitterRequest>();
            var expectedResult = A.Fake<ITwitterResult<IUserDTO>>();

            A.CallTo(() => _fakeUserQueryExecutor.UnfollowUser(followUserParameters, request)).Returns(expectedResult);

            // Act
            var result = await controller.UnfollowUser(followUserParameters, request);

            // Assert
            Assert.Equal(result, expectedResult);
        }

        // ONGOING REQUESTS

        [Fact]
        public async Task GetUserIdsRequestingFriendship_NextPage_ReturnsAllPages()
        {
            // arrange
            var accountController = CreateUserController();
            var parameters = new GetUserIdsRequestingFriendshipParameters();

            var iterator = accountController.GetUserIdsRequestingFriendshipIterator(parameters, A.Fake<ITwitterRequest>());
            var iteratorTestRunner = new TwitterIdsIteratorTestRunner(iterator);

            iteratorTestRunner.Arrange(A.CallTo(() => _fakeUserQueryExecutor.GetUserIdsRequestingFriendship(It.IsAny<IGetUserIdsRequestingFriendshipParameters>(), It.IsAny<ITwitterRequest>())));

            // act
            await iteratorTestRunner.Act();

            // assert
            await iteratorTestRunner.Assert();
        }

        [Fact]
        public async Task GetUserIdsYouRequestedToFollow_NextPage_ReturnsAllPages()
        {
            // arrange
            var accountController = CreateUserController();
            var parameters = new GetUserIdsYouRequestedToFollowParameters();

            var iterator = accountController.GetUserIdsYouRequestedToFollowIterator(parameters, A.Fake<ITwitterRequest>());
            var iteratorTestRunner = new TwitterIdsIteratorTestRunner(iterator);

            iteratorTestRunner.Arrange(A.CallTo(() => _fakeUserQueryExecutor.GetUserIdsYouRequestedToFollow(It.IsAny<IGetUserIdsYouRequestedToFollowParameters>(), It.IsAny<ITwitterRequest>())));

            // act
            await iteratorTestRunner.Act();

            // assert
            await iteratorTestRunner.Assert();
        }

        // FRIENDSHIPS

        [Fact]
        public async Task UpdateRelationship_ReturnsFromUserQueryExecutor()
        {
            // Arrange
            var controller = CreateUserController();

            var parameters = new UpdateRelationshipParameters(42);
            var request = A.Fake<ITwitterRequest>();
            var expectedResult = A.Fake<ITwitterResult<IRelationshipDetailsDTO>>();

            A.CallTo(() => _fakeUserQueryExecutor.UpdateRelationship(parameters, request)).Returns(expectedResult);

            // Act
            var result = await controller.UpdateRelationship(parameters, request);

            // Assert
            Assert.Equal(result, expectedResult);
        }

        [Fact]
        public async Task GetRelationshipsWith_ReturnsFromUserQueryExecutor()
        {
            // Arrange
            var controller = CreateUserController();

            var parameters = new GetRelationshipsWithParameters(new long[] { 42 });
            var request = A.Fake<ITwitterRequest>();
            var expectedResult = A.Fake<ITwitterResult<IRelationshipStateDTO[]>>();

            A.CallTo(() => _fakeUserQueryExecutor.GetRelationshipsWith(parameters, request)).Returns(expectedResult);

            // Act
            var result = await controller.GetRelationshipsWith(parameters, request);

            // Assert
            Assert.Equal(result, expectedResult);
        }

        // MUTE

        [Fact]
        public async Task GetUserIdsWhoseRetweetsAreMuted_ReturnsFromUserQueryExecutor()
        {
            // arrange
            var accountController = CreateUserController();
            var parameters = new GetUserIdsWhoseRetweetsAreMutedParameters();
            var request = A.Fake<ITwitterRequest>();
            var expectedResult = A.Fake<ITwitterResult<long[]>>();

            A.CallTo(() => _fakeUserQueryExecutor.GetUserIdsWhoseRetweetsAreMuted(parameters, request)).Returns(expectedResult);

            // act
            var result = await accountController.GetUserIdsWhoseRetweetsAreMuted(parameters, request);

            // assert
            Assert.Same(result, expectedResult);
        }

        [Fact]
        public async Task GetMutedUserIds_ReturnsAllPages()
        {
            // arrange
            var accountController = CreateUserController();
            var parameters = new GetMutedUserIdsParameters();

            var iterator = accountController.GetMutedUserIdsIterator(parameters, A.Fake<ITwitterRequest>());
            var testRunner = new TwitterIdsIteratorTestRunner(iterator);

            testRunner.Arrange(A.CallTo(() => _fakeUserQueryExecutor.GetMutedUserIds(It.IsAny<IGetMutedUserIdsParameters>(), It.IsAny<ITwitterRequest>())));

            // act
            await testRunner.Act();

            // assert
            await testRunner.Assert();
        }

        [Fact]
        public async Task GetMutedUsers_ReturnsAllPages()
        {
            // arrange
            var accountController = CreateUserController();
            var parameters = new GetMutedUsersParameters();

            var iterator = accountController.GetMutedUsersIterator(parameters, A.Fake<ITwitterRequest>());
            var iteratorTestRunner = new TwitterUsersIteratorTestRunner(iterator);

            iteratorTestRunner.Arrange(A.CallTo(() => _fakeUserQueryExecutor.GetMutedUsers(It.IsAny<IGetMutedUsersParameters>(), It.IsAny<ITwitterRequest>())));

            // act
            await iteratorTestRunner.Act();

            // assert
            await iteratorTestRunner.Assert();
        }

        [Fact]
        public async Task MuteUser_ReturnsFromUserQueryExecutor()
        {
            // arrange
            var accountController = CreateUserController();
            var parameters = new MuteUserParameters(42);
            var request = A.Fake<ITwitterRequest>();
            var expectedResult = A.Fake<ITwitterResult<IUserDTO>>();

            A.CallTo(() => _fakeUserQueryExecutor.MuteUser(parameters, request)).Returns(expectedResult);

            // act
            var result = await accountController.MuteUser(parameters, request);

            // assert
            Assert.Same(result, expectedResult);
        }

        [Fact]
        public async Task UnmuteUser_ReturnsFromUserQueryExecutor()
        {
            // arrange
            var accountController = CreateUserController();
            var parameters = new UnmuteUserParameters(42);
            var request = A.Fake<ITwitterRequest>();
            var expectedResult = A.Fake<ITwitterResult<IUserDTO>>();

            A.CallTo(() => _fakeUserQueryExecutor.UnmuteUser(parameters, request)).Returns(expectedResult);

            // act
            var result = await accountController.UnmuteUser(parameters, request);

            // assert
            Assert.Same(result, expectedResult);
        }
    }
}