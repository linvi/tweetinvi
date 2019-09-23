using System;
using System.IO;
using System.Threading.Tasks;
using FakeItEasy;
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
        public async Task BlockUser_ReturnsUserDTO()
        {
            // Arrange
            var queryExecutor = CreateUserQueryExecutor();
            var userDTO = A.Fake<IUserDTO>();
            var expectedQuery = TestHelper.GenerateString();

            var parameters = new BlockUserParameters(userDTO);
            var request = A.Fake<ITwitterRequest>();
            var expectedResult = A.Fake<ITwitterResult<IUserDTO>>();

            A.CallTo(() => _fakeUserQueryGenerator.FakedObject.GetBlockUserQuery(parameters)).Returns(expectedQuery);
            A.CallTo(() => _fakeTwitterAccessor.FakedObject.ExecuteRequest<IUserDTO>(request)).Returns(expectedResult);

            // Act
            var result = await queryExecutor.BlockUser(parameters, request);

            // Assert
            Assert.Equal(result, expectedResult);
        }

        [Fact]
        public async Task FollowUser_ReturnsUserDTO()
        {
            // Arrange
            var queryExecutor = CreateUserQueryExecutor();
            var userDTO = A.Fake<IUserDTO>();
            var expectedQuery = TestHelper.GenerateString();

            var parameters = new FollowUserParameters(userDTO);
            var request = A.Fake<ITwitterRequest>();
            var expectedResult = A.Fake<ITwitterResult<IUserDTO>>();

            A.CallTo(() => _fakeUserQueryGenerator.FakedObject.GetFollowUserQuery(parameters)).Returns(expectedQuery);
            A.CallTo(() => _fakeTwitterAccessor.FakedObject.ExecuteRequest<IUserDTO>(request)).Returns(expectedResult);

            // Act
            var result = await queryExecutor.FollowUser(parameters, request);

            // Assert
            Assert.Equal(result, expectedResult);
        }

        [Fact]
        public async Task GetBlockedUserIds_ReturnsUserDTO()
        {
            // Arrange
            var queryExecutor = CreateUserQueryExecutor();
            var expectedQuery = TestHelper.GenerateString();

            var parameters = new GetBlockedUserIdsParameters();
            var request = A.Fake<ITwitterRequest>();
            var expectedResult = A.Fake<ITwitterResult<IIdsCursorQueryResultDTO>>();

            A.CallTo(() => _fakeUserQueryGenerator.FakedObject.GetBlockedUserIdsQuery(parameters)).Returns(expectedQuery);
            A.CallTo(() => _fakeTwitterAccessor.FakedObject.ExecuteRequest<IIdsCursorQueryResultDTO>(request)).Returns(expectedResult);

            // Act
            var result = await queryExecutor.GetBlockedUserIds(parameters, request);

            // Assert
            Assert.Equal(result, expectedResult);
        }

        [Fact]
        public async Task UnblockUser_ReturnsUserDTO()
        {
            // Arrange
            var queryExecutor = CreateUserQueryExecutor();
            var userDTO = A.Fake<IUserDTO>();
            var expectedQuery = TestHelper.GenerateString();

            var parameters = new UnblockUserParameters(userDTO);
            var request = A.Fake<ITwitterRequest>();
            var expectedResult = A.Fake<ITwitterResult<IUserDTO>>();

            A.CallTo(() => _fakeUserQueryGenerator.FakedObject.GetUnblockUserQuery(parameters)).Returns(expectedQuery);
            A.CallTo(() => _fakeTwitterAccessor.FakedObject.ExecuteRequest<IUserDTO>(request)).Returns(expectedResult);

            // Act
            var result = await queryExecutor.UnblockUser(parameters, request);

            // Assert
            Assert.Equal(result, expectedResult);
        }

        [Fact]
        public async Task UnFollowUser_ReturnsUserDTO()
        {
            // Arrange
            var queryExecutor = CreateUserQueryExecutor();
            var userDTO = A.Fake<IUserDTO>();
            var expectedQuery = TestHelper.GenerateString();

            var parameters = new UnFollowUserParameters(userDTO);
            var request = A.Fake<ITwitterRequest>();
            var expectedResult = A.Fake<ITwitterResult<IUserDTO>>();

            A.CallTo(() => _fakeUserQueryGenerator.FakedObject.GetUnFollowUserQuery(parameters)).Returns(expectedQuery);
            A.CallTo(() => _fakeTwitterAccessor.FakedObject.ExecuteRequest<IUserDTO>(request)).Returns(expectedResult);

            // Act
            var result = await queryExecutor.UnFollowUser(parameters, request);

            // Assert
            Assert.Equal(result, expectedResult);
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