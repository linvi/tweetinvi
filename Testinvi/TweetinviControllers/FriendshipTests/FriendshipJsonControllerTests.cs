using System;
using System.Collections.Generic;
using FakeItEasy;
using FakeItEasy.ExtensionSyntax.Full;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Testinvi.Helpers;
using Testinvi.SetupHelpers;
using Tweetinvi.Controllers.Friendship;
using Tweetinvi.Core.Factories;
using Tweetinvi.Core.QueryGenerators;
using Tweetinvi.Core.Web;
using Tweetinvi.Models;
using Tweetinvi.Models.DTO;
using Tweetinvi.Models.DTO.QueryDTO;

namespace Testinvi.TweetinviControllers.FriendshipTests
{
    [TestClass]
    public class FriendshipJsonControllerTests
    {
        private FakeClassBuilder<FriendshipJsonController> _fakeBuilder;
        private Fake<IFriendshipQueryGenerator> _fakeFriendshipQueryGenerator;
        private Fake<IFriendshipFactory> _fakeFriendshipFactory;
        private Fake<ITwitterAccessor> _fakeTwitterAccessor;

        [TestInitialize]
        public void TestInitialize()
        {
            _fakeBuilder = new FakeClassBuilder<FriendshipJsonController>();
            _fakeFriendshipQueryGenerator = _fakeBuilder.GetFake<IFriendshipQueryGenerator>();
            _fakeFriendshipFactory = _fakeBuilder.GetFake<IFriendshipFactory>();
            _fakeTwitterAccessor = _fakeBuilder.GetFake<ITwitterAccessor>();

            ArrangeFriendshipFactoryAuthorizations();
        }

        [TestMethod]
        public void GetUserIdsRequestingFriendship_ReturnsCursorQuery()
        {
            var expectedResult = new List<string> {Guid.NewGuid().ToString()};
            
            // Arrange
            var jsonController = CreateFriendshipJsonController();
            string query = Guid.NewGuid().ToString();

            ArrangeGetUserIdsRequestingFriendshipQuery(query);
            _fakeTwitterAccessor.ArrangeExecuteJsonCursorGETQuery<IIdsCursorQueryResultDTO>(query, expectedResult);

            // Act
            var result = jsonController.GetUserIdsRequestingFriendship();

            // Assert
            Assert.AreEqual(result, expectedResult);
        }

        private void ArrangeGetUserIdsRequestingFriendshipQuery(string query)
        {
            _fakeFriendshipQueryGenerator
                .CallsTo(x => x.GetUserIdsRequestingFriendshipQuery())
                .Returns(query);
        }

        [TestMethod]
        public void GetUserIdsYouRequestedToFollow_ReturnsCursorQuery()
        {
            var expectedResult = new List<string> { Guid.NewGuid().ToString() };

            // Arrange
            var jsonController = CreateFriendshipJsonController();
            string query = Guid.NewGuid().ToString();

            ArrangeGetUserIdsYouRequestedToFollowQuery(query);
            _fakeTwitterAccessor.ArrangeExecuteJsonCursorGETQuery<IIdsCursorQueryResultDTO>(query, expectedResult);

            // Act
            var result = jsonController.GetUserIdsYouRequestedToFollow();

            // Assert
            Assert.AreEqual(result, expectedResult);
        }

        private void ArrangeGetUserIdsYouRequestedToFollowQuery(string query)
        {
            _fakeFriendshipQueryGenerator
                .CallsTo(x => x.GetUserIdsYouRequestedToFollowQuery())
                .Returns(query);
        }

        #region Create Friendship With

        [TestMethod]
        public void CreateFriendshipWith_User_ReturnsTwitterAccessorResult()
        {
            string expectedResult = Guid.NewGuid().ToString();

            // Arrange
            var jsonController = CreateFriendshipJsonController();

            string query = Guid.NewGuid().ToString();
            var fakeUser = A.Fake<IUser>();
            fakeUser.CallsTo(x => x.UserDTO).Returns(A.Fake<IUserDTO>());

            ArrangeCreateFriendshipQueryGenerator(fakeUser, query);
            _fakeTwitterAccessor.ArrangeExecuteJsonPOSTQuery(query, expectedResult);

            // Act
            var result = jsonController.CreateFriendshipWith(fakeUser);

            // Assert
            Assert.AreEqual(result, expectedResult);
        }

