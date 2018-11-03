using System.Linq;
using FakeItEasy;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Testinvi.Helpers;
using Testinvi.SetupHelpers;
using Tweetinvi.Controllers.User;
using Tweetinvi.Core.Parameters;
using Tweetinvi.Core.QueryGenerators;
using Tweetinvi.Core.Web;
using Tweetinvi.Models;
using Tweetinvi.Models.DTO;
using Tweetinvi.Models.DTO.QueryDTO;

namespace Testinvi.TweetinviControllers.UserTests
{
    [TestClass]
    public class UserJsonControllerTests
    {
        private FakeClassBuilder<UserJsonController> _fakeBuilder;

        private IUserQueryGenerator _userQueryGenerator;
        private ITwitterAccessor _twitterAccessor;

        [TestInitialize]
        public void TestInitialize()
        {
            _fakeBuilder = new FakeClassBuilder<UserJsonController>();
            _userQueryGenerator = _fakeBuilder.GetFake<IUserQueryGenerator>().FakedObject;
            _twitterAccessor = _fakeBuilder.GetFake<ITwitterAccessor>().FakedObject;
        }

        #region FriendsIds

        [TestMethod]
        public void GetFriendIdsWithNullUser_ReturnsNull()
        {
            // Arrange
            var queryExecutor = CreateUserJsonController();
            var maximumNumberOfFriends = TestHelper.GenerateRandomInt();
            var expectedQuery = TestHelper.GenerateString();
            var expectedResult = GenerateExpectedCursorResults();

            A.CallTo(() => _userQueryGenerator.GetFriendIdsQuery(A<IUserDTO>.Ignored, A<int>.Ignored))
                .Returns(expectedQuery);
            _twitterAccessor.ArrangeExecuteJsonCursorGETQuery<IIdsCursorQueryResultDTO>(expectedQuery, expectedResult);

            // Act
            var result = queryExecutor.GetFriendIds((IUser)null, maximumNumberOfFriends);

            // Assert
            Assert.AreEqual(result, expectedResult);
        }

        [TestMethod]
        public void GetFriendIdsWithUser_ReturnsTwitterAccessorResult()
        {
            // Arrange
            var queryExecutor = CreateUserJsonController();
            var user = A.Fake<IUserIdentifier>();
            var maximumNumberOfFriends = TestHelper.GenerateRandomInt();
            var expectedQuery = TestHelper.GenerateString();
            var expectedCursorResults = GenerateExpectedCursorResults();

            A.CallTo(() => _userQueryGenerator.GetFriendIdsQuery(user, maximumNumberOfFriends)).Returns(expectedQuery);
            _twitterAccessor.ArrangeExecuteJsonCursorGETQuery<IIdsCursorQueryResultDTO>(expectedQuery, expectedCursorResults);

            // Act
            var result = queryExecutor.GetFriendIds(user, maximumNumberOfFriends);

            // Assert
            Assert.AreEqual(result, expectedCursorResults);
        }

        [TestMethod]
        public void GetFriendIdsWithUserDTO_AnyData_ReturnsTwitterAccessorResult()
        {
            // Arrange
            var queryExecutor = CreateUserJsonController();
            var userDTO = A.Fake<IUserDTO>();
            var maximumNumberOfFriends = TestHelper.GenerateRandomInt();
            var expectedQuery = TestHelper.GenerateString();
            var expectedCursorResults = GenerateExpectedCursorResults();

            A.CallTo(() => _userQueryGenerator.GetFriendIdsQuery(userDTO, maximumNumberOfFriends))
                .Returns(expectedQuery);
            _twitterAccessor.ArrangeExecuteJsonCursorGETQuery<IIdsCursorQueryResultDTO>(expectedQuery, expectedCursorResults);

            // Act
            var result = queryExecutor.GetFriendIds(userDTO, maximumNumberOfFriends);

            // Assert
            Assert.IsTrue(result.ContainsAll(expectedCursorResults));
        }

        [TestMethod]
        public void GetFriendIdsWithUserScreenName_AnyData_ReturnsTwitterAccessorResult()
        {
            // Arrange
            var queryExecutor = CreateUserJsonController();
            var userScreenName = TestHelper.GenerateString();
            var maximumNumberOfFriends = TestHelper.GenerateRandomInt();
            var expectedQuery = TestHelper.GenerateString();
            var expectedCursorResults = GenerateExpectedCursorResults();

            A.CallTo(() =>
                    _userQueryGenerator.GetFriendIdsQuery(
                        A<IUserIdentifier>.That.Matches(u => u.ScreenName == userScreenName), maximumNumberOfFriends))
                .Returns(expectedQuery);
            _twitterAccessor.ArrangeExecuteJsonCursorGETQuery<IIdsCursorQueryResultDTO>(expectedQuery, expectedCursorResults);

            // Act
            var result = queryExecutor.GetFriendIds(userScreenName, maximumNumberOfFriends);

            // Assert
            Assert.IsTrue(result.ContainsAll(expectedCursorResults));
        }

