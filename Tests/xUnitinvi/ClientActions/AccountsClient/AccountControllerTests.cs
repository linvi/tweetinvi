using System;
using System.Threading.Tasks;
using FakeItEasy;
using Tweetinvi.Controllers.Account;
using Tweetinvi.Core.DTO.Cursor;
using Tweetinvi.Core.Web;
using Tweetinvi.Exceptions;
using Tweetinvi.Models;
using Tweetinvi.Models.DTO;
using Tweetinvi.Models.DTO.QueryDTO;
using Tweetinvi.Parameters;
using Tweetinvi.WebLogic;
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

        [Fact]
        public async Task GetUserIdsRequestingFriendship_MoveToNextPage_ReturnsAllPages()
        {
            // arrange
            var accountController = CreateAccountController();
            var parameters = new GetUserIdsRequestingFriendshipParameters();

            ITwitterResult<IIdsCursorQueryResultDTO>[] results =
            {
                new TwitterResult<IIdsCursorQueryResultDTO>(null)
                {
                    DataTransferObject = new IdsCursorQueryResultDTO
                    {
                        Ids = new long[] { 42, 43 },
                        NextCursorStr = "cursor_to_page_2",
                    },
                    Response = new TwitterResponse()
                },
                new TwitterResult<IIdsCursorQueryResultDTO>(null)
                {
                    DataTransferObject = new IdsCursorQueryResultDTO
                    {
                        Ids = new long[] { 44, 45 },
                        NextCursorStr = "0",
                    },
                    Response = new TwitterResponse()
                }
            };

            A.CallTo(() => _fakeAccountQueryExecutor.GetUserIdsRequestingFriendship(It.IsAny<IGetUserIdsRequestingFriendshipParameters>(), It.IsAny<ITwitterRequest>()))
                .ReturnsNextFromSequence(results);

            var result = accountController.GetUserIdsRequestingFriendship(parameters, A.Fake<ITwitterRequest>());

            // act
            var page1 = await result.MoveToNextPage();
            var page2 = await result.MoveToNextPage();

            // assert
            Assert.Equal(page1.Content, results[0]);
            Assert.False(page1.IsLastPage);

            Assert.Equal(page2.Content, results[1]);
            Assert.True(page2.IsLastPage);

            await Assert.ThrowsAsync<TwitterIteratorAlreadyCompletedException>(() => result.MoveToNextPage());
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
    }
}