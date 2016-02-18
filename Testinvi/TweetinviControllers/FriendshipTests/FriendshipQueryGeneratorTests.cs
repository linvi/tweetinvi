using System;
using FakeItEasy;
using FakeItEasy.ExtensionSyntax.Full;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Testinvi.Helpers;
using Testinvi.SetupHelpers;
using Tweetinvi.Controllers.Friendship;
using Tweetinvi.Controllers.Properties;
using Tweetinvi.Core.Interfaces.DTO;
using Tweetinvi.Core.Interfaces.Models;
using Tweetinvi.Core.Interfaces.QueryGenerators;
using Tweetinvi.Core.Interfaces.QueryValidators;

namespace Testinvi.TweetinviControllers.FriendshipTests
{
    [TestClass]
    public class FriendshipQueryGeneratorTests
    {
        private FakeClassBuilder<FriendshipQueryGenerator> _fakeBuilder;
        private Fake<IUserQueryParameterGenerator> _fakeUserQueryParameterGenerator;
        private Fake<IUserQueryValidator> _fakeUserQueryValidator;

        [TestInitialize]
        public void TestInitialize()
        {
            _fakeBuilder = new FakeClassBuilder<FriendshipQueryGenerator>();
            _fakeUserQueryParameterGenerator = _fakeBuilder.GetFake<IUserQueryParameterGenerator>();
            _fakeUserQueryValidator = _fakeBuilder.GetFake<IUserQueryValidator>();

            _fakeUserQueryParameterGenerator.ArrangeGenerateIdParameter();
            _fakeUserQueryParameterGenerator.ArrangeGenerateScreenNameParameter();
            _fakeUserQueryParameterGenerator.ArrangeGenerateIdOrScreenNameParameter();
            _fakeUserQueryValidator.ArrangeIsUserIdValid();
        }

        [TestMethod]
        public void GetUserIdsRequestingFriendshipQuery_ReturnsResources()
        {
            // Arrange
            var queryGenerator = CreateFrienshipQueryGenerator();

            // Act
            var query = queryGenerator.GetUserIdsRequestingFriendshipQuery();

            // Assert
            Assert.AreEqual(query, Resources.Friendship_GetIncomingIds);
        }

        [TestMethod]
        public void GetUserIdsYouRequestedToFollowQuery_ReturnsResources()
        {
            // Arrange
            var queryGenerator = CreateFrienshipQueryGenerator();

            // Act
            var query = queryGenerator.GetUserIdsYouRequestedToFollowQuery();

            // Assert
            Assert.AreEqual(query, Resources.Friendship_GetOutgoingIds);
        }

        #region Create Friendship

        [TestMethod]
        public void GetCreateFriendshipWithQuery_WithValidUserDTO_ReturnsIdQuery()
        {
            // Arrange
            var queryGenerator = CreateFrienshipQueryGenerator();
            var fakeUserDTO = CreateUserDTO(true);

            // Act
            var query = queryGenerator.GetCreateFriendshipWithQuery(fakeUserDTO);

            // Assert
            string expectedUserIdParameter = UserQueryGeneratorHelper.GenerateParameterExpectedResult(fakeUserDTO);
            Assert.AreEqual(query, string.Format(Resources.Friendship_Create, expectedUserIdParameter));
        }

        [TestMethod]
        public void GetCreateFriendshipWithQueryWithUserDTO_NotHasAValidIdentifier_ReturnsNull()
        {
            // Arrange
            var queryGenerator = CreateFrienshipQueryGenerator();
            var fakeUserDTO = CreateUserDTO(false);

            // Act
            var query = queryGenerator.GetCreateFriendshipWithQuery(fakeUserDTO);

            // Assert
            Assert.AreEqual(query, null);
        }

        [TestMethod]
        public void GetCreateFriendshipWithQueryWithUserScreeName_IsValid_ReturnsScreenNameQuery()
        {
            string screenName = Guid.NewGuid().ToString();

            // Arrange
            var queryGenerator = CreateFrienshipQueryGenerator();
            _fakeUserQueryValidator.CallsTo(x => x.IsScreenNameValid(screenName)).Returns(true);

            // Act
            var query = queryGenerator.GetCreateFriendshipWithQuery(screenName);

            // Assert
            string expectedUserScreenNameParameter = UserQueryGeneratorHelper.GenerateParameterExpectedResult(screenName);
            Assert.AreEqual(query, string.Format(Resources.Friendship_Create, expectedUserScreenNameParameter));
        }

