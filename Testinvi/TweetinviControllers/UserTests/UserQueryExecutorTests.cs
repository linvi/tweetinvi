using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using FakeItEasy;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Testinvi.Helpers;
using Testinvi.SetupHelpers;
using Tweetinvi.Controllers.User;
using Tweetinvi.Core.Helpers;
using Tweetinvi.Core.Parameters;
using Tweetinvi.Core.QueryGenerators;
using Tweetinvi.Core.Web;
using Tweetinvi.Models;
using Tweetinvi.Models.DTO;
using Tweetinvi.Models.DTO.QueryDTO;
using Tweetinvi.Parameters;

namespace Testinvi.TweetinviControllers.UserTests
{
    [TestClass]
    public class UserQueryExecutorTests
    {
        private FakeClassBuilder<UserQueryExecutor> _fakeBuilder;
        private Fake<IUserQueryGenerator> _fakeUserQueryGenerator;
        private Fake<ITwitterAccessor> _fakeTwitterAccessor;
        private Fake<IWebHelper> _fakeWebHelper;

        private List<long> _cursorQueryIds;

        [TestInitialize]
        public void TestInitialize()
        {
            _fakeBuilder = new FakeClassBuilder<UserQueryExecutor>();
            _fakeUserQueryGenerator = _fakeBuilder.GetFake<IUserQueryGenerator>();
            _fakeTwitterAccessor = _fakeBuilder.GetFake<ITwitterAccessor>();
            _fakeWebHelper = _fakeBuilder.GetFake<IWebHelper>();

            _cursorQueryIds = new List<long>();
        }

        #region FriendIds

        // This tests that if the CursorQuery returns null, the accessor returns null
        [TestMethod]
        public async Task GetFriendIdsWithUserDTOs_TwitterAccessorReturnsNull_ReturnsNull()
        {
            // Arrange
            var queryExecutor = CreateUserQueryExecutor();
            var userDTO = A.Fake<IUserDTO>();
            var maximumNumberOfFriends = TestHelper.GenerateRandomInt();
            var expectedQuery = TestHelper.GenerateString();
            var request = A.Fake<ITwitterRequest>();

            var FriendIdsParameter = new GetFriendIdsParameters(userDTO)
            {
                MaximumNumberOfResults = maximumNumberOfFriends
            };

            _fakeUserQueryGenerator.CallsTo(x => x.GetFriendIdsQuery(FriendIdsParameter)).Returns(expectedQuery);
            _fakeTwitterAccessor
                .CallsTo(x => x.ExecuteRequest<IIdsCursorQueryResultDTO>(A<ITwitterRequest>.That.Matches(twitterRequest => twitterRequest.Query.Url == expectedQuery)))
                .ReturnsLazily<ITwitterResult<IIdsCursorQueryResultDTO>>(() => null);

            // Act
            var result = await queryExecutor.GetFriendIds(FriendIdsParameter, request);

            // Assert
            Assert.IsNull(result);
        }

        [TestMethod]
        public async Task GetFriendIdsWithUserDTO_AnyData_ReturnsTwitterAccessorResult()
        {
            // Arrange
            var queryExecutor = CreateUserQueryExecutor();
            var userDTO = A.Fake<IUserDTO>();
            var maximumNumberOfFriends = TestHelper.GenerateRandomInt();
            var expectedQuery = TestHelper.GenerateString();
            var request = A.Fake<ITwitterRequest>();
            var expectedResult = A.Fake<ITwitterResult<IIdsCursorQueryResultDTO>>();

            var friendIdsParameter = new GetFriendIdsParameters(userDTO)
            {
                MaximumNumberOfResults = maximumNumberOfFriends
            };

            _fakeUserQueryGenerator.CallsTo(x => x.GetFriendIdsQuery(friendIdsParameter)).Returns(expectedQuery);

            _fakeTwitterAccessor
                .CallsTo(x => x.ExecuteRequest<IIdsCursorQueryResultDTO>(A<ITwitterRequest>.That.Matches(twitterRequest => twitterRequest.Query.Url == expectedQuery)))
                .ReturnsLazily(() => expectedResult);

            // Act
            var result = await queryExecutor.GetFriendIds(friendIdsParameter, request);

            // Assert
            Assert.IsTrue(result.DataTransferObject.Ids.ContainsAll(_cursorQueryIds));
        }

