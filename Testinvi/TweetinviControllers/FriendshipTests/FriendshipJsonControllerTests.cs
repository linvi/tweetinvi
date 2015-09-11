using System;
using System.Collections.Generic;
using FakeItEasy;
using FakeItEasy.ExtensionSyntax.Full;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Testinvi.Helpers;
using Testinvi.SetupHelpers;
using Tweetinvi.Controllers.Friendship;
using Tweetinvi.Core.Interfaces;
using Tweetinvi.Core.Interfaces.Credentials;
using Tweetinvi.Core.Interfaces.DTO;
using Tweetinvi.Core.Interfaces.DTO.QueryDTO;
using Tweetinvi.Core.Interfaces.Factories;
using Tweetinvi.Core.Interfaces.Models;
using Tweetinvi.Core.Interfaces.QueryGenerators;

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
        [ExpectedException(typeof(ArgumentException))]
        public void CreateFriendshipWith_UserIsNull_ThrowsArgumentException()
        {
            // Arrange
            var jsonController = CreateFriendshipJsonController();

            // Act
            jsonController.CreateFriendshipWith((IUser)null);
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
                .CallsTo(x => x.GetCreateFriendshipWithQuery(user.UserDTO))
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

        [TestMethod]
        public void CreateFriendshipWith_UserScreenName_ReturnsTwitterAccessorResult()
        {
            string expectedResult = Guid.NewGuid().ToString();

            // Arrange
            var jsonController = CreateFriendshipJsonController();

            string query = Guid.NewGuid().ToString();
            string screenName = Guid.NewGuid().ToString();

            ArrangeCreateFriendshipQueryGenerator(screenName, query);
            _fakeTwitterAccessor.ArrangeExecuteJsonPOSTQuery(query, expectedResult);

            // Act
            var result = jsonController.CreateFriendshipWith(screenName);

            // Assert
            Assert.AreEqual(result, expectedResult);
        }

        [TestMethod]
        public void CreateFriendshipWith_UserScreenNameIsNull_ReturnsTwitterAccessorResult()
        {
            string expectedResult = Guid.NewGuid().ToString();

            // Arrange
            var jsonController = CreateFriendshipJsonController();

            string query = Guid.NewGuid().ToString();

            ArrangeCreateFriendshipQueryGenerator((string)null, query);
            _fakeTwitterAccessor.ArrangeExecuteJsonPOSTQuery(query, expectedResult);

            // Act
            var result = jsonController.CreateFriendshipWith((string)null);

            // Assert
            Assert.AreEqual(result, expectedResult);
        }

        [TestMethod]
        public void CreateFriendshipWith_UserScreenNameIsEmpty_ReturnsTwitterAccessorResult()
        {
            string expectedResult = Guid.NewGuid().ToString();

            // Arrange
            var jsonController = CreateFriendshipJsonController();

            string query = Guid.NewGuid().ToString();

            ArrangeCreateFriendshipQueryGenerator(String.Empty, query);
            _fakeTwitterAccessor.ArrangeExecuteJsonPOSTQuery(query, expectedResult);

            // Act
            var result = jsonController.CreateFriendshipWith(String.Empty);

            // Assert
            Assert.AreEqual(result, expectedResult);
        }

        private void ArrangeCreateFriendshipQueryGenerator(string screenName, string query)
        {
            _fakeFriendshipQueryGenerator
                .CallsTo(x => x.GetCreateFriendshipWithQuery(screenName))
                .Returns(query);
        }

        [TestMethod]
        public void CreateFriendshipWith_UserId_ReturnsTwitterAccessorResult()
        {
            string expectedResult = Guid.NewGuid().ToString();

            // Arrange
            var jsonController = CreateFriendshipJsonController();

            string query = Guid.NewGuid().ToString();
            long userId = TestHelper.GenerateRandomLong();

            ArrangeCreateFriendshipQueryGenerator(userId, query);
            _fakeTwitterAccessor.ArrangeExecuteJsonPOSTQuery(query, expectedResult);

            // Act
            var result = jsonController.CreateFriendshipWith(userId);

            // Assert
            Assert.AreEqual(result, expectedResult);
        }

        private void ArrangeCreateFriendshipQueryGenerator(long userId, string query)
        {
            _fakeFriendshipQueryGenerator
                .CallsTo(x => x.GetCreateFriendshipWithQuery(userId))
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
        [ExpectedException(typeof(ArgumentException))]
        public void DestroyFriendshipWith_UserIsNull_ThrowsArgumentException()
        {
            // Arrange
            var jsonController = CreateFriendshipJsonController();

            // Act
            jsonController.DestroyFriendshipWith((IUser)null);
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
                .CallsTo(x => x.GetDestroyFriendshipWithQuery(user.UserDTO))
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

        [TestMethod]
        public void DestroyFriendshipWith_UserScreenName_ReturnsTwitterAccessorResult()
        {
            string expectedResult = Guid.NewGuid().ToString();

            // Arrange
            var jsonController = CreateFriendshipJsonController();

            string query = Guid.NewGuid().ToString();
            string screenName = Guid.NewGuid().ToString();

            ArrangeDestroyFriendshipQueryGenerator(screenName, query);
            _fakeTwitterAccessor.ArrangeExecuteJsonPOSTQuery(query, expectedResult);

            // Act
            var result = jsonController.DestroyFriendshipWith(screenName);

            // Assert
            Assert.AreEqual(result, expectedResult);
        }

        [TestMethod]
        public void DestroyFriendshipWith_UserScreenNameIsNull_ReturnsTwitterAccessorResult()
        {
            string expectedResult = Guid.NewGuid().ToString();

            // Arrange
            var jsonController = CreateFriendshipJsonController();

            string query = Guid.NewGuid().ToString();

            ArrangeDestroyFriendshipQueryGenerator((string)null, query);
            _fakeTwitterAccessor.ArrangeExecuteJsonPOSTQuery(query, expectedResult);

            // Act
            var result = jsonController.DestroyFriendshipWith((string)null);

            // Assert
            Assert.AreEqual(result, expectedResult);
        }

        [TestMethod]
        public void DestroyFriendshipWith_UserScreenNameIsEmpty_ReturnsTwitterAccessorResult()
        {
            string expectedResult = Guid.NewGuid().ToString();

            // Arrange
            var jsonController = CreateFriendshipJsonController();

            string query = Guid.NewGuid().ToString();

            ArrangeDestroyFriendshipQueryGenerator(String.Empty, query);
            _fakeTwitterAccessor.ArrangeExecuteJsonPOSTQuery(query, expectedResult);

            // Act
            var result = jsonController.DestroyFriendshipWith(String.Empty);

            // Assert
            Assert.AreEqual(result, expectedResult);
        }

        private void ArrangeDestroyFriendshipQueryGenerator(string screenName, string query)
        {
            _fakeFriendshipQueryGenerator
                .CallsTo(x => x.GetDestroyFriendshipWithQuery(screenName))
                .Returns(query);
        }

        [TestMethod]
        public void DestroyFriendshipWith_UserId_ReturnsTwitterAccessorResult()
        {
            string expectedResult = Guid.NewGuid().ToString();

            // Arrange
            var jsonController = CreateFriendshipJsonController();

            string query = Guid.NewGuid().ToString();
            long userId = TestHelper.GenerateRandomLong();

            ArrangeDestroyFriendshipQueryGenerator(userId, query);
            _fakeTwitterAccessor.ArrangeExecuteJsonPOSTQuery(query, expectedResult);

            // Act
            var result = jsonController.DestroyFriendshipWith(userId);

            // Assert
            Assert.AreEqual(result, expectedResult);
        }

        private void ArrangeDestroyFriendshipQueryGenerator(long userId, string query)
        {
            _fakeFriendshipQueryGenerator
                .CallsTo(x => x.GetDestroyFriendshipWithQuery(userId))
                .Returns(query);
        }

        #endregion

        #region Update Friendship Authorizations

        // User
        [TestMethod]
        public void UpdateRelationshipAuthorizations_UserIsNull_ReturnsNull()
        {
            // Arrange
            var jsonController = CreateFriendshipJsonController();

            // Act
            try
            {
                jsonController.UpdateRelationshipAuthorizationsWith((IUser) null, false, false);
                Assert.Fail();
            }
            catch (ArgumentException) {}

            try
            {
                jsonController.UpdateRelationshipAuthorizationsWith((IUser)null, false, true);
                Assert.Fail();
            }
            catch (ArgumentException) { }

            try
            {
                jsonController.UpdateRelationshipAuthorizationsWith((IUser)null, true, false);
                Assert.Fail();
            }
            catch (ArgumentException) { }

            try
            {
                jsonController.UpdateRelationshipAuthorizationsWith((IUser)null, true, true);
                Assert.Fail();
            }
            catch (ArgumentException) { }
        }

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

        [TestMethod]
        public void UpdateRelationshipAuthorizations_User_ReturnsTwitterAccessorResult()
        {
            string expectedResult1 = Guid.NewGuid().ToString();
            string expectedResult2 = Guid.NewGuid().ToString();
            string expectedResult3 = Guid.NewGuid().ToString();
            string expectedResult4 = Guid.NewGuid().ToString();

            // Arrange - Act
            var result1 = UpdateRelationshipAuthorizations_User_QueryExecutorReturns(true, true, expectedResult1);
            var result2 = UpdateRelationshipAuthorizations_User_QueryExecutorReturns(true, false, expectedResult2);
            var result3 = UpdateRelationshipAuthorizations_User_QueryExecutorReturns(false, true, expectedResult3);
            var result4 = UpdateRelationshipAuthorizations_User_QueryExecutorReturns(false, false, expectedResult4);

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

        private string UpdateRelationshipAuthorizations_User_QueryExecutorReturns(bool retweetsEnabled, bool notification, string returnValue)
        {
            var userDTO = A.Fake<IUserDTO>();
            var user = A.Fake<IUser>();
            user.CallsTo(x => x.UserDTO).Returns(userDTO);

            return ArrangeUpdateRelationshipAuthorizations_User(user, retweetsEnabled, notification, returnValue);
        }

        private string ArrangeUpdateRelationshipAuthorizations_User(IUser user, bool retweetsEnabled, bool notification, string returnValue)
        {
            // Arrange
            var query = Guid.NewGuid().ToString();
            var jsonController = CreateFriendshipJsonController();
            var fakeAuthorizations = GenerateFriendshipAuthorizations(retweetsEnabled, notification);

            ArrangeGetUpdateRelationshipAuthorizationQuery(user.UserDTO, fakeAuthorizations, query);
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
            var userDTO = A.Fake<IUserDTO>();
            return ArrangeUpdateRelationshipAuthorizations_UserDTO(userDTO, retweetsEnabled, notification, returnValue);
        }

        private string ArrangeUpdateRelationshipAuthorizations_UserDTO(IUserDTO userDTO, bool retweetsEnabled, bool notification, string returnValue)
        {
            // Arrange
            var query = Guid.NewGuid().ToString();
            var jsonController = CreateFriendshipJsonController();
            var fakeAuthorizations = GenerateFriendshipAuthorizations(retweetsEnabled, notification);

            ArrangeGetUpdateRelationshipAuthorizationQuery(userDTO, fakeAuthorizations, query);
            _fakeTwitterAccessor.ArrangeExecuteJsonPOSTQuery(query, returnValue);

            // Act
            return jsonController.UpdateRelationshipAuthorizationsWith(userDTO, retweetsEnabled, notification);
        }

        private void ArrangeGetUpdateRelationshipAuthorizationQuery(IUserDTO userDTO, IFriendshipAuthorizations authorizations, string query)
        {
            _fakeFriendshipQueryGenerator
                .CallsTo(x => x.GetUpdateRelationshipAuthorizationsWithQuery(userDTO,
                    A<IFriendshipAuthorizations>.That.Matches(a => a.RetweetsEnabled == authorizations.RetweetsEnabled &&
                                                                   a.DeviceNotificationEnabled == authorizations.DeviceNotificationEnabled)))
                .Returns(query);
        }

        // User ScreenName
        [TestMethod]
        public void UpdateRelationshipAuthorizations_UserScreenNameIsNull_ReturnsTwitterAccessorResult()
        {
            string expectedResult1 = Guid.NewGuid().ToString();
            string expectedResult2 = Guid.NewGuid().ToString();
            string expectedResult3 = Guid.NewGuid().ToString();
            string expectedResult4 = Guid.NewGuid().ToString();

            // Arrange - Act
            var result1 = UpdateRelationshipAuthorizations_UserScreenNameIsNull_QueryExecutorReturns(true, true, expectedResult1);
            var result2 = UpdateRelationshipAuthorizations_UserScreenNameIsNull_QueryExecutorReturns(true, false, expectedResult2);
            var result3 = UpdateRelationshipAuthorizations_UserScreenNameIsNull_QueryExecutorReturns(false, true, expectedResult3);
            var result4 = UpdateRelationshipAuthorizations_UserScreenNameIsNull_QueryExecutorReturns(false, false, expectedResult4);

            // Assert
            Assert.AreEqual(result1, expectedResult1);
            Assert.AreEqual(result2, expectedResult2);
            Assert.AreEqual(result3, expectedResult3);
            Assert.AreEqual(result4, expectedResult4);
        }

        [TestMethod]
        public void UpdateRelationshipAuthorizations_UserScreenNameIsEmpty_ReturnsTwitterAccessorResult()
        {
            string expectedResult1 = Guid.NewGuid().ToString();
            string expectedResult2 = Guid.NewGuid().ToString();
            string expectedResult3 = Guid.NewGuid().ToString();
            string expectedResult4 = Guid.NewGuid().ToString();

            // Arrange - Act
            var result1 = UpdateRelationshipAuthorizations_UserScreenNameIsEmpty_QueryExecutorReturns(true, true, expectedResult1);
            var result2 = UpdateRelationshipAuthorizations_UserScreenNameIsEmpty_QueryExecutorReturns(true, false, expectedResult2);
            var result3 = UpdateRelationshipAuthorizations_UserScreenNameIsEmpty_QueryExecutorReturns(false, true, expectedResult3);
            var result4 = UpdateRelationshipAuthorizations_UserScreenNameIsEmpty_QueryExecutorReturns(false, false, expectedResult4);

            // Assert
            Assert.AreEqual(result1, expectedResult1);
            Assert.AreEqual(result2, expectedResult2);
            Assert.AreEqual(result3, expectedResult3);
            Assert.AreEqual(result4, expectedResult4);
        }

        [TestMethod]
        public void UpdateRelationshipAuthorizations_UserScreenName_ReturnsTwitterAccessorResult()
        {
            string expectedResult1 = Guid.NewGuid().ToString();
            string expectedResult2 = Guid.NewGuid().ToString();
            string expectedResult3 = Guid.NewGuid().ToString();
            string expectedResult4 = Guid.NewGuid().ToString();

            // Arrange - Act
            var result1 = UpdateRelationshipAuthorizations_UserScreenName_QueryExecutorReturns(true, true, expectedResult1);
            var result2 = UpdateRelationshipAuthorizations_UserScreenName_QueryExecutorReturns(true, false, expectedResult2);
            var result3 = UpdateRelationshipAuthorizations_UserScreenName_QueryExecutorReturns(false, true, expectedResult3);
            var result4 = UpdateRelationshipAuthorizations_UserScreenName_QueryExecutorReturns(false, false, expectedResult4);

            // Assert
            Assert.AreEqual(result1, expectedResult1);
            Assert.AreEqual(result2, expectedResult2);
            Assert.AreEqual(result3, expectedResult3);
            Assert.AreEqual(result4, expectedResult4);
        }

        private string UpdateRelationshipAuthorizations_UserScreenNameIsNull_QueryExecutorReturns(bool retweetsEnabled, bool notification, string returnValue)
        {
            return ArrangeUpdateRelationshipAuthorizations_UserScreenName(null, retweetsEnabled, notification, returnValue);
        }

        private string UpdateRelationshipAuthorizations_UserScreenNameIsEmpty_QueryExecutorReturns(bool retweetsEnabled, bool notification, string returnValue)
        {
            return ArrangeUpdateRelationshipAuthorizations_UserScreenName(String.Empty, retweetsEnabled, notification, returnValue);
        }

        private string UpdateRelationshipAuthorizations_UserScreenName_QueryExecutorReturns(bool retweetsEnabled, bool notification, string returnValue)
        {
            var screenName = Guid.NewGuid().ToString();
            return ArrangeUpdateRelationshipAuthorizations_UserScreenName(screenName, retweetsEnabled, notification, returnValue);
        }

        private string ArrangeUpdateRelationshipAuthorizations_UserScreenName(string screenName, bool retweetsEnabled, bool notification, string returnValue)
        {
            // Arrange
            var query = Guid.NewGuid().ToString();
            var jsonController = CreateFriendshipJsonController();
            var fakeAuthorizations = GenerateFriendshipAuthorizations(retweetsEnabled, notification);

            ArrangeGetUpdateRelationshipAuthorizationQuery(screenName, fakeAuthorizations, query);
            _fakeTwitterAccessor.ArrangeExecuteJsonPOSTQuery(query, returnValue);

            // Act
            return jsonController.UpdateRelationshipAuthorizationsWith(screenName, retweetsEnabled, notification);
        }

        private void ArrangeGetUpdateRelationshipAuthorizationQuery(string screenName, IFriendshipAuthorizations authorizations, string query)
        {
            _fakeFriendshipQueryGenerator
                .CallsTo(x => x.GetUpdateRelationshipAuthorizationsWithQuery(screenName,
                    A<IFriendshipAuthorizations>.That.Matches(a => a.RetweetsEnabled == authorizations.RetweetsEnabled &&
                                                                   a.DeviceNotificationEnabled == authorizations.DeviceNotificationEnabled)))
                .Returns(query);
        }

        // User ID
        [TestMethod]
        public void UpdateRelationshipAuthorizations_UserId_ReturnsTwitterAccessorResult()
        {
            string expectedResult1 = Guid.NewGuid().ToString();
            string expectedResult2 = Guid.NewGuid().ToString();
            string expectedResult3 = Guid.NewGuid().ToString();
            string expectedResult4 = Guid.NewGuid().ToString();

            // Arrange - Act
            var result1 = UpdateRelationshipAuthorizations_UserId_QueryExecutorReturns(true, true, expectedResult1);
            var result2 = UpdateRelationshipAuthorizations_UserId_QueryExecutorReturns(true, false, expectedResult2);
            var result3 = UpdateRelationshipAuthorizations_UserId_QueryExecutorReturns(false, true, expectedResult3);
            var result4 = UpdateRelationshipAuthorizations_UserId_QueryExecutorReturns(false, false, expectedResult4);

            // Assert
            Assert.AreEqual(result1, expectedResult1);
            Assert.AreEqual(result2, expectedResult2);
            Assert.AreEqual(result3, expectedResult3);
            Assert.AreEqual(result4, expectedResult4);
        }

        private string UpdateRelationshipAuthorizations_UserId_QueryExecutorReturns(bool retweetsEnabled, bool notification, string returnValue)
        {
            var userId = TestHelper.GenerateRandomLong();

            // Arrange
            var query = Guid.NewGuid().ToString();
            var jsonController = CreateFriendshipJsonController();
            var fakeAuthorizations = GenerateFriendshipAuthorizations(retweetsEnabled, notification);

            ArrangeGetUpdateRelationshipAuthorizationQuery(userId, fakeAuthorizations, query);
            _fakeTwitterAccessor.ArrangeExecuteJsonPOSTQuery(query, returnValue);

            // Act
            return jsonController.UpdateRelationshipAuthorizationsWith(userId, retweetsEnabled, notification);
        }

        private void ArrangeGetUpdateRelationshipAuthorizationQuery(long userId, IFriendshipAuthorizations authorizations, string query)
        {
            _fakeFriendshipQueryGenerator
                .CallsTo(x => x.GetUpdateRelationshipAuthorizationsWithQuery(userId,
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