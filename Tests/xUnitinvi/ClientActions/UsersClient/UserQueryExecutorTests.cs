using System.Threading.Tasks;
using FakeItEasy;
using Tweetinvi;
using Tweetinvi.Controllers.User;
using Tweetinvi.Core.Helpers;
using Tweetinvi.Core.QueryGenerators;
using Tweetinvi.Core.Web;
using Tweetinvi.Models;
using Tweetinvi.Models.DTO;
using Tweetinvi.Models.DTO.QueryDTO;
using Tweetinvi.Parameters;
using Xunit;
using xUnitinvi.TestHelpers;
using Stream = System.IO.Stream;

namespace xUnitinvi.ClientActions.UsersClient
{
    public class UserQueryExecutorTests
    {
        public UserQueryExecutorTests()
        {
            _fakeBuilder = new FakeClassBuilder<UserQueryExecutor>();
            _fakeUserQueryGenerator = _fakeBuilder.GetFake<IUserQueryGenerator>().FakedObject;
            _fakeTwitterAccessor = _fakeBuilder.GetFake<ITwitterAccessor>().FakedObject;
            _fakeWebHelper = _fakeBuilder.GetFake<IWebHelper>().FakedObject;
        }

        private readonly FakeClassBuilder<UserQueryExecutor> _fakeBuilder;
        private readonly IUserQueryGenerator _fakeUserQueryGenerator;
        private readonly ITwitterAccessor _fakeTwitterAccessor;
        private readonly IWebHelper _fakeWebHelper;

        private UserQueryExecutor CreateUserQueryExecutor()
        {
            return _fakeBuilder.GenerateClass();
        }

        [Fact]
        public async Task GetUser_ReturnsUserDTOAsync()
        {
            // Arrange
            var queryExecutor = CreateUserQueryExecutor();
            var request = A.Fake<ITwitterRequest>();
            var url = TestHelper.GenerateString();
            var parameter = new GetUserParameters(42);
            var expectedResult = A.Fake<ITwitterResult<IUserDTO>>();

            A.CallTo(() => _fakeUserQueryGenerator.GetUserQuery(parameter)).Returns(url);
            A.CallTo(() => _fakeTwitterAccessor.ExecuteRequestAsync<IUserDTO>(request)).Returns(expectedResult);

            // Act
            var result = await queryExecutor.GetUserAsync(parameter, request);

            // Assert
            Assert.Equal(result, expectedResult);
            Assert.Equal(request.Query.Url, url);
            Assert.Equal(HttpMethod.GET, request.Query.HttpMethod);
        }

        [Fact]
        public async Task GetUsers_ReturnsUserDTOAsync()
        {
            // Arrange
            var queryExecutor = CreateUserQueryExecutor();
            var request = A.Fake<ITwitterRequest>();
            var url = TestHelper.GenerateString();
            var parameter = new GetUsersParameters(new long[] { 42 });
            var expectedResult = A.Fake<ITwitterResult<IUserDTO[]>>();

            A.CallTo(() => _fakeUserQueryGenerator.GetUsersQuery(parameter)).Returns(url);
            A.CallTo(() => _fakeTwitterAccessor.ExecuteRequestAsync<IUserDTO[]>(request)).Returns(expectedResult);

            // Act
            var result = await queryExecutor.GetUsersAsync(parameter, request);

            // Assert
            Assert.Equal(result, expectedResult);
            Assert.Equal(request.Query.Url, url);
            Assert.Equal(HttpMethod.GET, request.Query.HttpMethod);
        }

        [Fact]
        public async Task GetFollowerIds_ReturnsUserDTOAsync()
        {
            // Arrange
            var queryExecutor = CreateUserQueryExecutor();
            var request = A.Fake<ITwitterRequest>();
            var url = TestHelper.GenerateString();
            var parameter = new GetFollowerIdsParameters(42);
            var expectedResult = A.Fake<ITwitterResult<IIdsCursorQueryResultDTO>>();

            A.CallTo(() => _fakeUserQueryGenerator.GetFollowerIdsQuery(parameter)).Returns(url);
            A.CallTo(() => _fakeTwitterAccessor.ExecuteRequestAsync<IIdsCursorQueryResultDTO>(request)).Returns(expectedResult);

            // Act
            var result = await queryExecutor.GetFollowerIdsAsync(parameter, request);

            // Assert
            Assert.Equal(result, expectedResult);
            Assert.Equal(request.Query.Url, url);
            Assert.Equal(HttpMethod.GET, request.Query.HttpMethod);
        }