        #endregion

        #region FollowerIds

        // This tests that if the CursorQuery returns null, the accessor returns null
        [TestMethod]
        public async Task GetFollowerIdsWithUserDTOs_TwitterAccessorReturnsNull_ReturnsNull()
        {
            // Arrange
            var queryExecutor = CreateUserQueryExecutor();
            var userDTO = A.Fake<IUserDTO>();
            var maximumNumberOfFollowers = TestHelper.GenerateRandomInt();
            var expectedQuery = TestHelper.GenerateString();
            var request = A.Fake<ITwitterRequest>();

            var followerIdsParameter = new GetFollowerIdsParameters(userDTO)
            {
                MaximumNumberOfResults = maximumNumberOfFollowers
            };

            _fakeUserQueryGenerator.CallsTo(x => x.GetFollowerIdsQuery(followerIdsParameter)).Returns(expectedQuery);
            _fakeTwitterAccessor
                .CallsTo(x => x.ExecuteRequest<IIdsCursorQueryResultDTO>(A<ITwitterRequest>.That.Matches(twitterRequest => twitterRequest.Query.Url == expectedQuery)))
                .ReturnsLazily<ITwitterResult<IIdsCursorQueryResultDTO>>(() => null);

            // Act
            var result = await queryExecutor.GetFollowerIds(followerIdsParameter, request);

            // Assert
            Assert.IsNull(result);
        }

        [TestMethod]
        public async Task GetFollowerIdsWithUserDTO_AnyData_ReturnsTwitterAccessorResult()
        {
            // Arrange
            var queryExecutor = CreateUserQueryExecutor();
            var userDTO = A.Fake<IUserDTO>();
            var maximumNumberOfFollowers = TestHelper.GenerateRandomInt();
            var expectedQuery = TestHelper.GenerateString();
            var request = A.Fake<ITwitterRequest>();
            var expectedResult = A.Fake<ITwitterResult<IIdsCursorQueryResultDTO>>();

            var followerIdsParameter = new GetFollowerIdsParameters(userDTO)
            {
                MaximumNumberOfResults = maximumNumberOfFollowers
            };

            _fakeUserQueryGenerator.CallsTo(x => x.GetFollowerIdsQuery(followerIdsParameter)).Returns(expectedQuery);

            _fakeTwitterAccessor
                .CallsTo(x => x.ExecuteRequest<IIdsCursorQueryResultDTO>(A<ITwitterRequest>.That.Matches(twitterRequest => twitterRequest.Query.Url == expectedQuery)))
                .ReturnsLazily(() => expectedResult);

            // Act
            var result = await queryExecutor.GetFollowerIds(followerIdsParameter, request);

            // Assert
            Assert.IsTrue(result.DataTransferObject.Ids.ContainsAll(_cursorQueryIds));
        }

        #endregion

        #region FavouriteTweets

        [TestMethod]
        public async Task GetFavouriteTweetsWithUserDTO_AnyData_ReturnsTwitterAccessorResult()
        {
            // Arrange
            var queryExecutor = CreateUserQueryExecutor();
            TestHelper.GenerateRandomInt();
            var expectedQuery = TestHelper.GenerateString();
            IEnumerable<ITweetDTO> expectedResult = new[] { A.Fake<ITweetDTO>() };
            var parameters = It.IsAny<IGetUserFavoritesQueryParameters>();

            _fakeUserQueryGenerator.CallsTo(x => x.GetFavoriteTweetsQuery(parameters)).Returns(expectedQuery);
            _fakeTwitterAccessor.ArrangeExecuteGETQuery(expectedQuery, expectedResult);

            // Act
            var result = await queryExecutor.GetFavoriteTweets(parameters);

            // Assert
            Assert.AreEqual(result, expectedResult);
        }

        #endregion

