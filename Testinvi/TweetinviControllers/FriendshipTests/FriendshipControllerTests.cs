using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FakeItEasy;
using FakeItEasy.ExtensionSyntax.Full;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Testinvi.Helpers;
using Tweetinvi.Controllers.Friendship;
using Tweetinvi.Core.Factories;
using Tweetinvi.Models;
using Tweetinvi.Models.DTO;

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
        public async Task GetUserIdsRequestingFriendship_ReturnsQueryExecutorIds()
        {
            var ids = new List<long> { Int64.MaxValue, Int32.MaxValue };

            // Arrange
            var controller = CreateFriendshipController();
            ArrangeGetUserIdsRequestingFriendship(ids);

            // Act
            var result = await controller.GetUserIdsRequestingFriendship();

            // Assert
            Assert.AreEqual(result, ids);
        }

        [TestMethod]
        public async Task GetUsersRequestingFriendship_ReturnsUsersGeneratedFromUserFactory()
        {
            var ids = new List<long> { Int64.MaxValue, Int32.MaxValue };
            var users = new List<IUser> { A.Fake<IUser>() };

            // Arrange
            var controller = CreateFriendshipController();

            ArrangeGetUserIdsRequestingFriendship(ids);
            ArrangeGetUsersFromIds(ids, users);

            // Act
            var result = await controller.GetUsersRequestingFriendship();

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
        public async Task GetUserIdsYouRequestedToFollow_ReturnsQueryExecutorIds()
        {
            var ids = new List<long> { Int64.MaxValue, Int32.MaxValue };

            // Arrange
            var controller = CreateFriendshipController();
            ArrangeGetUserIdsYouRequestedToFollow(ids);

            // Act
            var result = await controller.GetUserIdsYouRequestedToFollow();

            // Assert
            Assert.AreEqual(result, ids);
        }

        [TestMethod]
        public async Task GetUsersYouRequestedToFollow_ReturnsUsersGeneratedFromUserFactory()
        {
            var ids = new List<long> { Int64.MaxValue, Int32.MaxValue };
            var users = new List<IUser> { A.Fake<IUser>() };

            // Arrange
            var controller = CreateFriendshipController();

            ArrangeGetUserIdsYouRequestedToFollow(ids);
            ArrangeGetUsersFromIds(ids, users);

            // Act
            var result = await controller.GetUsersYouRequestedToFollow();

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

        #region Destroy Friendship

        [TestMethod]
        public async Task DestroyFriendship_UserDTOIsNull_ReturnsQueryExecutorResult()
        {
            // Arrange 
            var shouldSuccess = await DestroyFriendship_UserDTO_QueryExecutorReturns(true, true);
            var shouldFail = await DestroyFriendship_UserDTO_QueryExecutorReturns(false, true);

            // Assert
            Assert.IsTrue(shouldSuccess);
            Assert.IsFalse(shouldFail);
        }

        [TestMethod]
        public async Task DestroyFriendship_UserDTO_ReturnsQueryExecutorResult()
        {
            // Arrange - Act
            var shouldSuccess = await DestroyFriendship_UserDTO_QueryExecutorReturns(true);
            var shouldFail = await DestroyFriendship_UserDTO_QueryExecutorReturns(false);

            // Assert
            Assert.IsTrue(shouldSuccess);
            Assert.IsFalse(shouldFail);
        }

        private async Task<bool> DestroyFriendship_UserDTO_QueryExecutorReturns(bool returnValue, bool isNull = false)
        {
            var userDTO = isNull ? null : A.Fake<IUserDTO>();

            // Arrange
            var controller = CreateFriendshipController();
            ArrangeDestroyFriendshipWithDTO(userDTO, returnValue);

            // Act
            return await controller.DestroyFriendshipWith(userDTO);
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
        public async Task UpdateRelationshipAuthorizations_User_ReturnsQueryExecutorResult()
        {
            // Arrange - Act
            var shouldSuccess1 = await UpdateRelationshipAuthorizations_User_QueryExecutorReturns(true, true, true);
            var shouldSuccess2 = await UpdateRelationshipAuthorizations_User_QueryExecutorReturns(true, false, true);
            var shouldSuccess3 = await UpdateRelationshipAuthorizations_User_QueryExecutorReturns(false, true, true);
            var shouldSuccess4 = await UpdateRelationshipAuthorizations_User_QueryExecutorReturns(false, false, true);

            var shouldFail1 = await UpdateRelationshipAuthorizations_User_QueryExecutorReturns(true, true, false);
            var shouldFail2 = await UpdateRelationshipAuthorizations_User_QueryExecutorReturns(true, false, false);
            var shouldFail3 = await UpdateRelationshipAuthorizations_User_QueryExecutorReturns(false, true, false);
            var shouldFail4 = await UpdateRelationshipAuthorizations_User_QueryExecutorReturns(false, false, false);

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

        private async Task<bool> UpdateRelationshipAuthorizations_User_QueryExecutorReturns(bool retweetsEnabled, bool notification, bool returnValue)
        {
            var fakeUser = A.Fake<IUser>();
            fakeUser.CallsTo(x => x.UserDTO).Returns(A.Fake<IUserDTO>());

            // Arrange
            var controller = CreateFriendshipController();
            ArrangeUpdateRelationshipAuthorizationsWith(fakeUser, retweetsEnabled, notification, returnValue);

            // Act
            var result = await controller.UpdateRelationshipAuthorizationsWith(fakeUser, retweetsEnabled, notification);
            VerifyUpdateRelationshipAuthorizationsWith(fakeUser, retweetsEnabled, notification);

            return result;
        }
        
        // UserDTO
        [TestMethod]
        public async Task UpdateRelationshipAuthorizations_UserDTOIsNull_ReturnsTrue()
        {
            // Arrange 
            var shouldSuccess1 = await UpdateRelationshipAuthorizations_UserDTO_QueryExecutorReturns(true, true, true, true);
            var shouldSuccess2 = await UpdateRelationshipAuthorizations_UserDTO_QueryExecutorReturns(true, false, true, true);
            var shouldSuccess3 = await UpdateRelationshipAuthorizations_UserDTO_QueryExecutorReturns(false, true, true, true);
            var shouldSuccess4 = await UpdateRelationshipAuthorizations_UserDTO_QueryExecutorReturns(false, false, true, true);

            // Assert
            Assert.IsTrue(shouldSuccess1);
            Assert.IsTrue(shouldSuccess2);
            Assert.IsTrue(shouldSuccess3);
            Assert.IsTrue(shouldSuccess4);
        }

        [TestMethod]
        public async Task UpdateRelationshipAuthorizations_UserDTOIsNull_ReturnsFalse()
        {
            // Arrange 
            var shouldFail1 = await UpdateRelationshipAuthorizations_UserDTO_QueryExecutorReturns(true, true, false, true);
            var shouldFail2 = await UpdateRelationshipAuthorizations_UserDTO_QueryExecutorReturns(true, false, false, true);
            var shouldFail3 = await UpdateRelationshipAuthorizations_UserDTO_QueryExecutorReturns(false, true, false, true);
            var shouldFail4 = await UpdateRelationshipAuthorizations_UserDTO_QueryExecutorReturns(false, false, false, true);

            // Assert
            Assert.IsFalse(shouldFail1);
            Assert.IsFalse(shouldFail2);
            Assert.IsFalse(shouldFail3);
            Assert.IsFalse(shouldFail4);
        }

        [TestMethod]
        public async Task UpdateRelationshipAuthorizations_UserDTO_ReturnsTrue()
        {
            // Arrange - Act
            var shouldSuccess1 = await UpdateRelationshipAuthorizations_UserDTO_QueryExecutorReturns(true, true, true);
            var shouldSuccess2 = await UpdateRelationshipAuthorizations_UserDTO_QueryExecutorReturns(true, false, true);
            var shouldSuccess3 = await UpdateRelationshipAuthorizations_UserDTO_QueryExecutorReturns(false, true, true);
            var shouldSuccess4 = await UpdateRelationshipAuthorizations_UserDTO_QueryExecutorReturns(false, false, true);

            // Assert
            Assert.IsTrue(shouldSuccess1);
            Assert.IsTrue(shouldSuccess2);
            Assert.IsTrue(shouldSuccess3);
            Assert.IsTrue(shouldSuccess4);
        }

        [TestMethod]
        public async Task UpdateRelationshipAuthorizations_UserDTO_ReturnsFalse()
        {
            // Arrange - Act
            var shouldFail1 = await UpdateRelationshipAuthorizations_UserDTO_QueryExecutorReturns(true, true, false);
            var shouldFail2 = await UpdateRelationshipAuthorizations_UserDTO_QueryExecutorReturns(true, false, false);
            var shouldFail3 = await UpdateRelationshipAuthorizations_UserDTO_QueryExecutorReturns(false, true, false);
            var shouldFail4 = await UpdateRelationshipAuthorizations_UserDTO_QueryExecutorReturns(false, false, false);

            // Assert
            Assert.IsFalse(shouldFail1);
            Assert.IsFalse(shouldFail2);
            Assert.IsFalse(shouldFail3);
            Assert.IsFalse(shouldFail4);
        }

        private async Task<bool> UpdateRelationshipAuthorizations_UserDTO_QueryExecutorReturns(bool retweetsEnabled, bool notification, bool returnValue, bool isNull = false)
        {
            var user = isNull ? null : A.Fake<IUser>();

            // Arrange
            var controller = CreateFriendshipController();
            ArrangeUpdateRelationshipAuthorizationsWith(user, retweetsEnabled, notification, returnValue);

            // Act
            var result = await controller.UpdateRelationshipAuthorizationsWith(user, retweetsEnabled, notification);
            
            if (!isNull)
            {
                VerifyUpdateRelationshipAuthorizationsWith(user, retweetsEnabled, notification);
            }

            return result;
        }

        private void ArrangeUpdateRelationshipAuthorizationsWith(IUser user, bool retweetsEnabled, bool notification, bool returnValue)
        {
            _fakeFriendshipQueryExecutor
                .CallsTo(x => x.UpdateRelationshipAuthorizationsWith(user,
                    A<IFriendshipAuthorizations>.That.Matches(a => a.RetweetsEnabled == retweetsEnabled &&
                                                                   a.DeviceNotificationEnabled == notification)))
                .Returns(returnValue);
        }

        private void VerifyUpdateRelationshipAuthorizationsWith(IUser user, bool retweetsEnabled, bool notification)
        {
            _fakeFriendshipQueryExecutor
                .CallsTo(x => x.UpdateRelationshipAuthorizationsWith(user,
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

        private FriendshipController CreateFriendshipController()
        {
            return _fakeBuilder.GenerateClass();
        }
    }
}