        [Fact]
        public async Task GetFriendIds_ReturnsUserDTOAsync()
        {
            // Arrange
            var queryExecutor = CreateUserQueryExecutor();
            var request = A.Fake<ITwitterRequest>();
            var url = TestHelper.GenerateString();
            var parameter = new GetFriendsParameters(42);
            var expectedResult = A.Fake<ITwitterResult<IIdsCursorQueryResultDTO>>();

            A.CallTo(() => _fakeUserQueryGenerator.GetFriendIdsQuery(parameter)).Returns(url);
            A.CallTo(() => _fakeTwitterAccessor.ExecuteRequestAsync<IIdsCursorQueryResultDTO>(request)).Returns(expectedResult);

            // Act
            var result = await queryExecutor.GetFriendIdsAsync(parameter, request);

            // Assert
            Assert.Equal(result, expectedResult);
            Assert.Equal(request.Query.Url, url);
            Assert.Equal(HttpMethod.GET, request.Query.HttpMethod);
        }

        [Fact]
        public async Task GetRelationshipBetween_ReturnsRelationshipDTOAsync()
        {
            // Arrange
            var queryExecutor = CreateUserQueryExecutor();
            var request = A.Fake<ITwitterRequest>();
            var url = TestHelper.GenerateString();
            var parameter = new GetRelationshipBetweenParameters(42, 43);
            var expectedResult = A.Fake<ITwitterResult<IRelationshipDetailsDTO>>();

            A.CallTo(() => _fakeUserQueryGenerator.GetRelationshipBetweenQuery(parameter)).Returns(url);
            A.CallTo(() => _fakeTwitterAccessor.ExecuteRequestAsync<IRelationshipDetailsDTO>(request)).Returns(expectedResult);

            // Act
            var result = await queryExecutor.GetRelationshipBetweenAsync(parameter, request);

            // Assert
            Assert.Equal(result, expectedResult);
            Assert.Equal(request.Query.Url, url);
            Assert.Equal(HttpMethod.GET, request.Query.HttpMethod);
        }

        [Fact]
        public async Task GetProfileImageStream_ReturnsWebHelperResultAsync()
        {
            // Arrange
            var queryExecutor = CreateUserQueryExecutor();
            var stream = A.Fake<Stream>();
            var url = TestHelper.GenerateString();
            var request = A.Fake<ITwitterRequest>();

            var parameter = new GetProfileImageParameters("some url");

            A.CallTo(() => _fakeUserQueryGenerator.DownloadProfileImageURL(parameter)).Returns(url);
            A.CallTo(() => _fakeWebHelper.GetResponseStreamAsync(request)).Returns(stream);

            // Act
            var result = await queryExecutor.GetProfileImageStreamAsync(parameter, request);

            // Assert
            Assert.Equal(result, stream);
            Assert.Equal(request.Query.Url, url);
            Assert.Equal(HttpMethod.GET, request.Query.HttpMethod);
        }

        [Fact]
        public async Task GetAuthenticatedUser_ReturnsUserDTOAsync()
        {
            // Arrange
            var queryExecutor = CreateUserQueryExecutor();

            var url = TestHelper.GenerateString();
            var parameters = new GetAuthenticatedUserParameters();
            var request = A.Fake<ITwitterRequest>();
            var expectedResult = A.Fake<ITwitterResult<IUserDTO>>();

            request.ExecutionContext.TweetMode = TweetMode.Extended;

            A.CallTo(() => _fakeUserQueryGenerator.GetAuthenticatedUserQuery(parameters)).Returns(url);
            A.CallTo(() => _fakeTwitterAccessor.ExecuteRequestAsync<IUserDTO>(request)).Returns(expectedResult);

            // Act
            var result = await queryExecutor.GetAuthenticatedUserAsync(parameters, request);

            // Assert
            Assert.Equal(result, expectedResult);
            Assert.Equal(request.Query.Url, url);
            Assert.Equal(HttpMethod.GET, request.Query.HttpMethod);
        }

