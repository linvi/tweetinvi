using System;
using System.Collections.Generic;
using System.Net;
using FakeItEasy;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Testinvi.Helpers;
using Testinvi.SetupHelpers;
using Tweetinvi.Controllers.Tweet;
using Tweetinvi.Core.Exceptions;
using Tweetinvi.Core.QueryGenerators;
using Tweetinvi.Core.Web;
using Tweetinvi.Exceptions;
using Tweetinvi.Logic.Exceptions;
using Tweetinvi.Models;
using Tweetinvi.Models.DTO;
using Tweetinvi.Models.DTO.QueryDTO;
using Tweetinvi.Parameters;

namespace Testinvi.TweetinviControllers.TweetTests
{
    [TestClass]
    public class TweetQueryExecutorTests
    {
        private FakeClassBuilder<TweetQueryExecutor> _fakeBuilder;

        private ITweetQueryGenerator _tweetQueryGenerator;
        private ITwitterAccessor _twitterAccessor;
        private TwitterException _twitterException;
        private TwitterException _otherTwitterException;

        private List<long> _cursorQueryIds;

        [TestInitialize]
        public void TestInitialize()
        {
            _fakeBuilder = new FakeClassBuilder<TweetQueryExecutor>();
            _tweetQueryGenerator = _fakeBuilder.GetFake<ITweetQueryGenerator>().FakedObject;
            _twitterAccessor = _fakeBuilder.GetFake<ITwitterAccessor>().FakedObject;

            var webExceptionInfoExtractor = A.Fake<IWebExceptionInfoExtractor>();

            var twitter139ExceptionInfos = new TwitterExceptionInfo { Code = 139 };
            A.CallTo(() => webExceptionInfoExtractor.GetTwitterExceptionInfo(It.IsAny<WebException>()))
                .Returns(new[] {twitter139ExceptionInfos});
            _twitterException = new TwitterException(webExceptionInfoExtractor, new WebException(), A.Fake<ITwitterQuery>());

            var twitterOtherExceptionInfos = new TwitterExceptionInfo { Code = 1 };
            A.CallTo(() => webExceptionInfoExtractor.GetTwitterExceptionInfo(It.IsAny<WebException>()))
                .Returns(new[] {twitterOtherExceptionInfos});
            _otherTwitterException = new TwitterException(webExceptionInfoExtractor, new WebException(), A.Fake<ITwitterQuery>());

            _cursorQueryIds = new List<long>();
        }

        #region Publish Tweet

        [TestMethod]
        public void PublishTweet_ReturnsTwitterAccessorResult()
        {
            // Arrange
            var queryExecutor = CreateTweetQueryExecutor();

            var parameters = A.Fake<IPublishTweetParameters>();

            var query = TestHelper.GenerateString();
            var expectedResult = A.Fake<ITweetDTO>();

            A.CallTo(() => _tweetQueryGenerator.GetPublishTweetQuery(parameters)).Returns(query);
            _twitterAccessor.ArrangeExecutePOSTQuery(query, expectedResult);

            // Act
            var result = queryExecutor.PublishTweet(parameters);

            // Assert
            Assert.AreEqual(result, expectedResult);
        }

        #endregion

        #region Publish Retweet

        [TestMethod]
        public void PublishRetweet_WithTweetDTO_ReturnsTwitterAccessorResult()
        {
            // Arrange
            var queryExecutor = CreateTweetQueryExecutor();
            var tweetDTO = A.Fake<ITweetDTO>();
            var query = TestHelper.GenerateString();
            var expectedResult = A.Fake<ITweetDTO>();

            A.CallTo(() => _tweetQueryGenerator.GetPublishRetweetQuery(tweetDTO)).Returns(query);
            _twitterAccessor.ArrangeExecutePOSTQuery(query, expectedResult);

            // Act
            var result = queryExecutor.PublishRetweet(tweetDTO);

            // Assert
            Assert.AreEqual(result, expectedResult);
        }

        [TestMethod]
        public void PublishRetweet_WithTweetId_ReturnsTwitterAccessorResult()
        {
            // Arrange
            var queryExecutor = CreateTweetQueryExecutor();
            var tweetId = TestHelper.GenerateRandomLong();
            var query = TestHelper.GenerateString();
            var expectedResult = A.Fake<ITweetDTO>();

            A.CallTo(() => _tweetQueryGenerator.GetPublishRetweetQuery(tweetId)).Returns(query);
            _twitterAccessor.ArrangeExecutePOSTQuery(query, expectedResult);

            // Act
            var result = queryExecutor.PublishRetweet(tweetId);

            // Assert
            Assert.AreEqual(result, expectedResult);
        }

