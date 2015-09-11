using System;
using System.Collections.Generic;
using System.Linq;
using FakeItEasy;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Testinvi.Helpers;
using Testinvi.SetupHelpers;
using Tweetinvi.Controllers.Friendship;
using Tweetinvi.Core.Interfaces.Credentials;
using Tweetinvi.Core.Interfaces.DTO;
using Tweetinvi.Core.Interfaces.DTO.QueryDTO;
using Tweetinvi.Core.Interfaces.Models;
using Tweetinvi.Core.Interfaces.QueryGenerators;
using Tweetinvi.Core.Interfaces.QueryValidators;

namespace Testinvi.TweetinviControllers.FriendshipTests
{
    [TestClass]
    public class FriendshipQueryExecutorTests
    {
        private FakeClassBuilder<FriendshipQueryExecutor> _fakeBuilder;
        private Fake<IFriendshipQueryGenerator> _fakeFriendshipQueryGenerator;
        private Fake<IUserQueryValidator> _fakeUserQueryValidator;
        private Fake<ITwitterAccessor> _fakeTwitterAccessor;

        [TestInitialize]
        public void TestInitialize()
        {
            _fakeBuilder = new FakeClassBuilder<FriendshipQueryExecutor>();
            _fakeFriendshipQueryGenerator = _fakeBuilder.GetFake<IFriendshipQueryGenerator>();
            _fakeUserQueryValidator = _fakeBuilder.GetFake<IUserQueryValidator>();
            _fakeTwitterAccessor = _fakeBuilder.GetFake<ITwitterAccessor>();
        }

        #region Get UserIds Requesting Friendship

        [TestMethod]
        public void GetUserIdsRequestingFriendship_ReturnsQueryExecutor()
        {
            string query = Guid.NewGuid().ToString();
            var ids = new long[] { new Random().Next(), new Random().Next() };

            // Arrange
            var queryExecutor = CreateFrienshipQueryExecutor();
            ArrangeGetUserIdsRequestingFriendshipQuery(query);

            IEnumerable<IIdsCursorQueryResultDTO> userIds = new List<IIdsCursorQueryResultDTO>
            {
                GenerateIdsCursorQueryResult(ids)
            };

            _fakeTwitterAccessor.ArrangeExecuteCursorGETQuery<long, IIdsCursorQueryResultDTO>(query, ids);

            // Act
            var result = queryExecutor.GetUserIdsRequestingFriendship(TestHelper.GenerateRandomInt());

            // Assert
            Assert.IsTrue(result.All(ids.Contains));
            Assert.IsTrue(ids.All(result.Contains));
        }

        [TestMethod]
        public void GetUserIdsRequestingFriendship_MultipleCursorResult_ContainsAllIds()
        {
            string query = Guid.NewGuid().ToString();
            var ids = new long[] { new Random().Next(), new Random().Next() };
            var ids2 = new long[] { new Random().Next(), new Random().Next() };

            // Arrange
            var queryExecutor = CreateFrienshipQueryExecutor();
            ArrangeGetUserIdsRequestingFriendshipQuery(query);

            IEnumerable<IIdsCursorQueryResultDTO> userIds = new List<IIdsCursorQueryResultDTO>
            {
                GenerateIdsCursorQueryResult(ids),
                GenerateIdsCursorQueryResult(ids2)
            };

            _fakeTwitterAccessor.ArrangeExecuteCursorGETQuery<long, IIdsCursorQueryResultDTO>(query, ids);

            // Act
            var result = queryExecutor.GetUserIdsRequestingFriendship(TestHelper.GenerateRandomInt());

            // Assert
            Assert.IsTrue(result.All(x => ids.Contains(x) || ids2.Contains(x)));
            Assert.IsTrue(ids.ContainsAll(ids2));
        }

