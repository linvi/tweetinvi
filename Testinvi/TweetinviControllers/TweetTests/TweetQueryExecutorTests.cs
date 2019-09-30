using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using FakeItEasy;
using FakeItEasy.ExtensionSyntax.Full;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Testinvi.Helpers;
using Testinvi.SetupHelpers;
using Tweetinvi;
using Tweetinvi.Controllers.Tweet;
using Tweetinvi.Core.Client;
using Tweetinvi.Core.Exceptions;
using Tweetinvi.Core.QueryGenerators;
using Tweetinvi.Core.Web;
using Tweetinvi.Exceptions;
using Tweetinvi.Logic.Exceptions;
using Tweetinvi.Models;
using Tweetinvi.Models.DTO;
using Tweetinvi.Models.DTO.QueryDTO;
using Tweetinvi.Models.Interfaces;
using Tweetinvi.Parameters;

namespace Testinvi.TweetinviControllers.TweetTests
{
    [TestClass]
    public class TweetQueryExecutorTests
    {
        private FakeClassBuilder<TweetQueryExecutor> _fakeBuilder;
        private Fake<ITweetQueryGenerator> _fakeTweetQueryGenerator;
        private Fake<ITwitterAccessor> _fakeTwitterAccessor;

        private TwitterException _fakeTwitterException;
        private TwitterException _fakeOtherTwitterException;

        private List<long> _cursorQueryIds;

        [TestInitialize]
        public void TestInitialize()
        {
            _fakeBuilder = new FakeClassBuilder<TweetQueryExecutor>();
            _fakeTweetQueryGenerator = _fakeBuilder.GetFake<ITweetQueryGenerator>();
            _fakeTwitterAccessor = _fakeBuilder.GetFake<ITwitterAccessor>();

            var fakeWebExceptionInfoExtractor = A.Fake<IWebExceptionInfoExtractor>();

            var twitter139ExceptionInfos = new TwitterExceptionInfo { Code = 139 };
            fakeWebExceptionInfoExtractor.CallsTo(x => x.GetTwitterExceptionInfo(It.IsAny<WebException>())).Returns(new[] { twitter139ExceptionInfos });
            _fakeTwitterException = new TwitterException(fakeWebExceptionInfoExtractor, new WebException(), A.Fake<ITwitterRequest>());

            var twitterOtherExceptionInfos = new TwitterExceptionInfo { Code = 1 };
            fakeWebExceptionInfoExtractor.CallsTo(x => x.GetTwitterExceptionInfo(It.IsAny<WebException>())).Returns(new[] { twitterOtherExceptionInfos });
            _fakeOtherTwitterException = new TwitterException(fakeWebExceptionInfoExtractor, new WebException(), A.Fake<ITwitterRequest>());

            _cursorQueryIds = new List<long>();
        }

        #region Publish Tweet

        [TestMethod]
        public async Task PublishTweet_ReturnsTwitterAccessorResult()
        {
            // Arrange
            var queryExecutor = CreateTweetQueryExecutor();

            var parameters = A.Fake<IPublishTweetParameters>();

            var query = TestHelper.GenerateString();
            var expectedResult = A.Fake<ITwitterResult<ITweetDTO>>();
            var request = A.Fake<ITwitterRequest>();

            _fakeTweetQueryGenerator.CallsTo(x => x.GetPublishTweetQuery(parameters, It.IsAny<TweetMode?>())).Returns(query);
            _fakeTwitterAccessor.ArrangeExecutePOSTQuery(query, expectedResult);
            _fakeTwitterAccessor.CallsTo(x => x.ExecuteRequest<ITweetDTO>(request)).ReturnsLazily(() => expectedResult);

            // Act
            var result = await queryExecutor.PublishTweet(parameters, request);

            // Assert
            Assert.AreEqual(result, expectedResult);
        }

        #endregion

        #region Publish Retweet

        [TestMethod]
        public async Task PublishRetweet_ReturnsTwitterAccessorResult()
        {
            // Arrange
            var queryExecutor = CreateTweetQueryExecutor();
            var tweetId = new TweetIdentifier(TestHelper.GenerateRandomInt());
            var queryUrl = TestHelper.GenerateString();
            var expectedResult = A.Fake<ITwitterResult<ITweetDTO>>();
            var request = A.Fake<ITwitterRequest>();

            _fakeTweetQueryGenerator.CallsTo(x => x.GetPublishRetweetQuery(tweetId, null)).Returns(queryUrl);
            _fakeTwitterAccessor
                .CallsTo(x => x.ExecuteRequest<ITweetDTO>(A<ITwitterRequest>.That.Matches(twitterRequest => twitterRequest.Query.Url == queryUrl)))
                .ReturnsLazily(() => expectedResult);

            // Act
            var result = await queryExecutor.PublishRetweet(tweetId, request);

            // Assert
            Assert.AreEqual(result, expectedResult);
        }