        [TestMethod]
        public void GetCreateFriendshipWithQueryWithUserScreeName_IsNotValid_ReturnsNull()
        {
            var screenName = Guid.NewGuid().ToString();

            // Arrange
            var queryGenerator = CreateFrienshipQueryGenerator();
            _fakeUserQueryValidator.CallsTo(x => x.IsScreenNameValid(screenName)).Returns(false);

            // Act
            var query = queryGenerator.GetCreateFriendshipWithQuery(screenName);

            // Assert
            Assert.AreEqual(query, null);
        }

        [TestMethod]
        public void GetCreateFriendshipWithQueryWithUserId_UserIdIsDefault_ReturnsNull()
        {
            // Arrange
            var queryGenerator = CreateFrienshipQueryGenerator();

            // Act
            var query = queryGenerator.GetCreateFriendshipWithQuery(TestHelper.DefaultId());

            // Assert
            Assert.AreEqual(query, null);
        }

        [TestMethod]
        public void GetCreateFriendshipWithQueryWithUserId_ReturnsIdQuery()
        {
            long userId = TestHelper.GenerateRandomLong();

            // Arrange
            var queryGenerator = CreateFrienshipQueryGenerator();

            // Act
            var query = queryGenerator.GetCreateFriendshipWithQuery(userId);

            // Assert
            string expectedUserIdParameter = UserQueryGeneratorHelper.GenerateParameterExpectedResult(userId);
            Assert.AreEqual(query, string.Format(Resources.Friendship_Create, expectedUserIdParameter));
        }

        #endregion

        #region Destroy Friendship

        [TestMethod]
        public void GetDestroyFriendshipWithQuery_WithValidUserDTO_ReturnsIdQuery()
        {
            // Arrange
            var queryGenerator = CreateFrienshipQueryGenerator();
            var userDTO = CreateUserDTO(true);

            // Act
            var query = queryGenerator.GetDestroyFriendshipWithQuery(userDTO);

            // Assert
            string expectedUserIdParameter = UserQueryGeneratorHelper.GenerateParameterExpectedResult(userDTO);
            Assert.AreEqual(query, string.Format(Resources.Friendship_Destroy, expectedUserIdParameter));
        }

        [TestMethod]
        public void GetDestroyFriendshipWithQuery_WithInvalidUserDTO_ReturnsNull()
        {
            // Arrange
            var queryGenerator = CreateFrienshipQueryGenerator();
            var fakeUserDTO = CreateUserDTO(false);

            // Act
            var query = queryGenerator.GetDestroyFriendshipWithQuery(fakeUserDTO);

            // Assert
            Assert.AreEqual(query, null);
        }

        [TestMethod]
        public void GetDestroyFriendshipWithQueryWithUserScreeName_IsNotValid_ReturnsNull()
        {
            var screenName = Guid.NewGuid().ToString();

            // Arrange
            var queryGenerator = CreateFrienshipQueryGenerator();
            _fakeUserQueryValidator.CallsTo(x => x.IsScreenNameValid(screenName)).Returns(false);

            // Act
            var query = queryGenerator.GetDestroyFriendshipWithQuery(screenName);

            // Assert
            Assert.AreEqual(query, null);
        }

        [TestMethod]
        public void GetDestroyFriendshipWithQueryWithUserScreeName_IsValid_ReturnsScreenNameQuery()
        {
            string screenName = Guid.NewGuid().ToString();

            // Arrange
            var queryGenerator = CreateFrienshipQueryGenerator();
            _fakeUserQueryValidator.CallsTo(x => x.IsScreenNameValid(screenName)).Returns(true);

            // Act
            var query = queryGenerator.GetDestroyFriendshipWithQuery(screenName);

            // Assert
            string expectedUserScreenNameParameter = UserQueryGeneratorHelper.GenerateParameterExpectedResult(screenName);
            Assert.AreEqual(query, string.Format(Resources.Friendship_Destroy, expectedUserScreenNameParameter));
        }

        [TestMethod]
        public void GetDestroyFriendshipWithQueryWithUserId_UserIdIsDefault_ReturnsNull()
        {
            // Arrange
            var queryGenerator = CreateFrienshipQueryGenerator();

            // Act
            var query = queryGenerator.GetDestroyFriendshipWithQuery(TestHelper.DefaultId());

            // Assert
            Assert.AreEqual(query, null);
        }

