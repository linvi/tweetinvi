using System;
using System.Threading.Tasks;
using FakeItEasy;
using Tweetinvi.Controllers.Account;
using Tweetinvi.Core.Web;
using Tweetinvi.Models;
using Tweetinvi.Models.DTO;
using Tweetinvi.Parameters;
using Xunit;
using xUnitinvi.TestHelpers;

namespace xUnitinvi.ClientActions.AccountsClient
{
    public class AccountControllerTests
    {
        public AccountControllerTests()
        {
            _fakeBuilder = new FakeClassBuilder<AccountController>();
            _fakeAccountQueryExecutor = _fakeBuilder.GetFake<IAccountQueryExecutor>().FakedObject;
            _fakeTwitterResultFactory = _fakeBuilder.GetFake<ITwitterResultFactory>().FakedObject;
        }

        private readonly FakeClassBuilder<AccountController> _fakeBuilder;
        private readonly IAccountQueryExecutor _fakeAccountQueryExecutor;
        private readonly ITwitterResultFactory _fakeTwitterResultFactory;

        private AccountController CreateAccountController()
        {
            return _fakeBuilder.GenerateClass();
        }

        [Fact]
        public async Task GetAuthenticatedUser_ReturnsFromUserQueryExecutor()
        {
            // Arrange
            var controller = CreateAccountController();
            var parameters = new GetAuthenticatedUserParameters();
            var request = A.Fake<ITwitterRequest>();
            var twitterResult = A.Fake<ITwitterResult<IUserDTO>>();
            var expectedResult = A.Fake<ITwitterResult<IUserDTO, IAuthenticatedUser>>();

            A.CallTo(() => _fakeAccountQueryExecutor.GetAuthenticatedUser(parameters, request)).Returns(twitterResult);
            A.CallTo(() => _fakeTwitterResultFactory.Create(twitterResult, It.IsAny<Func<IUserDTO, IAuthenticatedUser>>())).Returns(expectedResult);

            // Act
            var result = await controller.GetAuthenticatedUser(parameters, request);

            // Assert
            Assert.Equal(result, expectedResult);
        }

        // BLOCK
        [Fact]
        public async Task GetBlockedUserIds_ReturnsAllPages()
        {
            // arrange
            var accountController = CreateAccountController();
            var parameters = new GetBlockedUserIdsParameters();

            var iterator = accountController.GetBlockedUserIds(parameters, A.Fake<ITwitterRequest>());
            var iteratorTestRunner = new TwitterIdsIteratorTestRunner(iterator);

            iteratorTestRunner.Arrange(A.CallTo(() => _fakeAccountQueryExecutor.GetBlockedUserIds(It.IsAny<IGetBlockedUserIdsParameters>(), It.IsAny<ITwitterRequest>())));

            // act
            await iteratorTestRunner.Act();
            
            // assert
            await iteratorTestRunner.Assert();
        }
        
        [Fact]
        public async Task GetBlockedUsers_ReturnsAllPages()
        {
            // arrange
            var accountController = CreateAccountController();
            var parameters = new GetBlockedUsersParameters();

            var iterator = accountController.GetBlockedUsers(parameters, A.Fake<ITwitterRequest>());
            var iteratorTestRunner = new TwitterUsersIteratorTestRunner(iterator);

            iteratorTestRunner.Arrange(A.CallTo(() => _fakeAccountQueryExecutor.GetBlockedUsers(It.IsAny<IGetBlockedUsersParameters>(), It.IsAny<ITwitterRequest>())));

            // act
            await iteratorTestRunner.Act();
            
            // assert
            await iteratorTestRunner.Assert();
        }

        [Fact]
        public async Task BlockUser_ReturnsFromUserQueryExecutor()
        {
            // Arrange
            var controller = CreateAccountController();
            var userDTO = A.Fake<IUserDTO>();

            var blockUserParameters = new BlockUserParameters(userDTO);
            var request = A.Fake<ITwitterRequest>();
            var expectedResult = A.Fake<ITwitterResult<IUserDTO>>();

            A.CallTo(() => _fakeAccountQueryExecutor.BlockUser(blockUserParameters, request)).Returns(expectedResult);

            // Act
            var result = await controller.BlockUser(blockUserParameters, request);

            // Assert
            Assert.Equal(result, expectedResult);
        }