        #endregion

        #region Get Retweets

        [TestMethod]
        public async Task GetRetweets_FromTweetIdentifier_ReturnsTwitterAccessorResult()
        {
            // Arrange
            var queryExecutor = CreateTweetQueryExecutor();
            var tweetIdentifier = A.Fake<ITweetIdentifier>();
            var query = TestHelper.GenerateString();
            var maxRetweetsToRetrieve = TestHelper.GenerateRandomInt();
            var request = A.Fake<ITwitterRequest>();
            var expectedResult = A.Fake<ITwitterResult<ITweetDTO[]>>();

            _fakeTweetQueryGenerator
                .CallsTo(x => x.GetRetweetsQuery(tweetIdentifier, maxRetweetsToRetrieve, It.IsAny<ITwitterExecutionContext>()))
                .Returns(query);

            _fakeTwitterAccessor
                .CallsTo(x => x.ExecuteRequest<ITweetDTO[]>(A<ITwitterRequest>.That.Matches(twitterRequest => twitterRequest.Query.Url == query)))
                .ReturnsLazily(() => expectedResult);

            // Act
            var result = await queryExecutor.GetRetweets(tweetIdentifier, maxRetweetsToRetrieve, request);

            // Assert
            Assert.AreEqual(result, expectedResult);
        }

        #endregion

        #region Get Retweeters Ids

        [TestMethod]
        public async Task GetRetweetersIds_FromTweetIdentifier_ReturnsTwitterAccessorResult()
        {
            // Arrange
            var queryExecutor = CreateTweetQueryExecutor();
            var tweetIdentifier = A.Fake<ITweetIdentifier>();
            var maxRetweetersToRetrieve = TestHelper.GenerateRandomInt();
            var query = TestHelper.GenerateString();
            var expectedCursorResults = GenerateExpectedCursorResults();

            _fakeTweetQueryGenerator.CallsTo(x => x.GetRetweeterIdsQuery(tweetIdentifier, maxRetweetersToRetrieve)).Returns(query);
            _fakeTwitterAccessor.ArrangeExecuteCursorGETQuery<long, IIdsCursorQueryResultDTO>(query, expectedCursorResults);

            // Act
            var result = await queryExecutor.GetRetweetersIds(tweetIdentifier, maxRetweetersToRetrieve);

            // Assert
            Assert.IsTrue(result.ContainsAll(_cursorQueryIds));
        }

        #endregion

        #region Destroy Tweet

        [TestMethod]
        public async Task DestroyTweet_WithTweetIdSucceed_ReturnsExpectedResult()
        {
            // Arrange
            var queryExecutor = CreateTweetQueryExecutor();
            var tweetId = TestHelper.GenerateRandomLong();
            var query = TestHelper.GenerateString();
            var expectedResult = A.Fake<ITwitterResult>();

            _fakeTweetQueryGenerator.CallsTo(x => x.GetDestroyTweetQuery(tweetId)).Returns(query);
            _fakeTwitterAccessor
                .CallsTo(x => x.ExecuteRequest(A<ITwitterRequest>.That.Matches(request => request.Query.Url == query)))
                .ReturnsLazily(() => expectedResult);

            // Act
            var result = await queryExecutor.DestroyTweet(tweetId, A.Fake<ITwitterRequest>());

            // Assert
            Assert.AreEqual(result, expectedResult);
        }

        #endregion

        #region Favourite Tweet

        [TestMethod]
        public async Task FavouriteTweet_WithTweetDTOSucceed_ReturnsTrue()
        {
            // Arrange
            var queryExecutor = CreateTweetQueryExecutor();
            var tweetDTO = A.Fake<ITweetDTO>();
            var query = TestHelper.GenerateString();

            _fakeTweetQueryGenerator.CallsTo(x => x.GetFavoriteTweetQuery(tweetDTO)).Returns(query);
            _fakeTwitterAccessor.ArrangeTryExecutePOSTQuery(query, true);

            // Act
            var result = await queryExecutor.FavoriteTweet(tweetDTO);

            // Assert
            Assert.IsTrue(result);
        }