        #region Block User

        [TestMethod]
        public async Task BlockUser_WithUserDTO_ReturnsTrue()
        {
            // Arrange
            var queryExecutor = CreateUserQueryExecutor();
            var userDTO = A.Fake<IUserDTO>();
            var expectedQuery = TestHelper.GenerateString();

            _fakeUserQueryGenerator.CallsTo(x => x.GetBlockUserQuery(userDTO)).Returns(expectedQuery);
            _fakeTwitterAccessor.ArrangeTryExecutePOSTQuery(expectedQuery, true);

            // Act
            var result = await queryExecutor.BlockUser(userDTO);

            // Assert
            Assert.AreEqual(result, true);
        }

        [TestMethod]
        public async Task BlockUser_WithUserDTO_ReturnsFalse()
        {
            // Arrange
            var queryExecutor = CreateUserQueryExecutor();
            var userDTO = A.Fake<IUserDTO>();
            var expectedQuery = TestHelper.GenerateString();

            _fakeUserQueryGenerator.CallsTo(x => x.GetBlockUserQuery(userDTO)).Returns(expectedQuery);
            _fakeTwitterAccessor.ArrangeTryExecutePOSTQuery(expectedQuery, false);

            // Act
            var result = await queryExecutor.BlockUser(userDTO);

            // Assert
            Assert.AreEqual(result, false);
        }

        #endregion

        #region Stream Profile Image

        [TestMethod]
        public void GetProfileImageStream_ReturnsWebHelperResult()
        {
            // Arrange
            var queryExecutor = CreateUserQueryExecutor();
            var stream = A.Fake<Stream>();
            var userDTO = A.Fake<IUserDTO>();
            var url = TestHelper.GenerateString();

            _fakeUserQueryGenerator.CallsTo(x => x.DownloadProfileImageURL(userDTO, ImageSize.bigger)).Returns(url);
            _fakeWebHelper.CallsTo(x => x.GetResponseStream(url)).Returns(stream);

            // Act
            var result = queryExecutor.GetProfileImageStream(userDTO, ImageSize.bigger);

            // Assert
            Assert.AreEqual(result, stream);
        }

        #endregion

        #region Spam

        [TestMethod]
        public async Task ReportUserForSpam_WithUserDTO_ReturnsTrue()
        {
            // Arrange
            var queryExecutor = CreateUserQueryExecutor();
            var userDTO = A.Fake<IUserDTO>();
            var expectedQuery = TestHelper.GenerateString();

            _fakeUserQueryGenerator.CallsTo(x => x.GetReportUserForSpamQuery(userDTO)).Returns(expectedQuery);
            _fakeTwitterAccessor.ArrangeTryExecutePOSTQuery(expectedQuery, true);

            // Act
            var result = await queryExecutor.ReportUserForSpam(userDTO);

            // Assert
            Assert.AreEqual(result, true);
        }

        [TestMethod]
        public async Task ReportUserForSpam_WithUserDTO_ReturnsFalse()
        {
            // Arrange
            var queryExecutor = CreateUserQueryExecutor();
            var userDTO = A.Fake<IUserDTO>();
            var expectedQuery = TestHelper.GenerateString();

            _fakeUserQueryGenerator.CallsTo(x => x.GetReportUserForSpamQuery(userDTO)).Returns(expectedQuery);
            _fakeTwitterAccessor.ArrangeTryExecutePOSTQuery(expectedQuery, false);

            // Act
            var result = await queryExecutor.ReportUserForSpam(userDTO);

            // Assert
            Assert.AreEqual(result, false);
        }

        #endregion

        private IEnumerable<long> GenerateExpectedCursorResults()
        {
            var queryId1 = TestHelper.GenerateRandomLong();
            var queryId2 = TestHelper.GenerateRandomLong();

            _cursorQueryIds.Add(queryId1);
            _cursorQueryIds.Add(queryId2);

            return new[] {queryId1, queryId2};
        }

        private UserQueryExecutor CreateUserQueryExecutor()
        {
            return _fakeBuilder.GenerateClass();
        }
    }
}