        [Fact]
        public async Task UnblockUser_ReturnsFromUserQueryExecutor()
        {
            // Arrange
            var controller = CreateAccountController();
            var userDTO = A.Fake<IUserDTO>();

            var parameters = new UnblockUserParameters(userDTO);
            var request = A.Fake<ITwitterRequest>();
            var expectedResult = A.Fake<ITwitterResult<IUserDTO>>();

            A.CallTo(() => _fakeAccountQueryExecutor.UnblockUser(parameters, request)).Returns(expectedResult);

            // Act
            var result = await controller.UnblockUser(parameters, request);

            // Assert
            Assert.Equal(result, expectedResult);
        }

        [Fact]
        public async Task ReportUserFromSpam_ReturnsFromUserQueryExecutor()
        {
            // Arrange
            var controller = CreateAccountController();
            var userDTO = A.Fake<IUserDTO>();

            var parameters = new ReportUserForSpamParameters(userDTO);
            var request = A.Fake<ITwitterRequest>();
            var expectedResult = A.Fake<ITwitterResult<IUserDTO>>();

            A.CallTo(() => _fakeAccountQueryExecutor.ReportUserForSpam(parameters, request)).Returns(expectedResult);

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
            var controller = CreateAccountController();
            var userDTO = A.Fake<IUserDTO>();

            var followUserParameters = new FollowUserParameters(userDTO);
            var request = A.Fake<ITwitterRequest>();
            var expectedResult = A.Fake<ITwitterResult<IUserDTO>>();

            A.CallTo(() => _fakeAccountQueryExecutor.FollowUser(followUserParameters, request)).Returns(expectedResult);

            // Act
            var result = await controller.FollowUser(followUserParameters, request);

            // Assert
            Assert.Equal(result, expectedResult);
        }

        [Fact]
        public async Task UnFollowUser_ReturnsFromUserQueryExecutor()
        {
            // Arrange
            var controller = CreateAccountController();
            var userDTO = A.Fake<IUserDTO>();

            var followUserParameters = new UnFollowUserParameters(userDTO);
            var request = A.Fake<ITwitterRequest>();
            var expectedResult = A.Fake<ITwitterResult<IUserDTO>>();

            A.CallTo(() => _fakeAccountQueryExecutor.UnFollowUser(followUserParameters, request)).Returns(expectedResult);

            // Act
            var result = await controller.UnFollowUser(followUserParameters, request);

            // Assert
            Assert.Equal(result, expectedResult);
        }

        // ONGOING REQUESTS

        [Fact]
        public async Task GetUserIdsRequestingFriendship_MoveToNextPage_ReturnsAllPages()
        {
            // arrange
            var accountController = CreateAccountController();
            var parameters = new GetUserIdsRequestingFriendshipParameters();

            var iterator = accountController.GetUserIdsRequestingFriendship(parameters, A.Fake<ITwitterRequest>());
            var iteratorTestRunner = new TwitterIdsIteratorTestRunner(iterator);

            iteratorTestRunner.Arrange(A.CallTo(() => _fakeAccountQueryExecutor.GetUserIdsRequestingFriendship(It.IsAny<IGetUserIdsRequestingFriendshipParameters>(), It.IsAny<ITwitterRequest>())));

            // act
            await iteratorTestRunner.Act();

            // assert
            await iteratorTestRunner.Assert();
        }

        [Fact]
        public async Task GetUserIdsYouRequestedToFollow_MoveToNextPage_ReturnsAllPages()
        {
            // arrange
            var accountController = CreateAccountController();
            var parameters = new GetUserIdsYouRequestedToFollowParameters();

            var iterator = accountController.GetUserIdsYouRequestedToFollow(parameters, A.Fake<ITwitterRequest>());
            var iteratorTestRunner = new TwitterIdsIteratorTestRunner(iterator);

            iteratorTestRunner.Arrange(A.CallTo(() => _fakeAccountQueryExecutor.GetUserIdsYouRequestedToFollow(It.IsAny<IGetUserIdsYouRequestedToFollowParameters>(), It.IsAny<ITwitterRequest>())));

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
            var controller = CreateAccountController();

            var parameters = new UpdateRelationshipParameters(42);
            var request = A.Fake<ITwitterRequest>();
            var expectedResult = A.Fake<ITwitterResult<IRelationshipDetailsDTO>>();

            A.CallTo(() => _fakeAccountQueryExecutor.UpdateRelationship(parameters, request)).Returns(expectedResult);

            // Act
            var result = await controller.UpdateRelationship(parameters, request);

            // Assert
            Assert.Equal(result, expectedResult);
        }