        [TestMethod]
        public void CreateFriendshipWith_UserAndDTOIsNull_ReturnsNull()
        {
            string expectedResult = Guid.NewGuid().ToString();

            // Arrange
            var jsonController = CreateFriendshipJsonController();

            string query = Guid.NewGuid().ToString();
            var fakeUser = A.Fake<IUser>();
            fakeUser.CallsTo(x => x.UserDTO).Returns(null);

            ArrangeCreateFriendshipQueryGenerator(fakeUser, query);
            _fakeTwitterAccessor.ArrangeExecuteJsonPOSTQuery(query, expectedResult);

            // Act
            var result = jsonController.CreateFriendshipWith(fakeUser);

            // Assert
            Assert.AreEqual(result, expectedResult);
        }

        private void ArrangeCreateFriendshipQueryGenerator(IUser user, string query)
        {
            _fakeFriendshipQueryGenerator
                .CallsTo(x => x.GetCreateFriendshipWithQuery(user))
                .Returns(query);
        }

        [TestMethod]
        public void CreateFriendshipWith_UserDTO_ReturnsTwitterAccessorResult()
        {
            string expectedResult = Guid.NewGuid().ToString();

            // Arrange
            var jsonController = CreateFriendshipJsonController();

            string query = Guid.NewGuid().ToString();
            var fakeUserDTO = A.Fake<IUserDTO>();

            ArrangeCreateFriendshipQueryGenerator(fakeUserDTO, query);
            _fakeTwitterAccessor.ArrangeExecuteJsonPOSTQuery(query, expectedResult);

            // Act
            var result = jsonController.CreateFriendshipWith(fakeUserDTO);

            // Assert
            Assert.AreEqual(result, expectedResult);
        }

        [TestMethod]
        public void CreateFriendshipWith_UserDTOIsNull_ReturnsTwitterAccessorResult()
        {
            string expectedResult = Guid.NewGuid().ToString();

            // Arrange
            var jsonController = CreateFriendshipJsonController();

            string query = Guid.NewGuid().ToString();

            ArrangeCreateFriendshipQueryGenerator((IUserDTO)null, query);
            _fakeTwitterAccessor.ArrangeExecuteJsonPOSTQuery(query, expectedResult);

            // Act
            var result = jsonController.CreateFriendshipWith((IUserDTO)null);

            // Assert
            Assert.AreEqual(result, expectedResult);
        }

        private void ArrangeCreateFriendshipQueryGenerator(IUserDTO userDTO, string query)
        {
            _fakeFriendshipQueryGenerator
                .CallsTo(x => x.GetCreateFriendshipWithQuery(userDTO))
                .Returns(query);
        }

        #endregion

        #region Destroy Friendship With

        [TestMethod]
        public void DestroyFriendshipWith_User_ReturnsTwitterAccessorResult()
        {
            string expectedResult = Guid.NewGuid().ToString();

            // Arrange
            var jsonController = CreateFriendshipJsonController();

            string query = Guid.NewGuid().ToString();
            var fakeUser = A.Fake<IUser>();
            fakeUser.CallsTo(x => x.UserDTO).Returns(A.Fake<IUserDTO>());

            ArrangeDestroyFriendshipQueryGenerator(fakeUser, query);
            _fakeTwitterAccessor.ArrangeExecuteJsonPOSTQuery(query, expectedResult);

            // Act
            var result = jsonController.DestroyFriendshipWith(fakeUser);

            // Assert
            Assert.AreEqual(result, expectedResult);
        }

        [TestMethod]
        public void DestroyFriendshipWith_UserAndDTOIsNull_ReturnsNull()
        {
            string expectedResult = Guid.NewGuid().ToString();

            // Arrange
            var jsonController = CreateFriendshipJsonController();

            string query = Guid.NewGuid().ToString();
            var fakeUser = A.Fake<IUser>();
            fakeUser.CallsTo(x => x.UserDTO).Returns(null);

            ArrangeDestroyFriendshipQueryGenerator(fakeUser, query);
            _fakeTwitterAccessor.ArrangeExecuteJsonPOSTQuery(query, expectedResult);

            // Act
            var result = jsonController.DestroyFriendshipWith(fakeUser);

            // Assert
            Assert.AreEqual(result, expectedResult);
        }

