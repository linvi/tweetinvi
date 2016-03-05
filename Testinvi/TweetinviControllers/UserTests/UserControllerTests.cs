using System;
using System.IO;
using FakeItEasy;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Testinvi.Helpers;
using Tweetinvi.Controllers.User;
using Tweetinvi.Core.Enum;
using Tweetinvi.Core.Interfaces;
using Tweetinvi.Core.Interfaces.DTO;
using Tweetinvi.Core.Interfaces.Factories;
using Tweetinvi.Core.Parameters.QueryParameters;

namespace Testinvi.TweetinviControllers.UserTests
{
    [TestClass]
    public class UserControllerTests
    {
        private FakeClassBuilder<UserController> _fakeBuilder;
        private Fake<IUserQueryExecutor> _fakeUserQueryExecutor;
        private Fake<ITweetFactory> _fakeTweetFactory;
        private Fake<IUserFactory> _fakeUserFactory;

        [TestInitialize]
        public void TestInitialize()
        {
            _fakeBuilder = new FakeClassBuilder<UserController>();
            _fakeUserQueryExecutor = _fakeBuilder.GetFake<IUserQueryExecutor>();
            _fakeTweetFactory = _fakeBuilder.GetFake<ITweetFactory>();
            _fakeUserFactory = _fakeBuilder.GetFake<IUserFactory>();
        }

        #region Get FriendIds

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void GetFriendIds_WithNullUser_ThrowsArgumentException()
        {
            // Arrange
            var controller = CreateUserController();
            var maximumNumberOfUsers = TestHelper.GenerateRandomInt();

            // Act
            controller.GetFriendIds((IUser)null, maximumNumberOfUsers);
        }

        [TestMethod]
        public void GetFriendIds_WithUser_ReturnsUserExecutorResult()
        {
            // Arrange
            var controller = CreateUserController();
            var userDTO = A.Fake<IUserDTO>();
            var user = TestHelper.GenerateUser(userDTO);
            var maximumNumberOfUsers = TestHelper.GenerateRandomInt();
            var friendIds = new[] { TestHelper.GenerateRandomLong() };

            _fakeUserQueryExecutor.CallsTo(x => x.GetFriendIds(userDTO, maximumNumberOfUsers)).Returns(friendIds);

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

            _fakeUserQueryExecutor.CallsTo(x => x.GetFriendIds(userDTO, maximumNumberOfUsers)).Returns(friendIds);

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

            _fakeUserQueryExecutor.CallsTo(x => x.GetFriendIds(userScreenName, maximumNumberOfUsers)).Returns(friendIds);

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

            _fakeUserQueryExecutor.CallsTo(x => x.GetFriendIds(userId, maximumNumberOfUsers)).Returns(friendIds);

            // Act
            var result = controller.GetFriendIds(userId, maximumNumberOfUsers);

            // Assert
            Assert.AreEqual(result, friendIds);
        }

        #endregion

        #region Get Friends

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void GetFriends_WithNullUser_ThrowsArgumentException()
        {
            // Arrange
            var controller = CreateUserController();
            var maximumNumberOfUsers = TestHelper.GenerateRandomInt();

            // Act
            controller.GetFriends((IUser)null, maximumNumberOfUsers);
        }

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

            _fakeUserQueryExecutor.CallsTo(x => x.GetFriendIds(userDTO, maximumNumberOfUsers)).Returns(friendIds);
            _fakeUserFactory.CallsTo(x => x.GetUsersFromIds(friendIds)).Returns(friends);

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

            _fakeUserQueryExecutor.CallsTo(x => x.GetFriendIds(userDTO, maximumNumberOfUsers)).Returns(friendIds);
            _fakeUserFactory.CallsTo(x => x.GetUsersFromIds(friendIds)).Returns(friends);

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

            _fakeUserQueryExecutor.CallsTo(x => x.GetFriendIds(userScreenName, maximumNumberOfUsers)).Returns(friendIds);
            _fakeUserFactory.CallsTo(x => x.GetUsersFromIds(friendIds)).Returns(friends);

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

            _fakeUserQueryExecutor.CallsTo(x => x.GetFriendIds(userId, maximumNumberOfUsers)).Returns(friendIds);
            _fakeUserFactory.CallsTo(x => x.GetUsersFromIds(friendIds)).Returns(friends);

            // Act
            var result = controller.GetFriends(userId, maximumNumberOfUsers);

            // Assert
            Assert.AreEqual(result, friends);
        }

        #endregion

        #region Get FollowerIds

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void GetFollowerIds_WithNullUser_ThrowsArgumentException()
        {
            // Arrange
            var controller = CreateUserController();
            var maximumNumberOfUsers = TestHelper.GenerateRandomInt();

            // Act
            controller.GetFollowerIds((IUser)null, maximumNumberOfUsers);
        }

        [TestMethod]
        public void GetFollowerIds_WithUser_ReturnsUserExecutorResult()
        {
            // Arrange
            var controller = CreateUserController();
            var userDTO = A.Fake<IUserDTO>();
            var user = TestHelper.GenerateUser(userDTO);
            var maximumNumberOfUsers = TestHelper.GenerateRandomInt();
            var followersIds = new[] { TestHelper.GenerateRandomLong() };

            _fakeUserQueryExecutor.CallsTo(x => x.GetFollowerIds(userDTO, maximumNumberOfUsers)).Returns(followersIds);

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

            _fakeUserQueryExecutor.CallsTo(x => x.GetFollowerIds(userDTO, maximumNumberOfUsers)).Returns(followersIds);

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

            _fakeUserQueryExecutor.CallsTo(x => x.GetFollowerIds(userScreenName, maximumNumberOfUsers)).Returns(followersIds);

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

            _fakeUserQueryExecutor.CallsTo(x => x.GetFollowerIds(userId, maximumNumberOfUsers)).Returns(followersIds);

            // Act
            var result = controller.GetFollowerIds(userId, maximumNumberOfUsers);

            // Assert
            Assert.AreEqual(result, followersIds);
        }