        [TestMethod]
        public async Task FavouriteTweet_WithTweetDTO_ReturnsNull_ReturnsFalse()
        {
            // Arrange
            var queryExecutor = CreateTweetQueryExecutor();
            var tweetDTO = A.Fake<ITweetDTO>();
            var query = TestHelper.GenerateString();

            _fakeTweetQueryGenerator.CallsTo(x => x.GetFavoriteTweetQuery(tweetDTO)).Returns(query);
            _fakeTwitterAccessor.ArrangeExecutePOSTQuery(query, null);

            // Act
            var result = await queryExecutor.FavoriteTweet(tweetDTO);

            // Assert
            Assert.IsFalse(result);
        }

        [TestMethod]
        public async Task FavouriteTweet_WithTweetDTO_ThrowTwitterException_WithCode139_ReturnsFalse()
        {
            // Arrange
            var queryExecutor = CreateTweetQueryExecutor();
            var tweetDTO = A.Fake<ITweetDTO>();
            var query = TestHelper.GenerateString();

            _fakeTweetQueryGenerator.CallsTo(x => x.GetFavoriteTweetQuery(tweetDTO)).Returns(query);
            _fakeTwitterAccessor.CallsTo(x => x.ExecutePOSTQuery(query)).Throws(_fakeTwitterException);

            // Act
            var result = await queryExecutor.FavoriteTweet(tweetDTO);

            // Assert
            Assert.IsFalse(result);
        }

        [TestMethod]
        public async Task FavouriteTweet_WithTweetDTO_ThrowTwitterException_WithOtherCode_ThrowsException()
        {
            // Arrange
            var queryExecutor = CreateTweetQueryExecutor();
            var tweetDTO = A.Fake<ITweetDTO>();
            var query = TestHelper.GenerateString();

            _fakeTweetQueryGenerator.CallsTo(x => x.GetFavoriteTweetQuery(tweetDTO)).Returns(query);
            _fakeTwitterAccessor.CallsTo(x => x.ExecutePOSTQuery(query)).Throws(_fakeOtherTwitterException);

            // Act
            try
            {
                await queryExecutor.FavoriteTweet(tweetDTO);
            }
            catch (Exception ex)
            {
                Assert.AreEqual(ex, _fakeOtherTwitterException);
                return;
            }

            // Assert
            Assert.Fail("Error should have been thrown");
        }

        [TestMethod]
        public async Task FavouriteTweet_WithTweetIdSucceed_ReturnsTrue()
        {
            // Arrange
            var queryExecutor = CreateTweetQueryExecutor();
            var tweetId = TestHelper.GenerateRandomLong();
            var query = TestHelper.GenerateString();

            _fakeTweetQueryGenerator.CallsTo(x => x.GetFavoriteTweetQuery(tweetId)).Returns(query);
            _fakeTwitterAccessor.ArrangeTryExecutePOSTQuery(query, true);

            // Act
            var result = await queryExecutor.FavoriteTweet(tweetId);

            // Assert
            Assert.IsTrue(result);
        }

        [TestMethod]
        public async Task FavouriteTweet_WithTweetId_ReturnsNull_ReturnsFalse()
        {
            // Arrange
            var queryExecutor = CreateTweetQueryExecutor();
            var tweetId = TestHelper.GenerateRandomLong();
            var query = TestHelper.GenerateString();

            _fakeTweetQueryGenerator.CallsTo(x => x.GetFavoriteTweetQuery(tweetId)).Returns(query);
            _fakeTwitterAccessor.ArrangeExecutePOSTQuery(query, null);

            // Act
            var result = await queryExecutor.FavoriteTweet(tweetId);

            // Assert
            Assert.IsFalse(result);
        }

        #endregion

        #region UnFavourite Tweet

        [TestMethod]
        public async Task UnFavouriteTweet_WithTweetDTOSucceed_ReturnsTrue()
        {
            // Arrange
            var queryExecutor = CreateTweetQueryExecutor();
            var tweetDTO = A.Fake<ITweetDTO>();
            var query = TestHelper.GenerateString();

            _fakeTweetQueryGenerator.CallsTo(x => x.GetUnFavoriteTweetQuery(tweetDTO)).Returns(query);
            _fakeTwitterAccessor.ArrangeTryExecutePOSTQuery(query, true);

            // Act
            var result = await queryExecutor.UnFavoriteTweet(tweetDTO);

            // Assert
            Assert.IsTrue(result);
        }