        private void ArrangeDestroyFriendshipQueryGenerator(IUser user, string query)
        {
            _fakeFriendshipQueryGenerator
                .CallsTo(x => x.GetDestroyFriendshipWithQuery(user))
                .Returns(query);
        }

        [TestMethod]
        public void DestroyFriendshipWith_UserDTO_ReturnsTwitterAccessorResult()
        {
            string expectedResult = Guid.NewGuid().ToString();

            // Arrange
            var jsonController = CreateFriendshipJsonController();

            string query = Guid.NewGuid().ToString();
            var fakeUserDTO = A.Fake<IUserDTO>();

            ArrangeDestroyFriendshipQueryGenerator(fakeUserDTO, query);
            _fakeTwitterAccessor.ArrangeExecuteJsonPOSTQuery(query, expectedResult);

            // Act
            var result = jsonController.DestroyFriendshipWith(fakeUserDTO);

            // Assert
            Assert.AreEqual(result, expectedResult);
        }

        [TestMethod]
        public void DestroyFriendshipWith_UserDTOIsNull_ReturnsTwitterAccessorResult()
        {
            string expectedResult = Guid.NewGuid().ToString();

            // Arrange
            var jsonController = CreateFriendshipJsonController();

            string query = Guid.NewGuid().ToString();

            ArrangeDestroyFriendshipQueryGenerator((IUserDTO)null, query);
            _fakeTwitterAccessor.ArrangeExecuteJsonPOSTQuery(query, expectedResult);

            // Act
            var result = jsonController.DestroyFriendshipWith((IUserDTO)null);

            // Assert
            Assert.AreEqual(result, expectedResult);
        }

        private void ArrangeDestroyFriendshipQueryGenerator(IUserDTO userDTO, string query)
        {
            _fakeFriendshipQueryGenerator
                .CallsTo(x => x.GetDestroyFriendshipWithQuery(userDTO))
                .Returns(query);
        }

        #endregion

        #region Update Friendship Authorizations

        // User
        [TestMethod]
        public void UpdateRelationshipAuthorizations_UserWithNullUserDTO_ReturnsTwitterAccessorResult()
        {
            string expectedResult1 = Guid.NewGuid().ToString();
            string expectedResult2 = Guid.NewGuid().ToString();
            string expectedResult3 = Guid.NewGuid().ToString();
            string expectedResult4 = Guid.NewGuid().ToString();

            // Arrange - Act
            var result1 = UpdateRelationshipAuthorizations_UserWithNullUserDTO_QueryExecutorReturns(true, true, expectedResult1);
            var result2 = UpdateRelationshipAuthorizations_UserWithNullUserDTO_QueryExecutorReturns(true, false, expectedResult2);
            var result3 = UpdateRelationshipAuthorizations_UserWithNullUserDTO_QueryExecutorReturns(false, true, expectedResult3);
            var result4 = UpdateRelationshipAuthorizations_UserWithNullUserDTO_QueryExecutorReturns(false, false, expectedResult4);

            // Assert
            Assert.AreEqual(result1, expectedResult1);
            Assert.AreEqual(result2, expectedResult2);
            Assert.AreEqual(result3, expectedResult3);
            Assert.AreEqual(result4, expectedResult4);
        }

        private string UpdateRelationshipAuthorizations_UserWithNullUserDTO_QueryExecutorReturns(bool retweetsEnabled, bool notification, string returnValue)
        {
            var user = A.Fake<IUser>();
            user.CallsTo(x => x.UserDTO).Returns(null);

            return ArrangeUpdateRelationshipAuthorizations_User(user, retweetsEnabled, notification, returnValue);
        }

        private string ArrangeUpdateRelationshipAuthorizations_User(IUser user, bool retweetsEnabled, bool notification, string returnValue)
        {
            // Arrange
            var query = Guid.NewGuid().ToString();
            var jsonController = CreateFriendshipJsonController();
            var fakeAuthorizations = GenerateFriendshipAuthorizations(retweetsEnabled, notification);

            ArrangeGetUpdateRelationshipAuthorizationQuery(user, fakeAuthorizations, query);
            _fakeTwitterAccessor.ArrangeExecuteJsonPOSTQuery(query, returnValue);

            // Act
            return jsonController.UpdateRelationshipAuthorizationsWith(user, retweetsEnabled, notification);
        }