        // BLOCK
        [Fact]
        public async Task BlockUser_ReturnsUserDTOAsync()
        {
            // Arrange
            var queryExecutor = CreateUserQueryExecutor();
            var userDTO = A.Fake<IUserDTO>();

            var url = TestHelper.GenerateString();
            var parameters = new BlockUserParameters(userDTO);
            var request = A.Fake<ITwitterRequest>();
            var expectedResult = A.Fake<ITwitterResult<IUserDTO>>();

            A.CallTo(() => _fakeUserQueryGenerator.GetBlockUserQuery(parameters)).Returns(url);
            A.CallTo(() => _fakeTwitterAccessor.ExecuteRequestAsync<IUserDTO>(request)).Returns(expectedResult);

            // Act
            var result = await queryExecutor.BlockUserAsync(parameters, request);

            // Assert
            Assert.Equal(result, expectedResult);
            Assert.Equal(request.Query.Url, url);
            Assert.Equal(HttpMethod.POST, request.Query.HttpMethod);
        }

        [Fact]
        public async Task UnblockUser_ReturnsUserDTOAsync()
        {
            // Arrange
            var queryExecutor = CreateUserQueryExecutor();
            var userDTO = A.Fake<IUserDTO>();

            var url = TestHelper.GenerateString();
            var parameters = new UnblockUserParameters(userDTO);
            var request = A.Fake<ITwitterRequest>();
            var expectedResult = A.Fake<ITwitterResult<IUserDTO>>();

            A.CallTo(() => _fakeUserQueryGenerator.GetUnblockUserQuery(parameters)).Returns(url);
            A.CallTo(() => _fakeTwitterAccessor.ExecuteRequestAsync<IUserDTO>(request)).Returns(expectedResult);

            // Act
            var result = await queryExecutor.UnblockUserAsync(parameters, request);

            // Assert
            Assert.Equal(result, expectedResult);
            Assert.Equal(request.Query.Url, url);
            Assert.Equal(HttpMethod.POST, request.Query.HttpMethod);
        }

        [Fact]
        public async Task ReportUserForSpam_ReturnsUserDTOAsync()
        {
            // Arrange
            var queryExecutor = CreateUserQueryExecutor();

            var url = TestHelper.GenerateString();
            var parameters = new ReportUserForSpamParameters(42);
            var request = A.Fake<ITwitterRequest>();
            var expectedResult = A.Fake<ITwitterResult<IUserDTO>>();

            A.CallTo(() => _fakeUserQueryGenerator.GetReportUserForSpamQuery(parameters)).Returns(url);
            A.CallTo(() => _fakeTwitterAccessor.ExecuteRequestAsync<IUserDTO>(request)).Returns(expectedResult);

            // Act
            var result = await queryExecutor.ReportUserForSpamAsync(parameters, request);

            // Assert
            Assert.Equal(result, expectedResult);
            Assert.Equal(request.Query.Url, url);
            Assert.Equal(HttpMethod.POST, request.Query.HttpMethod);
        }

        [Fact]
        public async Task GetBlockedUserIds_ReturnsUserIdsAsync()
        {
            // Arrange
            var queryExecutor = CreateUserQueryExecutor();

            var url = TestHelper.GenerateString();
            var parameters = new GetBlockedUserIdsParameters();
            var request = A.Fake<ITwitterRequest>();
            var expectedResult = A.Fake<ITwitterResult<IIdsCursorQueryResultDTO>>();

            A.CallTo(() => _fakeUserQueryGenerator.GetBlockedUserIdsQuery(parameters)).Returns(url);
            A.CallTo(() => _fakeTwitterAccessor.ExecuteRequestAsync<IIdsCursorQueryResultDTO>(request)).Returns(expectedResult);

            // Act
            var result = await queryExecutor.GetBlockedUserIdsAsync(parameters, request);

            // Assert
            Assert.Equal(result, expectedResult);
            Assert.Equal(request.Query.Url, url);
            Assert.Equal(HttpMethod.GET, request.Query.HttpMethod);
        }

