using System;
using System.Collections.Generic;
using FakeItEasy;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Testinvi.Helpers;
using Testinvi.SetupHelpers;
using Tweetinvi.Controllers.User;
using Tweetinvi.Core.Interfaces;
using Tweetinvi.Core.Interfaces.Credentials;
using Tweetinvi.Core.Interfaces.DTO;
using Tweetinvi.Core.Interfaces.DTO.QueryDTO;
using Tweetinvi.Core.Interfaces.QueryGenerators;
using Tweetinvi.Core.Parameters;
using Tweetinvi.Core.Parameters.QueryParameters;

namespace Testinvi.TweetinviControllers.UserTests
{
    [TestClass]
    public class UserJsonControllerTests
    {
        private FakeClassBuilder<UserJsonController> _fakeBuilder;
        private Fake<IUserQueryGenerator> _fakeUserQueryGenerator;
        private Fake<ITwitterAccessor> _fakeTwitterAccessor;

        [TestInitialize]
        public void TestInitialize()
        {
            _fakeBuilder = new FakeClassBuilder<UserJsonController>();
            _fakeUserQueryGenerator = _fakeBuilder.GetFake<IUserQueryGenerator>();
            _fakeTwitterAccessor = _fakeBuilder.GetFake<ITwitterAccessor>();
        }

        #region FriendsIds

        [TestMethod]
        public void GetFriendIdsWithNullUser_ReturnsNull()
        {
            // Arrange
            var queryExecutor = CreateUserJsonController();
            var maximumNumberOfFriends = TestHelper.GenerateRandomInt();
            var expectedQuery = TestHelper.GenerateString();

            _fakeUserQueryGenerator.CallsTo(x => x.GetFriendIdsQuery(A<IUserDTO>.Ignored, A<int>.Ignored)).Returns(expectedQuery);
            _fakeTwitterAccessor.ArrangeExecuteJsonCursorGETQuery<IIdsCursorQueryResultDTO>(expectedQuery, A.Fake<IEnumerable<string>>());

            // Act
            var result = queryExecutor.GetFriendIds((IUser)null, maximumNumberOfFriends);

            // Assert
            Assert.IsNull(result);
        }

        [TestMethod]
        public void GetFriendIdsWithUser_ReturnsTwitterAccessorResult()
        {
            // Arrange
            var queryExecutor = CreateUserJsonController();
            var userDTO = A.Fake<IUserDTO>();
            var user = TestHelper.GenerateUser(userDTO);
            var maximumNumberOfFriends = TestHelper.GenerateRandomInt();
            var expectedQuery = TestHelper.GenerateString();
            var expectedCursorResults = GenerateExpectedCursorResults();

            _fakeUserQueryGenerator.CallsTo(x => x.GetFriendIdsQuery(userDTO, maximumNumberOfFriends)).Returns(expectedQuery);
            _fakeTwitterAccessor.ArrangeExecuteJsonCursorGETQuery<IIdsCursorQueryResultDTO>(expectedQuery, expectedCursorResults);

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

            _fakeUserQueryGenerator.CallsTo(x => x.GetFriendIdsQuery(userDTO, maximumNumberOfFriends)).Returns(expectedQuery);
            _fakeTwitterAccessor.ArrangeExecuteJsonCursorGETQuery<IIdsCursorQueryResultDTO>(expectedQuery, expectedCursorResults);

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

            _fakeUserQueryGenerator.CallsTo(x => x.GetFriendIdsQuery(userScreenName, maximumNumberOfFriends)).Returns(expectedQuery);
            _fakeTwitterAccessor.ArrangeExecuteJsonCursorGETQuery<IIdsCursorQueryResultDTO>(expectedQuery, expectedCursorResults);

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

            _fakeUserQueryGenerator.CallsTo(x => x.GetFriendIdsQuery(userId, maximumNumberOfFriends)).Returns(expectedQuery);
            _fakeTwitterAccessor.ArrangeExecuteJsonCursorGETQuery<IIdsCursorQueryResultDTO>(expectedQuery, expectedCursorResults);

            // Act
            var result = queryExecutor.GetFriendIds(userId, maximumNumberOfFriends);

            // Assert
            Assert.IsTrue(result.ContainsAll(expectedCursorResults));
        }

        #endregion

        #region FollowerIds

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void GetFollowerIdsWithNullUser_ThrowsArgumentException()
        {
            // Arrange
            var queryExecutor = CreateUserJsonController();
            var maximumNumberOfFollowers = TestHelper.GenerateRandomInt();
            var expectedQuery = TestHelper.GenerateString();

            _fakeUserQueryGenerator.CallsTo(x => x.GetFollowerIdsQuery(A<IUserDTO>.Ignored, A<int>.Ignored)).Returns(expectedQuery);
            _fakeTwitterAccessor.ArrangeExecuteJsonCursorGETQuery<IIdsCursorQueryResultDTO>(expectedQuery, A.Fake<IEnumerable<string>>());

            // Act
            queryExecutor.GetFollowerIds((IUser)null, maximumNumberOfFollowers);
        }