        // User DTO
        [TestMethod]
        public void UpdateRelationshipAuthorizations_UserDTO_ReturnsTwitterAccessorResult()
        {
            string expectedResult1 = Guid.NewGuid().ToString();
            string expectedResult2 = Guid.NewGuid().ToString();
            string expectedResult3 = Guid.NewGuid().ToString();
            string expectedResult4 = Guid.NewGuid().ToString();

            // Arrange - Act
            var result1 = UpdateRelationshipAuthorizations_UserDTO_QueryExecutorReturns(true, true, expectedResult1);
            var result2 = UpdateRelationshipAuthorizations_UserDTO_QueryExecutorReturns(true, false, expectedResult2);
            var result3 = UpdateRelationshipAuthorizations_UserDTO_QueryExecutorReturns(false, true, expectedResult3);
            var result4 = UpdateRelationshipAuthorizations_UserDTO_QueryExecutorReturns(false, false, expectedResult4);

            // Assert
            Assert.AreEqual(result1, expectedResult1);
            Assert.AreEqual(result2, expectedResult2);
            Assert.AreEqual(result3, expectedResult3);
            Assert.AreEqual(result4, expectedResult4);
        }

        private string UpdateRelationshipAuthorizations_UserDTO_QueryExecutorReturns(bool retweetsEnabled, bool notification, string returnValue)
        {
            var user = A.Fake<IUser>();
            return ArrangeUpdateRelationshipAuthorizations_UserDTO(user, retweetsEnabled, notification, returnValue);
        }

        private string ArrangeUpdateRelationshipAuthorizations_UserDTO(IUser user, bool retweetsEnabled, bool notification, string returnValue)
        {
            // Arrange
            var query = Guid.NewGuid().ToString();
            var jsonController = CreateFriendshipJsonController();
            var fakeAuthorizations = GenerateFriendshipAuthorizations(retweetsEnabled, notification);

            ArrangeGetUpdateRelationshipAuthorizationQuery(user, fakeAuthorizations, query);
            _fakeTwitterAccessor.ArrangeExecuteJsonPOSTQuery(query, returnValue);

            // Act
            return jsonController.UpdateRelationshipAuthorizationsWith(user, retweetsEnabled, notification);
        }

        private void ArrangeGetUpdateRelationshipAuthorizationQuery(IUser user, IFriendshipAuthorizations authorizations, string query)
        {
            _fakeFriendshipQueryGenerator
                .CallsTo(x => x.GetUpdateRelationshipAuthorizationsWithQuery(user,
                    A<IFriendshipAuthorizations>.That.Matches(a => a.RetweetsEnabled == authorizations.RetweetsEnabled &&
                                                                   a.DeviceNotificationEnabled == authorizations.DeviceNotificationEnabled)))
                .Returns(query);
        }

        #endregion

        private void ArrangeFriendshipFactoryAuthorizations()
        {
            _fakeFriendshipFactory
                .CallsTo(x => x.GenerateFriendshipAuthorizations(A<bool>.Ignored, A<bool>.Ignored))
                .ReturnsLazily((bool retweetsEnabled, bool notificationsEnabled) =>
                {
                    return GenerateFriendshipAuthorizations(retweetsEnabled, notificationsEnabled);
                });
        }

        private IFriendshipAuthorizations GenerateFriendshipAuthorizations(bool retweetsEnabled, bool notification)
        {
            var fakeFriendshipAuthorization = A.Fake<IFriendshipAuthorizations>();

            fakeFriendshipAuthorization.CallsTo(x => x.RetweetsEnabled).Returns(retweetsEnabled);
            fakeFriendshipAuthorization.CallsTo(x => x.DeviceNotificationEnabled).Returns(notification);

            return fakeFriendshipAuthorization;
        }

        public FriendshipJsonController CreateFriendshipJsonController()
        {
            return _fakeBuilder.GenerateClass();
        }
    }
}