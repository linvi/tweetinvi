using System;
using System.IO;
using FakeItEasy;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Testinvi.Helpers;
using Tweetinvi.Controllers.User;
using Tweetinvi.Core.Factories;
using Tweetinvi.Core.Parameters;
using Tweetinvi.Models;
using Tweetinvi.Models.DTO;

namespace Testinvi.TweetinviControllers.UserTests
{
    [TestClass]
    public class UserControllerTests
    {
        private FakeClassBuilder<UserController> _fakeBuilder;

        private IUserQueryExecutor _userQueryExecutor;
        private ITweetFactory _tweetFactory;
        private IUserFactory _userFactory;

        [TestInitialize]
        public void TestInitialize()
        {
            _fakeBuilder = new FakeClassBuilder<UserController>();
            _userQueryExecutor = _fakeBuilder.GetFake<IUserQueryExecutor>().FakedObject;
            _tweetFactory = _fakeBuilder.GetFake<ITweetFactory>().FakedObject;
            _userFactory = _fakeBuilder.GetFake<IUserFactory>().FakedObject;
        }

        #region Get FriendIds

        [TestMethod]
        public void GetFriendIds_WithUser_ReturnsUserExecutorResult()
        {
            // Arrange
            var controller = CreateUserController();
            var userDTO = A.Fake<IUserDTO>();
            var user = TestHelper.GenerateUser(userDTO);
            var maximumNumberOfUsers = TestHelper.GenerateRandomInt();
            var friendIds = new[] { TestHelper.GenerateRandomLong() };

            A.CallTo(() => _userQueryExecutor.GetFriendIds(user, maximumNumberOfUsers)).Returns(friendIds);

            // Act
            var result = controller.GetFriendIds(user, maximumNumberOfUsers);

            // Assert
            Assert.AreEqual(result, friendIds);
        }

        [TestMethod]
        public void GetFriendIds_WithUserDTO_ReturnsUserExecutorResult()
        {
            // Arrange
            var controller = CreateUserController();
            var userDTO = A.Fake<IUserDTO>();
            var maximumNumberOfUsers = TestHelper.GenerateRandomInt();
            var friendIds = new[] { TestHelper.GenerateRandomLong() };

            A.CallTo(() => _userQueryExecutor.GetFriendIds(userDTO, maximumNumberOfUsers)).Returns(friendIds);

            // Act
            var result = controller.GetFriendIds(userDTO, maximumNumberOfUsers);

            // Assert
            Assert.AreEqual(result, friendIds);
        }

        [TestMethod]
        public void GetFriendIds_WithUserScreenName_ReturnsUserExecutorResult()
        {
            // Arrange
            var controller = CreateUserController();
            var userScreenName = TestHelper.GenerateString();
            var maximumNumberOfUsers = TestHelper.GenerateRandomInt();
            var friendIds = new[] { TestHelper.GenerateRandomLong() };

            A.CallTo(() =>
                _userQueryExecutor.GetFriendIds(A<IUserIdentifier>.That.Matches(u => u.ScreenName == userScreenName),
                    maximumNumberOfUsers)).Returns(friendIds);

            // Act
            var result = controller.GetFriendIds(userScreenName, maximumNumberOfUsers);

            // Assert
            Assert.AreEqual(result, friendIds);
        }

        [TestMethod]
        public void GetFriendIds_WithUserId_ReturnsUserExecutorResult()
        {
            // Arrange
            var controller = CreateUserController();
            var userId = TestHelper.GenerateRandomLong();
            var maximumNumberOfUsers = TestHelper.GenerateRandomInt();
            var friendIds = new[] { TestHelper.GenerateRandomLong() };

            A.CallTo(() =>
                _userQueryExecutor.GetFriendIds(A<IUserIdentifier>.That.Matches(u => u.Id == userId),
                    maximumNumberOfUsers)).Returns(friendIds);

            // Act
            var result = controller.GetFriendIds(userId, maximumNumberOfUsers);

            // Assert
            Assert.AreEqual(result, friendIds);
        }

        #endregion

        #region Get Friends

        [TestMethod]
        public void GetFriends_WithUser_ReturnsUserExecutorResult()
        {
            // Arrange
            var controller = CreateUserController();
            var userDTO = A.Fake<IUserDTO>();
            var user = TestHelper.GenerateUser(userDTO);
            var maximumNumberOfUsers = TestHelper.GenerateRandomInt();
            var friendIds = new[] { TestHelper.GenerateRandomLong() };
            var friends = new[] { A.Fake<IUser>() };

            A.CallTo(() => _userQueryExecutor.GetFriendIds(user, maximumNumberOfUsers)).Returns(friendIds);
            A.CallTo(() => _userFactory.GetUsersFromIds(friendIds)).Returns(friends);

            // Act
            var result = controller.GetFriends(user, maximumNumberOfUsers);

            // Assert
            Assert.AreEqual(result, friends);
        }

