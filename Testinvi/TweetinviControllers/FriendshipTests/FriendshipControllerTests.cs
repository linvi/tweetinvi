using System;
using System.Collections.Generic;
using FakeItEasy;
using FakeItEasy.ExtensionSyntax.Full;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Testinvi.Helpers;
using Tweetinvi.Controllers.Friendship;
using Tweetinvi.Core.Interfaces;
using Tweetinvi.Core.Interfaces.DTO;
using Tweetinvi.Core.Interfaces.Factories;
using Tweetinvi.Core.Interfaces.Models;

namespace Testinvi.TweetinviControllers.FriendshipTests
{
    [TestClass]
    public class FriendshipControllerTests
    {
        private FakeClassBuilder<FriendshipController> _fakeBuilder;
        private Fake<IFriendshipQueryExecutor> _fakeFriendshipQueryExecutor;
        private Fake<IFriendshipFactory> _fakeFriendshipFactory;
        private Fake<IUserFactory> _fakeUserFactory;

        [TestInitialize]
        public void TestInitialize()
        {
            _fakeBuilder = new FakeClassBuilder<FriendshipController>();
            _fakeFriendshipQueryExecutor = _fakeBuilder.GetFake<IFriendshipQueryExecutor>();
            _fakeUserFactory = _fakeBuilder.GetFake<IUserFactory>();
            _fakeFriendshipFactory = _fakeBuilder.GetFake<IFriendshipFactory>();

            _fakeFriendshipFactory
                .CallsTo(x => x.GenerateFriendshipAuthorizations(A<bool>.Ignored, A<bool>.Ignored))
                .ReturnsLazily((bool retweets, bool notification) =>
                {
                    var fakeAuthorization = A.Fake<IFriendshipAuthorizations>();
                    fakeAuthorization.RetweetsEnabled = retweets;
                    fakeAuthorization.DeviceNotificationEnabled = notification;
                    return fakeAuthorization;
                });
        }

        #region GetUsers Requesting Friendship
        [TestMethod]
        public void GetUserIdsRequestingFriendship_ReturnsQueryExecutorIds()
        {
            var ids = new List<long> { Int64.MaxValue, Int32.MaxValue };

            // Arrange
            var controller = CreateFriendshipController();
            ArrangeGetUserIdsRequestingFriendship(ids);

            // Act
            var result = controller.GetUserIdsRequestingFriendship();

            // Assert
            Assert.AreEqual(result, ids);
        }

        [TestMethod]
        public void GetUsersRequestingFriendship_ReturnsUsersGeneratedFromUserFactory()
        {
            var ids = new List<long> { Int64.MaxValue, Int32.MaxValue };
            var users = new List<IUser> { A.Fake<IUser>() };

            // Arrange
            var controller = CreateFriendshipController();

            ArrangeGetUserIdsRequestingFriendship(ids);
            ArrangeGetUsersFromIds(ids, users);

            // Act
            var result = controller.GetUsersRequestingFriendship();

            // Assert
            Assert.AreEqual(result, users);
        }

        private void ArrangeGetUserIdsRequestingFriendship(IEnumerable<long> userIds)
        {
            _fakeFriendshipQueryExecutor
                .CallsTo(x => x.GetUserIdsRequestingFriendship(It.IsAny<int>()))
                .Returns(userIds);
        }
        #endregion

        #region GetUsers you requested to follow
        [TestMethod]
        public void GetUserIdsYouRequestedToFollow_ReturnsQueryExecutorIds()
        {
            var ids = new List<long> { Int64.MaxValue, Int32.MaxValue };

            // Arrange
            var controller = CreateFriendshipController();
            ArrangeGetUserIdsYouRequestedToFollow(ids);

            // Act
            var result = controller.GetUserIdsYouRequestedToFollow();

            // Assert
            Assert.AreEqual(result, ids);
        }