        [TestMethod]
        public void GetFollowerIdsWithUser_ReturnsTwitterAccessorResult()
        {
            // Arrange
            var queryExecutor = CreateUserJsonController();
            var userDTO = A.Fake<IUserDTO>();
            var user = TestHelper.GenerateUser(userDTO);
            var maximumNumberOfFollowers = TestHelper.GenerateRandomInt();
            var expectedQuery = TestHelper.GenerateString();
            var expectedCursorResults = GenerateExpectedCursorResults();

            _fakeUserQueryGenerator.CallsTo(x => x.GetFollowerIdsQuery(userDTO, maximumNumberOfFollowers)).Returns(expectedQuery);
            _fakeTwitterAccessor.ArrangeExecuteJsonCursorGETQuery<IIdsCursorQueryResultDTO>(expectedQuery, expectedCursorResults);

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

            _fakeUserQueryGenerator.CallsTo(x => x.GetFollowerIdsQuery(userDTO, maximumNumberOfFollowers)).Returns(expectedQuery);
            _fakeTwitterAccessor.ArrangeExecuteJsonCursorGETQuery<IIdsCursorQueryResultDTO>(expectedQuery, expectedCursorResults);

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

            _fakeUserQueryGenerator.CallsTo(x => x.GetFollowerIdsQuery(userScreenName, maximumNumberOfFollowers)).Returns(expectedQuery);
            _fakeTwitterAccessor.ArrangeExecuteJsonCursorGETQuery<IIdsCursorQueryResultDTO>(expectedQuery, expectedCursorResults);

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

            _fakeUserQueryGenerator.CallsTo(x => x.GetFollowerIdsQuery(userId, maximumNumberOfFollowers)).Returns(expectedQuery);
            _fakeTwitterAccessor.ArrangeExecuteJsonCursorGETQuery<IIdsCursorQueryResultDTO>(expectedQuery, expectedCursorResults);

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
            var parameters = It.IsAny<IGetUserFavoritesQueryParameters>();

            _fakeUserQueryGenerator.CallsTo(x => x.GetFavoriteTweetsQuery(parameters)).Returns(expectedQuery);
            _fakeTwitterAccessor.ArrangeExecuteJsonGETQuery(expectedQuery, expectedResult);

            // Act
            var result = queryExecutor.GetFavoriteTweets(parameters);

            // Assert
            Assert.AreEqual(result, expectedResult);
        }

        #endregion

        #region Block User

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void BlockUser_WithNullUser_ThrowsArgumentException()
        {
            // Arrange
            var queryExecutor = CreateUserJsonController();
            var expectedQuery = TestHelper.GenerateString();
            var expectedResult = TestHelper.GenerateString();

            _fakeUserQueryGenerator.CallsTo(x => x.GetBlockUserQuery(A<IUserDTO>.Ignored)).Returns(expectedQuery);
            _fakeTwitterAccessor.ArrangeExecuteJsonPOSTQuery(expectedQuery, expectedResult);

            // Act
            queryExecutor.BlockUser((IUser)null);
        }

        [TestMethod]
        public void BlockUser_WithUser_ReturnsTwitterAccessorResult()
        {
            // Arrange
            var queryExecutor = CreateUserJsonController();
            var userDTO = A.Fake<IUserDTO>();
            var user = TestHelper.GenerateUser(userDTO);
            var expectedQuery = TestHelper.GenerateString();
            var expectedResult = TestHelper.GenerateString();

            _fakeUserQueryGenerator.CallsTo(x => x.GetBlockUserQuery(userDTO)).Returns(expectedQuery);
            _fakeTwitterAccessor.ArrangeExecuteJsonPOSTQuery(expectedQuery, expectedResult);

            // Act
            var result = queryExecutor.BlockUser(user);

            // Assert
            Assert.AreEqual(result, expectedResult);
        }

        [TestMethod]
        public void BlockUser_WithUserDTO_ReturnsTwitterAccessorResult()
        {
            // Arrange
            var queryExecutor = CreateUserJsonController();
            var userDTO = A.Fake<IUserDTO>();
            var expectedQuery = TestHelper.GenerateString();
            var expectedResult = TestHelper.GenerateString();

            _fakeUserQueryGenerator.CallsTo(x => x.GetBlockUserQuery(userDTO)).Returns(expectedQuery);
            _fakeTwitterAccessor.ArrangeExecuteJsonPOSTQuery(expectedQuery, expectedResult);

            // Act
            var result = queryExecutor.BlockUser(userDTO);

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

            _fakeUserQueryGenerator.CallsTo(x => x.GetBlockUserQuery(userScreenName)).Returns(expectedQuery);
            _fakeTwitterAccessor.ArrangeExecuteJsonPOSTQuery(expectedQuery, expectedResult);

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

            _fakeUserQueryGenerator.CallsTo(x => x.GetBlockUserQuery(userId)).Returns(expectedQuery);
            _fakeTwitterAccessor.ArrangeExecuteJsonPOSTQuery(expectedQuery, expectedResult);

            // Act
            var result = queryExecutor.BlockUser(userId);

            // Assert
            Assert.AreEqual(result, expectedResult);
        }

        #endregion

        private IEnumerable<string> GenerateExpectedCursorResults()
        {
            return new List<string>();
        }

        public UserJsonController CreateUserJsonController()
        {
            return _fakeBuilder.GenerateClass();
        }
    }
}