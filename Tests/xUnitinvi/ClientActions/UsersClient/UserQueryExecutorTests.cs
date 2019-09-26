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
            _fakeUserQueryGenerator = _fakeBuilder.GetFake<IUserQueryGenerator>();
            _fakeTwitterAccessor = _fakeBuilder.GetFake<ITwitterAccessor>();
            _fakeWebHelper = _fakeBuilder.GetFake<IWebHelper>();
        }

        private readonly FakeClassBuilder<UserQueryExecutor> _fakeBuilder;
        private readonly Fake<IUserQueryGenerator> _fakeUserQueryGenerator;
        private readonly Fake<ITwitterAccessor> _fakeTwitterAccessor;
        private readonly Fake<IWebHelper> _fakeWebHelper;

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
            
            _fakeUserQueryGenerator.CallsTo(x => x.GetUserQuery(parameter, It.IsAny<TweetMode?>())).Returns(url);
            _fakeTwitterAccessor.CallsTo(x => x.ExecuteRequest<IUserDTO>(request)).Returns(expectedResult);

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
            
            _fakeUserQueryGenerator.CallsTo(x => x.GetUsersQuery(parameter, It.IsAny<TweetMode?>())).Returns(url);
            _fakeTwitterAccessor.CallsTo(x => x.ExecuteRequest<IUserDTO[]>(request)).Returns(expectedResult);

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
            
            _fakeUserQueryGenerator.CallsTo(x => x.GetFollowerIdsQuery(parameter)).Returns(url);
            _fakeTwitterAccessor.CallsTo(x => x.ExecuteRequest<IIdsCursorQueryResultDTO>(request)).Returns(expectedResult);

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
            
            _fakeUserQueryGenerator.CallsTo(x => x.GetFriendIdsQuery(parameter)).Returns(url);
            _fakeTwitterAccessor.CallsTo(x => x.ExecuteRequest<IIdsCursorQueryResultDTO>(request)).Returns(expectedResult);

            // Act
            var result = await queryExecutor.GetFriendIds(parameter, request);

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

            _fakeUserQueryGenerator.CallsTo(x => x.DownloadProfileImageURL(parameter)).Returns(url);
            _fakeWebHelper.CallsTo(x => x.GetResponseStreamAsync(request)).Returns(stream);

            // Act
            var result = await queryExecutor.GetProfileImageStream(parameter, request);

            // Assert
            Assert.Equal(result, stream);
        }
    }
}