        #endregion

        #region Get Retweets

        [TestMethod]
        public void GetRetweets_FromTweetIdentifier_ReturnsTwitterAccessorResult()
        {
            // Arrange
            var queryExecutor = CreateTweetQueryExecutor();
            var tweetIdentifier = A.Fake<ITweetIdentifier>();
            var query = TestHelper.GenerateString();
            var maxRetweetsToRetrieve = TestHelper.GenerateRandomInt();
            var expectedResult = GenerateExpectedIRetweetsCursorResults();

            A.CallTo(() => _tweetQueryGenerator.GetRetweetsQuery(tweetIdentifier, maxRetweetsToRetrieve))
                .Returns(query);
            _twitterAccessor.ArrangeExecuteGETQuery<IEnumerable<ITweetDTO>>(query, expectedResult);

            // Act
            var result = queryExecutor.GetRetweets(tweetIdentifier, maxRetweetsToRetrieve);

            // Assert
            Assert.AreEqual(result, expectedResult);
        }

        #endregion

        #region Get Retweeters Ids

        [TestMethod]
        public void GetRetweetersIds_FromTweetIdentifier_ReturnsTwitterAccessorResult()
        {
            // Arrange
            var queryExecutor = CreateTweetQueryExecutor();
            var tweetIdentifier = A.Fake<ITweetIdentifier>();
            var maxRetweetersToRetrieve = TestHelper.GenerateRandomInt();
            var query = TestHelper.GenerateString();
            var expectedCursorResults = GenerateExpectedCursorResults();

            A.CallTo(() => _tweetQueryGenerator.GetRetweeterIdsQuery(tweetIdentifier, maxRetweetersToRetrieve))
                .Returns(query);
            _twitterAccessor.ArrangeExecuteCursorGETQuery<long, IIdsCursorQueryResultDTO>(query, expectedCursorResults);

            // Act
            var result = queryExecutor.GetRetweetersIds(tweetIdentifier, maxRetweetersToRetrieve);

            // Assert
            Assert.IsTrue(result.ContainsAll(_cursorQueryIds));
        }

        #endregion

        #region Destroy Tweet