        [TestMethod]
        public void GetFriends_WithUserDTO_ReturnsUserExecutorResult()
        {
            // Arrange
            var controller = CreateUserController();
            var userDTO = A.Fake<IUserDTO>();
            var maximumNumberOfUsers = TestHelper.GenerateRandomInt();
            var friendIds = new[] { TestHelper.GenerateRandomLong() };
            var friends = new[] { A.Fake<IUser>() };

            A.CallTo(() => _userQueryExecutor.GetFriendIds(userDTO, maximumNumberOfUsers)).Returns(friendIds);
            A.CallTo(() => _userFactory.GetUsersFromIds(friendIds)).Returns(friends);

            // Act
            var result = controller.GetFriends(userDTO, maximumNumberOfUsers);

            // Assert
            Assert.AreEqual(result, friends);
        }

        [TestMethod]
        public void GetFriends_WithUserScreenName_ReturnsUserExecutorResult()
        {
            // Arrange
            var controller = CreateUserController();
            var userScreenName = TestHelper.GenerateString();
            var maximumNumberOfUsers = TestHelper.GenerateRandomInt();
            var friendIds = new[] { TestHelper.GenerateRandomLong() };
            var friends = new[] { A.Fake<IUser>() };

            A.CallTo(() =>
                _userQueryExecutor.GetFriendIds(A<IUserIdentifier>.That.Matches(u => u.ScreenName == userScreenName),
                    maximumNumberOfUsers)).Returns(friendIds);
            A.CallTo(() => _userFactory.GetUsersFromIds(friendIds)).Returns(friends);

            // Act
            var result = controller.GetFriends(userScreenName, maximumNumberOfUsers);

            // Assert
            Assert.AreEqual(result, friends);
        }

        [TestMethod]
        public void GetFriends_WithUserId_ReturnsUserExecutorResult()
        {
            // Arrange
            var controller = CreateUserController();
            var userId = TestHelper.GenerateRandomLong();
            var maximumNumberOfUsers = TestHelper.GenerateRandomInt();
            var friendIds = new[] { TestHelper.GenerateRandomLong() };
            var friends = new[] { A.Fake<IUser>() };

            A.CallTo(() =>
                _userQueryExecutor.GetFriendIds(A<IUserIdentifier>.That.Matches(u => u.Id == userId),
                    maximumNumberOfUsers)).Returns(friendIds);
            A.CallTo(() => _userFactory.GetUsersFromIds(friendIds)).Returns(friends);

            // Act
            var result = controller.GetFriends(userId, maximumNumberOfUsers);

            // Assert
            Assert.AreEqual(result, friends);
        }

        #endregion

        #region Get FollowerIds

        [TestMethod]
        public void GetFollowerIds_WithUser_ReturnsUserExecutorResult()
        {
            // Arrange
            var controller = CreateUserController();
            var userDTO = A.Fake<IUserDTO>();
            var user = TestHelper.GenerateUser(userDTO);
            var maximumNumberOfUsers = TestHelper.GenerateRandomInt();
            var followersIds = new[] { TestHelper.GenerateRandomLong() };

            A.CallTo(() => _userQueryExecutor.GetFollowerIds(user, maximumNumberOfUsers)).Returns(followersIds);

            // Act
            var result = controller.GetFollowerIds(user, maximumNumberOfUsers);

            // Assert
            Assert.AreEqual(result, followersIds);
        }

        [TestMethod]
        public void GetFollowerIds_WithUserDTO_ReturnsUserExecutorResult()
        {
            // Arrange
            var controller = CreateUserController();
            var userDTO = A.Fake<IUserDTO>();
            var maximumNumberOfUsers = TestHelper.GenerateRandomInt();
            var followersIds = new[] { TestHelper.GenerateRandomLong() };

            A.CallTo(() => _userQueryExecutor.GetFollowerIds(userDTO, maximumNumberOfUsers)).Returns(followersIds);

            // Act
            var result = controller.GetFollowerIds(userDTO, maximumNumberOfUsers);

            // Assert
            Assert.AreEqual(result, followersIds);
        }

