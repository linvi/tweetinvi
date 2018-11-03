using System.Collections.Generic;
using System.IO;
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

namespace Testinvi.TweetinviControllers.UserTests
{
    [TestClass]
    public class UserQueryExecutorTests
    {
        private FakeClassBuilder<UserQueryExecutor> _fakeBuilder;

        private IUserQueryGenerator _userQueryGenerator;
        private ITwitterAccessor _twitterAccessor;
        private IWebHelper _webHelper;

        private List<long> _cursorQueryIds;

        [TestInitialize]
        public void TestInitialize()
        {
            _fakeBuilder = new FakeClassBuilder<UserQueryExecutor>();
            _userQueryGenerator = _fakeBuilder.GetFake<IUserQueryGenerator>().FakedObject;
            _twitterAccessor = _fakeBuilder.GetFake<ITwitterAccessor>().FakedObject;
            _webHelper = _fakeBuilder.GetFake<IWebHelper>().FakedObject;

            _cursorQueryIds = new List<long>();
        }

        #region FriendIds

        // This tests that if the CursorQuery returns null, the accessor returns null
        [TestMethod]
        public void GetFriendIdWithUserDTOs_TwitterAccessorReturnsNull_ReturnsNull()
        {
            // Arrange
            var queryExecutor = CreateUserQueryExecutor();
            var userDTO = A.Fake<IUserDTO>();
            var maximumNumberOfFriends = TestHelper.GenerateRandomInt();
            var expectedQuery = TestHelper.GenerateString();

            A.CallTo(() => _userQueryGenerator.GetFriendIdsQuery(userDTO, maximumNumberOfFriends)).Returns(expectedQuery);
            _twitterAccessor.ArrangeExecuteCursorGETQuery<long, IIdsCursorQueryResultDTO>(expectedQuery, null);

            // Act
            var result = queryExecutor.GetFriendIds(userDTO, maximumNumberOfFriends);

            // Assert
            Assert.IsNull(result);
        }
        [TestMethod]
        public void GetFriendIdsWithUserDTO_AnyData_ReturnsTwitterAccessorResult()
        {
            // Arrange
            var queryExecutor = CreateUserQueryExecutor();
            var userDTO = A.Fake<IUserDTO>();
            var maximumNumberOfFriends = TestHelper.GenerateRandomInt();
            var expectedQuery = TestHelper.GenerateString();
            var expectedCursorResults = GenerateExpectedCursorResults();

            A.CallTo(() => _userQueryGenerator.GetFriendIdsQuery(userDTO, maximumNumberOfFriends))
                .Returns(expectedQuery);
            _twitterAccessor.ArrangeExecuteCursorGETQuery<long, IIdsCursorQueryResultDTO>(expectedQuery, expectedCursorResults);

            // Act
            var result = queryExecutor.GetFriendIds(userDTO, maximumNumberOfFriends);

            // Assert
            Assert.IsTrue(result.ContainsAll(_cursorQueryIds));
        }

        #endregion

        #region FollowerIds

        // This tests that if the CursorQuery returns null, the accessor returns null
        [TestMethod]
        public void GetFollowerIdWithUserDTOs_TwitterAccessorReturnsNull_ReturnsNull()
        {
            // Arrange
            var queryExecutor = CreateUserQueryExecutor();
            var userDTO = A.Fake<IUserDTO>();
            var maximumNumberOfFollowers = TestHelper.GenerateRandomInt();
            var expectedQuery = TestHelper.GenerateString();

            A.CallTo(() => _userQueryGenerator.GetFollowerIdsQuery(userDTO, maximumNumberOfFollowers))
                .Returns(expectedQuery);
            _twitterAccessor.ArrangeExecuteCursorGETQuery<long, IIdsCursorQueryResultDTO>(expectedQuery, null);

            // Act
            var result = queryExecutor.GetFollowerIds(userDTO, maximumNumberOfFollowers);

            // Assert
            Assert.IsNull(result);
        }
        [TestMethod]
        public void GetFollowerIdsWithUserDTO_AnyData_ReturnsTwitterAccessorResult()
        {
            // Arrange
            var queryExecutor = CreateUserQueryExecutor();
            var userDTO = A.Fake<IUserDTO>();
            var maximumNumberOfFollowers = TestHelper.GenerateRandomInt();
            var expectedQuery = TestHelper.GenerateString();
            var expectedCursorResults = GenerateExpectedCursorResults();

            A.CallTo(() => _userQueryGenerator.GetFollowerIdsQuery(userDTO, maximumNumberOfFollowers))
                .Returns(expectedQuery);
            _twitterAccessor.ArrangeExecuteCursorGETQuery<long, IIdsCursorQueryResultDTO>(expectedQuery, expectedCursorResults);

            // Act
            var result = queryExecutor.GetFollowerIds(userDTO, maximumNumberOfFollowers);

            // Assert
            Assert.IsTrue(result.ContainsAll(_cursorQueryIds));
        }