        [Fact]
        public async Task GetRelationshipsWith_ReturnsFromUserQueryExecutor()
        {
            // Arrange
            var controller = CreateAccountController();

            var parameters = new GetRelationshipsWithParameters(new long[] { 42 });
            var request = A.Fake<ITwitterRequest>();
            var expectedResult = A.Fake<ITwitterResult<IRelationshipStateDTO[]>>();

            A.CallTo(() => _fakeAccountQueryExecutor.GetRelationshipsWith(parameters, request)).Returns(expectedResult);

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
            var accountController = CreateAccountController();
            var parameters = new GetUserIdsWhoseRetweetsAreMutedParameters();
            var request = A.Fake<ITwitterRequest>();
            var expectedResult = A.Fake<ITwitterResult<long[]>>();

            A.CallTo(() => _fakeAccountQueryExecutor.GetUserIdsWhoseRetweetsAreMuted(parameters, request)).Returns(expectedResult);
            
            // act
            var result = await accountController.GetUserIdsWhoseRetweetsAreMuted(parameters, request);

            // assert
            Assert.Same(result, expectedResult);
        }
        
        [Fact]
        public async Task GetMutedUserIds_ReturnsAllPages()
        {
            // arrange
            var accountController = CreateAccountController();
            var parameters = new GetMutedUserIdsParameters();

            var iterator = accountController.GetMutedUserIds(parameters, A.Fake<ITwitterRequest>());
            var testRunner = new TwitterIdsIteratorTestRunner(iterator);
            
            testRunner.Arrange(A.CallTo(() => _fakeAccountQueryExecutor.GetMutedUserIds(It.IsAny<IGetMutedUserIdsParameters>(), It.IsAny<ITwitterRequest>())));

            // act
            await testRunner.Act();

            // assert
            await testRunner.Assert();
        }
        
        [Fact]
        public async Task GetMutedUsers_ReturnsAllPages()
        {
            // arrange
            var accountController = CreateAccountController();
            var parameters = new GetMutedUsersParameters();

            var iterator = accountController.GetMutedUsers(parameters, A.Fake<ITwitterRequest>());
            var iteratorTestRunner = new TwitterUsersIteratorTestRunner(iterator);

            iteratorTestRunner.Arrange(A.CallTo(() => _fakeAccountQueryExecutor.GetMutedUsers(It.IsAny<IGetMutedUsersParameters>(), It.IsAny<ITwitterRequest>())));

            // act
            await iteratorTestRunner.Act();
            
            // assert
            await iteratorTestRunner.Assert();
        }
        
        [Fact]
        public async Task MuteUser_ReturnsFromUserQueryExecutor()
        {
            // arrange
            var accountController = CreateAccountController();
            var parameters = new MuteUserParameters(42);
            var request = A.Fake<ITwitterRequest>();
            var expectedResult = A.Fake<ITwitterResult<IUserDTO>>();

            A.CallTo(() => _fakeAccountQueryExecutor.MuteUser(parameters, request)).Returns(expectedResult);
            
            // act
            var result = await accountController.MuteUser(parameters, request);

            // assert
            Assert.Same(result, expectedResult);
        }
        
        [Fact]
        public async Task UnMuteUser_ReturnsFromUserQueryExecutor()
        {
            // arrange
            var accountController = CreateAccountController();
            var parameters = new UnMuteUserParameters(42);
            var request = A.Fake<ITwitterRequest>();
            var expectedResult = A.Fake<ITwitterResult<IUserDTO>>();

            A.CallTo(() => _fakeAccountQueryExecutor.UnMuteUser(parameters, request)).Returns(expectedResult);
            
            // act
            var result = await accountController.UnMuteUser(parameters, request);

            // assert
            Assert.Same(result, expectedResult);
        }
    }
}