using System;
using FakeItEasy;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Testinvi.Helpers;
using Testinvi.SetupHelpers;
using Tweetinvi.Controllers.Friendship;
using Tweetinvi.Controllers.Properties;
using Tweetinvi.Core.QueryGenerators;
using Tweetinvi.Core.QueryValidators;
using Tweetinvi.Models;
using Tweetinvi.Models.DTO;

namespace Testinvi.TweetinviControllers.FriendshipTests
{
    [TestClass]
    public class FriendshipQueryGeneratorTests
    {
        private FakeClassBuilder<FriendshipQueryGenerator> _fakeBuilder;
        private IUserQueryParameterGenerator _userQueryParameterGenerator;
        private IUserQueryValidator _userQueryValidator;

        [TestInitialize]
        public void TestInitialize()
        {
            _fakeBuilder = new FakeClassBuilder<FriendshipQueryGenerator>();
            _userQueryParameterGenerator = _fakeBuilder.GetFake<IUserQueryParameterGenerator>().FakedObject;
            _userQueryValidator = _fakeBuilder.GetFake<IUserQueryValidator>().FakedObject;

            _userQueryParameterGenerator.ArrangeGenerateIdParameter();
            _userQueryParameterGenerator.ArrangeGenerateScreenNameParameter();
            _userQueryParameterGenerator.ArrangeGenerateIdOrScreenNameParameter();
            _userQueryValidator.ArrangeIsUserIdValid();
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

            A.CallTo(() => _userQueryValidator.ThrowIfUserCannotBeIdentified(fakeUserDTO)).MustHaveHappened();
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

            A.CallTo(() => _userQueryValidator.ThrowIfUserCannotBeIdentified(userDTO)).MustHaveHappened();
        }

        #endregion

        #region Update Friendship Authorizations

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void UpdateRelationshipAuthorizations_WithUserDTO_AuthorizationsObjectIsNull_ReturnsNull()
        {
            var userDTO = CreateUserDTO(true);

            // Arrange
            var queryGenerator = CreateFrienshipQueryGenerator();

            // Act
            queryGenerator.GetUpdateRelationshipAuthorizationsWithQuery(userDTO, null);
        }

        [TestMethod]
        public void UpdateRelationshipAuthorizations_WithValidUserDTO_AuthorizationsObjectIsValid_ValidQuery()
        {
            // Arrange
            var queryGenerator = CreateFrienshipQueryGenerator();
            var userDTO = A.Fake<IUserDTO>();
            
            _userQueryValidator.ArrangeCanUserBeIdentified(userDTO, true);
            _userQueryParameterGenerator.ArrangeGenerateIdOrScreenNameParameter();

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

            A.CallTo(() => _userQueryValidator.ThrowIfUserCannotBeIdentified(userDTO))
                .MustHaveHappened(Repeated.Exactly.Times(4));
        }

        private IFriendshipAuthorizations GenerateFriendshipAuthorizations(bool retweetEnabled, bool notificationEnabled)
        {
            var friendshipAuthorizations = A.Fake<IFriendshipAuthorizations>();
            A.CallTo(() => friendshipAuthorizations.RetweetsEnabled).Returns(retweetEnabled);
            A.CallTo(() => friendshipAuthorizations.DeviceNotificationEnabled).Returns(notificationEnabled);
            return friendshipAuthorizations;
        }

        private string GenerateUpdateQuery(bool retweetEnabled, bool notificationEnabled, string identifierParameter)
        {
            return string.Format(Resources.Friendship_Update, retweetEnabled.ToString().ToLowerInvariant(), notificationEnabled.ToString().ToLowerInvariant(), identifierParameter);
        }

        #endregion

        private IUserDTO CreateUserDTO(bool isValid)
        {
            var fakeUserDTO = A.Fake<IUserDTO>();

            A.CallTo(() => _userQueryValidator.CanUserBeIdentified(fakeUserDTO)).Returns(isValid);
            _userQueryParameterGenerator.ArrangeGenerateIdOrScreenNameParameter();

            return fakeUserDTO;
        }

        public FriendshipQueryGenerator CreateFrienshipQueryGenerator()
        {
            return _fakeBuilder.GenerateClass();
        }
    }
}