        [TestMethod]
        public void GetDestroyFriendshipWithQueryWithUserId_ReturnsIdQuery()
        {
            long userId = TestHelper.GenerateRandomLong();

            // Arrange
            var queryGenerator = CreateFrienshipQueryGenerator();

            // Act
            var query = queryGenerator.GetDestroyFriendshipWithQuery(userId);

            // Assert
            string expectedUserIdParameter = UserQueryGeneratorHelper.GenerateParameterExpectedResult(userId);
            Assert.AreEqual(query, string.Format(Resources.Friendship_Destroy, expectedUserIdParameter));
        }

        #endregion

        #region Update Friendship Authorizations

        [TestMethod]
        public void UpdateRelationshipAuthorizations_WithUserDTO_AuthorizationsObjectIsNull_ReturnsNull()
        {
            var userDTO = CreateUserDTO(true);

            // Arrange
            var queryGenerator = CreateFrienshipQueryGenerator();

            // Act
            var result = queryGenerator.GetUpdateRelationshipAuthorizationsWithQuery(userDTO, null);

            // Assert
            Assert.AreEqual(result, null);
        }

        [TestMethod]
        public void UpdateRelationshipAuthorizations_WithInvalidUserDTO_ReturnsNull()
        {
            // Arrange
            var queryGenerator = CreateFrienshipQueryGenerator();
            var userDTO = CreateUserDTO(false);

            // Act
            var result1 = queryGenerator.GetUpdateRelationshipAuthorizationsWithQuery(userDTO, GenerateFriendshipAuthorizations(true, true));
            var result2 = queryGenerator.GetUpdateRelationshipAuthorizationsWithQuery(userDTO, GenerateFriendshipAuthorizations(true, false));
            var result3 = queryGenerator.GetUpdateRelationshipAuthorizationsWithQuery(userDTO, GenerateFriendshipAuthorizations(false, true));
            var result4 = queryGenerator.GetUpdateRelationshipAuthorizationsWithQuery(userDTO, GenerateFriendshipAuthorizations(false, false));

            // Assert
            Assert.AreEqual(result1, null);
            Assert.AreEqual(result2, null);
            Assert.AreEqual(result3, null);
            Assert.AreEqual(result4, null);
        }

        [TestMethod]
        public void UpdateRelationshipAuthorizations_WithValidUserDTO_AuthorizationsObjectIsValid_ValidQuery()
        {
            

            // Arrange
            var queryGenerator = CreateFrienshipQueryGenerator();
            var userDTO = A.Fake<IUserDTO>();
            
            _fakeUserQueryValidator.ArrangeCanUserBeIdentified(userDTO, true);
            _fakeUserQueryParameterGenerator.ArrangeGenerateIdOrScreenNameParameter();

            // Act
            var result1 = queryGenerator.GetUpdateRelationshipAuthorizationsWithQuery(userDTO, GenerateFriendshipAuthorizations(true, true));
            var result2 = queryGenerator.GetUpdateRelationshipAuthorizationsWithQuery(userDTO, GenerateFriendshipAuthorizations(true, false));
            var result3 = queryGenerator.GetUpdateRelationshipAuthorizationsWithQuery(userDTO, GenerateFriendshipAuthorizations(false, true));
            var result4 = queryGenerator.GetUpdateRelationshipAuthorizationsWithQuery(userDTO, GenerateFriendshipAuthorizations(false, false));

            // Assert
            string idParameter = UserQueryGeneratorHelper.GenerateParameterExpectedResult(userDTO);

            Assert.AreEqual(result1, GenerateUpdateQuery(true, true, idParameter));
            Assert.AreEqual(result2, GenerateUpdateQuery(true, false, idParameter));
            Assert.AreEqual(result3, GenerateUpdateQuery(false, true, idParameter));
            Assert.AreEqual(result4, GenerateUpdateQuery(false, false, idParameter));
        }