        [TestMethod]
        public void GetUsersYouRequestedToFollow_ReturnsUsersGeneratedFromUserFactory()
        {
            var ids = new List<long> { Int64.MaxValue, Int32.MaxValue };
            var users = new List<IUser> { A.Fake<IUser>() };

            // Arrange
            var controller = CreateFriendshipController();

            ArrangeGetUserIdsYouRequestedToFollow(ids);
            ArrangeGetUsersFromIds(ids, users);

            // Act
            var result = controller.GetUsersYouRequestedToFollow();

            // Assert
            Assert.AreEqual(result, users);
        }

        private void ArrangeGetUserIdsYouRequestedToFollow(IEnumerable<long> userIds)
        {
            _fakeFriendshipQueryExecutor
                .CallsTo(x => x.GetUserIdsYouRequestedToFollow(It.IsAny<int>()))
                .Returns(userIds);
        }

        #endregion

        #region Create Friendship With

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void CreateFriendship_UserIsNull_ThrowsArgumentException()
        {
            // Arrange 
            var controller = CreateFriendshipController();

            // Act
            controller.CreateFriendshipWith((IUser)null);
        }

        [TestMethod]
        public void CreateFriendship_User_ReturnsQueryExecutorResult()
        {
            // Arrange - Act
            var shouldSuccess = CreateFriendship_User_QueryExecutorReturns(true);
            var shouldFail = CreateFriendship_User_QueryExecutorReturns(false);

            // Assert
            Assert.IsTrue(shouldSuccess);
            Assert.IsFalse(shouldFail);
        }

        private bool CreateFriendship_User_QueryExecutorReturns(bool returnValue)
        {
            var fakeUser = A.Fake<IUser>();
            fakeUser.CallsTo(x => x.UserDTO).Returns(A.Fake<IUserDTO>());

            // Arrange
            var controller = CreateFriendshipController();
            ArrangeCreateFriendshipWithDTO(fakeUser.UserDTO, returnValue);

            // Act
            return controller.CreateFriendshipWith(fakeUser);
        }

        [TestMethod]
        public void CreateFriendship_UserDTOIsNull_ReturnsQueryExecutorResult()
        {
            // Arrange 
            var shouldSuccess = CreateFriendship_UserDTO_QueryExecutorReturns(true, true);
            var shouldFail = CreateFriendship_UserDTO_QueryExecutorReturns(false, true);

            // Assert
            Assert.IsTrue(shouldSuccess);
            Assert.IsFalse(shouldFail);
        }

        [TestMethod]
        public void CreateFriendship_UserDTO_ReturnsQueryExecutorResult()
        {
            // Arrange - Act
            var shouldSuccess = CreateFriendship_UserDTO_QueryExecutorReturns(true);
            var shouldFail = CreateFriendship_UserDTO_QueryExecutorReturns(false);

            // Assert
            Assert.IsTrue(shouldSuccess);
            Assert.IsFalse(shouldFail);
        }

        private bool CreateFriendship_UserDTO_QueryExecutorReturns(bool returnValue, bool isNull = false)
        {
            var userDTO = isNull ? null : A.Fake<IUserDTO>();

            // Arrange
            var controller = CreateFriendshipController();
            ArrangeCreateFriendshipWithDTO(userDTO, returnValue);

            // Act
            return controller.CreateFriendshipWith(userDTO);
        }

        [TestMethod]
        public void CreateFriendship_UserScreenNameIsNull_ReturnsQueryExecutorResult()
        {
            // Arrange - Act
            var shouldSuccess = CreateFriendship_UserScreeName_QueryExecutorReturns(true, true);
            var shouldFail = CreateFriendship_UserScreeName_QueryExecutorReturns(false, true);

            // Assert
            Assert.IsTrue(shouldSuccess);
            Assert.IsFalse(shouldFail);
        }

        [TestMethod]
        public void CreateFriendship_UserScreenName_ReturnsQueryExecutorResult()
        {
            // Arrange - Act
            var shouldSuccess = CreateFriendship_UserScreeName_QueryExecutorReturns(true, false);
            var shouldFail = CreateFriendship_UserScreeName_QueryExecutorReturns(false, false);

            // Assert
            Assert.IsTrue(shouldSuccess);
            Assert.IsFalse(shouldFail);
        }