        [TestMethod]
        public void GetFollowerIds_WithUserScreenName_ReturnsUserExecutorResult()
        {
            // Arrange
            var controller = CreateUserController();
            var userScreenName = TestHelper.GenerateString();
            var maximumNumberOfUsers = TestHelper.GenerateRandomInt();
            var followersIds = new[] { TestHelper.GenerateRandomLong() };

            A.CallTo(() =>
                _userQueryExecutor.GetFollowerIds(A<IUserIdentifier>.That.Matches(u => u.ScreenName == userScreenName),
                    maximumNumberOfUsers)).Returns(followersIds);

            // Act
            var result = controller.GetFollowerIds(userScreenName, maximumNumberOfUsers);

            // Assert
            Assert.AreEqual(result, followersIds);
        }

        [TestMethod]
        public void GetFollowerIds_WithUserId_ReturnsUserExecutorResult()
        {
            // Arrange
            var controller = CreateUserController();
            var userId = TestHelper.GenerateRandomLong();
            var maximumNumberOfUsers = TestHelper.GenerateRandomInt();
            var followersIds = new[] { TestHelper.GenerateRandomLong() };

            A.CallTo(() =>
                _userQueryExecutor.GetFollowerIds(A<IUserIdentifier>.That.Matches(u => u.Id == userId),
                    maximumNumberOfUsers)).Returns(followersIds);

            // Act
            var result = controller.GetFollowerIds(userId, maximumNumberOfUsers);

            // Assert
            Assert.AreEqual(result, followersIds);
        }

        #endregion

        #region Get Followers

        [TestMethod]
        public void GetFollowers_WithUser_ReturnsUserExecutorResult()
        {
            // Arrange
            var controller = CreateUserController();
            var userDTO = A.Fake<IUserDTO>();
            var user = TestHelper.GenerateUser(userDTO);
            var maximumNumberOfUsers = TestHelper.GenerateRandomInt();
            var followerIds = new[] { TestHelper.GenerateRandomLong() };
            var followers = new[] { A.Fake<IUser>() };

            A.CallTo(() => _userQueryExecutor.GetFollowerIds(user, maximumNumberOfUsers)).Returns(followerIds);
            A.CallTo(() => _userFactory.GetUsersFromIds(followerIds)).Returns(followers);

            // Act
            var result = controller.GetFollowers(user, maximumNumberOfUsers);

            // Assert
            Assert.AreEqual(result, followers);
        }

        [TestMethod]
        public void GetFollowers_WithUserDTO_ReturnsUserExecutorResult()
        {
            // Arrange
            var controller = CreateUserController();
            var userDTO = A.Fake<IUserDTO>();
            var maximumNumberOfUsers = TestHelper.GenerateRandomInt();
            var followerIds = new[] { TestHelper.GenerateRandomLong() };
            var followers = new[] { A.Fake<IUser>() };

            A.CallTo(() => _userQueryExecutor.GetFollowerIds(userDTO, maximumNumberOfUsers)).Returns(followerIds);
            A.CallTo(() => _userFactory.GetUsersFromIds(followerIds)).Returns(followers);

            // Act
            var result = controller.GetFollowers(userDTO, maximumNumberOfUsers);

            // Assert
            Assert.AreEqual(result, followers);
        }

        [TestMethod]
        public void GetFollowers_WithUserScreenName_ReturnsUserExecutorResult()
        {
            // Arrange
            var controller = CreateUserController();
            var userScreenName = TestHelper.GenerateString();
            var maximumNumberOfUsers = TestHelper.GenerateRandomInt();
            var followerIds = new[] { TestHelper.GenerateRandomLong() };
            var followers = new[] { A.Fake<IUser>() };

            A.CallTo(() =>
                _userQueryExecutor.GetFollowerIds(A<IUserIdentifier>.That.Matches(u => u.ScreenName == userScreenName),
                    maximumNumberOfUsers)).Returns(followerIds);
            A.CallTo(() => _userFactory.GetUsersFromIds(followerIds)).Returns(followers);

            // Act
            var result = controller.GetFollowers(userScreenName, maximumNumberOfUsers);

            // Assert
            Assert.AreEqual(result, followers);
        }

        [TestMethod]
        public void GetFollowers_WithUserId_ReturnsUserExecutorResult()
        {
            // Arrange
            var controller = CreateUserController();
            var userId = TestHelper.GenerateRandomLong();
            var maximumNumberOfUsers = TestHelper.GenerateRandomInt();
            var followerIds = new[] { TestHelper.GenerateRandomLong() };
            var followers = new[] { A.Fake<IUser>() };

            A.CallTo(() =>
                _userQueryExecutor.GetFollowerIds(A<IUserIdentifier>.That.Matches(u => u.Id == userId),
                    maximumNumberOfUsers)).Returns(followerIds);
            A.CallTo(() => _userFactory.GetUsersFromIds(followerIds)).Returns(followers);

            // Act
            var result = controller.GetFollowers(userId, maximumNumberOfUsers);

            // Assert
            Assert.AreEqual(result, followers);
        }