        [TestMethod]
        public void GetUserIdsRequestingFriendship_QueryExecutorReturnsNull_ReturnsNull()
        {
            string query = Guid.NewGuid().ToString();

            // Arrange
            var queryExecutor = CreateFrienshipQueryExecutor();
            ArrangeGetUserIdsRequestingFriendshipQuery(query);

            _fakeTwitterAccessor.ArrangeExecuteCursorGETQuery<long, IIdsCursorQueryResultDTO>(query, null);

            // Act
            var result = queryExecutor.GetUserIdsRequestingFriendship(TestHelper.GenerateRandomInt());

            // Assert
            Assert.AreEqual(result, null);
        }

        private void ArrangeGetUserIdsRequestingFriendshipQuery(string query)
        {
            _fakeFriendshipQueryGenerator
                .CallsTo(x => x.GetUserIdsRequestingFriendshipQuery())
                .Returns(query);
        }

        #endregion

        #region Get UserIds Requesting Friendship

        [TestMethod]
        public void GetUserIdsYouRequestedToFollow_ReturnsQueryExecutor()
        {
            string query = Guid.NewGuid().ToString();
            var ids = new long[] { new Random().Next(), new Random().Next() };

            // Arrange
            var queryExecutor = CreateFrienshipQueryExecutor();
            ArrangeGetUserIdsYouRequestedToFollowQuery(query);

            IEnumerable<IIdsCursorQueryResultDTO> userIds = new List<IIdsCursorQueryResultDTO>
            {
                GenerateIdsCursorQueryResult(ids)
            };

            _fakeTwitterAccessor.ArrangeExecuteCursorGETQuery<long, IIdsCursorQueryResultDTO>(query, ids);

            // Act
            var result = queryExecutor.GetUserIdsYouRequestedToFollow(TestHelper.GenerateRandomInt());

            // Assert
            Assert.IsTrue(result.All(ids.Contains));
            Assert.IsTrue(ids.All(result.Contains));
        }

        [TestMethod]
        public void GetUserIdsYouRequestedToFollow_MultipleCursorResult_ContainsAllIds()
        {
            string query = Guid.NewGuid().ToString();
            var ids = new long[] { new Random().Next(), new Random().Next() };
            var ids2 = new long[] { new Random().Next(), new Random().Next() };

            // Arrange
            var queryExecutor = CreateFrienshipQueryExecutor();
            ArrangeGetUserIdsYouRequestedToFollowQuery(query);

            IEnumerable<IIdsCursorQueryResultDTO> userIds = new List<IIdsCursorQueryResultDTO>
            {
                GenerateIdsCursorQueryResult(ids),
                GenerateIdsCursorQueryResult(ids2)
            };

            _fakeTwitterAccessor.ArrangeExecuteCursorGETQuery<long, IIdsCursorQueryResultDTO>(query, ids);

            // Act
            var result = queryExecutor.GetUserIdsYouRequestedToFollow(TestHelper.GenerateRandomInt());

            // Assert
            Assert.IsTrue(result.All(x => ids.Contains(x) || ids2.Contains(x)));
            Assert.IsTrue(ids.All(result.Contains));
            Assert.IsTrue(ids2.All(result.Contains));
        }

        [TestMethod]
        public void GetUserIdsYouRequestedToFollow_QueryExecutorReturnsNull_ReturnsNull()
        {
            string query = Guid.NewGuid().ToString();

            // Arrange
            var queryExecutor = CreateFrienshipQueryExecutor();
            ArrangeGetUserIdsYouRequestedToFollowQuery(query);

            _fakeTwitterAccessor.ArrangeExecuteCursorGETQuery<long, IIdsCursorQueryResultDTO>(query, null);

            // Act
            var result = queryExecutor.GetUserIdsYouRequestedToFollow(TestHelper.GenerateRandomInt());

            // Assert
            Assert.AreEqual(result, null);
        }

        private void ArrangeGetUserIdsYouRequestedToFollowQuery(string query)
        {
            _fakeFriendshipQueryGenerator
                .CallsTo(x => x.GetUserIdsYouRequestedToFollowQuery())
                .Returns(query);
        }

        #endregion

        #region Create Friendship With