        private bool CreateFriendship_UserScreeName_QueryExecutorReturns(bool returnValue, bool isNull)
        {
            var screenName = isNull ? null : Guid.NewGuid().ToString();

            // Arrange
            var controller = CreateFriendshipController();
            ArrangeCreateFriendshipWithScreenName(screenName, returnValue);

            // Act
            return controller.CreateFriendshipWith(screenName);
        }

        [TestMethod]
        public void CreateFriendship_UserId_ReturnsQueryExecutorResult()
        {
            // Arrange - Act
            var shouldSuccess = CreateFriendship_UserId_QueryExecutorReturns(true);
            var shouldFail = CreateFriendship_UserId_QueryExecutorReturns(false);

            // Assert
            Assert.IsTrue(shouldSuccess);
            Assert.IsFalse(shouldFail);
        }

        private bool CreateFriendship_UserId_QueryExecutorReturns(bool returnValue)
        {
            var id = TestHelper.GenerateRandomLong();

            // Arrange
            var controller = CreateFriendshipController();
            ArrangeCreateFriendshipWithId(id, returnValue);

            // Act
            return controller.CreateFriendshipWith(id);
        }

        private void ArrangeCreateFriendshipWithId(long userId, bool returnValue)
        {
            _fakeFriendshipQueryExecutor
                .CallsTo(x => x.CreateFriendshipWith(userId))
                .Returns(returnValue);
        }

        private void ArrangeCreateFriendshipWithScreenName(string userScreeName, bool returnValue)
        {
            _fakeFriendshipQueryExecutor
                .CallsTo(x => x.CreateFriendshipWith(userScreeName))
                .Returns(returnValue);
        }

        private void ArrangeCreateFriendshipWithDTO(IUserDTO userDTO, bool returnValue)
        {
            _fakeFriendshipQueryExecutor
                .CallsTo(x => x.CreateFriendshipWith(userDTO))
                .Returns(returnValue);
        }

        #endregion

        #region Destroy Friendship

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void DestroyFriendship_UserIsNull_ThrowsArgumentException()
        {
            // Arrange 
            var controller = CreateFriendshipController();

            // Act
            controller.DestroyFriendshipWith((IUser)null);
        }

        [TestMethod]
        public void DestroyFriendship_User_ReturnsQueryExecutorResult()
        {
            // Arrange - Act
            var shouldSuccess = DestroyFriendship_User_QueryExecutorReturns(true);
            var shouldFail = DestroyFriendship_User_QueryExecutorReturns(false);

            // Assert
            Assert.IsTrue(shouldSuccess);
            Assert.IsFalse(shouldFail);
        }

        private bool DestroyFriendship_User_QueryExecutorReturns(bool returnValue)
        {
            var fakeUser = A.Fake<IUser>();
            fakeUser.CallsTo(x => x.UserDTO).Returns(A.Fake<IUserDTO>());

            // Arrange
            var controller = CreateFriendshipController();
            ArrangeDestroyFriendshipWithDTO(fakeUser.UserDTO, returnValue);

            // Act
            return controller.DestroyFriendshipWith(fakeUser);
        }

        [TestMethod]
        public void DestroyFriendship_UserDTOIsNull_ReturnsQueryExecutorResult()
        {
            // Arrange 
            var shouldSuccess = DestroyFriendship_UserDTO_QueryExecutorReturns(true, true);
            var shouldFail = DestroyFriendship_UserDTO_QueryExecutorReturns(false, true);

            // Assert
            Assert.IsTrue(shouldSuccess);
            Assert.IsFalse(shouldFail);
        }

        [TestMethod]
        public void DestroyFriendship_UserDTO_ReturnsQueryExecutorResult()
        {
            // Arrange - Act
            var shouldSuccess = DestroyFriendship_UserDTO_QueryExecutorReturns(true);
            var shouldFail = DestroyFriendship_UserDTO_QueryExecutorReturns(false);

            // Assert
            Assert.IsTrue(shouldSuccess);
            Assert.IsFalse(shouldFail);
        }