        #endregion

        #region FavouriteTweets

        [TestMethod]
        public void GetFavouriteTweetsWithUserDTO_AnyData_ReturnsTwitterAccessorResult()
        {
            // Arrange
            var queryExecutor = CreateUserQueryExecutor();
            TestHelper.GenerateRandomInt();
            var expectedQuery = TestHelper.GenerateString();
            IEnumerable<ITweetDTO> expectedResult = new[] { A.Fake<ITweetDTO>() };
            var parameters = A.Fake<IGetUserFavoritesQueryParameters>();

            A.CallTo(() => _userQueryGenerator.GetFavoriteTweetsQuery(parameters)).Returns(expectedQuery);
            _twitterAccessor.ArrangeExecuteGETQuery(expectedQuery, expectedResult);

            // Act
            var result = queryExecutor.GetFavoriteTweets(parameters);

            // Assert
            Assert.AreEqual(result, expectedResult);
        }

        #endregion

        #region Block User

        [TestMethod]
        public void BlockUser_WithUserDTO_ReturnsTrue()
        {
            // Arrange
            var queryExecutor = CreateUserQueryExecutor();
            var userDTO = A.Fake<IUserDTO>();
            var expectedQuery = TestHelper.GenerateString();

            A.CallTo(() => _userQueryGenerator.GetBlockUserQuery(userDTO)).Returns(expectedQuery);
            _twitterAccessor.ArrangeTryExecutePOSTQuery(expectedQuery, true);

            // Act
            var result = queryExecutor.BlockUser(userDTO);

            // Assert
            Assert.AreEqual(result, true);
        }

        [TestMethod]
        public void BlockUser_WithUserDTO_ReturnsFalse()
        {
            // Arrange
            var queryExecutor = CreateUserQueryExecutor();
            var userDTO = A.Fake<IUserDTO>();
            var expectedQuery = TestHelper.GenerateString();

            A.CallTo(() => _userQueryGenerator.GetBlockUserQuery(userDTO)).Returns(expectedQuery);
            _twitterAccessor.ArrangeTryExecutePOSTQuery(expectedQuery, false);

            // Act
            var result = queryExecutor.BlockUser(userDTO);

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

            A.CallTo(() => _userQueryGenerator.DownloadProfileImageURL(userDTO, ImageSize.bigger)).Returns(url);
            A.CallTo(() => _webHelper.GetResponseStream(url)).Returns(stream);

            // Act
            var result = queryExecutor.GetProfileImageStream(userDTO, ImageSize.bigger);

            // Assert
            Assert.AreEqual(result, stream);
        }

        #endregion

        #region Spam

        [TestMethod]
        public void ReportUserForSpam_WithUserDTO_ReturnsTrue()
        {
            // Arrange
            var queryExecutor = CreateUserQueryExecutor();
            var userDTO = A.Fake<IUserDTO>();
            var expectedQuery = TestHelper.GenerateString();

            A.CallTo(() => _userQueryGenerator.GetReportUserForSpamQuery(userDTO)).Returns(expectedQuery);
            _twitterAccessor.ArrangeTryExecutePOSTQuery(expectedQuery, true);

            // Act
            var result = queryExecutor.ReportUserForSpam(userDTO);

            // Assert
            Assert.AreEqual(result, true);
        }

        [TestMethod]
        public void ReportUserForSpam_WithUserDTO_ReturnsFalse()
        {
            // Arrange
            var queryExecutor = CreateUserQueryExecutor();
            var userDTO = A.Fake<IUserDTO>();
            var expectedQuery = TestHelper.GenerateString();

            A.CallTo(() => _userQueryGenerator.GetReportUserForSpamQuery(userDTO)).Returns(expectedQuery);
            _twitterAccessor.ArrangeTryExecutePOSTQuery(expectedQuery, false);

            // Act
            var result = queryExecutor.ReportUserForSpam(userDTO);

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

        public UserQueryExecutor CreateUserQueryExecutor()
        {
            return _fakeBuilder.GenerateClass();
        }
    }
}