        [TestMethod]
        public void CreateFriendshipWith_UserDTO_ReturnsQueryExecutor()
        {
            // Arrange - Act
            var shouldSucceed = CreateFriendshipWith_UserDTO_Returns(true);
            var shouldFail = CreateFriendshipWith_UserDTO_Returns(false);

            // Assert
            Assert.IsTrue(shouldSucceed);
            Assert.IsFalse(shouldFail);
        }

        [TestMethod]
        public void CreateFriendshipWith_UserScreenName_ReturnsQueryExecutor()
        {
            // Arrange - Act
            var shouldSucceed = CreateFriendshipWith_UserScreenName_Returns(true);
            var shouldFail = CreateFriendshipWith_UserScreenName_Returns(false);

            // Assert
            Assert.IsTrue(shouldSucceed);
            Assert.IsFalse(shouldFail);
        }

        [TestMethod]
        public void CreateFriendshipWith_UserId_ReturnsQueryExecutor()
        {
            // Arrange - Act
            var shouldSucceed = CreateFriendshipWith_UserId_Returns(true);
            var shouldFail = CreateFriendshipWith_UserId_Returns(false);

            // Assert
            Assert.IsTrue(shouldSucceed);
            Assert.IsFalse(shouldFail);
        }

        private bool CreateFriendshipWith_UserDTO_Returns(bool result)
        {
            var userDTO = A.Fake<IUserDTO>();
            string query = Guid.NewGuid().ToString();

            // Arrange
            var queryExecutor = CreateFrienshipQueryExecutor();
            ArrangeCreateFriendshipWithUserDTO(userDTO, query);
            _fakeTwitterAccessor.ArrangeTryExecutePOSTQuery(query, result);

            // Act
            return queryExecutor.CreateFriendshipWith(userDTO);
        }

        private void ArrangeCreateFriendshipWithUserDTO(IUserDTO userDTO, string query)
        {
            _fakeFriendshipQueryGenerator
                .CallsTo(x => x.GetCreateFriendshipWithQuery(userDTO))
                .Returns(query);
        }

        private bool CreateFriendshipWith_UserScreenName_Returns(bool result)
        {
            string userScreenName = Guid.NewGuid().ToString();
            string query = Guid.NewGuid().ToString();

            // Arrange
            var queryExecutor = CreateFrienshipQueryExecutor();
            ArrangeCreateFriendshipWithUserScreenName(userScreenName, query);
            _fakeTwitterAccessor.ArrangeTryExecutePOSTQuery(query, result);

            // Act
            return queryExecutor.CreateFriendshipWith(userScreenName);
        }

        private void ArrangeCreateFriendshipWithUserScreenName(string userScreenName, string query)
        {
            _fakeFriendshipQueryGenerator
                .CallsTo(x => x.GetCreateFriendshipWithQuery(userScreenName))
                .Returns(query);
        }

        private bool CreateFriendshipWith_UserId_Returns(bool result)
        {
            var userId = TestHelper.GenerateRandomLong();
            string query = Guid.NewGuid().ToString();

            // Arrange
            var queryExecutor = CreateFrienshipQueryExecutor();
            ArrangeCreateFriendshipWithUserDTO(userId, query);
            _fakeTwitterAccessor.ArrangeTryExecutePOSTQuery(query, result);

            // Act
            return queryExecutor.CreateFriendshipWith(userId);
        }

        private void ArrangeCreateFriendshipWithUserDTO(long userId, string query)
        {
            _fakeFriendshipQueryGenerator
                .CallsTo(x => x.GetCreateFriendshipWithQuery(userId))
                .Returns(query);
        }

        #endregion

        #region Update Friendship With