        [TestMethod]
        public void DestroyTweet_WithTweetDTOSucceed_ReturnsTrue()
        {
            // Arrange
            var queryExecutor = CreateTweetQueryExecutor();
            var tweetDTO = A.Fake<ITweetDTO>();
            var query = TestHelper.GenerateString();

            A.CallTo(() => _tweetQueryGenerator.GetDestroyTweetQuery(tweetDTO)).Returns(query);
            _twitterAccessor.ArrangeTryExecutePOSTQuery(query, true);

            // Act
            var result = queryExecutor.DestroyTweet(tweetDTO);

            // Assert
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void DestroyTweet_WithTweetDTOFailed_ReturnsFalse()
        {
            // Arrange
            var queryExecutor = CreateTweetQueryExecutor();
            var tweetDTO = A.Fake<ITweetDTO>();
            var query = TestHelper.GenerateString();

            A.CallTo(() => _tweetQueryGenerator.GetDestroyTweetQuery(tweetDTO)).Returns(query);
            _twitterAccessor.ArrangeTryExecutePOSTQuery(query, false);

            // Act
            var result = queryExecutor.DestroyTweet(tweetDTO);

            // Assert
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void DestroyTweet_WithTweetIdSucceed_ReturnsTrue()
        {
            // Arrange
            var queryExecutor = CreateTweetQueryExecutor();
            var tweetId = TestHelper.GenerateRandomLong();
            var query = TestHelper.GenerateString();

            A.CallTo(() => _tweetQueryGenerator.GetDestroyTweetQuery(tweetId)).Returns(query);
            _twitterAccessor.ArrangeTryExecutePOSTQuery(query, true);

            // Act
            var result = queryExecutor.DestroyTweet(tweetId);

            // Assert
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void DestroyTweet_WithTweetIdFailed_ReturnsFalse()
        {
            // Arrange
            var queryExecutor = CreateTweetQueryExecutor();
            var tweetId = TestHelper.GenerateRandomLong();
            var query = TestHelper.GenerateString();

            A.CallTo(() => _tweetQueryGenerator.GetDestroyTweetQuery(tweetId)).Returns(query);
            _twitterAccessor.ArrangeTryExecutePOSTQuery(query, false);

            // Act
            var result = queryExecutor.DestroyTweet(tweetId);

            // Assert
            Assert.IsFalse(result);
        }

        #endregion

        #region Favourite Tweet

        [TestMethod]
        public void FavouriteTweet_WithTweetDTOSucceed_ReturnsTrue()
        {
            // Arrange
            var queryExecutor = CreateTweetQueryExecutor();
            var tweetDTO = A.Fake<ITweetDTO>();
            var query = TestHelper.GenerateString();

            A.CallTo(() => _tweetQueryGenerator.GetFavoriteTweetQuery(tweetDTO)).Returns(query);
            _twitterAccessor.ArrangeTryExecutePOSTQuery(query, true);

            // Act
            var result = queryExecutor.FavoriteTweet(tweetDTO);

            // Assert
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void FavouriteTweet_WithTweetDTO_ReturnsNull_ReturnsFalse()
        {
            // Arrange
            var queryExecutor = CreateTweetQueryExecutor();
            var tweetDTO = A.Fake<ITweetDTO>();
            var query = TestHelper.GenerateString();

            A.CallTo(() => _tweetQueryGenerator.GetFavoriteTweetQuery(tweetDTO)).Returns(query);
            _twitterAccessor.ArrangeExecutePOSTQuery(query, null);

            // Act
            var result = queryExecutor.FavoriteTweet(tweetDTO);

            // Assert
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void FavouriteTweet_WithTweetDTO_ThrowTwitterException_WithCode139_ReturnsFalse()
        {
            // Arrange
            var queryExecutor = CreateTweetQueryExecutor();
            var tweetDTO = A.Fake<ITweetDTO>();
            var query = TestHelper.GenerateString();

            A.CallTo(() => _tweetQueryGenerator.GetFavoriteTweetQuery(tweetDTO)).Returns(query);
            A.CallTo(() => _twitterAccessor.ExecutePOSTQuery(query)).Throws(_twitterException);

            // Act
            var result = queryExecutor.FavoriteTweet(tweetDTO);

            // Assert
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void FavouriteTweet_WithTweetDTO_ThrowTwitterException_WithOtherCode_ThrowsException()
        {
            // Arrange
            var queryExecutor = CreateTweetQueryExecutor();
            var tweetDTO = A.Fake<ITweetDTO>();
            var query = TestHelper.GenerateString();

            A.CallTo(() => _tweetQueryGenerator.GetFavoriteTweetQuery(tweetDTO)).Returns(query);
            A.CallTo(() => _twitterAccessor.ExecutePOSTQuery(query)).Throws(_otherTwitterException);

            // Act
            try
            {
                queryExecutor.FavoriteTweet(tweetDTO);
            }
            catch (Exception ex)
            {
                Assert.AreEqual(ex, _otherTwitterException);
                return;
            }

            // Assert
            Assert.Fail("Error should have been thrown");
        }

        [TestMethod]
        public void FavouriteTweet_WithTweetIdSucceed_ReturnsTrue()
        {
            // Arrange
            var queryExecutor = CreateTweetQueryExecutor();
            var tweetId = TestHelper.GenerateRandomLong();
            var query = TestHelper.GenerateString();

            A.CallTo(() => _tweetQueryGenerator.GetFavoriteTweetQuery(tweetId)).Returns(query);
            _twitterAccessor.ArrangeTryExecutePOSTQuery(query, true);

            // Act
            var result = queryExecutor.FavoriteTweet(tweetId);

            // Assert
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void FavouriteTweet_WithTweetId_ReturnsNull_ReturnsFalse()
        {
            // Arrange
            var queryExecutor = CreateTweetQueryExecutor();
            var tweetId = TestHelper.GenerateRandomLong();
            var query = TestHelper.GenerateString();

            A.CallTo(() => _tweetQueryGenerator.GetFavoriteTweetQuery(tweetId)).Returns(query);
            _twitterAccessor.ArrangeExecutePOSTQuery(query, null);

            // Act
            var result = queryExecutor.FavoriteTweet(tweetId);

            // Assert
            Assert.IsFalse(result);
        }

        #endregion

        #region UnFavourite Tweet

        [TestMethod]
        public void UnFavouriteTweet_WithTweetDTOSucceed_ReturnsTrue()
        {
            // Arrange
            var queryExecutor = CreateTweetQueryExecutor();
            var tweetDTO = A.Fake<ITweetDTO>();
            var query = TestHelper.GenerateString();

            A.CallTo(() => _tweetQueryGenerator.GetUnFavoriteTweetQuery(tweetDTO)).Returns(query);
            _twitterAccessor.ArrangeTryExecutePOSTQuery(query, true);

            // Act
            var result = queryExecutor.UnFavoriteTweet(tweetDTO);

            // Assert
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void UnFavouriteTweet_WithTweetDTOFailed_ReturnsFalse()
        {
            // Arrange
            var queryExecutor = CreateTweetQueryExecutor();
            var tweetDTO = A.Fake<ITweetDTO>();
            var query = TestHelper.GenerateString();

            A.CallTo(() => _tweetQueryGenerator.GetUnFavoriteTweetQuery(tweetDTO)).Returns(query);
            _twitterAccessor.ArrangeTryExecutePOSTQuery(query, false);

            // Act
            var result = queryExecutor.UnFavoriteTweet(tweetDTO);

            // Assert
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void UnFavouriteTweet_WithTweetIdSucceed_ReturnsTrue()
        {
            // Arrange
            var queryExecutor = CreateTweetQueryExecutor();
            var tweetId = TestHelper.GenerateRandomLong();
            var query = TestHelper.GenerateString();

            A.CallTo(() => _tweetQueryGenerator.GetUnFavoriteTweetQuery(tweetId)).Returns(query);
            _twitterAccessor.ArrangeTryExecutePOSTQuery(query, true);

            // Act
            var result = queryExecutor.UnFavoriteTweet(tweetId);

            // Assert
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void UnFavouriteTweet_WithTweetIdFailed_ReturnsFalse()
        {
            // Arrange
            var queryExecutor = CreateTweetQueryExecutor();
            var tweetId = TestHelper.GenerateRandomLong();
            var query = TestHelper.GenerateString();

            A.CallTo(() => _tweetQueryGenerator.GetUnFavoriteTweetQuery(tweetId)).Returns(query);
            _twitterAccessor.ArrangeTryExecutePOSTQuery(query, false);

            // Act
            var result = queryExecutor.UnFavoriteTweet(tweetId);

            // Assert
            Assert.IsFalse(result);
        }

        #endregion

        #region GetOEmbedTweet

        [TestMethod]
        public void GenerateOEmbedTweet_WithTweetDTO_ReturnsTwitterAccessorResult()
        {
            // Arrange
            var queryExecutor = CreateTweetQueryExecutor();
            var tweetDTO = A.Fake<ITweetDTO>();
            var query = TestHelper.GenerateString();
            var expectedResult = A.Fake<IOEmbedTweetDTO>();

            A.CallTo(() => _tweetQueryGenerator.GetGenerateOEmbedTweetQuery(tweetDTO)).Returns(query);
            _twitterAccessor.ArrangeExecuteGETQuery(query, expectedResult);

            // Act
            var result = queryExecutor.GenerateOEmbedTweet(tweetDTO);

            // Assert
            Assert.AreEqual(result, expectedResult);
        }

        [TestMethod]
        public void GenerateOEmbedTweet_WithTweetId_ReturnsTwitterAccessorResult()
        {
            // Arrange
            var queryExecutor = CreateTweetQueryExecutor();
            var tweetId = TestHelper.GenerateRandomLong();
            var query = TestHelper.GenerateString();
            var expectedResult = A.Fake<IOEmbedTweetDTO>();

            A.CallTo(() => _tweetQueryGenerator.GetGenerateOEmbedTweetQuery(tweetId)).Returns(query);
            _twitterAccessor.ArrangeExecuteGETQuery(query, expectedResult);

            // Act
            var result = queryExecutor.GenerateOEmbedTweet(tweetId);

            // Assert
            Assert.AreEqual(result, expectedResult);
        }

        #endregion

        public TweetQueryExecutor CreateTweetQueryExecutor()
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

        private ITweetDTO[] GenerateExpectedIRetweetsCursorResults()
        {
            return new[] { A.Fake<ITweetDTO>(), A.Fake<ITweetDTO>() };
        }
    }
}