        private bool DestroyFriendship_UserDTO_QueryExecutorReturns(bool returnValue, bool isNull = false)
        {
            var userDTO = isNull ? null : A.Fake<IUserDTO>();

            // Arrange
            var controller = CreateFriendshipController();
            ArrangeDestroyFriendshipWithDTO(userDTO, returnValue);

            // Act
            return controller.DestroyFriendshipWith(userDTO);
        }

        [TestMethod]
        public void DestroyFriendship_UserScreenNameIsNull_ReturnsQueryExecutorResult()
        {
            // Arrange - Act
            var shouldSuccess = DestroyFriendship_UserScreeName_QueryExecutorReturns(true, true);
            var shouldFail = DestroyFriendship_UserScreeName_QueryExecutorReturns(false, true);

            // Assert
            Assert.IsTrue(shouldSuccess);
            Assert.IsFalse(shouldFail);
        }

        [TestMethod]
        public void DestroyFriendship_UserScreenName_ReturnsQueryExecutorResult()
        {
            // Arrange - Act
            var shouldSuccess = DestroyFriendship_UserScreeName_QueryExecutorReturns(true, false);
            var shouldFail = DestroyFriendship_UserScreeName_QueryExecutorReturns(false, false);

            // Assert
            Assert.IsTrue(shouldSuccess);
            Assert.IsFalse(shouldFail);
        }

        private bool DestroyFriendship_UserScreeName_QueryExecutorReturns(bool returnValue, bool isNull)
        {
            var screenName = isNull ? null : Guid.NewGuid().ToString();

            // Arrange
            var controller = CreateFriendshipController();
            ArrangeDestroyFriendshipWithScreenName(screenName, returnValue);

            // Act
            return controller.DestroyFriendshipWith(screenName);
        }

        [TestMethod]
        public void DestroyFriendship_UserId_ReturnsQueryExecutorResult()
        {
            // Arrange - Act
            var shouldSuccess = DestroyFriendship_UserId_QueryExecutorReturns(true);
            var shouldFail = DestroyFriendship_UserId_QueryExecutorReturns(false);

            // Assert
            Assert.IsTrue(shouldSuccess);
            Assert.IsFalse(shouldFail);
        }

        private bool DestroyFriendship_UserId_QueryExecutorReturns(bool returnValue)
        {
            var id = TestHelper.GenerateRandomLong();

            // Arrange
            var controller = CreateFriendshipController();
            ArrangeDestroyFriendshipWithId(id, returnValue);

            // Act
            return controller.DestroyFriendshipWith(id);
        }

        private void ArrangeDestroyFriendshipWithId(long userId, bool returnValue)
        {
            _fakeFriendshipQueryExecutor
                .CallsTo(x => x.DestroyFriendshipWith(userId))
                .Returns(returnValue);
        }

        private void ArrangeDestroyFriendshipWithScreenName(string userScreeName, bool returnValue)
        {
            _fakeFriendshipQueryExecutor
                .CallsTo(x => x.DestroyFriendshipWith(userScreeName))
                .Returns(returnValue);
        }

        private void ArrangeDestroyFriendshipWithDTO(IUserDTO userDTO, bool returnValue)
        {
            _fakeFriendshipQueryExecutor
                .CallsTo(x => x.DestroyFriendshipWith(userDTO))
                .Returns(returnValue);
        }

        #endregion

        #region Update Friendship Authorizations

        // User
        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void UpdateFriendshipAuthorizations_UserIsNull_ThrowArgumentException()
        {
            // Arrange 
            var controller = CreateFriendshipController();

            // Act
            controller.UpdateRelationshipAuthorizationsWith((IUser)null, true, false);
        }