        [TestMethod]
        public void UpdateRelationshipAuthorizationsWith_UserDTO_ReturnsQueryExecutor()
        {
            // Arrange - Act
            var shouldFail1 = UpdateRelationshipAuthorizationsWith_UserDTO_Returns(false, false, false, false);
            var shouldFail2 = UpdateRelationshipAuthorizationsWith_UserDTO_Returns(false, false, false, true);
            var shouldFail3 = UpdateRelationshipAuthorizationsWith_UserDTO_Returns(false, false, true, false);
            var shouldFail4 = UpdateRelationshipAuthorizationsWith_UserDTO_Returns(false, false, true, true);
            var shouldFail5 = UpdateRelationshipAuthorizationsWith_UserDTO_Returns(false, true, false, false);
            var shouldFail6 = UpdateRelationshipAuthorizationsWith_UserDTO_Returns(false, true, false, true);
            var shouldFail7 = UpdateRelationshipAuthorizationsWith_UserDTO_Returns(false, true, true, false);
            var shouldFail8 = UpdateRelationshipAuthorizationsWith_UserDTO_Returns(false, true, true, true);

            var shouldFail9 = UpdateRelationshipAuthorizationsWith_UserDTO_Returns(true, false, false, false);
            var shouldFail10 = UpdateRelationshipAuthorizationsWith_UserDTO_Returns(true, false, false, true);
            var shouldFail11 = UpdateRelationshipAuthorizationsWith_UserDTO_Returns(true, false, true, false);
            var shouldFail12 = UpdateRelationshipAuthorizationsWith_UserDTO_Returns(true, false, true, true);
            var shouldFail13 = UpdateRelationshipAuthorizationsWith_UserDTO_Returns(true, true, false, false);
            var shouldSucceed1 = UpdateRelationshipAuthorizationsWith_UserDTO_Returns(true, true, false, true);
            var shouldFail14 = UpdateRelationshipAuthorizationsWith_UserDTO_Returns(true, true, true, false);
            var shouldSucceed2 = UpdateRelationshipAuthorizationsWith_UserDTO_Returns(true, true, true, true);

            // Assert
            Assert.IsTrue(shouldSucceed1);
            Assert.IsTrue(shouldSucceed2);

            Assert.IsFalse(shouldFail1);
            Assert.IsFalse(shouldFail2);
            Assert.IsFalse(shouldFail3);
            Assert.IsFalse(shouldFail4);
            Assert.IsFalse(shouldFail5);
            Assert.IsFalse(shouldFail6);
            Assert.IsFalse(shouldFail7);
            Assert.IsFalse(shouldFail8);
            Assert.IsFalse(shouldFail9);
            Assert.IsFalse(shouldFail10);
            Assert.IsFalse(shouldFail11);
            Assert.IsFalse(shouldFail12);
            Assert.IsFalse(shouldFail13);
            Assert.IsFalse(shouldFail14);
        }

        [TestMethod]
        public void UpdateRelationshipAuthorizationsWith_UserScreenName_ReturnsQueryExecutor()
        {
            // Arrange - Act
            var shouldSucceed1 = UpdateRelationshipAuthorizationsWith_UserScreenName_Returns(true, true);
            var shouldFail1 = UpdateRelationshipAuthorizationsWith_UserScreenName_Returns(true, false);
            var shouldSucceed2 = UpdateRelationshipAuthorizationsWith_UserScreenName_Returns(false, true);
            var shouldFail2 = UpdateRelationshipAuthorizationsWith_UserScreenName_Returns(false, false);

            // Assert
            Assert.IsTrue(shouldSucceed1);
            Assert.IsTrue(shouldSucceed2);
            Assert.IsFalse(shouldFail1);
            Assert.IsFalse(shouldFail2);
        }

        [TestMethod]
        public void UpdateRelationshipAuthorizationsWith_UserId_ReturnsQueryExecutor()
        {
            // Arrange - Act
            var shouldSucceed1 = UpdateRelationshipAuthorizationsWith_UserId_Returns(true, true);
            var shouldFail1 = UpdateRelationshipAuthorizationsWith_UserId_Returns(true, false);
            var shouldSucceed2 = UpdateRelationshipAuthorizationsWith_UserId_Returns(false, true);
            var shouldFail2 = UpdateRelationshipAuthorizationsWith_UserId_Returns(false, false);

            // Assert
            Assert.IsTrue(shouldSucceed1);
            Assert.IsTrue(shouldSucceed2);
            Assert.IsFalse(shouldFail1);
            Assert.IsFalse(shouldFail2);
        }

