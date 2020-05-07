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
        public async Task GetUser_ReturnsFromUserQueryExecutorAsync()
        {
            // Arrange
            var controller = CreateUserController();

            var parameters = new GetUserParameters("username");
            var expectedResult = A.Fake<ITwitterResult<IUserDTO>>();

            A.CallTo(() => _fakeUserQueryExecutor.GetUserAsync(parameters, A<ITwitterRequest>.Ignored)).Returns(expectedResult);

            // Act
            var twitterResultUser = await controller.GetUserAsync(parameters, A.Fake<ITwitterRequest>());

            // Assert
            Assert.Same(twitterResultUser, expectedResult);
        }

        [Fact]
        public async Task GetUsers_ReturnsFromUserQueryExecutorAsync()
        {
            // Arrange
            var controller = CreateUserController();

            var parameters = new GetUsersParameters(new[] { "username" });
            var expectedResult = A.Fake<ITwitterResult<IUserDTO[]>>();

            A.CallTo(() => _fakeUserQueryExecutor.GetUsersAsync(parameters, A<ITwitterRequest>.Ignored)).Returns(expectedResult);

            // Act
            var twitterResultUser = await controller.GetUsersAsync(parameters, A.Fake<ITwitterRequest>());

            // Assert
            Assert.Same(twitterResultUser, expectedResult);
        }

        [Fact]
        public async Task GetFollowerIds_ReturnsFromUserQueryExecutorAsync()
        {
            // Arrange
            var controller = CreateUserController();
            var parameters = new GetFollowerIdsParameters("username");

            var iterator = controller.GetFollowerIdsIterator(parameters, A.Fake<ITwitterRequest>());
            var iteratorTestRunner = new TwitterIdsIteratorTestRunner(iterator);

            iteratorTestRunner.Arrange(A.CallTo(() => _fakeUserQueryExecutor.GetFollowerIdsAsync(A<IGetFollowerIdsParameters>.Ignored, A<ITwitterRequest>.Ignored)));
            await iteratorTestRunner.ActAsync();
            await iteratorTestRunner.AssertAsync();
        }

        [Fact]
        public async Task GetFriendIds_ReturnsFromUserQueryExecutorAsync()
        {
            // Arrange
            var controller = CreateUserController();

            var parameters = new GetFriendIdsParameters("username");

            var iterator = controller.GetFriendIdsIterator(parameters, A.Fake<ITwitterRequest>());
            var iteratorTestRunner = new TwitterIdsIteratorTestRunner(iterator);

            iteratorTestRunner.Arrange(A.CallTo(() => _fakeUserQueryExecutor.GetFriendIdsAsync(A<IGetFriendIdsParameters>.Ignored, A<ITwitterRequest>.Ignored)));
            await iteratorTestRunner.ActAsync();
            await iteratorTestRunner.AssertAsync();
        }

        [Fact]
        public async Task GetRelationshipBetween_ReturnsFromUserQueryExecutorAsync()
        {
            // Arrange
            var controller = CreateUserController();
            var parameters = new GetRelationshipBetweenParameters(42, 43);
            var request = A.Fake<ITwitterRequest>();
            var expectedResult = A.Fake<ITwitterResult<IRelationshipDetailsDTO>>();

            A.CallTo(() => _fakeUserQueryExecutor.GetRelationshipBetweenAsync(parameters, request)).Returns(expectedResult);

            // Act
            var result = await controller.GetRelationshipBetweenAsync(parameters, request);

            // Assert
            Assert.Equal(result, expectedResult);
        }

        [Fact]
        public async Task GetProfileImageStream_ReturnsFromUserQueryExecutorAsync()
        {
            // Arrange
            var controller = CreateUserController();
            var stream = A.Fake<Stream>();
            var parameters = A.Fake<IGetProfileImageParameters>();
            var request = A.Fake<ITwitterRequest>();

            A.CallTo(() => _fakeUserQueryExecutor.GetProfileImageStreamAsync(parameters, It.IsAny<ITwitterRequest>())).Returns(stream);

            // Act
            var result = await controller.GetProfileImageStreamAsync(parameters, request);

            // Assert
            Assert.Equal(result, stream);
        }

        [Fact]
        public async Task GetAuthenticatedUser_ReturnsFromUserQueryExecutorAsync()
        {
            // Arrange
            var controller = CreateUserController();
            var parameters = new GetAuthenticatedUserParameters();
            var request = A.Fake<ITwitterRequest>();
            var twitterResult = A.Fake<ITwitterResult<IUserDTO>>();

            A.CallTo(() => _fakeUserQueryExecutor.GetAuthenticatedUserAsync(parameters, request)).Returns(twitterResult);

            // Act
            var result = await controller.GetAuthenticatedUserAsync(parameters, request);

            // Assert
            Assert.Equal(result, twitterResult);
        }

        // BLOCK
        [Fact]
        public async Task GetBlockedUserIds_ReturnsAllPagesAsync()
        {
            // arrange
            var accountController = CreateUserController();
            var parameters = new GetBlockedUserIdsParameters();

            var iterator = accountController.GetBlockedUserIdsIterator(parameters, A.Fake<ITwitterRequest>());
            var iteratorTestRunner = new TwitterIdsIteratorTestRunner(iterator);

            iteratorTestRunner.Arrange(A.CallTo(() => _fakeUserQueryExecutor.GetBlockedUserIdsAsync(It.IsAny<IGetBlockedUserIdsParameters>(), It.IsAny<ITwitterRequest>())));

            // act
            await iteratorTestRunner.ActAsync();

            // assert
            await iteratorTestRunner.AssertAsync();
        }

        [Fact]
        public async Task GetBlockedUsers_ReturnsAllPagesAsync()
        {
            // arrange
            var accountController = CreateUserController();
            var parameters = new GetBlockedUsersParameters();

            var iterator = accountController.GetBlockedUsersIterator(parameters, A.Fake<ITwitterRequest>());
            var iteratorTestRunner = new TwitterUsersIteratorTestRunner(iterator);

            iteratorTestRunner.Arrange(A.CallTo(() => _fakeUserQueryExecutor.GetBlockedUsersAsync(It.IsAny<IGetBlockedUsersParameters>(), It.IsAny<ITwitterRequest>())));

            // act
            await iteratorTestRunner.ActAsync();

            // assert
            await iteratorTestRunner.AssertAsync();
        }

        [Fact]
        public async Task BlockUser_ReturnsFromUserQueryExecutorAsync()
        {
            // Arrange
            var controller = CreateUserController();
            var userDTO = A.Fake<IUserDTO>();

            var blockUserParameters = new BlockUserParameters(userDTO);
            var request = A.Fake<ITwitterRequest>();
            var expectedResult = A.Fake<ITwitterResult<IUserDTO>>();

            A.CallTo(() => _fakeUserQueryExecutor.BlockUserAsync(blockUserParameters, request)).Returns(expectedResult);

            // Act
            var result = await controller.BlockUserAsync(blockUserParameters, request);

            // Assert
            Assert.Equal(result, expectedResult);
        }

        [Fact]
        public async Task UnblockUser_ReturnsFromUserQueryExecutorAsync()
        {
            // Arrange
            var controller = CreateUserController();
            var userDTO = A.Fake<IUserDTO>();

            var parameters = new UnblockUserParameters(userDTO);
            var request = A.Fake<ITwitterRequest>();
            var expectedResult = A.Fake<ITwitterResult<IUserDTO>>();

            A.CallTo(() => _fakeUserQueryExecutor.UnblockUserAsync(parameters, request)).Returns(expectedResult);

            // Act
            var result = await controller.UnblockUserAsync(parameters, request);

            // Assert
            Assert.Equal(result, expectedResult);
        }

        [Fact]
        public async Task ReportUserFromSpam_ReturnsFromUserQueryExecutorAsync()
        {
            // Arrange
            var controller = CreateUserController();
            var userDTO = A.Fake<IUserDTO>();

            var parameters = new ReportUserForSpamParameters(userDTO);
            var request = A.Fake<ITwitterRequest>();
            var expectedResult = A.Fake<ITwitterResult<IUserDTO>>();

            A.CallTo(() => _fakeUserQueryExecutor.ReportUserForSpamAsync(parameters, request)).Returns(expectedResult);

            // Act
            var result = await controller.ReportUserForSpamAsync(parameters, request);

            // Assert
            Assert.Equal(result, expectedResult);
        }

        // FOLLOWERS

        [Fact]
        public async Task FollowUser_ReturnsFromUserQueryExecutorAsync()
        {
            // Arrange
            var controller = CreateUserController();
            var userDTO = A.Fake<IUserDTO>();

            var followUserParameters = new FollowUserParameters(userDTO);
            var request = A.Fake<ITwitterRequest>();
            var expectedResult = A.Fake<ITwitterResult<IUserDTO>>();

            A.CallTo(() => _fakeUserQueryExecutor.FollowUserAsync(followUserParameters, request)).Returns(expectedResult);

            // Act
            var result = await controller.FollowUserAsync(followUserParameters, request);

            // Assert
            Assert.Equal(result, expectedResult);
        }

        [Fact]
        public async Task UnfollowUser_ReturnsFromUserQueryExecutorAsync()
        {
            // Arrange
            var controller = CreateUserController();
            var userDTO = A.Fake<IUserDTO>();

            var followUserParameters = new UnfollowUserParameters(userDTO);
            var request = A.Fake<ITwitterRequest>();
            var expectedResult = A.Fake<ITwitterResult<IUserDTO>>();

            A.CallTo(() => _fakeUserQueryExecutor.UnfollowUserAsync(followUserParameters, request)).Returns(expectedResult);

            // Act
            var result = await controller.UnfollowUserAsync(followUserParameters, request);

            // Assert
            Assert.Equal(result, expectedResult);
        }

        // ONGOING REQUESTS

        [Fact]
        public async Task GetUserIdsRequestingFriendship_NextPage_ReturnsAllPagesAsync()
        {
            // arrange
            var accountController = CreateUserController();
            var parameters = new GetUserIdsRequestingFriendshipParameters();

            var iterator = accountController.GetUserIdsRequestingFriendshipIterator(parameters, A.Fake<ITwitterRequest>());
            var iteratorTestRunner = new TwitterIdsIteratorTestRunner(iterator);

            iteratorTestRunner.Arrange(A.CallTo(() => _fakeUserQueryExecutor.GetUserIdsRequestingFriendshipAsync(It.IsAny<IGetUserIdsRequestingFriendshipParameters>(), It.IsAny<ITwitterRequest>())));

            // act
            await iteratorTestRunner.ActAsync();

            // assert
            await iteratorTestRunner.AssertAsync();
        }

        [Fact]
        public async Task GetUserIdsYouRequestedToFollow_NextPage_ReturnsAllPagesAsync()
        {
            // arrange
            var accountController = CreateUserController();
            var parameters = new GetUserIdsYouRequestedToFollowParameters();

            var iterator = accountController.GetUserIdsYouRequestedToFollowIterator(parameters, A.Fake<ITwitterRequest>());
            var iteratorTestRunner = new TwitterIdsIteratorTestRunner(iterator);

            iteratorTestRunner.Arrange(A.CallTo(() => _fakeUserQueryExecutor.GetUserIdsYouRequestedToFollowAsync(It.IsAny<IGetUserIdsYouRequestedToFollowParameters>(), It.IsAny<ITwitterRequest>())));

            // act
            await iteratorTestRunner.ActAsync();

            // assert
            await iteratorTestRunner.AssertAsync();
        }

        // FRIENDSHIPS

        [Fact]
        public async Task UpdateRelationship_ReturnsFromUserQueryExecutorAsync()
        {
            // Arrange
            var controller = CreateUserController();

            var parameters = new UpdateRelationshipParameters(42);
            var request = A.Fake<ITwitterRequest>();
            var expectedResult = A.Fake<ITwitterResult<IRelationshipDetailsDTO>>();

            A.CallTo(() => _fakeUserQueryExecutor.UpdateRelationshipAsync(parameters, request)).Returns(expectedResult);

            // Act
            var result = await controller.UpdateRelationshipAsync(parameters, request);

            // Assert
            Assert.Equal(result, expectedResult);
        }

        [Fact]
        public async Task GetRelationshipsWith_ReturnsFromUserQueryExecutorAsync()
        {
            // Arrange
            var controller = CreateUserController();

            var parameters = new GetRelationshipsWithParameters(new long[] { 42 });
            var request = A.Fake<ITwitterRequest>();
            var expectedResult = A.Fake<ITwitterResult<IRelationshipStateDTO[]>>();

            A.CallTo(() => _fakeUserQueryExecutor.GetRelationshipsWithAsync(parameters, request)).Returns(expectedResult);

            // Act
            var result = await controller.GetRelationshipsWithAsync(parameters, request);

            // Assert
            Assert.Equal(result, expectedResult);
        }

        // MUTE

        [Fact]
        public async Task GetUserIdsWhoseRetweetsAreMuted_ReturnsFromUserQueryExecutorAsync()
        {
            // arrange
            var accountController = CreateUserController();
            var parameters = new GetUserIdsWhoseRetweetsAreMutedParameters();
            var request = A.Fake<ITwitterRequest>();
            var expectedResult = A.Fake<ITwitterResult<long[]>>();

            A.CallTo(() => _fakeUserQueryExecutor.GetUserIdsWhoseRetweetsAreMutedAsync(parameters, request)).Returns(expectedResult);

            // act
            var result = await accountController.GetUserIdsWhoseRetweetsAreMutedAsync(parameters, request);

            // assert
            Assert.Same(result, expectedResult);
        }

        [Fact]
        public async Task GetMutedUserIds_ReturnsAllPagesAsync()
        {
            // arrange
            var accountController = CreateUserController();
            var parameters = new GetMutedUserIdsParameters();

            var iterator = accountController.GetMutedUserIdsIterator(parameters, A.Fake<ITwitterRequest>());
            var testRunner = new TwitterIdsIteratorTestRunner(iterator);

            testRunner.Arrange(A.CallTo(() => _fakeUserQueryExecutor.GetMutedUserIdsAsync(It.IsAny<IGetMutedUserIdsParameters>(), It.IsAny<ITwitterRequest>())));

            // act
            await testRunner.ActAsync();

            // assert
            await testRunner.AssertAsync();
        }

        [Fact]
        public async Task GetMutedUsers_ReturnsAllPagesAsync()
        {
            // arrange
            var accountController = CreateUserController();
            var parameters = new GetMutedUsersParameters();

            var iterator = accountController.GetMutedUsersIterator(parameters, A.Fake<ITwitterRequest>());
            var iteratorTestRunner = new TwitterUsersIteratorTestRunner(iterator);

            iteratorTestRunner.Arrange(A.CallTo(() => _fakeUserQueryExecutor.GetMutedUsersAsync(It.IsAny<IGetMutedUsersParameters>(), It.IsAny<ITwitterRequest>())));

            // act
            await iteratorTestRunner.ActAsync();

            // assert
            await iteratorTestRunner.AssertAsync();
        }

        [Fact]
        public async Task MuteUser_ReturnsFromUserQueryExecutorAsync()
        {
            // arrange
            var accountController = CreateUserController();
            var parameters = new MuteUserParameters(42);
            var request = A.Fake<ITwitterRequest>();
            var expectedResult = A.Fake<ITwitterResult<IUserDTO>>();

            A.CallTo(() => _fakeUserQueryExecutor.MuteUserAsync(parameters, request)).Returns(expectedResult);

            // act
            var result = await accountController.MuteUserAsync(parameters, request);

            // assert
            Assert.Same(result, expectedResult);
        }

        [Fact]
        public async Task UnmuteUser_ReturnsFromUserQueryExecutorAsync()
        {
            // arrange
            var accountController = CreateUserController();
            var parameters = new UnmuteUserParameters(42);
            var request = A.Fake<ITwitterRequest>();
            var expectedResult = A.Fake<ITwitterResult<IUserDTO>>();

            A.CallTo(() => _fakeUserQueryExecutor.UnmuteUserAsync(parameters, request)).Returns(expectedResult);

            // act
            var result = await accountController.UnmuteUserAsync(parameters, request);

            // assert
            Assert.Same(result, expectedResult);
        }
    }
}