        [Fact]
        public async Task GetBlockedUsers_ReturnsUserDTOsAsync()
        {
            // Arrange
            var queryExecutor = CreateUserQueryExecutor();

            var url = TestHelper.GenerateString();
            var parameters = new GetBlockedUsersParameters();
            var request = A.Fake<ITwitterRequest>();
            var expectedResult = A.Fake<ITwitterResult<IUserCursorQueryResultDTO>>();

            A.CallTo(() => _fakeUserQueryGenerator.GetBlockedUsersQuery(parameters)).Returns(url);
            A.CallTo(() => _fakeTwitterAccessor.ExecuteRequestAsync<IUserCursorQueryResultDTO>(request)).Returns(expectedResult);

            // Act
            var result = await queryExecutor.GetBlockedUsersAsync(parameters, request);

            // Assert
            Assert.Equal(result, expectedResult);
            Assert.Equal(request.Query.Url, url);
            Assert.Equal(HttpMethod.GET, request.Query.HttpMethod);
        }

        // FOLLOWERS
        [Fact]
        public async Task FollowUser_ReturnsUserDTOAsync()
        {
            // Arrange
            var queryExecutor = CreateUserQueryExecutor();
            var userDTO = A.Fake<IUserDTO>();

            var url = TestHelper.GenerateString();
            var parameters = new FollowUserParameters(userDTO);
            var request = A.Fake<ITwitterRequest>();
            var expectedResult = A.Fake<ITwitterResult<IUserDTO>>();

            A.CallTo(() => _fakeUserQueryGenerator.GetFollowUserQuery(parameters)).Returns(url);
            A.CallTo(() => _fakeTwitterAccessor.ExecuteRequestAsync<IUserDTO>(request)).Returns(expectedResult);

            // Act
            var result = await queryExecutor.FollowUserAsync(parameters, request);

            // Assert
            Assert.Equal(result, expectedResult);
            Assert.Equal(request.Query.Url, url);
            Assert.Equal(HttpMethod.POST, request.Query.HttpMethod);
        }
        [Fact]
        public async Task UnfollowUser_ReturnsUserDTOAsync()
        {
            // Arrange
            var queryExecutor = CreateUserQueryExecutor();
            var userDTO = A.Fake<IUserDTO>();

            var url = TestHelper.GenerateString();
            var parameters = new UnfollowUserParameters(userDTO);
            var request = A.Fake<ITwitterRequest>();
            var expectedResult = A.Fake<ITwitterResult<IUserDTO>>();

            A.CallTo(() => _fakeUserQueryGenerator.GetUnfollowUserQuery(parameters)).Returns(url);
            A.CallTo(() => _fakeTwitterAccessor.ExecuteRequestAsync<IUserDTO>(request)).Returns(expectedResult);

            // Act
            var result = await queryExecutor.UnfollowUserAsync(parameters, request);

            // Assert
            Assert.Equal(result, expectedResult);
            Assert.Equal(request.Query.Url, url);
            Assert.Equal(HttpMethod.POST, request.Query.HttpMethod);
        }

        [Fact]
        public async Task GetUserIdsRequestingFriendship_ReturnsPendingFollowersAsync()
        {
            // Arrange
            var queryExecutor = CreateUserQueryExecutor();
            var parameters = new GetUserIdsRequestingFriendshipParameters();

            var url = TestHelper.GenerateString();
            var request = A.Fake<ITwitterRequest>();
            var expectedResult = A.Fake<ITwitterResult<IIdsCursorQueryResultDTO>>();

            A.CallTo(() => _fakeUserQueryGenerator.GetUserIdsRequestingFriendshipQuery(parameters)).Returns(url);
            A.CallTo(() => _fakeTwitterAccessor.ExecuteRequestAsync<IIdsCursorQueryResultDTO>(request)).Returns(expectedResult);

            // Act
            var result = await queryExecutor.GetUserIdsRequestingFriendshipAsync(parameters, request);

            // Assert
            Assert.Equal(result, expectedResult);
            Assert.Equal(request.Query.Url, url);
            Assert.Equal(HttpMethod.GET, request.Query.HttpMethod);
        }