        #endregion

        #region Get Favourites

        [TestMethod]
        public void GetFavouriteTweets_WithUser_ReturnsUserExecutorResult()
        {
            // Arrange
            var controller = CreateUserController();
            var tweetsDTO = new[] { A.Fake<ITweetDTO>() };
            var tweets = new[] { A.Fake<ITweet>() };
            var parameters = A.Fake<IGetUserFavoritesQueryParameters>();

            A.CallTo(() => _userQueryExecutor.GetFavoriteTweets(parameters)).Returns(tweetsDTO);
            A.CallTo(() => _tweetFactory.GenerateTweetsFromDTO(tweetsDTO, null)).Returns(tweets);

            // Act
            var result = controller.GetFavoriteTweets(parameters);

            // Assert
            Assert.AreEqual(result, tweets);
        }

        #endregion

        #region Block User

        [TestMethod]
        public void BlockUser_WithUser_ReturnsUserExecutorResult_False()
        {
            // Arrange
            var controller = CreateUserController();
            var userDTO = A.Fake<IUserDTO>();
            var user = TestHelper.GenerateUser(userDTO);

            A.CallTo(() => _userQueryExecutor.BlockUser(userDTO)).Returns(false);

            // Act
            var result = controller.BlockUser(user);

            // Assert
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void BlockUser_WithUser_ReturnsUserExecutorResult_True()
        {
            // Arrange
            var controller = CreateUserController();
            var user = TestHelper.GenerateUser(A.Fake<IUserDTO>());

            A.CallTo(() => _userQueryExecutor.BlockUser(user)).Returns(true);

            // Act
            var result = controller.BlockUser(user);

            // Assert
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void BlockUser_WithUserDTO_ReturnsUserExecutorResult_False()
        {
            // Arrange
            var controller = CreateUserController();
            var userDTO = A.Fake<IUserDTO>();

            A.CallTo(() => _userQueryExecutor.BlockUser(userDTO)).Returns(false);

            // Act
            var result = controller.BlockUser(userDTO);

            // Assert
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void BlockUser_WithUserDTO_ReturnsUserExecutorResult_True()
        {
            // Arrange
            var controller = CreateUserController();
            var userDTO = A.Fake<IUserDTO>();

            A.CallTo(() => _userQueryExecutor.BlockUser(userDTO)).Returns(true);

            // Act
            var result = controller.BlockUser(userDTO);

            // Assert
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void BlockUser_WithUserScreenName_ReturnsUserExecutorResult_False()
        {
            // Arrange
            var controller = CreateUserController();
            var userScreenName = TestHelper.GenerateString();

            A.CallTo(() =>
                    _userQueryExecutor.BlockUser(A<IUserIdentifier>.That.Matches(u => u.ScreenName == userScreenName)))
                .Returns(false);

            // Act
            var result = controller.BlockUser(userScreenName);

            // Assert
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void BlockUser_WithUserScreenName_ReturnsUserExecutorResult_True()
        {
            // Arrange
            var controller = CreateUserController();
            var userScreenName = TestHelper.GenerateString();

            A.CallTo(() =>
                    _userQueryExecutor.BlockUser(A<IUserIdentifier>.That.Matches(u => u.ScreenName == userScreenName)))
                .Returns(true);

            // Act
            var result = controller.BlockUser(userScreenName);

            // Assert
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void BlockUser_WithUserId_ReturnsUserExecutorResult_False()
        {
            // Arrange
            var controller = CreateUserController();
            var userId = TestHelper.GenerateRandomLong();

            A.CallTo(() => _userQueryExecutor.BlockUser(A<IUserIdentifier>.That.Matches(u => u.Id == userId)))
                .Returns(false);

            // Act
            var result = controller.BlockUser(userId);

            // Assert
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void BlockUser_WithUserId_ReturnsUserExecutorResult_True()
        {
            // Arrange
            var controller = CreateUserController();
            var userId = TestHelper.GenerateRandomLong();

            A.CallTo(() => _userQueryExecutor.BlockUser(A<IUserIdentifier>.That.Matches(u => u.Id == userId)))
                .Returns(true);

            // Act
            var result = controller.BlockUser(userId);

            // Assert
            Assert.IsTrue(result);
        }

        #endregion

        #region Stream Profile Image

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void GenerateProfileImageStream_WithNullUser_ThrowsArgumentException()
        {
            // Arrange
            var controller = CreateUserController();
            
            // Act
            controller.GetProfileImageStream((IUser)null, ImageSize.bigger);
        }

        [TestMethod]
        public void GenerateProfileImageStream_WithUser_ReturnQueryExecutorStream()
        {
            // Arrange
            var controller = CreateUserController();
            var userDTO = A.Fake<IUserDTO>();
            var user = TestHelper.GenerateUser(userDTO);
            var stream = A.Fake<Stream>();

            A.CallTo(() => _userQueryExecutor.GetProfileImageStream(userDTO, ImageSize.bigger)).Returns(stream);

            // Act
            var result = controller.GetProfileImageStream(user, ImageSize.bigger);

            // Assert
            Assert.AreEqual(result, stream);
        }

        [TestMethod]
        public void GetProfileImageStream_WithUserDTO_ReturnQueryExecutorStream()
        {
            // Arrange
            var controller = CreateUserController();
            var userDTO = A.Fake<IUserDTO>();
            var stream = A.Fake<Stream>();

            A.CallTo(() => _userQueryExecutor.GetProfileImageStream(userDTO, ImageSize.bigger)).Returns(stream);

            // Act
            var result = controller.GetProfileImageStream(userDTO, ImageSize.bigger);

            // Assert
            Assert.AreEqual(result, stream);
        } 
        #endregion

        #region Spam

        [TestMethod]
        public void ReportUserForSpam_WithUser_ReturnsUserExecutorResult_False()
        {
            // Arrange
            var controller = CreateUserController();
            var userDTO = A.Fake<IUserDTO>();
            var user = TestHelper.GenerateUser(userDTO);

            A.CallTo(() => _userQueryExecutor.ReportUserForSpam(userDTO)).Returns(false);

            // Act
            var result = controller.ReportUserForSpam(user);

            // Assert
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void ReportUserForSpam_WithUserDTO_ReturnsUserExecutorResult_False()
        {
            // Arrange
            var controller = CreateUserController();
            var userDTO = A.Fake<IUserDTO>();

            A.CallTo(() => _userQueryExecutor.ReportUserForSpam(userDTO)).Returns(false);

            // Act
            var result = controller.ReportUserForSpam(userDTO);

            // Assert
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void ReportUserForSpam_WithUserDTO_ReturnsUserExecutorResult_True()
        {
            // Arrange
            var controller = CreateUserController();
            var userDTO = A.Fake<IUserDTO>();

            A.CallTo(() => _userQueryExecutor.ReportUserForSpam(userDTO)).Returns(true);

            // Act
            var result = controller.ReportUserForSpam(userDTO);

            // Assert
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void ReportUserForSpam_WithUserScreenName_ReturnsUserExecutorResult_False()
        {
            // Arrange
            var controller = CreateUserController();
            var userScreenName = TestHelper.GenerateString();

            A.CallTo(() =>
                _userQueryExecutor.ReportUserForSpam(
                    A<IUserIdentifier>.That.Matches(u => u.ScreenName == userScreenName))).Returns(false);

            // Act
            var result = controller.ReportUserForSpam(userScreenName);

            // Assert
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void ReportUserForSpam_WithUserScreenName_ReturnsUserExecutorResult_True()
        {
            // Arrange
            var controller = CreateUserController();
            var userScreenName = TestHelper.GenerateString();

            A.CallTo(() =>
                _userQueryExecutor.ReportUserForSpam(
                    A<IUserIdentifier>.That.Matches(u => u.ScreenName == userScreenName))).Returns(true);

            // Act
            var result = controller.ReportUserForSpam(userScreenName);

            // Assert
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void ReportUserForSpam_WithUserId_ReturnsUserExecutorResult_False()
        {
            // Arrange
            var controller = CreateUserController();
            var userId = TestHelper.GenerateRandomLong();

            A.CallTo(() => _userQueryExecutor.ReportUserForSpam(A<IUserIdentifier>.That.Matches(u => u.Id == userId)))
                .Returns(false);

            // Act
            var result = controller.ReportUserForSpam(userId);

            // Assert
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void ReportUserForSpam_WithUserId_ReturnsUserExecutorResult_True()
        {
            // Arrange
            var controller = CreateUserController();
            var userId = TestHelper.GenerateRandomLong();

            A.CallTo(() => _userQueryExecutor.ReportUserForSpam(A<IUserIdentifier>.That.Matches(u => u.Id == userId)))
                .Returns(true);

            // Act
            var result = controller.ReportUserForSpam(userId);

            // Assert
            Assert.IsTrue(result);
        }

        #endregion

        public UserController CreateUserController()
        {
            return _fakeBuilder.GenerateClass();
        }
    }
}