        [TestMethod]
        public void GetFriendIdsWithUserId_AnyData_ReturnsTwitterAccessorResult()
        {
            // Arrange
            var queryExecutor = CreateUserJsonController();
            var userId = TestHelper.GenerateRandomLong();
            var maximumNumberOfFriends = TestHelper.GenerateRandomInt();
            var expectedQuery = TestHelper.GenerateString();
            var expectedCursorResults = GenerateExpectedCursorResults();

            A.CallTo(() => _userQueryGenerator.GetFriendIdsQuery(A<IUserIdentifier>.That.Matches(u => u.Id == userId),
                maximumNumberOfFriends)).Returns(expectedQuery);
            _twitterAccessor.ArrangeExecuteJsonCursorGETQuery<IIdsCursorQueryResultDTO>(expectedQuery, expectedCursorResults);

            // Act
            var result = queryExecutor.GetFriendIds(userId, maximumNumberOfFriends);

            // Assert
            Assert.IsTrue(result.ContainsAll(expectedCursorResults));
        }

        #endregion

        #region FollowerIds

        [TestMethod]
        public void GetFollowerIdsWithNullUser()
        {
            // Arrange
            var queryExecutor = CreateUserJsonController();
            var maximumNumberOfFollowers = TestHelper.GenerateRandomInt();
            var expectedQuery = TestHelper.GenerateString();
            var expectedResult = new string[0];

            A.CallTo(() => _userQueryGenerator.GetFriendIdsQuery(A<IUserIdentifier>.Ignored, It.IsAny<int>()))
                .Returns(expectedQuery);
            _twitterAccessor.ArrangeExecuteJsonCursorGETQuery<IIdsCursorQueryResultDTO>(expectedQuery, expectedResult);

            // Act
            var result = queryExecutor.GetFollowerIds((IUserIdentifier)null, maximumNumberOfFollowers);

            // Assert
            CollectionAssert.AreEqual(result.ToArray(), expectedResult);
        }

        [TestMethod]
        public void GetFollowerIdsWithUser_ReturnsTwitterAccessorResult()
        {
            // Arrange
            var queryExecutor = CreateUserJsonController();
            var user = A.Fake<IUserIdentifier>();
            var maximumNumberOfFollowers = TestHelper.GenerateRandomInt();
            var expectedQuery = TestHelper.GenerateString();
            var expectedCursorResults = GenerateExpectedCursorResults();

            A.CallTo(() => _userQueryGenerator.GetFollowerIdsQuery(user, maximumNumberOfFollowers))
                .Returns(expectedQuery);
            _twitterAccessor.ArrangeExecuteJsonCursorGETQuery<IIdsCursorQueryResultDTO>(expectedQuery, expectedCursorResults);

            // Act
            var result = queryExecutor.GetFollowerIds(user, maximumNumberOfFollowers);

            // Assert
            Assert.AreEqual(result, expectedCursorResults);
        }

        [TestMethod]
        public void GetFollowerIdsWithUserDTO_AnyData_ReturnsTwitterAccessorResult()
        {
            // Arrange
            var queryExecutor = CreateUserJsonController();
            var userDTO = A.Fake<IUserDTO>();
            var maximumNumberOfFollowers = TestHelper.GenerateRandomInt();
            var expectedQuery = TestHelper.GenerateString();
            var expectedCursorResults = GenerateExpectedCursorResults();

            A.CallTo(() => _userQueryGenerator.GetFollowerIdsQuery(userDTO, maximumNumberOfFollowers))
                .Returns(expectedQuery);
            _twitterAccessor.ArrangeExecuteJsonCursorGETQuery<IIdsCursorQueryResultDTO>(expectedQuery, expectedCursorResults);

            // Act
            var result = queryExecutor.GetFollowerIds(userDTO, maximumNumberOfFollowers);

            // Assert
            Assert.IsTrue(result.ContainsAll(expectedCursorResults));
        }

        [TestMethod]
        public void GetFollowerIdsWithUserScreenName_AnyData_ReturnsTwitterAccessorResult()
        {
            // Arrange
            var queryExecutor = CreateUserJsonController();
            var userScreenName = TestHelper.GenerateString();
            var maximumNumberOfFollowers = TestHelper.GenerateRandomInt();
            var expectedQuery = TestHelper.GenerateString();
            var expectedCursorResults = GenerateExpectedCursorResults();

            A.CallTo(() =>
                    _userQueryGenerator.GetFollowerIdsQuery(
                        A<IUserIdentifier>.That.Matches(u => u.ScreenName == userScreenName), maximumNumberOfFollowers))
                .Returns(expectedQuery);
            _twitterAccessor.ArrangeExecuteJsonCursorGETQuery<IIdsCursorQueryResultDTO>(expectedQuery, expectedCursorResults);

            // Act
            var result = queryExecutor.GetFollowerIds(userScreenName, maximumNumberOfFollowers);

            // Assert
            Assert.IsTrue(result.ContainsAll(expectedCursorResults));
        }