        public void UpdateRelationshipAuthorizations_WithValidUserDTO_ReturnsQueryWithId()
        {
            var userId = TestHelper.GenerateRandomLong();

            // Arrange
            var queryGenerator = CreateFrienshipQueryGenerator();
            var userDTO = CreateUserDTO(true);

            // Act
            var result1 = queryGenerator.GetUpdateRelationshipAuthorizationsWithQuery(userDTO, GenerateFriendshipAuthorizations(true, true));
            var result2 = queryGenerator.GetUpdateRelationshipAuthorizationsWithQuery(userDTO, GenerateFriendshipAuthorizations(true, false));
            var result3 = queryGenerator.GetUpdateRelationshipAuthorizationsWithQuery(userDTO, GenerateFriendshipAuthorizations(false, true));
            var result4 = queryGenerator.GetUpdateRelationshipAuthorizationsWithQuery(userDTO, GenerateFriendshipAuthorizations(false, false));

            // Assert
            string idParameter = UserQueryGeneratorHelper.GenerateParameterExpectedResult(userId);

            Assert.AreEqual(result1, GenerateUpdateQuery(true, true, idParameter));
            Assert.AreEqual(result2, GenerateUpdateQuery(true, false, idParameter));
            Assert.AreEqual(result3, GenerateUpdateQuery(false, true, idParameter));
            Assert.AreEqual(result4, GenerateUpdateQuery(false, false, idParameter));
        }

        [TestMethod]
        public void UpdateRelationshipAuthorizations_WithValidUserScreenName_AuthorizationsObjectIsNull_ReturnsNull()
        {
            var screenName = Guid.NewGuid().ToString();

            // Arrange
            var queryGenerator = CreateFrienshipQueryGenerator();
            _fakeUserQueryValidator.ArrangeIsScreenNameValid(screenName, true);

            // Act
            var result1 = queryGenerator.GetUpdateRelationshipAuthorizationsWithQuery(screenName, null);

            // Assert

            Assert.AreEqual(result1, null);
        }

        [TestMethod]
        public void UpdateRelationshipAuthorizations_WithInvalidUserScreenName_AuthorizationsObjectIsValid_ReturnsNull()
        {
            string screenName = TestHelper.GenerateString();

            // Arrange
            var queryGenerator = CreateFrienshipQueryGenerator();
            _fakeUserQueryValidator.ArrangeIsScreenNameValid(screenName, false);

            // Act
            var result1 = queryGenerator.GetUpdateRelationshipAuthorizationsWithQuery(screenName, GenerateFriendshipAuthorizations(true, true));
            var result2 = queryGenerator.GetUpdateRelationshipAuthorizationsWithQuery(screenName, GenerateFriendshipAuthorizations(true, false));
            var result3 = queryGenerator.GetUpdateRelationshipAuthorizationsWithQuery(screenName, GenerateFriendshipAuthorizations(false, true));
            var result4 = queryGenerator.GetUpdateRelationshipAuthorizationsWithQuery(screenName, GenerateFriendshipAuthorizations(false, false));

            // Assert
            Assert.AreEqual(result1, null);
            Assert.AreEqual(result2, null);
            Assert.AreEqual(result3, null);
            Assert.AreEqual(result4, null);
        }

        [TestMethod]
        public void UpdateRelationshipAuthorizations_WithValidUserScreenName_AuthorizationsObjectIsValid_ValidQuery()
        {
            var screenName = Guid.NewGuid().ToString();

            // Arrange
            var queryGenerator = CreateFrienshipQueryGenerator();
            _fakeUserQueryValidator.ArrangeIsScreenNameValid(screenName, true);

            // Act
            var result1 = queryGenerator.GetUpdateRelationshipAuthorizationsWithQuery(screenName, GenerateFriendshipAuthorizations(true, true));
            var result2 = queryGenerator.GetUpdateRelationshipAuthorizationsWithQuery(screenName, GenerateFriendshipAuthorizations(true, false));
            var result3 = queryGenerator.GetUpdateRelationshipAuthorizationsWithQuery(screenName, GenerateFriendshipAuthorizations(false, true));
            var result4 = queryGenerator.GetUpdateRelationshipAuthorizationsWithQuery(screenName, GenerateFriendshipAuthorizations(false, false));

            // Assert
            string idParameter = UserQueryGeneratorHelper.GenerateParameterExpectedResult(screenName);

            Assert.AreEqual(result1, GenerateUpdateQuery(true, true, idParameter));
            Assert.AreEqual(result2, GenerateUpdateQuery(true, false, idParameter));
            Assert.AreEqual(result3, GenerateUpdateQuery(false, true, idParameter));
            Assert.AreEqual(result4, GenerateUpdateQuery(false, false, idParameter));
        }

