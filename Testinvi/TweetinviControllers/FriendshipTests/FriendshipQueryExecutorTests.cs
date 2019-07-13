using System;
using System.Linq;
using System.Threading.Tasks;
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
        public async Task GetUserIdsRequestingFriendship_ReturnsQueryExecutor()
        {
            string query = Guid.NewGuid().ToString();
            var ids = new long[] { new Random().Next(), new Random().Next() };

            // Arrange
            var queryExecutor = CreateFriendshipQueryExecutor();
            ArrangeGetUserIdsRequestingFriendshipQuery(query);

            _fakeTwitterAccessor.ArrangeExecuteCursorGETQuery<long, IIdsCursorQueryResultDTO>(query, ids);

            // Act
            var result = (await queryExecutor.GetUserIdsRequestingFriendship(TestHelper.GenerateRandomInt())).ToArray();

            // Assert
            Assert.IsTrue(result.All(ids.Contains));
            Assert.IsTrue(ids.All(result.Contains));
        }

        [TestMethod]
        public async Task GetUserIdsRequestingFriendship_MultipleCursorResult_ContainsAllIds()
        {
            string query = Guid.NewGuid().ToString();
            var ids = new long[] { new Random().Next(), new Random().Next() };
            var ids2 = new long[] { new Random().Next(), new Random().Next() };

            // Arrange
            var queryExecutor = CreateFriendshipQueryExecutor();
            ArrangeGetUserIdsRequestingFriendshipQuery(query);

            _fakeTwitterAccessor.ArrangeExecuteCursorGETQuery<long, IIdsCursorQueryResultDTO>(query, ids);

            // Act
            var result = await queryExecutor.GetUserIdsRequestingFriendship(TestHelper.GenerateRandomInt());

            // Assert
            Assert.IsTrue(result.All(x => ids.Contains(x) || ids2.Contains(x)));
            Assert.IsTrue(ids.ContainsAll(ids2));
        }

        [TestMethod]
        public async Task GetUserIdsRequestingFriendship_QueryExecutorReturnsNull_ReturnsNull()
        {
            string query = Guid.NewGuid().ToString();

            // Arrange
            var queryExecutor = CreateFriendshipQueryExecutor();
            ArrangeGetUserIdsRequestingFriendshipQuery(query);

            _fakeTwitterAccessor.ArrangeExecuteCursorGETQuery<long, IIdsCursorQueryResultDTO>(query, null);

            // Act
            var result = await queryExecutor.GetUserIdsRequestingFriendship(TestHelper.GenerateRandomInt());

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
        public async Task GetUserIdsYouRequestedToFollow_ReturnsQueryExecutor()
        {
            string query = Guid.NewGuid().ToString();
            var ids = new long[] { new Random().Next(), new Random().Next() };

            // Arrange
            var queryExecutor = CreateFriendshipQueryExecutor();
            ArrangeGetUserIdsYouRequestedToFollowQuery(query);

            _fakeTwitterAccessor.ArrangeExecuteCursorGETQuery<long, IIdsCursorQueryResultDTO>(query, ids);

            // Act
            var result = (await queryExecutor.GetUserIdsYouRequestedToFollow(TestHelper.GenerateRandomInt())).ToArray();

            // Assert
            Assert.IsTrue(result.All(ids.Contains));
            Assert.IsTrue(ids.All(result.Contains));
        }

        [TestMethod]
        public async Task GetUserIdsYouRequestedToFollow_MultipleCursorResult_ContainsAllIds()
        {
            string query = Guid.NewGuid().ToString();
            var ids = new long[] { new Random().Next(), new Random().Next() };
            var ids2 = new long[] { new Random().Next(), new Random().Next() };

            // Arrange
            var queryExecutor = CreateFriendshipQueryExecutor();
            ArrangeGetUserIdsYouRequestedToFollowQuery(query);

            _fakeTwitterAccessor.ArrangeExecuteCursorGETQuery<long, IIdsCursorQueryResultDTO>(query, ids);

            // Act
            var result = (await queryExecutor.GetUserIdsYouRequestedToFollow(TestHelper.GenerateRandomInt())).ToArray();

            // Assert
            Assert.IsTrue(result.All(x => ids.Contains(x) || ids2.Contains(x)));
            Assert.IsTrue(ids.All(result.Contains));
            Assert.IsTrue(ids2.All(result.Contains));
        }

        [TestMethod]
        public async Task GetUserIdsYouRequestedToFollow_QueryExecutorReturnsNull_ReturnsNull()
        {
            string query = Guid.NewGuid().ToString();

            // Arrange
            var queryExecutor = CreateFriendshipQueryExecutor();
            ArrangeGetUserIdsYouRequestedToFollowQuery(query);

            _fakeTwitterAccessor.ArrangeExecuteCursorGETQuery<long, IIdsCursorQueryResultDTO>(query, null);

            // Act
            var result = await queryExecutor.GetUserIdsYouRequestedToFollow(TestHelper.GenerateRandomInt());

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
        public async Task CreateFriendshipWith_UserDTO_ReturnsQueryExecutor()
        {
            // Arrange - Act
            var shouldSucceed = await CreateFriendshipWith_UserDTO_Returns(true);
            var shouldFail = await CreateFriendshipWith_UserDTO_Returns(false);

            // Assert
            Assert.IsTrue(shouldSucceed);
            Assert.IsFalse(shouldFail);
        }

        private async Task<bool> CreateFriendshipWith_UserDTO_Returns(bool result)
        {
            var userDTO = A.Fake<IUserDTO>();
            string query = Guid.NewGuid().ToString();

            // Arrange
            var queryExecutor = CreateFriendshipQueryExecutor();
            ArrangeCreateFriendshipWithUserDTO(userDTO, query);
            _fakeTwitterAccessor.ArrangeTryExecutePOSTQuery(query, result);

            // Act
            return await queryExecutor.CreateFriendshipWith(userDTO);
        }

        private void ArrangeCreateFriendshipWithUserDTO(IUserDTO userDTO, string query)
        {
            _fakeFriendshipQueryGenerator
                .CallsTo(x => x.GetCreateFriendshipWithQuery(userDTO))
                .Returns(query);
        }

        #endregion

        #region Update Friendship With

        [TestMethod]
        public async Task UpdateRelationshipAuthorizationsWith_UserDTO_ReturnsQueryExecutor()
        {
            // Arrange - Act
            var shouldFail1 = await UpdateRelationshipAuthorizationsWith_UserDTO_Returns(false, false, false, false);
            var shouldFail2 = await UpdateRelationshipAuthorizationsWith_UserDTO_Returns(false, false, false, true);
            var shouldFail3 = await UpdateRelationshipAuthorizationsWith_UserDTO_Returns(false, false, true, false);
            var shouldFail4 = await UpdateRelationshipAuthorizationsWith_UserDTO_Returns(false, false, true, true);
            var shouldFail5 = await UpdateRelationshipAuthorizationsWith_UserDTO_Returns(false, true, false, false);
            var shouldFail6 = await UpdateRelationshipAuthorizationsWith_UserDTO_Returns(false, true, false, true);
            var shouldFail7 = await UpdateRelationshipAuthorizationsWith_UserDTO_Returns(false, true, true, false);
            var shouldFail8 = await UpdateRelationshipAuthorizationsWith_UserDTO_Returns(false, true, true, true);

            var shouldFail9 = await UpdateRelationshipAuthorizationsWith_UserDTO_Returns(true, false, false, false);
            var shouldFail10 = await UpdateRelationshipAuthorizationsWith_UserDTO_Returns(true, false, false, true);
            var shouldFail11 = await UpdateRelationshipAuthorizationsWith_UserDTO_Returns(true, false, true, false);
            var shouldFail12 = await UpdateRelationshipAuthorizationsWith_UserDTO_Returns(true, false, true, true);
            var shouldFail13 = await UpdateRelationshipAuthorizationsWith_UserDTO_Returns(true, true, false, false);
            var shouldSucceed1 = await UpdateRelationshipAuthorizationsWith_UserDTO_Returns(true, true, false, true);
            var shouldFail14 = await UpdateRelationshipAuthorizationsWith_UserDTO_Returns(true, true, true, false);
            var shouldSucceed2 = await UpdateRelationshipAuthorizationsWith_UserDTO_Returns(true, true, true, true);

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

        private async Task<bool> UpdateRelationshipAuthorizationsWith_UserDTO_Returns(bool isValid, bool userDTOIsNotNull, bool authParamsAreNotNull, bool result)
        {
            var userDTO = userDTOIsNotNull ? A.Fake<IUserDTO>() : null;

            if (userDTOIsNotNull)
            {
                _fakeUserQueryValidator.CallsTo(x => x.CanUserBeIdentified(userDTO)).Returns(isValid);
            }

            var authorizationParameter = authParamsAreNotNull ? A.Fake<IFriendshipAuthorizations>() : null;

            string query = Guid.NewGuid().ToString();

            // Arrange
            var queryExecutor = CreateFriendshipQueryExecutor();
            ArrangeUpdateRelationshipAuthorizationsWithUserDTO(userDTO, authorizationParameter, query);
            _fakeTwitterAccessor.ArrangeTryExecutePOSTQuery(query, result);

            // Act
            return await queryExecutor.UpdateRelationshipAuthorizationsWith(userDTO, authorizationParameter);
        }

        private void ArrangeUpdateRelationshipAuthorizationsWithUserDTO(IUserDTO userDTO, IFriendshipAuthorizations auth, string query)
        {
            _fakeFriendshipQueryGenerator
                .CallsTo(x => x.GetUpdateRelationshipAuthorizationsWithQuery(userDTO, auth))
                .Returns(query);
        }

        #endregion

        #region Destroy Friendship With

        [TestMethod]
        public async Task DestroyFriendshipWith_UserDTO_ReturnsQueryExecutor()
        {
            // Arrange - Act
            var shouldSucceed = await DestroyFriendshipWith_UserDTO_Returns(true, true);
            var shouldFail1 = await DestroyFriendshipWith_UserDTO_Returns(true, false);
            var shouldFail2 = await DestroyFriendshipWith_UserDTO_Returns(false, true);
            var shouldFail3 = await DestroyFriendshipWith_UserDTO_Returns(false, false);

            // Assert
            Assert.IsTrue(shouldSucceed);
            Assert.IsFalse(shouldFail1);
            Assert.IsFalse(shouldFail2);
            Assert.IsFalse(shouldFail3);
        }

        private async Task<bool> DestroyFriendshipWith_UserDTO_Returns(bool isValid, bool result)
        {
            var userDTO = A.Fake<IUserDTO>();
            _fakeUserQueryValidator.CallsTo(x => x.CanUserBeIdentified(userDTO)).Returns(isValid);

            string query = Guid.NewGuid().ToString();

            // Arrange
            var queryExecutor = CreateFriendshipQueryExecutor();
            ArrangeDestroyFriendshipWithUserDTO(userDTO, query);
            _fakeTwitterAccessor.ArrangeTryExecutePOSTQuery(query, result);

            // Act
            return await queryExecutor.DestroyFriendshipWith(userDTO);
        }

        private void ArrangeDestroyFriendshipWithUserDTO(IUserDTO userDTO, string query)
        {
            _fakeFriendshipQueryGenerator
                .CallsTo(x => x.GetDestroyFriendshipWithQuery(userDTO))
                .Returns(query);
        }

       
        #endregion

        private FriendshipQueryExecutor CreateFriendshipQueryExecutor()
        {
            return _fakeBuilder.GenerateClass();
        }
    }
}