        private bool UpdateRelationshipAuthorizationsWith_UserDTO_Returns(bool isValid, bool userDTOIsNotNull, bool authParamsAreNotNull, bool result)
        {
            var userDTO = userDTOIsNotNull ? A.Fake<IUserDTO>() : null;

            if (userDTOIsNotNull)
            {
                _fakeUserQueryValidator.CallsTo(x => x.CanUserBeIdentified(userDTO)).Returns(isValid);
            }

            var authorizationParameter = authParamsAreNotNull ? A.Fake<IFriendshipAuthorizations>() : null;

            string query = Guid.NewGuid().ToString();

            // Arrange
            var queryExecutor = CreateFrienshipQueryExecutor();
            ArrangeUpdateRelationshipAuthorizationsWithUserDTO(userDTO, authorizationParameter, query);
            _fakeTwitterAccessor.ArrangeTryExecutePOSTQuery(query, result);

            // Act
            return queryExecutor.UpdateRelationshipAuthorizationsWith(userDTO, authorizationParameter);
        }

        private void ArrangeUpdateRelationshipAuthorizationsWithUserDTO(IUserDTO userDTO, IFriendshipAuthorizations auth, string query)
        {
            _fakeFriendshipQueryGenerator
                .CallsTo(x => x.GetUpdateRelationshipAuthorizationsWithQuery(userDTO, auth))
                .Returns(query);
        }

        private bool UpdateRelationshipAuthorizationsWith_UserScreenName_Returns(bool authParamsAreNotNull, bool result)
        {
            string userScreenName = Guid.NewGuid().ToString();
            string query = Guid.NewGuid().ToString();
            var authorizationParameter = authParamsAreNotNull ? A.Fake<IFriendshipAuthorizations>() : null;

            // Arrange
            var queryExecutor = CreateFrienshipQueryExecutor();
            ArrangeUpdateRelationshipAuthorizationsWithUserScreenName(userScreenName, authorizationParameter, query);
            _fakeTwitterAccessor.ArrangeTryExecutePOSTQuery(query, result);

            // Act
            return queryExecutor.UpdateRelationshipAuthorizationsWith(userScreenName, authorizationParameter);
        }

        private void ArrangeUpdateRelationshipAuthorizationsWithUserScreenName(string userScreenName, IFriendshipAuthorizations auth, string query)
        {
            _fakeFriendshipQueryGenerator
                .CallsTo(x => x.GetUpdateRelationshipAuthorizationsWithQuery(userScreenName, auth))
                .Returns(query);
        }

        private bool UpdateRelationshipAuthorizationsWith_UserId_Returns(bool authParamsAreNotNull, bool result)
        {
            var userId = TestHelper.GenerateRandomLong();
            string query = Guid.NewGuid().ToString();
            var authorizationParameter = authParamsAreNotNull ? A.Fake<IFriendshipAuthorizations>() : null;

            // Arrange
            var queryExecutor = CreateFrienshipQueryExecutor();
            ArrangeUpdateRelationshipAuthorizationsWithUserDTO(userId, authorizationParameter, query);
            _fakeTwitterAccessor.ArrangeTryExecutePOSTQuery(query, result);

            // Act
            return queryExecutor.UpdateRelationshipAuthorizationsWith(userId, authorizationParameter);
        }

        private void ArrangeUpdateRelationshipAuthorizationsWithUserDTO(long userId, IFriendshipAuthorizations auth, string query)
        {
            _fakeFriendshipQueryGenerator
                .CallsTo(x => x.GetUpdateRelationshipAuthorizationsWithQuery(userId, auth))
                .Returns(query);
        }

        #endregion

        #region Destroy Friendship With

        [TestMethod]
        public void DestroyFriendshipWith_UserDTO_ReturnsQueryExecutor()
        {
            // Arrange - Act
            var shouldSucceed = DestroyFriendshipWith_UserDTO_Returns(true, true);
            var shouldFail1 = DestroyFriendshipWith_UserDTO_Returns(true, false);
            var shouldFail2 = DestroyFriendshipWith_UserDTO_Returns(false, true);
            var shouldFail3 = DestroyFriendshipWith_UserDTO_Returns(false, false);

            // Assert
            Assert.IsTrue(shouldSucceed);
            Assert.IsFalse(shouldFail1);
            Assert.IsFalse(shouldFail2);
            Assert.IsFalse(shouldFail3);
        }