        #endregion

        #region Get Followers

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void GetFollowers_WithNullUser_ThrowsArgumentException()
        {
            // Arrange
            var controller = CreateUserController();
            var maximumNumberOfUsers = TestHelper.GenerateRandomInt();

            // Act
            controller.GetFollowers((IUser)null, maximumNumberOfUsers);
        }

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

            _fakeUserQueryExecutor.CallsTo(x => x.GetFollowerIds(userDTO, maximumNumberOfUsers)).Returns(followerIds);
            _fakeUserFactory.CallsTo(x => x.GetUsersFromIds(followerIds)).Returns(followers);

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

            _fakeUserQueryExecutor.CallsTo(x => x.GetFollowerIds(userDTO, maximumNumberOfUsers)).Returns(followerIds);
            _fakeUserFactory.CallsTo(x => x.GetUsersFromIds(followerIds)).Returns(followers);

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

            _fakeUserQueryExecutor.CallsTo(x => x.GetFollowerIds(userScreenName, maximumNumberOfUsers)).Returns(followerIds);
            _fakeUserFactory.CallsTo(x => x.GetUsersFromIds(followerIds)).Returns(followers);

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

            _fakeUserQueryExecutor.CallsTo(x => x.GetFollowerIds(userId, maximumNumberOfUsers)).Returns(followerIds);
            _fakeUserFactory.CallsTo(x => x.GetUsersFromIds(followerIds)).Returns(followers);

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
            var parameters = It.IsAny<IGetUserFavoritesQueryParameters>();

            _fakeUserQueryExecutor.CallsTo(x => x.GetFavoriteTweets(parameters)).Returns(tweetsDTO);
            _fakeTweetFactory.CallsTo(x => x.GenerateTweetsFromDTO(tweetsDTO)).Returns(tweets);

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

            _fakeUserQueryExecutor.CallsTo(x => x.BlockUser(userDTO)).Returns(false);

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

            _fakeUserQueryExecutor.CallsTo(x => x.BlockUser(user)).Returns(true);

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

            _fakeUserQueryExecutor.CallsTo(x => x.BlockUser(userDTO)).Returns(false);

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

            _fakeUserQueryExecutor.CallsTo(x => x.BlockUser(userDTO)).Returns(true);

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

            _fakeUserQueryExecutor.CallsTo(x => x.BlockUser(userScreenName)).Returns(false);

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

            _fakeUserQueryExecutor.CallsTo(x => x.BlockUser(userScreenName)).Returns(true);

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

            _fakeUserQueryExecutor.CallsTo(x => x.BlockUser(userId)).Returns(false);

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

            _fakeUserQueryExecutor.CallsTo(x => x.BlockUser(userId)).Returns(true);

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

            _fakeUserQueryExecutor.CallsTo(x => x.GetProfileImageStream(userDTO, ImageSize.bigger)).Returns(stream);

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

            _fakeUserQueryExecutor.CallsTo(x => x.GetProfileImageStream(userDTO, ImageSize.bigger)).Returns(stream);

            // Act
            var result = controller.GetProfileImageStream(userDTO, ImageSize.bigger);

            // Assert
            Assert.AreEqual(result, stream);
        } 
        #endregion

        #region Spam

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void ReportUserForSpam_WithNullUser_ThrowsArgumentException()
        {
            // Arrange
            var controller = CreateUserController();

            _fakeUserQueryExecutor.CallsTo(x => x.ReportUserForSpam(A<IUserDTO>.Ignored)).Returns(true);

            // Act
            controller.ReportUserForSpam((IUser)null);
        }

        [TestMethod]
        public void ReportUserForSpam_WithUser_ReturnsUserExecutorResult_False()
        {
            // Arrange
            var controller = CreateUserController();
            var userDTO = A.Fake<IUserDTO>();
            var user = TestHelper.GenerateUser(userDTO);

            _fakeUserQueryExecutor.CallsTo(x => x.ReportUserForSpam(userDTO)).Returns(false);

            // Act
            var result = controller.ReportUserForSpam(user);

            // Assert
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void ReportUserForSpam_WithUser_ReturnsUserExecutorResult_True()
        {
            // Arrange
            var controller = CreateUserController();
            var userDTO = A.Fake<IUserDTO>();
            var user = TestHelper.GenerateUser(userDTO);

            _fakeUserQueryExecutor.CallsTo(x => x.ReportUserForSpam(userDTO)).Returns(true);

            // Act
            var result = controller.ReportUserForSpam(user);

            // Assert
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void ReportUserForSpam_WithUserDTO_ReturnsUserExecutorResult_False()
        {
            // Arrange
            var controller = CreateUserController();
            var userDTO = A.Fake<IUserDTO>();

            _fakeUserQueryExecutor.CallsTo(x => x.ReportUserForSpam(userDTO)).Returns(false);

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

            _fakeUserQueryExecutor.CallsTo(x => x.ReportUserForSpam(userDTO)).Returns(true);

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

            _fakeUserQueryExecutor.CallsTo(x => x.ReportUserForSpam(userScreenName)).Returns(false);

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

            _fakeUserQueryExecutor.CallsTo(x => x.ReportUserForSpam(userScreenName)).Returns(true);

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

            _fakeUserQueryExecutor.CallsTo(x => x.ReportUserForSpam(userId)).Returns(false);

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

            _fakeUserQueryExecutor.CallsTo(x => x.ReportUserForSpam(userId)).Returns(true);

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