        [TestMethod]
        public void UpdateRelationshipAuthorizations_WithValidUserId_AuthorizationsObjectIsNull_ReturnsNull()
        {
            var userId = TestHelper.GenerateRandomLong();

            // Arrange
            var queryGenerator = CreateFrienshipQueryGenerator();

            // Act
            var result = queryGenerator.GetUpdateRelationshipAuthorizationsWithQuery(userId, null);

            // Assert
            Assert.AreEqual(result, null);
        }

        [TestMethod]
        public void UpdateRelationshipAuthorizations_WithInvalidUserId_AuthorizationsObjectIsValid_ReturnsNull()
        {
            // Arrange
            var queryGenerator = CreateFrienshipQueryGenerator();

            // Act
            var result1 = queryGenerator.GetUpdateRelationshipAuthorizationsWithQuery(TestHelper.DefaultId(), GenerateFriendshipAuthorizations(true, true));
            var result2 = queryGenerator.GetUpdateRelationshipAuthorizationsWithQuery(TestHelper.DefaultId(), GenerateFriendshipAuthorizations(true, false));
            var result3 = queryGenerator.GetUpdateRelationshipAuthorizationsWithQuery(TestHelper.DefaultId(), GenerateFriendshipAuthorizations(false, true));
            var result4 = queryGenerator.GetUpdateRelationshipAuthorizationsWithQuery(TestHelper.DefaultId(), GenerateFriendshipAuthorizations(false, false));

            // Assert
            Assert.AreEqual(result1, null);
            Assert.AreEqual(result2, null);
            Assert.AreEqual(result3, null);
            Assert.AreEqual(result4, null);
        }

        [TestMethod]
        public void UpdateRelationshipAuthorizations_WithUserId_AuthorizationsValid_ValidQuery()
        {
            var userId = TestHelper.GenerateRandomLong();

            // Arrange
            var queryGenerator = CreateFrienshipQueryGenerator();

            // Act
            var result1 = queryGenerator.GetUpdateRelationshipAuthorizationsWithQuery(userId, GenerateFriendshipAuthorizations(true, true));
            var result2 = queryGenerator.GetUpdateRelationshipAuthorizationsWithQuery(userId, GenerateFriendshipAuthorizations(true, false));
            var result3 = queryGenerator.GetUpdateRelationshipAuthorizationsWithQuery(userId, GenerateFriendshipAuthorizations(false, true));
            var result4 = queryGenerator.GetUpdateRelationshipAuthorizationsWithQuery(userId, GenerateFriendshipAuthorizations(false, false));

            // Assert
            string idParameter = UserQueryGeneratorHelper.GenerateParameterExpectedResult(userId);

            Assert.AreEqual(result1, GenerateUpdateQuery(true, true, idParameter));
            Assert.AreEqual(result2, GenerateUpdateQuery(true, false, idParameter));
            Assert.AreEqual(result3, GenerateUpdateQuery(false, true, idParameter));
            Assert.AreEqual(result4, GenerateUpdateQuery(false, false, idParameter));
        }

        private IFriendshipAuthorizations GenerateFriendshipAuthorizations(bool retweetEnabled, bool notificationEnabled)
        {
            var authorization = A.Fake<IFriendshipAuthorizations>();
            authorization.CallsTo(x => x.RetweetsEnabled).Returns(retweetEnabled);
            authorization.CallsTo(x => x.DeviceNotificationEnabled).Returns(notificationEnabled);
            return authorization;
        }

        private string GenerateUpdateQuery(bool retweetEnabled, bool notificationEnabled, string identifierParameter)
        {
            return string.Format(Resources.Friendship_Update, retweetEnabled.ToString().ToLowerInvariant(), notificationEnabled.ToString().ToLowerInvariant(), identifierParameter);
        }

        #endregion

        private IUserDTO CreateUserDTO(bool isValid)
        {
            var fakeUserDTO = A.Fake<IUserDTO>();

            _fakeUserQueryValidator.CallsTo(x => x.CanUserBeIdentified(fakeUserDTO)).Returns(isValid);
            _fakeUserQueryParameterGenerator.ArrangeGenerateIdOrScreenNameParameter();

            return fakeUserDTO;
        }

        public FriendshipQueryGenerator CreateFrienshipQueryGenerator()
        {
            return _fakeBuilder.GenerateClass();
        }
    }
}