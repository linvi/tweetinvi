using System;
using System.Collections.Generic;
using System.Net;
using FakeItEasy;
using FakeItEasy.ExtensionSyntax.Full;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Testinvi.Helpers;
using Testinvi.SetupHelpers;
using Tweetinvi.Controllers.Tweet;
using Tweetinvi.Core.Exceptions;
using Tweetinvi.Core.Interfaces.Credentials;
using Tweetinvi.Core.Interfaces.DTO;
using Tweetinvi.Core.Interfaces.QueryGenerators;
using Tweetinvi.Core.Parameters;
using Tweetinvi.Core.Parameters.QueryParameters;
using Tweetinvi.Logic.Exceptions;

namespace Testinvi.TweetinviControllers.TweetTests
{
    [TestClass]
    public class TweetQueryExecutorTests
    {
        private FakeClassBuilder<TweetQueryExecutor> _fakeBuilder;
        private Fake<ITweetQueryGenerator> _fakeTweetQueryGenerator;
        private Fake<ITwitterAccessor> _fakeTwitterAccessor;

        private TwitterException _fake139TwitterException;
        private TwitterException _fakeOtherTwitterException;

        [TestInitialize]
        public void TestInitialize()
        {
            _fakeBuilder = new FakeClassBuilder<TweetQueryExecutor>();
            _fakeTweetQueryGenerator = _fakeBuilder.GetFake<ITweetQueryGenerator>();
            _fakeTwitterAccessor = _fakeBuilder.GetFake<ITwitterAccessor>();

            var fakeWebExceptionInfoExtractor = A.Fake<IWebExceptionInfoExtractor>();

            var twitter139ExceptionInfos = new TwitterExceptionInfo { Code = 139 };
            fakeWebExceptionInfoExtractor.CallsTo(x => x.GetTwitterExceptionInfo(It.IsAny<WebException>())).Returns(new [] { twitter139ExceptionInfos });
            _fake139TwitterException = new TwitterException(fakeWebExceptionInfoExtractor, new WebException(), TestHelper.GenerateString());

            var twitterOtherExceptionInfos = new TwitterExceptionInfo { Code = 1 };
            fakeWebExceptionInfoExtractor.CallsTo(x => x.GetTwitterExceptionInfo(It.IsAny<WebException>())).Returns(new[] { twitterOtherExceptionInfos });
            _fakeOtherTwitterException = new TwitterException(fakeWebExceptionInfoExtractor, new WebException(), TestHelper.GenerateString());
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

            _fakeTweetQueryGenerator.CallsTo(x => x.GetPublishTweetQuery(parameters)).Returns(query);
            _fakeTwitterAccessor.ArrangeExecutePOSTQuery(query, expectedResult);

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

            _fakeTweetQueryGenerator.CallsTo(x => x.GetPublishRetweetQuery(tweetDTO)).Returns(query);
            _fakeTwitterAccessor.ArrangeExecutePOSTQuery(query, expectedResult);

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

            _fakeTweetQueryGenerator.CallsTo(x => x.GetPublishRetweetQuery(tweetId)).Returns(query);
            _fakeTwitterAccessor.ArrangeExecutePOSTQuery(query, expectedResult);

            // Act
            var result = queryExecutor.PublishRetweet(tweetId);

            // Assert
            Assert.AreEqual(result, expectedResult);
        }

        #endregion

        #region Get Retweets

        [TestMethod]
        public void GetRetweets_FromTweetDTO_ReturnsTwitterAccessorResult()
        {
            // Arrange
            var queryExecutor = CreateTweetQueryExecutor();
            var tweetDTO = A.Fake<ITweetDTO>();
            var query = TestHelper.GenerateString();
            IEnumerable<ITweetDTO> expectedResult = new List<ITweetDTO>();

            _fakeTweetQueryGenerator.CallsTo(x => x.GetRetweetsQuery(tweetDTO)).Returns(query);
            _fakeTwitterAccessor.ArrangeExecuteGETQuery(query, expectedResult);

            // Act
            var result = queryExecutor.GetRetweets(tweetDTO);

            // Assert
            Assert.AreEqual(result, expectedResult);
        }