        [TestMethod]
        public void UpdateRelationshipAuthorizations_User_ReturnsQueryExecutorResult()
        {
            // Arrange - Act
            var shouldSuccess1 = UpdateRelationshipAuthorizations_User_QueryExecutorReturns(true, true, true);
            var shouldSuccess2 = UpdateRelationshipAuthorizations_User_QueryExecutorReturns(true, false, true);
            var shouldSuccess3 = UpdateRelationshipAuthorizations_User_QueryExecutorReturns(false, true, true);
            var shouldSuccess4 = UpdateRelationshipAuthorizations_User_QueryExecutorReturns(false, false, true);

            var shouldFail1 = UpdateRelationshipAuthorizations_User_QueryExecutorReturns(true, true, false);
            var shouldFail2 = UpdateRelationshipAuthorizations_User_QueryExecutorReturns(true, false, false);
            var shouldFail3 = UpdateRelationshipAuthorizations_User_QueryExecutorReturns(false, true, false);
            var shouldFail4 = UpdateRelationshipAuthorizations_User_QueryExecutorReturns(false, false, false);

            // Assert
            Assert.IsTrue(shouldSuccess1);
            Assert.IsTrue(shouldSuccess2);
            Assert.IsTrue(shouldSuccess3);
            Assert.IsTrue(shouldSuccess4);

            Assert.IsFalse(shouldFail1);
            Assert.IsFalse(shouldFail2);
            Assert.IsFalse(shouldFail3);
            Assert.IsFalse(shouldFail4);
        }

        private bool UpdateRelationshipAuthorizations_User_QueryExecutorReturns(bool retweetsEnabled, bool notification, bool returnValue)
        {
            var fakeUser = A.Fake<IUser>();
            fakeUser.CallsTo(x => x.UserDTO).Returns(A.Fake<IUserDTO>());

            // Arrange
            var controller = CreateFriendshipController();
            ArrangeUpdateRelationshipAuthorizationsWithDTO(fakeUser.UserDTO, retweetsEnabled, notification, returnValue);

            // Act
            var result = controller.UpdateRelationshipAuthorizationsWith(fakeUser, retweetsEnabled, notification);
            VerifyUpdateRelationshipAuthorizationsWithDTO(fakeUser.UserDTO, retweetsEnabled, notification);

            return result;
        }
        
        // UserDTO
        [TestMethod]
        public void UpdateRelationshipAuthorizations_UserDTOIsNull_ReturnsTrue()
        {
            // Arrange 
            var shouldSuccess1 = UpdateRelationshipAuthorizations_UserDTO_QueryExecutorReturns(true, true, true, true);
            var shouldSuccess2 = UpdateRelationshipAuthorizations_UserDTO_QueryExecutorReturns(true, false, true, true);
            var shouldSuccess3 = UpdateRelationshipAuthorizations_UserDTO_QueryExecutorReturns(false, true, true, true);
            var shouldSuccess4 = UpdateRelationshipAuthorizations_UserDTO_QueryExecutorReturns(false, false, true, true);

            // Assert
            Assert.IsTrue(shouldSuccess1);
            Assert.IsTrue(shouldSuccess2);
            Assert.IsTrue(shouldSuccess3);
            Assert.IsTrue(shouldSuccess4);
        }

        [TestMethod]
        public void UpdateRelationshipAuthorizations_UserDTOIsNull_ReturnsFalse()
        {
            // Arrange 
            var shouldFail1 = UpdateRelationshipAuthorizations_UserDTO_QueryExecutorReturns(true, true, false, true);
            var shouldFail2 = UpdateRelationshipAuthorizations_UserDTO_QueryExecutorReturns(true, false, false, true);
            var shouldFail3 = UpdateRelationshipAuthorizations_UserDTO_QueryExecutorReturns(false, true, false, true);
            var shouldFail4 = UpdateRelationshipAuthorizations_UserDTO_QueryExecutorReturns(false, false, false, true);

            // Assert
            Assert.IsFalse(shouldFail1);
            Assert.IsFalse(shouldFail2);
            Assert.IsFalse(shouldFail3);
            Assert.IsFalse(shouldFail4);
        }

        [TestMethod]
        public void UpdateRelationshipAuthorizations_UserDTO_ReturnsTrue()
        {
            // Arrange - Act
            var shouldSuccess1 = UpdateRelationshipAuthorizations_UserDTO_QueryExecutorReturns(true, true, true);
            var shouldSuccess2 = UpdateRelationshipAuthorizations_UserDTO_QueryExecutorReturns(true, false, true);
            var shouldSuccess3 = UpdateRelationshipAuthorizations_UserDTO_QueryExecutorReturns(false, true, true);
            var shouldSuccess4 = UpdateRelationshipAuthorizations_UserDTO_QueryExecutorReturns(false, false, true);

            // Assert
            Assert.IsTrue(shouldSuccess1);
            Assert.IsTrue(shouldSuccess2);
            Assert.IsTrue(shouldSuccess3);
            Assert.IsTrue(shouldSuccess4);
        }

