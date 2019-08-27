using System;
using System.IO;
using System.Threading.Tasks;
using FakeItEasy;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Testinvi.Helpers;
using Tweetinvi.Controllers.User;
using Tweetinvi.Core.DTO.Cursor;
using Tweetinvi.Core.Factories;
using Tweetinvi.Core.Parameters;
using Tweetinvi.Core.Web;
using Tweetinvi.Models;
using Tweetinvi.Models.DTO;
using Tweetinvi.Models.DTO.QueryDTO;
using Tweetinvi.Parameters;

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
        public void GetFriendIds_ReturnsUserExecutorResult()
        {
            // Arrange
            var controller = CreateUserController();

            var parameters = new GetFriendIdsParameters("username");
            var expectedResult = A.Fake<ITwitterResult<IIdsCursorQueryResultDTO>>();

            _fakeUserQueryExecutor.CallsTo(x => x.GetFriendIds(A<IGetFriendIdsParameters>.Ignored, A<ITwitterRequest>.Ignored)).Returns(expectedResult);

            // Act
            var result = controller.GetFriendIds(parameters, A.Fake<ITwitterRequest>());
            result.MoveNext();

            // Assert
            Assert.AreEqual(result.TwitterResults[0], expectedResult);
        }

        #endregion

        #region Get Friends

        [TestMethod]
        public void GetFriend_ReturnsUserExecutorResult()
        {
            throw new NotImplementedException("TO COMPLETE");

            // Arrange
            var controller = CreateUserController();

            var parameter = new GetFriendIdsParameters("username");
            var expectedResult = A.Fake<TwitterResult<IIdsCursorQueryResultDTO>>();
            var friendIds = new[] { TestHelper.GenerateRandomLong() };
            var friends = new[] { A.Fake<IUser>() };

            expectedResult.DataTransferObject = new IdsCursorQueryResultDTO()
            {
                Ids = friendIds
            };

            _fakeUserQueryExecutor.CallsTo(x => x.GetFriendIds(parameter, A<ITwitterRequest>.Ignored)).Returns(expectedResult);
            _fakeUserFactory.CallsTo(x => x.GetUsersFromIds(friendIds)).Returns(friends);

            // Act
            var result = controller.GetFriendIds(parameter, A.Fake<ITwitterRequest>());

            // Assert
            //Assert.AreEqual(result, );
        }

        #endregion

        #region Get FollowerIds

        [TestMethod]
        public async Task GetFollowerIds_WithUser_ReturnsUserExecutorResult()
        {
            // Arrange
            var controller = CreateUserController();
            var userDTO = A.Fake<IUserDTO>();
            var user = TestHelper.GenerateUser(userDTO);
            var maximumNumberOfUsers = TestHelper.GenerateRandomInt();
            var followersIds = new[] { TestHelper.GenerateRandomLong() };

            _fakeUserQueryExecutor.CallsTo(x => x.GetFollowerIds(user, maximumNumberOfUsers)).Returns(followersIds);

            // Act
            var result = await controller.GetFollowerIds(user, maximumNumberOfUsers);

            // Assert
            Assert.AreEqual(result, followersIds);
        }

        [TestMethod]
        public async Task GetFollowerIds_WithUserDTO_ReturnsUserExecutorResult()
        {
            // Arrange
            var controller = CreateUserController();
            var userDTO = A.Fake<IUserDTO>();
            var maximumNumberOfUsers = TestHelper.GenerateRandomInt();
            var followersIds = new[] { TestHelper.GenerateRandomLong() };

            _fakeUserQueryExecutor.CallsTo(x => x.GetFollowerIds(userDTO, maximumNumberOfUsers)).Returns(followersIds);

            // Act
            var result = await controller.GetFollowerIds(userDTO, maximumNumberOfUsers);

            // Assert
            Assert.AreEqual(result, followersIds);
        }

        [TestMethod]
        public async Task GetFollowerIds_WithUserScreenName_ReturnsUserExecutorResult()
        {
            // Arrange
            var controller = CreateUserController();
            var userScreenName = TestHelper.GenerateString();
            var maximumNumberOfUsers = TestHelper.GenerateRandomInt();
            var followersIds = new[] { TestHelper.GenerateRandomLong() };

            _fakeUserQueryExecutor.CallsTo(x => x.GetFollowerIds(A<IUserIdentifier>.That.Matches(u => u.ScreenName == userScreenName), maximumNumberOfUsers)).Returns(followersIds);

            // Act
            var result = await controller.GetFollowerIds(userScreenName, maximumNumberOfUsers);

            // Assert
            Assert.AreEqual(result, followersIds);
        }

        [TestMethod]
        public async Task GetFollowerIds_WithUserId_ReturnsUserExecutorResult()
        {
            // Arrange
            var controller = CreateUserController();
            var userId = TestHelper.GenerateRandomLong();
            var maximumNumberOfUsers = TestHelper.GenerateRandomInt();
            var followersIds = new[] { TestHelper.GenerateRandomLong() };

            _fakeUserQueryExecutor.CallsTo(x => x.GetFollowerIds(A<IUserIdentifier>.That.Matches(u => u.Id == userId), maximumNumberOfUsers)).Returns(followersIds);

            // Act
            var result = await controller.GetFollowerIds(userId, maximumNumberOfUsers);

            // Assert
            Assert.AreEqual(result, followersIds);
        }

        #endregion

        #region Get Followers

        [TestMethod]
        public async Task GetFollowers_WithUser_ReturnsUserExecutorResult()
        {
            // Arrange
            var controller = CreateUserController();
            var userDTO = A.Fake<IUserDTO>();
            var user = TestHelper.GenerateUser(userDTO);
            var maximumNumberOfUsers = TestHelper.GenerateRandomInt();
            var followerIds = new[] { TestHelper.GenerateRandomLong() };
            var followers = new[] { A.Fake<IUser>() };

            _fakeUserQueryExecutor.CallsTo(x => x.GetFollowerIds(user, maximumNumberOfUsers)).Returns(followerIds);
            _fakeUserFactory.CallsTo(x => x.GetUsersFromIds(followerIds)).Returns(followers);

            // Act
            var result = await controller.GetFollowers(user, maximumNumberOfUsers);

            // Assert
            Assert.AreEqual(result, followers);
        }

        [TestMethod]
        public async Task GetFollowers_WithUserDTO_ReturnsUserExecutorResult()
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
            var result = await controller.GetFollowers(userDTO, maximumNumberOfUsers);

            // Assert
            Assert.AreEqual(result, followers);
        }

        [TestMethod]
        public async Task GetFollowers_WithUserScreenName_ReturnsUserExecutorResult()
        {
            // Arrange
            var controller = CreateUserController();
            var userScreenName = TestHelper.GenerateString();
            var maximumNumberOfUsers = TestHelper.GenerateRandomInt();
            var followerIds = new[] { TestHelper.GenerateRandomLong() };
            var followers = new[] { A.Fake<IUser>() };

            _fakeUserQueryExecutor.CallsTo(x => x.GetFollowerIds(A<IUserIdentifier>.That.Matches(u => u.ScreenName == userScreenName), maximumNumberOfUsers)).Returns(followerIds);
            _fakeUserFactory.CallsTo(x => x.GetUsersFromIds(followerIds)).Returns(followers);

            // Act
            var result = await controller.GetFollowers(userScreenName, maximumNumberOfUsers);

            // Assert
            Assert.AreEqual(result, followers);
        }

        [TestMethod]
        public async Task GetFollowers_WithUserId_ReturnsUserExecutorResult()
        {
            // Arrange
            var controller = CreateUserController();
            var userId = TestHelper.GenerateRandomLong();
            var maximumNumberOfUsers = TestHelper.GenerateRandomInt();
            var followerIds = new[] { TestHelper.GenerateRandomLong() };
            var followers = new[] { A.Fake<IUser>() };

            _fakeUserQueryExecutor.CallsTo(x => x.GetFollowerIds(A<IUserIdentifier>.That.Matches(u => u.Id == userId), maximumNumberOfUsers)).Returns(followerIds);
            _fakeUserFactory.CallsTo(x => x.GetUsersFromIds(followerIds)).Returns(followers);

            // Act
            var result = await controller.GetFollowers(userId, maximumNumberOfUsers);

            // Assert
            Assert.AreEqual(result, followers);
        }

        #endregion

        #region Get Favourites

        [TestMethod]
        public async Task GetFavouriteTweets_WithUser_ReturnsUserExecutorResult()
        {
            // Arrange
            var controller = CreateUserController();
            var tweetsDTO = new[] { A.Fake<ITweetDTO>() };
            var tweets = new[] { A.Fake<ITweet>() };
            var parameters = It.IsAny<IGetUserFavoritesQueryParameters>();

            _fakeUserQueryExecutor.CallsTo(x => x.GetFavoriteTweets(parameters)).Returns(tweetsDTO);
            _fakeTweetFactory.CallsTo(x => x.GenerateTweetsFromDTO(tweetsDTO, null, null)).Returns(tweets);

            // Act
            var result = await controller.GetFavoriteTweets(parameters);

            // Assert
            Assert.AreEqual(result, tweets);
        }

        #endregion

        #region Block User

        [TestMethod]
        public async Task BlockUser_WithUser_ReturnsUserExecutorResult_False()
        {
            // Arrange
            var controller = CreateUserController();
            var userDTO = A.Fake<IUserDTO>();
            var user = TestHelper.GenerateUser(userDTO);

            _fakeUserQueryExecutor.CallsTo(x => x.BlockUser(userDTO)).Returns(false);

            // Act
            var result = await controller.BlockUser(user);

            // Assert
            Assert.IsFalse(result);
        }

        [TestMethod]
        public async Task BlockUser_WithUser_ReturnsUserExecutorResult_True()
        {
            // Arrange
            var controller = CreateUserController();
            var user = TestHelper.GenerateUser(A.Fake<IUserDTO>());

            _fakeUserQueryExecutor.CallsTo(x => x.BlockUser(user)).Returns(true);

            // Act
            var result = await controller.BlockUser(user);

            // Assert
            Assert.IsTrue(result);
        }

        [TestMethod]
        public async Task BlockUser_WithUserDTO_ReturnsUserExecutorResult_False()
        {
            // Arrange
            var controller = CreateUserController();
            var userDTO = A.Fake<IUserDTO>();

            _fakeUserQueryExecutor.CallsTo(x => x.BlockUser(userDTO)).Returns(false);

            // Act
            var result = await controller.BlockUser(userDTO);

            // Assert
            Assert.IsFalse(result);
        }

        [TestMethod]
        public async Task BlockUser_WithUserDTO_ReturnsUserExecutorResult_True()
        {
            // Arrange
            var controller = CreateUserController();
            var userDTO = A.Fake<IUserDTO>();

            _fakeUserQueryExecutor.CallsTo(x => x.BlockUser(userDTO)).Returns(true);

            // Act
            var result = await controller.BlockUser(userDTO);

            // Assert
            Assert.IsTrue(result);
        }

        [TestMethod]
        public async Task BlockUser_WithUserScreenName_ReturnsUserExecutorResult_False()
        {
            // Arrange
            var controller = CreateUserController();
            var userScreenName = TestHelper.GenerateString();

            _fakeUserQueryExecutor.CallsTo(x => x.BlockUser(A<IUserIdentifier>.That.Matches(u => u.ScreenName == userScreenName))).Returns(false);

            // Act
            var result = await controller.BlockUser(userScreenName);

            // Assert
            Assert.IsFalse(result);
        }

        [TestMethod]
        public async Task BlockUser_WithUserScreenName_ReturnsUserExecutorResult_True()
        {
            // Arrange
            var controller = CreateUserController();
            var userScreenName = TestHelper.GenerateString();

            _fakeUserQueryExecutor.CallsTo(x => x.BlockUser(A<IUserIdentifier>.That.Matches(u => u.ScreenName == userScreenName))).Returns(true);

            // Act
            var result = await controller.BlockUser(userScreenName);

            // Assert
            Assert.IsTrue(result);
        }

        [TestMethod]
        public async Task BlockUser_WithUserId_ReturnsUserExecutorResult_False()
        {
            // Arrange
            var controller = CreateUserController();
            var userId = TestHelper.GenerateRandomLong();

            _fakeUserQueryExecutor.CallsTo(x => x.BlockUser(A<IUserIdentifier>.That.Matches(u => u.Id == userId))).Returns(false);

            // Act
            var result = await controller.BlockUser(userId);

            // Assert
            Assert.IsFalse(result);
        }

        [TestMethod]
        public async Task BlockUser_WithUserId_ReturnsUserExecutorResult_True()
        {
            // Arrange
            var controller = CreateUserController();
            var userId = TestHelper.GenerateRandomLong();

            _fakeUserQueryExecutor.CallsTo(x => x.BlockUser(A<IUserIdentifier>.That.Matches(u => u.Id == userId))).Returns(true);

            // Act
            var result = await controller.BlockUser(userId);

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
        public async Task ReportUserForSpam_WithUser_ReturnsUserExecutorResult_False()
        {
            // Arrange
            var controller = CreateUserController();
            var userDTO = A.Fake<IUserDTO>();
            var user = TestHelper.GenerateUser(userDTO);

            _fakeUserQueryExecutor.CallsTo(x => x.ReportUserForSpam(userDTO)).Returns(false);

            // Act
            var result = await controller.ReportUserForSpam(user);

            // Assert
            Assert.IsFalse(result);
        }

        [TestMethod]
        public async Task ReportUserForSpam_WithUserDTO_ReturnsUserExecutorResult_False()
        {
            // Arrange
            var controller = CreateUserController();
            var userDTO = A.Fake<IUserDTO>();

            _fakeUserQueryExecutor.CallsTo(x => x.ReportUserForSpam(userDTO)).Returns(false);

            // Act
            var result = await controller.ReportUserForSpam(userDTO);

            // Assert
            Assert.IsFalse(result);
        }

        [TestMethod]
        public async Task ReportUserForSpam_WithUserDTO_ReturnsUserExecutorResult_True()
        {
            // Arrange
            var controller = CreateUserController();
            var userDTO = A.Fake<IUserDTO>();

            _fakeUserQueryExecutor.CallsTo(x => x.ReportUserForSpam(userDTO)).Returns(true);

            // Act
            var result = await controller.ReportUserForSpam(userDTO);

            // Assert
            Assert.IsTrue(result);
        }

        [TestMethod]
        public async Task ReportUserForSpam_WithUserScreenName_ReturnsUserExecutorResult_False()
        {
            // Arrange
            var controller = CreateUserController();
            var userScreenName = TestHelper.GenerateString();

            _fakeUserQueryExecutor.CallsTo(x => x.ReportUserForSpam(A<IUserIdentifier>.That.Matches(u => u.ScreenName == userScreenName))).Returns(false);

            // Act
            var result = await controller.ReportUserForSpam(userScreenName);

            // Assert
            Assert.IsFalse(result);
        }

        [TestMethod]
        public async Task ReportUserForSpam_WithUserScreenName_ReturnsUserExecutorResult_True()
        {
            // Arrange
            var controller = CreateUserController();
            var userScreenName = TestHelper.GenerateString();

            _fakeUserQueryExecutor.CallsTo(x => x.ReportUserForSpam(A<IUserIdentifier>.That.Matches(u => u.ScreenName == userScreenName))).Returns(true);

            // Act
            var result = await controller.ReportUserForSpam(userScreenName);

            // Assert
            Assert.IsTrue(result);
        }

        [TestMethod]
        public async Task ReportUserForSpam_WithUserId_ReturnsUserExecutorResult_False()
        {
            // Arrange
            var controller = CreateUserController();
            var userId = TestHelper.GenerateRandomLong();

            _fakeUserQueryExecutor.CallsTo(x => x.ReportUserForSpam(A<IUserIdentifier>.That.Matches(u => u.Id == userId))).Returns(false);

            // Act
            var result = await controller.ReportUserForSpam(userId);

            // Assert
            Assert.IsFalse(result);
        }

        [TestMethod]
        public async Task ReportUserForSpam_WithUserId_ReturnsUserExecutorResult_True()
        {
            // Arrange
            var controller = CreateUserController();
            var userId = TestHelper.GenerateRandomLong();

            _fakeUserQueryExecutor.CallsTo(x => x.ReportUserForSpam(A<IUserIdentifier>.That.Matches(u => u.Id == userId))).Returns(true);

            // Act
            var result = await controller.ReportUserForSpam(userId);

            // Assert
            Assert.IsTrue(result);
        }

        #endregion

        private UserController CreateUserController()
        {
            return _fakeBuilder.GenerateClass();
        }
    }
}