        [Fact]
        public async Task GetUserIdsYouRequestedToFollow_ReturnsPendingFollowersAsync()
        {
            // Arrange
            var queryExecutor = CreateUserQueryExecutor();
            var parameters = new GetUserIdsYouRequestedToFollowParameters();

            var url = TestHelper.GenerateString();
            var request = A.Fake<ITwitterRequest>();
            var expectedResult = A.Fake<ITwitterResult<IIdsCursorQueryResultDTO>>();

            A.CallTo(() => _fakeUserQueryGenerator.GetUserIdsYouRequestedToFollowQuery(parameters)).Returns(url);
            A.CallTo(() => _fakeTwitterAccessor.ExecuteRequestAsync<IIdsCursorQueryResultDTO>(request)).Returns(expectedResult);

            // Act
            var result = await queryExecutor.GetUserIdsYouRequestedToFollowAsync(parameters, request);

            // Assert
            Assert.Equal(result, expectedResult);
            Assert.Equal(request.Query.Url, url);
            Assert.Equal(HttpMethod.GET, request.Query.HttpMethod);
        }

        [Fact]
        public async Task UpdateRelationship_ReturnsRelationshipsAsync()
        {
            // Arrange
            var queryExecutor = CreateUserQueryExecutor();
            var parameters = new UpdateRelationshipParameters(42);

            var url = TestHelper.GenerateString();
            var request = A.Fake<ITwitterRequest>();
            var expectedResult = A.Fake<ITwitterResult<IRelationshipDetailsDTO>>();

            A.CallTo(() => _fakeUserQueryGenerator.GetUpdateRelationshipQuery(parameters)).Returns(url);
            A.CallTo(() => _fakeTwitterAccessor.ExecuteRequestAsync<IRelationshipDetailsDTO>(request)).Returns(expectedResult);

            // Act
            var result = await queryExecutor.UpdateRelationshipAsync(parameters, request);

            // Assert
            Assert.Equal(result, expectedResult);
            Assert.Equal(request.Query.Url, url);
            Assert.Equal(HttpMethod.POST, request.Query.HttpMethod);
        }

        [Fact]
        public async Task GetRelationshipsWith_ReturnsRelationshipsAsync()
        {
            // Arrange
            var queryExecutor = CreateUserQueryExecutor();
            var parameters = new GetRelationshipsWithParameters(new long[] { 42 });

            var url = TestHelper.GenerateString();
            var request = A.Fake<ITwitterRequest>();
            var expectedResult = A.Fake<ITwitterResult<IRelationshipStateDTO[]>>();

            A.CallTo(() => _fakeUserQueryGenerator.GetRelationshipsWithQuery(parameters)).Returns(url);
            A.CallTo(() => _fakeTwitterAccessor.ExecuteRequestAsync<IRelationshipStateDTO[]>(request)).Returns(expectedResult);

            // Act
            var result = await queryExecutor.GetRelationshipsWithAsync(parameters, request);

            // Assert
            Assert.Equal(result, expectedResult);
            Assert.Equal(request.Query.Url, url);
            Assert.Equal(HttpMethod.GET, request.Query.HttpMethod);
        }

        // MUTE
        [Fact]
        public async Task GetUserIdsWhoseRetweetsAreMuted_ReturnsUserIdsAsync()
        {
            // Arrange
            var queryExecutor = CreateUserQueryExecutor();

            var url = TestHelper.GenerateString();
            var parameters = new GetUserIdsWhoseRetweetsAreMutedParameters();
            var request = A.Fake<ITwitterRequest>();
            var expectedResult = A.Fake<ITwitterResult<long[]>>();

            A.CallTo(() => _fakeUserQueryGenerator.GetUserIdsWhoseRetweetsAreMutedQuery(parameters)).Returns(url);
            A.CallTo(() => _fakeTwitterAccessor.ExecuteRequestAsync<long[]>(request)).Returns(expectedResult);

            // Act
            var result = await queryExecutor.GetUserIdsWhoseRetweetsAreMutedAsync(parameters, request);

            // Assert
            Assert.Equal(result, expectedResult);
            Assert.Equal(request.Query.Url, url);
            Assert.Equal(HttpMethod.GET, request.Query.HttpMethod);
        }

