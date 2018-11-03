using System;
using System.Collections.Generic;
using System.Linq;
using FakeItEasy;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Testinvi.Helpers;
using Testinvi.SetupHelpers;
using Tweetinvi.Controllers.Friendship;
using Tweetinvi.Core.QueryGenerators;
using Tweetinvi.Core.QueryValidators;
using Tweetinvi.Core.Web;
using Tweetinvi.Models;
using Tweetinvi.Models.DTO;
using Tweetinvi.Models.DTO.QueryDTO;

namespace Testinvi.TweetinviControllers.FriendshipTests
{
    [TestClass]
    public class FriendshipQueryExecutorTests
    {
        private FakeClassBuilder<FriendshipQueryExecutor> _fakeBuilder;
        
        private IFriendshipQueryGenerator _friendshipQueryGenerator;
        private IUserQueryValidator _userQueryValidator;
        private ITwitterAccessor _twitterAccessor;

        [TestInitialize]
        public void TestInitialize()
        {
            _fakeBuilder = new FakeClassBuilder<FriendshipQueryExecutor>();
            _friendshipQueryGenerator = _fakeBuilder.GetFake<IFriendshipQueryGenerator>().FakedObject;
            _userQueryValidator = _fakeBuilder.GetFake<IUserQueryValidator>().FakedObject;
            _twitterAccessor = _fakeBuilder.GetFake<ITwitterAccessor>().FakedObject;
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

            _twitterAccessor.ArrangeExecuteCursorGETQuery<long, IIdsCursorQueryResultDTO>(query, ids);

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

            _twitterAccessor.ArrangeExecuteCursorGETQuery<long, IIdsCursorQueryResultDTO>(query, ids);

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

            _twitterAccessor.ArrangeExecuteCursorGETQuery<long, IIdsCursorQueryResultDTO>(query, null);

            // Act
            var result = queryExecutor.GetUserIdsRequestingFriendship(TestHelper.GenerateRandomInt());

            // Assert
            Assert.AreEqual(result, null);
        }

        private void ArrangeGetUserIdsRequestingFriendshipQuery(string query)
        {
            A.CallTo(() => _friendshipQueryGenerator.GetUserIdsRequestingFriendshipQuery()).Returns(query);
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

            _twitterAccessor.ArrangeExecuteCursorGETQuery<long, IIdsCursorQueryResultDTO>(query, ids);

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

            _twitterAccessor.ArrangeExecuteCursorGETQuery<long, IIdsCursorQueryResultDTO>(query, ids);

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

            _twitterAccessor.ArrangeExecuteCursorGETQuery<long, IIdsCursorQueryResultDTO>(query, null);

            // Act
            var result = queryExecutor.GetUserIdsYouRequestedToFollow(TestHelper.GenerateRandomInt());

            // Assert
            Assert.AreEqual(result, null);
        }

        private void ArrangeGetUserIdsYouRequestedToFollowQuery(string query)
        {
            A.CallTo(() => _friendshipQueryGenerator.GetUserIdsYouRequestedToFollowQuery()).Returns(query);
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

        private bool CreateFriendshipWith_UserDTO_Returns(bool result)
        {
            var userDTO = A.Fake<IUserDTO>();
            string query = Guid.NewGuid().ToString();

            // Arrange
            var queryExecutor = CreateFrienshipQueryExecutor();
            ArrangeCreateFriendshipWithUserDTO(userDTO, query);
            _twitterAccessor.ArrangeTryExecutePOSTQuery(query, result);

            // Act
            return queryExecutor.CreateFriendshipWith(userDTO);
        }

        private void ArrangeCreateFriendshipWithUserDTO(IUserDTO userDTO, string query)
        {
            A.CallTo(() => _friendshipQueryGenerator.GetCreateFriendshipWithQuery(userDTO)).Returns(query);
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

        private bool UpdateRelationshipAuthorizationsWith_UserDTO_Returns(bool isValid, bool userDTOIsNotNull, bool authParamsAreNotNull, bool result)
        {
            var userDTO = userDTOIsNotNull ? A.Fake<IUserDTO>() : null;

            if (userDTOIsNotNull)
            {
                A.CallTo(() => _userQueryValidator.CanUserBeIdentified(userDTO)).Returns(isValid);
            }

            var authorizationParameter = authParamsAreNotNull ? A.Fake<IFriendshipAuthorizations>() : null;

            string query = Guid.NewGuid().ToString();

            // Arrange
            var queryExecutor = CreateFrienshipQueryExecutor();
            ArrangeUpdateRelationshipAuthorizationsWithUserDTO(userDTO, authorizationParameter, query);
            _twitterAccessor.ArrangeTryExecutePOSTQuery(query, result);

            // Act
            return queryExecutor.UpdateRelationshipAuthorizationsWith(userDTO, authorizationParameter);
        }

        private void ArrangeUpdateRelationshipAuthorizationsWithUserDTO(IUserDTO userDTO, IFriendshipAuthorizations auth, string query)
        {
            A.CallTo(() => _friendshipQueryGenerator.GetUpdateRelationshipAuthorizationsWithQuery(userDTO, auth))
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

        private bool DestroyFriendshipWith_UserDTO_Returns(bool isValid, bool result)
        {
            var userDTO = A.Fake<IUserDTO>();
            A.CallTo(() => _userQueryValidator.CanUserBeIdentified(userDTO)).Returns(isValid);

            string query = Guid.NewGuid().ToString();

            // Arrange
            var queryExecutor = CreateFrienshipQueryExecutor();
            ArrangeDestroyFriendshipWithUserDTO(userDTO, query);
            _twitterAccessor.ArrangeTryExecutePOSTQuery(query, result);

            // Act
            return queryExecutor.DestroyFriendshipWith(userDTO);
        }

        private void ArrangeDestroyFriendshipWithUserDTO(IUserDTO userDTO, string query)
        {
            A.CallTo(() => _friendshipQueryGenerator.GetDestroyFriendshipWithQuery(userDTO)).Returns(query);
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