        [TestMethod]
        public void UpdateRelationshipAuthorizations_UserDTO_ReturnsFalse()
        {
            // Arrange - Act
            var shouldFail1 = UpdateRelationshipAuthorizations_UserDTO_QueryExecutorReturns(true, true, false);
            var shouldFail2 = UpdateRelationshipAuthorizations_UserDTO_QueryExecutorReturns(true, false, false);
            var shouldFail3 = UpdateRelationshipAuthorizations_UserDTO_QueryExecutorReturns(false, true, false);
            var shouldFail4 = UpdateRelationshipAuthorizations_UserDTO_QueryExecutorReturns(false, false, false);

            // Assert
            Assert.IsFalse(shouldFail1);
            Assert.IsFalse(shouldFail2);
            Assert.IsFalse(shouldFail3);
            Assert.IsFalse(shouldFail4);
        }

        private bool UpdateRelationshipAuthorizations_UserDTO_QueryExecutorReturns(bool retweetsEnabled, bool notification, bool returnValue, bool isNull = false)
        {
            var userDTO = isNull ? null : A.Fake<IUserDTO>();

            // Arrange
            var controller = CreateFriendshipController();
            ArrangeUpdateRelationshipAuthorizationsWithDTO(userDTO, retweetsEnabled, notification, returnValue);

            // Act
            var result = controller.UpdateRelationshipAuthorizationsWith(userDTO, retweetsEnabled, notification);
            
            if (!isNull)
            {
                VerifyUpdateRelationshipAuthorizationsWithDTO(userDTO, retweetsEnabled, notification);
            }

            return result;
        }

        // User ScreenName
        [TestMethod]
        public void UpdateRelationshipAuthorizations_UserScreeNameIsNull_ReturnsTrue()
        {
            // Arrange 
            var shouldSuccess1 = UpdateRelationshipAuthorizations_UserScreenName_QueryExecutorReturns(true, true, true, true);
            var shouldSuccess2 = UpdateRelationshipAuthorizations_UserScreenName_QueryExecutorReturns(true, false, true, true);
            var shouldSuccess3 = UpdateRelationshipAuthorizations_UserScreenName_QueryExecutorReturns(false, true, true, true);
            var shouldSuccess4 = UpdateRelationshipAuthorizations_UserScreenName_QueryExecutorReturns(false, false, true, true);

            // Assert
            Assert.IsTrue(shouldSuccess1);
            Assert.IsTrue(shouldSuccess2);
            Assert.IsTrue(shouldSuccess3);
            Assert.IsTrue(shouldSuccess4);

        }

        [TestMethod]
        public void UpdateRelationshipAuthorizations_UserScreeNameIsNull_ReturnsFalse()
        {
            // Arrange 
            var shouldFail1 = UpdateRelationshipAuthorizations_UserScreenName_QueryExecutorReturns(true, true, false, true);
            var shouldFail2 = UpdateRelationshipAuthorizations_UserScreenName_QueryExecutorReturns(true, false, false, true);
            var shouldFail3 = UpdateRelationshipAuthorizations_UserScreenName_QueryExecutorReturns(false, true, false, true);
            var shouldFail4 = UpdateRelationshipAuthorizations_UserScreenName_QueryExecutorReturns(false, false, false, true);

            // Assert
            Assert.IsFalse(shouldFail1);
            Assert.IsFalse(shouldFail2);
            Assert.IsFalse(shouldFail3);
            Assert.IsFalse(shouldFail4);
        }