        [TestMethod]
        public void DestroyFriendshipWith_UserScreenName_ReturnsQueryExecutor()
        {
            // Arrange - Act
            var shouldSucceed = DestroyFriendshipWith_UserScreenName_Returns(true);
            var shouldFail = DestroyFriendshipWith_UserScreenName_Returns(false);

            // Assert
            Assert.IsTrue(shouldSucceed);
            Assert.IsFalse(shouldFail);
        }

        [TestMethod]
        public void DestroyFriendshipWith_UserId_ReturnsQueryExecutor()
        {
            // Arrange - Act
            var shouldSucceed = DestroyFriendshipWith_UserId_Returns(true);
            var shouldFail = DestroyFriendshipWith_UserId_Returns(false);

            // Assert
            Assert.IsTrue(shouldSucceed);
            Assert.IsFalse(shouldFail);
        }

        private bool DestroyFriendshipWith_UserDTO_Returns(bool isValid, bool result)
        {
            var userDTO = A.Fake<IUserDTO>();
            _fakeUserQueryValidator.CallsTo(x => x.CanUserBeIdentified(userDTO)).Returns(isValid);

            string query = Guid.NewGuid().ToString();

            // Arrange
            var queryExecutor = CreateFrienshipQueryExecutor();
            ArrangeDestroyFriendshipWithUserDTO(userDTO, query);
            _fakeTwitterAccessor.ArrangeTryExecutePOSTQuery(query, result);

            // Act
            return queryExecutor.DestroyFriendshipWith(userDTO);
        }

        private void ArrangeDestroyFriendshipWithUserDTO(IUserDTO userDTO, string query)
        {
            _fakeFriendshipQueryGenerator
                .CallsTo(x => x.GetDestroyFriendshipWithQuery(userDTO))
                .Returns(query);
        }

        private bool DestroyFriendshipWith_UserScreenName_Returns(bool result)
        {
            string userScreenName = Guid.NewGuid().ToString();
            string query = Guid.NewGuid().ToString();

            // Arrange
            var queryExecutor = CreateFrienshipQueryExecutor();
            ArrangeDestroyFriendshipWithUserScreenName(userScreenName, query);
            _fakeTwitterAccessor.ArrangeTryExecutePOSTQuery(query, result);

            // Act
            return queryExecutor.DestroyFriendshipWith(userScreenName);
        }

        private void ArrangeDestroyFriendshipWithUserScreenName(string userScreenName, string query)
        {
            _fakeFriendshipQueryGenerator
                .CallsTo(x => x.GetDestroyFriendshipWithQuery(userScreenName))
                .Returns(query);
        }

        private bool DestroyFriendshipWith_UserId_Returns(bool result)
        {
            var userId = TestHelper.GenerateRandomLong();
            string query = Guid.NewGuid().ToString();

            // Arrange
            var queryExecutor = CreateFrienshipQueryExecutor();
            ArrangeDestroyFriendshipWithUserDTO(userId, query);
            _fakeTwitterAccessor.ArrangeTryExecutePOSTQuery(query, result);

            // Act
            return queryExecutor.DestroyFriendshipWith(userId);
        }

        private void ArrangeDestroyFriendshipWithUserDTO(long userId, string query)
        {
            _fakeFriendshipQueryGenerator
                .CallsTo(x => x.GetDestroyFriendshipWithQuery(userId))
                .Returns(query);
        }

        #endregion

        private IIdsCursorQueryResultDTO GenerateIdsCursorQueryResult(long[] ids)
        {
            var fakeIdsCursorQueryResult = A.Fake<IIdsCursorQueryResultDTO>();
            fakeIdsCursorQueryResult.Ids = ids;
            return fakeIdsCursorQueryResult;
        }

        public FriendshipQueryExecutor CreateFrienshipQueryExecutor()
        {
            return _fakeBuilder.GenerateClass();
        }
    }
}