        [TestMethod]
        public void GetFollowerIdsWithUserId_AnyData_ReturnsTwitterAccessorResult()
        {
            // Arrange
            var queryExecutor = CreateUserJsonController();
            var userId = TestHelper.GenerateRandomLong();
            var maximumNumberOfFollowers = TestHelper.GenerateRandomInt();
            var expectedQuery = TestHelper.GenerateString();
            var expectedCursorResults = GenerateExpectedCursorResults();

            A.CallTo(() => _userQueryGenerator.GetFollowerIdsQuery(A<IUserIdentifier>.That.Matches(u => u.Id == userId),
                maximumNumberOfFollowers)).Returns(expectedQuery);
            _twitterAccessor.ArrangeExecuteJsonCursorGETQuery<IIdsCursorQueryResultDTO>(expectedQuery, expectedCursorResults);

            // Act
            var result = queryExecutor.GetFollowerIds(userId, maximumNumberOfFollowers);

            // Assert
            Assert.IsTrue(result.ContainsAll(expectedCursorResults));
        }

        #endregion

        #region FavouriteTweets

        [TestMethod]
        public void GetFavoriteTweetsWithUser_AnyData_ReturnsTwitterAccessorResult()
        {
            // Arrange
            var queryExecutor = CreateUserJsonController();
            var expectedQuery = TestHelper.GenerateString();
            var expectedResult = TestHelper.GenerateString();
            var parameters = A.Fake<IGetUserFavoritesQueryParameters>();

            A.CallTo(() => _userQueryGenerator.GetFavoriteTweetsQuery(parameters)).Returns(expectedQuery);
            _twitterAccessor.ArrangeExecuteJsonGETQuery(expectedQuery, expectedResult);

            // Act
            var result = queryExecutor.GetFavoriteTweets(parameters);

            // Assert
            Assert.AreEqual(result, expectedResult);
        }

        #endregion

        #region Block User


        [TestMethod]
        public void BlockUser_WithUser_ReturnsTwitterAccessorResult()
        {
            // Arrange
            var queryExecutor = CreateUserJsonController();
            var user = A.Fake<IUserIdentifier>();
            var expectedQuery = TestHelper.GenerateString();
            var expectedResult = TestHelper.GenerateString();

            A.CallTo(() => _userQueryGenerator.GetBlockUserQuery(user)).Returns(expectedQuery);
            _twitterAccessor.ArrangeExecuteJsonPOSTQuery(expectedQuery, expectedResult);

            // Act
            var result = queryExecutor.BlockUser(user);

            // Assert
            Assert.AreEqual(result, expectedResult);
        }

        [TestMethod]
        public void BlockUser_WithUserScreenName_ReturnsTwitterAccessorResult()
        {
            // Arrange
            var queryExecutor = CreateUserJsonController();
            var userScreenName = TestHelper.GenerateString();
            var expectedQuery = TestHelper.GenerateString();
            var expectedResult = TestHelper.GenerateString();

            A.CallTo(() =>
                _userQueryGenerator.GetBlockUserQuery(
                    A<IUserIdentifier>.That.Matches(u => u.ScreenName == userScreenName))).Returns(expectedQuery);
            _twitterAccessor.ArrangeExecuteJsonPOSTQuery(expectedQuery, expectedResult);

            // Act
            var result = queryExecutor.BlockUser(userScreenName);

            // Assert
            Assert.AreEqual(result, expectedResult);
        }

        [TestMethod]
        public void BlockUser_WithUserId_ReturnsTwitterAccessorResult()
        {
            // Arrange
            var queryExecutor = CreateUserJsonController();
            var userId = TestHelper.GenerateRandomLong();
            var expectedQuery = TestHelper.GenerateString();
            var expectedResult = TestHelper.GenerateString();

            A.CallTo(() => _userQueryGenerator.GetBlockUserQuery(A<IUserIdentifier>.That.Matches(u => u.Id == userId)))
                .Returns(expectedQuery);
            _twitterAccessor.ArrangeExecuteJsonPOSTQuery(expectedQuery, expectedResult);

            // Act
            var result = queryExecutor.BlockUser(userId);

            // Assert
            Assert.AreEqual(result, expectedResult);
        }

        #endregion

        private string[] GenerateExpectedCursorResults()
        {
            return new string[0];
        }

        public UserJsonController CreateUserJsonController()
        {
            return _fakeBuilder.GenerateClass();
        }
    }
}