        [TestMethod]
        public void GetRetweets_FromTweetId_ReturnsTwitterAccessorResult()
        {
            // Arrange
            var queryExecutor = CreateTweetQueryExecutor();
            var tweetId = TestHelper.GenerateRandomLong();
            var query = TestHelper.GenerateString();
            IEnumerable<ITweetDTO> expectedResult = new List<ITweetDTO>();

            _fakeTweetQueryGenerator.CallsTo(x => x.GetRetweetsQuery(tweetId)).Returns(query);
            _fakeTwitterAccessor.ArrangeExecuteGETQuery(query, expectedResult);

            // Act
            var result = queryExecutor.GetRetweets(tweetId);

            // Assert
            Assert.AreEqual(result, expectedResult);
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

            _fakeTweetQueryGenerator.CallsTo(x => x.GetDestroyTweetQuery(tweetDTO)).Returns(query);
            _fakeTwitterAccessor.ArrangeTryExecutePOSTQuery(query, true);

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

            _fakeTweetQueryGenerator.CallsTo(x => x.GetDestroyTweetQuery(tweetDTO)).Returns(query);
            _fakeTwitterAccessor.ArrangeTryExecutePOSTQuery(query, false);

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

            _fakeTweetQueryGenerator.CallsTo(x => x.GetDestroyTweetQuery(tweetId)).Returns(query);
            _fakeTwitterAccessor.ArrangeTryExecutePOSTQuery(query, true);

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

            _fakeTweetQueryGenerator.CallsTo(x => x.GetDestroyTweetQuery(tweetId)).Returns(query);
            _fakeTwitterAccessor.ArrangeTryExecutePOSTQuery(query, false);

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

            _fakeTweetQueryGenerator.CallsTo(x => x.GetFavouriteTweetQuery(tweetDTO)).Returns(query);
            _fakeTwitterAccessor.ArrangeTryExecutePOSTQuery(query, true);

            // Act
            var result = queryExecutor.FavouriteTweet(tweetDTO);

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

            _fakeTweetQueryGenerator.CallsTo(x => x.GetFavouriteTweetQuery(tweetDTO)).Returns(query);
            _fakeTwitterAccessor.ArrangeExecutePOSTQuery(query, null);

            // Act
            var result = queryExecutor.FavouriteTweet(tweetDTO);

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

            _fakeTweetQueryGenerator.CallsTo(x => x.GetFavouriteTweetQuery(tweetDTO)).Returns(query);
            _fakeTwitterAccessor.CallsTo(x => x.ExecutePOSTQuery(query)).Throws(_fake139TwitterException);

            // Act
            var result = queryExecutor.FavouriteTweet(tweetDTO);

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

            _fakeTweetQueryGenerator.CallsTo(x => x.GetFavouriteTweetQuery(tweetDTO)).Returns(query);
            _fakeTwitterAccessor.CallsTo(x => x.ExecutePOSTQuery(query)).Throws(_fakeOtherTwitterException);

            // Act
            try
            {
                queryExecutor.FavouriteTweet(tweetDTO);
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
        public void FavouriteTweet_WithTweetIdSucceed_ReturnsTrue()
        {
            // Arrange
            var queryExecutor = CreateTweetQueryExecutor();
            var tweetId = TestHelper.GenerateRandomLong();
            var query = TestHelper.GenerateString();

            _fakeTweetQueryGenerator.CallsTo(x => x.GetFavouriteTweetQuery(tweetId)).Returns(query);
            _fakeTwitterAccessor.ArrangeTryExecutePOSTQuery(query, true);

            // Act
            var result = queryExecutor.FavouriteTweet(tweetId);

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

            _fakeTweetQueryGenerator.CallsTo(x => x.GetFavouriteTweetQuery(tweetId)).Returns(query);
            _fakeTwitterAccessor.ArrangeExecutePOSTQuery(query, null);

            // Act
            var result = queryExecutor.FavouriteTweet(tweetId);

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

            _fakeTweetQueryGenerator.CallsTo(x => x.GetUnFavouriteTweetQuery(tweetDTO)).Returns(query);
            _fakeTwitterAccessor.ArrangeTryExecutePOSTQuery(query, true);

            // Act
            var result = queryExecutor.UnFavouriteTweet(tweetDTO);

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

            _fakeTweetQueryGenerator.CallsTo(x => x.GetUnFavouriteTweetQuery(tweetDTO)).Returns(query);
            _fakeTwitterAccessor.ArrangeTryExecutePOSTQuery(query, false);

            // Act
            var result = queryExecutor.UnFavouriteTweet(tweetDTO);

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

            _fakeTweetQueryGenerator.CallsTo(x => x.GetUnFavouriteTweetQuery(tweetId)).Returns(query);
            _fakeTwitterAccessor.ArrangeTryExecutePOSTQuery(query, true);

            // Act
            var result = queryExecutor.UnFavouriteTweet(tweetId);

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

            _fakeTweetQueryGenerator.CallsTo(x => x.GetUnFavouriteTweetQuery(tweetId)).Returns(query);
            _fakeTwitterAccessor.ArrangeTryExecutePOSTQuery(query, false);

            // Act
            var result = queryExecutor.UnFavouriteTweet(tweetId);

            // Assert
            Assert.IsFalse(result);
        }

        #endregion

        #region GenerateOEmbedTweet

        [TestMethod]
        public void GenerateOEmbedTweet_WithTweetDTO_ReturnsTwitterAccessorResult()
        {
            // Arrange
            var queryExecutor = CreateTweetQueryExecutor();
            var tweetDTO = A.Fake<ITweetDTO>();
            var query = TestHelper.GenerateString();
            var expectedResult = A.Fake<IOEmbedTweetDTO>();

            _fakeTweetQueryGenerator.CallsTo(x => x.GetGenerateOEmbedTweetQuery(tweetDTO)).Returns(query);
            _fakeTwitterAccessor.ArrangeExecuteGETQuery(query, expectedResult);

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

            _fakeTweetQueryGenerator.CallsTo(x => x.GetGenerateOEmbedTweetQuery(tweetId)).Returns(query);
            _fakeTwitterAccessor.ArrangeExecuteGETQuery(query, expectedResult);

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
    }
}