        [TestMethod]
        public void UpdateRelationshipAuthorizations_UserScreenName_ReturnsTrue()
        {
            // Arrange - Act
            var shouldSuccess1 = UpdateRelationshipAuthorizations_UserScreenName_QueryExecutorReturns(true, true, true);
            var shouldSuccess2 = UpdateRelationshipAuthorizations_UserScreenName_QueryExecutorReturns(true, false, true);
            var shouldSuccess3 = UpdateRelationshipAuthorizations_UserScreenName_QueryExecutorReturns(false, true, true);
            var shouldSuccess4 = UpdateRelationshipAuthorizations_UserScreenName_QueryExecutorReturns(false, false, true);

            // Assert
            Assert.IsTrue(shouldSuccess1);
            Assert.IsTrue(shouldSuccess2);
            Assert.IsTrue(shouldSuccess3);
            Assert.IsTrue(shouldSuccess4);
        }

        [TestMethod]
        public void UpdateRelationshipAuthorizations_UserScreenName_ReturnsFalse()
        {
            // Arrange - Act
            var shouldFail1 = UpdateRelationshipAuthorizations_UserScreenName_QueryExecutorReturns(true, true, false);
            var shouldFail2 = UpdateRelationshipAuthorizations_UserScreenName_QueryExecutorReturns(true, false, false);
            var shouldFail3 = UpdateRelationshipAuthorizations_UserScreenName_QueryExecutorReturns(false, true, false);
            var shouldFail4 = UpdateRelationshipAuthorizations_UserScreenName_QueryExecutorReturns(false, false, false);

            // Assert
            Assert.IsFalse(shouldFail1);
            Assert.IsFalse(shouldFail2);
            Assert.IsFalse(shouldFail3);
            Assert.IsFalse(shouldFail4);
        }

        private bool UpdateRelationshipAuthorizations_UserScreenName_QueryExecutorReturns(bool retweetsEnabled, bool notification, bool returnValue, bool isNull = false)
        {
            var userScreenName = isNull ? null : Guid.NewGuid().ToString();

            // Arrange
            var controller = CreateFriendshipController();
            ArrangeUpdateRelationshipAuthorizationsWithScreenName(userScreenName, retweetsEnabled, notification, returnValue);

            // Act
            var result = controller.UpdateRelationshipAuthorizationsWith(userScreenName, retweetsEnabled, notification);

            if (!isNull)
            {
                VerifyUpdateRelationshipAuthorizationsWithScreenName(userScreenName, retweetsEnabled, notification);
            }

            return result;
        }

        // User Id
        [TestMethod]
        public void UpdateRelationshipAuthorizations_UserId_ReturnsTrue()
        {
            // Arrange - Act
            var shouldSuccess1 = UpdateRelationshipAuthorizations_UserId_QueryExecutorReturns(true, true, true);
            var shouldSuccess2 = UpdateRelationshipAuthorizations_UserId_QueryExecutorReturns(true, false, true);
            var shouldSuccess3 = UpdateRelationshipAuthorizations_UserId_QueryExecutorReturns(false, true, true);
            var shouldSuccess4 = UpdateRelationshipAuthorizations_UserId_QueryExecutorReturns(false, false, true);

            // Assert
            Assert.IsTrue(shouldSuccess1);
            Assert.IsTrue(shouldSuccess2);
            Assert.IsTrue(shouldSuccess3);
            Assert.IsTrue(shouldSuccess4);
        }

        [TestMethod]
        public void UpdateRelationshipAuthorizations_UserId_ReturnsFalse()
        {
            // Arrange - Act
            var shouldFail1 = UpdateRelationshipAuthorizations_UserId_QueryExecutorReturns(true, true, false);
            var shouldFail2 = UpdateRelationshipAuthorizations_UserId_QueryExecutorReturns(true, false, false);
            var shouldFail3 = UpdateRelationshipAuthorizations_UserId_QueryExecutorReturns(false, true, false);
            var shouldFail4 = UpdateRelationshipAuthorizations_UserId_QueryExecutorReturns(false, false, false);

            // Assert
            Assert.IsFalse(shouldFail1);
            Assert.IsFalse(shouldFail2);
            Assert.IsFalse(shouldFail3);
            Assert.IsFalse(shouldFail4);
        }