        [Fact]
        public async Task GetMutedUserIds_ReturnsUserIdsAsync()
        {
            // Arrange
            var queryExecutor = CreateUserQueryExecutor();

            var url = TestHelper.GenerateString();
            var parameters = new GetMutedUserIdsParameters();
            var request = A.Fake<ITwitterRequest>();
            var expectedResult = A.Fake<ITwitterResult<IIdsCursorQueryResultDTO>>();

            A.CallTo(() => _fakeUserQueryGenerator.GetMutedUserIdsQuery(parameters)).Returns(url);
            A.CallTo(() => _fakeTwitterAccessor.ExecuteRequestAsync<IIdsCursorQueryResultDTO>(request)).Returns(expectedResult);

            // Act
            var result = await queryExecutor.GetMutedUserIdsAsync(parameters, request);

            // Assert
            Assert.Equal(result, expectedResult);
            Assert.Equal(request.Query.Url, url);
            Assert.Equal(HttpMethod.GET, request.Query.HttpMethod);
        }

        [Fact]
        public async Task GetMutedUsers_ReturnsUserDTOsAsync()
        {
            // Arrange
            var queryExecutor = CreateUserQueryExecutor();

            var url = TestHelper.GenerateString();
            var parameters = new GetMutedUsersParameters();
            var request = A.Fake<ITwitterRequest>();
            var expectedResult = A.Fake<ITwitterResult<IUserCursorQueryResultDTO>>();

            A.CallTo(() => _fakeUserQueryGenerator.GetMutedUsersQuery(parameters)).Returns(url);
            A.CallTo(() => _fakeTwitterAccessor.ExecuteRequestAsync<IUserCursorQueryResultDTO>(request)).Returns(expectedResult);

            // Act
            var result = await queryExecutor.GetMutedUsersAsync(parameters, request);

            // Assert
            Assert.Equal(result, expectedResult);
            Assert.Equal(request.Query.Url, url);
            Assert.Equal(HttpMethod.GET, request.Query.HttpMethod);
        }

        [Fact]
        public async Task MutedUser_ReturnsUserDTOAsync()
        {
            // Arrange
            var queryExecutor = CreateUserQueryExecutor();

            var url = TestHelper.GenerateString();
            var parameters = new MuteUserParameters(42);
            var request = A.Fake<ITwitterRequest>();
            var expectedResult = A.Fake<ITwitterResult<IUserDTO>>();

            A.CallTo(() => _fakeUserQueryGenerator.GetMuteUserQuery(parameters)).Returns(url);
            A.CallTo(() => _fakeTwitterAccessor.ExecuteRequestAsync<IUserDTO>(request)).Returns(expectedResult);

            // Act
            var result = await queryExecutor.MuteUserAsync(parameters, request);

            // Assert
            Assert.Equal(result, expectedResult);
            Assert.Equal(request.Query.Url, url);
            Assert.Equal(HttpMethod.POST, request.Query.HttpMethod);
        }

        [Fact]
        public async Task UnmutedUser_ReturnsUserDTOAsync()
        {
            // Arrange
            var queryExecutor = CreateUserQueryExecutor();

            var url = TestHelper.GenerateString();
            var parameters = new UnmuteUserParameters(42);
            var request = A.Fake<ITwitterRequest>();
            var expectedResult = A.Fake<ITwitterResult<IUserDTO>>();

            A.CallTo(() => _fakeUserQueryGenerator.GetUnmuteUserQuery(parameters)).Returns(url);
            A.CallTo(() => _fakeTwitterAccessor.ExecuteRequestAsync<IUserDTO>(request)).Returns(expectedResult);

            // Act
            var result = await queryExecutor.UnmuteUserAsync(parameters, request);

            // Assert
            Assert.Equal(result, expectedResult);
            Assert.Equal(request.Query.Url, url);
            Assert.Equal(HttpMethod.POST, request.Query.HttpMethod);
        }
    }
}