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
        public async Task GetUser_ReturnsUserDTO()
        {
            // Arrange
            var queryExecutor = CreateUserQueryExecutor();
            var request = A.Fake<ITwitterRequest>();
            var url = TestHelper.GenerateString();
            var parameter = new GetUserParameters(42);
            var expectedResult = A.Fake<ITwitterResult<IUserDTO>>();
            
            A.CallTo(() => _fakeUserQueryGenerator.GetUserQuery(parameter, It.IsAny<TweetMode?>())).Returns(url);
            A.CallTo(() => _fakeTwitterAccessor.ExecuteRequest<IUserDTO>(request)).Returns(expectedResult);

            // Act
            var result = await queryExecutor.GetUser(parameter, request);

            // Assert
            Assert.Equal(result, expectedResult);
            Assert.Equal(request.Query.Url, url);
        }
        
        [Fact]
        public async Task GetUsers_ReturnsUserDTO()
        {
            // Arrange
            var queryExecutor = CreateUserQueryExecutor();
            var request = A.Fake<ITwitterRequest>();
            var url = TestHelper.GenerateString();
            var parameter = new GetUsersParameters(new long[] { 42 });
            var expectedResult = A.Fake<ITwitterResult<IUserDTO[]>>();
            
            A.CallTo(() => _fakeUserQueryGenerator.GetUsersQuery(parameter, It.IsAny<TweetMode?>())).Returns(url);
            A.CallTo(() => _fakeTwitterAccessor.ExecuteRequest<IUserDTO[]>(request)).Returns(expectedResult);

            // Act
            var result = await queryExecutor.GetUsers(parameter, request);

            // Assert
            Assert.Equal(result, expectedResult);
            Assert.Equal(request.Query.Url, url);
        }
        
        [Fact]
        public async Task GetFollowerIds_ReturnsUserDTO()
        {
            // Arrange
            var queryExecutor = CreateUserQueryExecutor();
            var request = A.Fake<ITwitterRequest>();
            var url = TestHelper.GenerateString();
            var parameter = new GetFollowerIdsParameters(42);
            var expectedResult = A.Fake<ITwitterResult<IIdsCursorQueryResultDTO>>();
            
            A.CallTo(() => _fakeUserQueryGenerator.GetFollowerIdsQuery(parameter)).Returns(url);
            A.CallTo(() => _fakeTwitterAccessor.ExecuteRequest<IIdsCursorQueryResultDTO>(request)).Returns(expectedResult);

            // Act
            var result = await queryExecutor.GetFollowerIds(parameter, request);

            // Assert
            Assert.Equal(result, expectedResult);
            Assert.Equal(request.Query.Url, url);
        }
        
        [Fact]
        public async Task GetFriendIds_ReturnsUserDTO()
        {
            // Arrange
            var queryExecutor = CreateUserQueryExecutor();
            var request = A.Fake<ITwitterRequest>();
            var url = TestHelper.GenerateString();
            var parameter = new GetFriendsParameters(42);
            var expectedResult = A.Fake<ITwitterResult<IIdsCursorQueryResultDTO>>();
            
            A.CallTo(() => _fakeUserQueryGenerator.GetFriendIdsQuery(parameter)).Returns(url);
            A.CallTo(() => _fakeTwitterAccessor.ExecuteRequest<IIdsCursorQueryResultDTO>(request)).Returns(expectedResult);

            // Act
            var result = await queryExecutor.GetFriendIds(parameter, request);

            // Assert
            Assert.Equal(result, expectedResult);
            Assert.Equal(request.Query.Url, url);
        }

        [Fact]
        public async Task GetRelationshipBetween_ReturnsRelationshipDTO()
        {
            // Arrange
            var queryExecutor = CreateUserQueryExecutor();
            var request = A.Fake<ITwitterRequest>();
            var url = TestHelper.GenerateString();
            var parameter = new GetRelationshipBetweenParameters(42, 43);
            var expectedResult = A.Fake<ITwitterResult<IRelationshipDetailsDTO>>();

            A.CallTo(() => _fakeUserQueryGenerator.GetRelationshipBetweenQuery(parameter)).Returns(url);
            A.CallTo(() => _fakeTwitterAccessor.ExecuteRequest<IRelationshipDetailsDTO>(request)).Returns(expectedResult);

            // Act
            var result = await queryExecutor.GetRelationshipBetween(parameter, request);

            // Assert
            Assert.Equal(result, expectedResult);
            Assert.Equal(request.Query.Url, url);
        }

        [Fact]
        public async Task GetProfileImageStream_ReturnsWebHelperResult()
        {
            // Arrange
            var queryExecutor = CreateUserQueryExecutor();
            var stream = A.Fake<Stream>();
            var url = TestHelper.GenerateString();
            var request = A.Fake<ITwitterRequest>();

            var parameter = new GetProfileImageParameters("url");

            A.CallTo(() => _fakeUserQueryGenerator.DownloadProfileImageURL(parameter)).Returns(url);
            A.CallTo(() => _fakeWebHelper.GetResponseStreamAsync(request)).Returns(stream);

            // Act
            var result = await queryExecutor.GetProfileImageStream(parameter, request);

            // Assert
            Assert.Equal(result, stream);
        }
    }
}