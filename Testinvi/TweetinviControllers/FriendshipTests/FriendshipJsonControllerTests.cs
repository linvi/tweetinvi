using System;
using System.Collections.Generic;
using System.Threading.Tasks;
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
        public async Task GetUserIdsRequestingFriendship_ReturnsCursorQuery()
        {
            var expectedResult = new List<string> {Guid.NewGuid().ToString()};
            
            // Arrange
            var jsonController = CreateFriendshipJsonController();
            string query = Guid.NewGuid().ToString();

            ArrangeGetUserIdsRequestingFriendshipQuery(query);
            _fakeTwitterAccessor.ArrangeExecuteJsonCursorGETQuery<IIdsCursorQueryResultDTO>(query, expectedResult);

            // Act
            var result = await jsonController.GetUserIdsRequestingFriendship();

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
        public async Task GetUserIdsYouRequestedToFollow_ReturnsCursorQuery()
        {
            var expectedResult = new List<string> { Guid.NewGuid().ToString() };

            // Arrange
            var jsonController = CreateFriendshipJsonController();
            string query = Guid.NewGuid().ToString();

            ArrangeGetUserIdsYouRequestedToFollowQuery(query);
            _fakeTwitterAccessor.ArrangeExecuteJsonCursorGETQuery<IIdsCursorQueryResultDTO>(query, expectedResult);

            // Act
            var result = await jsonController.GetUserIdsYouRequestedToFollow();

            // Assert
            Assert.AreEqual(result, expectedResult);
        }

        private void ArrangeGetUserIdsYouRequestedToFollowQuery(string query)
        {
            _fakeFriendshipQueryGenerator
                .CallsTo(x => x.GetUserIdsYouRequestedToFollowQuery())
                .Returns(query);
        }

        #region Update Friendship Authorizations

        // User
        [TestMethod]
        public async Task UpdateRelationshipAuthorizations_UserWithNullUserDTO_ReturnsTwitterAccessorResult()
        {
            string expectedResult1 = Guid.NewGuid().ToString();
            string expectedResult2 = Guid.NewGuid().ToString();
            string expectedResult3 = Guid.NewGuid().ToString();
            string expectedResult4 = Guid.NewGuid().ToString();

            // Arrange - Act
            var result1 = await UpdateRelationshipAuthorizations_UserWithNullUserDTO_QueryExecutorReturns(true, true, expectedResult1);
            var result2 = await UpdateRelationshipAuthorizations_UserWithNullUserDTO_QueryExecutorReturns(true, false, expectedResult2);
            var result3 = await UpdateRelationshipAuthorizations_UserWithNullUserDTO_QueryExecutorReturns(false, true, expectedResult3);
            var result4 = await UpdateRelationshipAuthorizations_UserWithNullUserDTO_QueryExecutorReturns(false, false, expectedResult4);

            // Assert
            Assert.AreEqual(result1, expectedResult1);
            Assert.AreEqual(result2, expectedResult2);
            Assert.AreEqual(result3, expectedResult3);
            Assert.AreEqual(result4, expectedResult4);
        }

        private async Task<string> UpdateRelationshipAuthorizations_UserWithNullUserDTO_QueryExecutorReturns(bool retweetsEnabled, bool notification, string returnValue)
        {
            var user = A.Fake<IUser>();
            user.CallsTo(x => x.UserDTO).Returns(null);

            return await ArrangeUpdateRelationshipAuthorizations_User(user, retweetsEnabled, notification, returnValue).ConfigureAwait(false);
        }

        private async Task<string> ArrangeUpdateRelationshipAuthorizations_User(IUser user, bool retweetsEnabled, bool notification, string returnValue)
        {
            // Arrange
            var query = Guid.NewGuid().ToString();
            var jsonController = CreateFriendshipJsonController();
            var fakeAuthorizations = GenerateFriendshipAuthorizations(retweetsEnabled, notification);

            ArrangeGetUpdateRelationshipAuthorizationQuery(user, fakeAuthorizations, query);
            _fakeTwitterAccessor.ArrangeExecuteJsonPOSTQuery(query, returnValue);

            // Act
            return await jsonController.UpdateRelationshipAuthorizationsWith(user, retweetsEnabled, notification);
        }

        // User DTO
        [TestMethod]
        public async Task UpdateRelationshipAuthorizations_UserDTO_ReturnsTwitterAccessorResult()
        {
            string expectedResult1 = Guid.NewGuid().ToString();
            string expectedResult2 = Guid.NewGuid().ToString();
            string expectedResult3 = Guid.NewGuid().ToString();
            string expectedResult4 = Guid.NewGuid().ToString();

            // Arrange - Act
            var result1 = await UpdateRelationshipAuthorizations_UserDTO_QueryExecutorReturns(true, true, expectedResult1);
            var result2 = await UpdateRelationshipAuthorizations_UserDTO_QueryExecutorReturns(true, false, expectedResult2);
            var result3 = await UpdateRelationshipAuthorizations_UserDTO_QueryExecutorReturns(false, true, expectedResult3);
            var result4 = await UpdateRelationshipAuthorizations_UserDTO_QueryExecutorReturns(false, false, expectedResult4);

            // Assert
            Assert.AreEqual(result1, expectedResult1);
            Assert.AreEqual(result2, expectedResult2);
            Assert.AreEqual(result3, expectedResult3);
            Assert.AreEqual(result4, expectedResult4);
        }

        private async Task<string> UpdateRelationshipAuthorizations_UserDTO_QueryExecutorReturns(bool retweetsEnabled, bool notification, string returnValue)
        {
            var user = A.Fake<IUser>();
            return await ArrangeUpdateRelationshipAuthorizations_UserDTO(user, retweetsEnabled, notification, returnValue);
        }

        private async Task<string> ArrangeUpdateRelationshipAuthorizations_UserDTO(IUser user, bool retweetsEnabled, bool notification, string returnValue)
        {
            // Arrange
            var query = Guid.NewGuid().ToString();
            var jsonController = CreateFriendshipJsonController();
            var fakeAuthorizations = GenerateFriendshipAuthorizations(retweetsEnabled, notification);

            ArrangeGetUpdateRelationshipAuthorizationQuery(user, fakeAuthorizations, query);
            _fakeTwitterAccessor.ArrangeExecuteJsonPOSTQuery(query, returnValue);

            // Act
            return await jsonController.UpdateRelationshipAuthorizationsWith(user, retweetsEnabled, notification);
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

        private FriendshipJsonController CreateFriendshipJsonController()
        {
            return _fakeBuilder.GenerateClass();
        }
    }
}