        private bool UpdateRelationshipAuthorizations_UserId_QueryExecutorReturns(bool retweetsEnabled, bool notification, bool returnValue)
        {
            var userId = TestHelper.GenerateRandomLong();

            // Arrange
            var controller = CreateFriendshipController();
            ArrangeUpdateRelationshipAuthorizationsWithId(userId, retweetsEnabled, notification, returnValue);

            // Act
            var result = controller.UpdateRelationshipAuthorizationsWith(userId, retweetsEnabled, notification);

            VerifyUpdateRelationshipAuthorizationsWithId(userId, retweetsEnabled, notification);

            return result;
        }

        private void ArrangeUpdateRelationshipAuthorizationsWithDTO(IUserDTO userDTO, bool retweetsEnabled, bool notification, bool returnValue)
        {
            _fakeFriendshipQueryExecutor
                .CallsTo(x => x.UpdateRelationshipAuthorizationsWith(userDTO,
                    A<IFriendshipAuthorizations>.That.Matches(a => a.RetweetsEnabled == retweetsEnabled &&
                                                                   a.DeviceNotificationEnabled == notification)))
                .Returns(returnValue);
        }

        private void ArrangeUpdateRelationshipAuthorizationsWithScreenName(string userScreeName, bool retweetsEnabled, bool notification, bool returnValue)
        {
            _fakeFriendshipQueryExecutor
                .CallsTo(x => x.UpdateRelationshipAuthorizationsWith(userScreeName,
                    A<IFriendshipAuthorizations>.That.Matches(a => a.RetweetsEnabled == retweetsEnabled &&
                                                                   a.DeviceNotificationEnabled == notification)))
                .Returns(returnValue);
        }

        private void ArrangeUpdateRelationshipAuthorizationsWithId(long userId, bool retweetsEnabled, bool notification, bool returnValue)
        {
            _fakeFriendshipQueryExecutor
                .CallsTo(x => x.UpdateRelationshipAuthorizationsWith(userId, 
                    A<IFriendshipAuthorizations>.That.Matches(a => a.RetweetsEnabled == retweetsEnabled &&
                                                                   a.DeviceNotificationEnabled == notification)))
                .Returns(returnValue);
        }

        private void VerifyUpdateRelationshipAuthorizationsWithDTO(IUserDTO userDTO, bool retweetsEnabled, bool notification)
        {
            _fakeFriendshipQueryExecutor
                .CallsTo(x => x.UpdateRelationshipAuthorizationsWith(userDTO,
                    A<IFriendshipAuthorizations>.That.Matches(a => a.RetweetsEnabled == retweetsEnabled &&
                                                                   a.DeviceNotificationEnabled == notification)))
                .MustHaveHappened(Repeated.Exactly.Once);
        }

        private void VerifyUpdateRelationshipAuthorizationsWithScreenName(string screenName, bool retweetsEnabled, bool notification)
        {
            _fakeFriendshipQueryExecutor
                .CallsTo(x => x.UpdateRelationshipAuthorizationsWith(screenName,
                    A<IFriendshipAuthorizations>.That.Matches(a => a.RetweetsEnabled == retweetsEnabled &&
                                                                   a.DeviceNotificationEnabled == notification)))
                .MustHaveHappened(Repeated.Exactly.Once);
        }

        private void VerifyUpdateRelationshipAuthorizationsWithId(long userId, bool retweetsEnabled, bool notification)
        {
            _fakeFriendshipQueryExecutor
                .CallsTo(x => x.UpdateRelationshipAuthorizationsWith(userId,
                    A<IFriendshipAuthorizations>.That.Matches(a => a.RetweetsEnabled == retweetsEnabled &&
                                                                   a.DeviceNotificationEnabled == notification)))
                .MustHaveHappened(Repeated.Exactly.Once);
        }

        #endregion

        private void ArrangeGetUsersFromIds(IEnumerable<long> userIds, IEnumerable<IUser> users)
        {
            _fakeUserFactory
                .CallsTo(x => x.GetUsersFromIds(userIds))
                .Returns(users);
        }

        public FriendshipController CreateFriendshipController()
        {
            return _fakeBuilder.GenerateClass();
        }
    }
}