        [TestMethod]
        public async Task UnFavouriteTweet_WithTweetDTOFailed_ReturnsFalse()
        {
            // Arrange
            var queryExecutor = CreateTweetQueryExecutor();
            var tweetDTO = A.Fake<ITweetDTO>();
            var query = TestHelper.GenerateString();

            _fakeTweetQueryGenerator.CallsTo(x => x.GetUnFavoriteTweetQuery(tweetDTO)).Returns(query);
            _fakeTwitterAccessor.ArrangeTryExecutePOSTQuery(query, false);

            // Act
            var result = await queryExecutor.UnFavoriteTweet(tweetDTO);

            // Assert
            Assert.IsFalse(result);
        }

        [TestMethod]
        public async Task UnFavouriteTweet_WithTweetIdSucceed_ReturnsTrue()
        {
            // Arrange
            var queryExecutor = CreateTweetQueryExecutor();
            var tweetId = TestHelper.GenerateRandomLong();
            var query = TestHelper.GenerateString();

            _fakeTweetQueryGenerator.CallsTo(x => x.GetUnFavoriteTweetQuery(tweetId)).Returns(query);
            _fakeTwitterAccessor.ArrangeTryExecutePOSTQuery(query, true);

            // Act
            var result = await queryExecutor.UnFavoriteTweet(tweetId);

            // Assert
            Assert.IsTrue(result);
        }

        [TestMethod]
        public async Task UnFavouriteTweet_WithTweetIdFailed_ReturnsFalse()
        {
            // Arrange
            var queryExecutor = CreateTweetQueryExecutor();
            var tweetId = TestHelper.GenerateRandomLong();
            var query = TestHelper.GenerateString();

            _fakeTweetQueryGenerator.CallsTo(x => x.GetUnFavoriteTweetQuery(tweetId)).Returns(query);
            _fakeTwitterAccessor.ArrangeTryExecutePOSTQuery(query, false);

            // Act
            var result = await queryExecutor.UnFavoriteTweet(tweetId);

            // Assert
            Assert.IsFalse(result);
        }

        #endregion

        #region GetOEmbedTweet

        [TestMethod]
        public async Task GenerateOEmbedTweet_WithTweetDTO_ReturnsTwitterAccessorResult()
        {
            // Arrange
            var queryExecutor = CreateTweetQueryExecutor();
            var tweetDTO = A.Fake<ITweetDTO>();
            var query = TestHelper.GenerateString();
            var expectedResult = A.Fake<IOEmbedTweetDTO>();

            _fakeTweetQueryGenerator.CallsTo(x => x.GetGenerateOEmbedTweetQuery(tweetDTO)).Returns(query);
            _fakeTwitterAccessor.ArrangeExecuteGETQuery(query, expectedResult);

            // Act
            var result = await queryExecutor.GenerateOEmbedTweet(tweetDTO);

            // Assert
            Assert.AreEqual(result, expectedResult);
        }

        [TestMethod]
        public async Task GenerateOEmbedTweet_WithTweetId_ReturnsTwitterAccessorResult()
        {
            // Arrange
            var queryExecutor = CreateTweetQueryExecutor();
            var tweetId = TestHelper.GenerateRandomLong();
            var query = TestHelper.GenerateString();
            var expectedResult = A.Fake<IOEmbedTweetDTO>();

            _fakeTweetQueryGenerator.CallsTo(x => x.GetGenerateOEmbedTweetQuery(tweetId)).Returns(query);
            _fakeTwitterAccessor.ArrangeExecuteGETQuery(query, expectedResult);

            // Act
            var result = await queryExecutor.GenerateOEmbedTweet(tweetId);

            // Assert
            Assert.AreEqual(result, expectedResult);
        }

        #endregion

        private TweetQueryExecutor CreateTweetQueryExecutor()
        {
            return _fakeBuilder.GenerateClass();
        }

        private IEnumerable<long> GenerateExpectedCursorResults()
        {
            var queryId1 = TestHelper.GenerateRandomLong();
            var queryId2 = TestHelper.GenerateRandomLong();

            _cursorQueryIds.Add(queryId1);
            _cursorQueryIds.Add(queryId2);

            return new[] { queryId1, queryId2 };
        }
    }
}