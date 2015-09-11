using System;
using FakeItEasy;
using FakeItEasy.ExtensionSyntax.Full;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Testinvi.Helpers;
using Testinvi.SetupHelpers;
using Tweetinvi.Controllers.Tweet;
using Tweetinvi.Core.Interfaces;
using Tweetinvi.Core.Interfaces.Credentials;
using Tweetinvi.Core.Interfaces.DTO;
using Tweetinvi.Core.Interfaces.QueryGenerators;

namespace Testinvi.TweetinviControllers.TweetTests
{
    [TestClass]
    public class TweetJsonControllerTests
    {
        private FakeClassBuilder<TweetJsonController> _fakeBuilder;
        private Fake<ITweetQueryGenerator> _fakeTweetQueryGenerator;
        private Fake<ITwitterAccessor> _fakeTwitterAccessor;

        [TestInitialize]
        public void TestInitialize()
        {
            _fakeBuilder = new FakeClassBuilder<TweetJsonController>();
            _fakeTweetQueryGenerator = _fakeBuilder.GetFake<ITweetQueryGenerator>();
            _fakeTwitterAccessor = _fakeBuilder.GetFake<ITwitterAccessor>();
        }

        #region Publish Retweet

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void PublishNullRetweet_WithTweet_ThrowArgumentException()
        {
            // Arrange
            var queryExecutor = CreateTweetJsonController();

            // Act
            queryExecutor.PublishRetweet((ITweet)null);
        }

        [TestMethod]
        public void PublishRetweet_WithTweet_ReturnsResponse()
        {
            // Arrange
            var queryExecutor = CreateTweetJsonController();

            var tweetDTO = A.Fake<ITweetDTO>();
            var tweet = A.Fake<ITweet>();
            tweet.CallsTo(x => x.TweetDTO).Returns(tweetDTO);

            var query = TestHelper.GenerateString();
            var expectedResult = TestHelper.GenerateString();

            _fakeTweetQueryGenerator.CallsTo(x => x.GetPublishRetweetQuery(tweetDTO)).Returns(query);
            _fakeTwitterAccessor.ArrangeExecuteJsonPOSTQuery(query, expectedResult);

            // Act
            var result = queryExecutor.PublishRetweet(tweet);

            // Assert
            Assert.AreEqual(result, expectedResult);
        }

        [TestMethod]
        public void PublishRetweet_WithTweetDTO_ReturnsResponse()
        {
            // Arrange
            var queryExecutor = CreateTweetJsonController();
            var tweetDTO = A.Fake<ITweetDTO>();
            var query = TestHelper.GenerateString();
            var expectedResult = TestHelper.GenerateString();

            _fakeTweetQueryGenerator.CallsTo(x => x.GetPublishRetweetQuery(tweetDTO)).Returns(query);
            _fakeTwitterAccessor.ArrangeExecuteJsonPOSTQuery(query, expectedResult);

            // Act
            var result = queryExecutor.PublishRetweet(tweetDTO);

            // Assert
            Assert.AreEqual(result, expectedResult);
        }

        [TestMethod]
        public void PublishRetweet_WithTweetId_ReturnsResponse()
        {
            // Arrange
            var queryExecutor = CreateTweetJsonController();
            var tweetId = TestHelper.GenerateRandomLong();
            var query = TestHelper.GenerateString();
            var expectedResult = TestHelper.GenerateString();

            _fakeTweetQueryGenerator.CallsTo(x => x.GetPublishRetweetQuery(tweetId)).Returns(query);
            _fakeTwitterAccessor.ArrangeExecuteJsonPOSTQuery(query, expectedResult);

            // Act
            var result = queryExecutor.PublishRetweet(tweetId);

            // Assert
            Assert.AreEqual(result, expectedResult);
        }

        #endregion

        #region Get Retweets

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void GetRetweets_FromNullTweet_ThrowArgumentException()
        {
            // Arrange
            var queryExecutor = CreateTweetJsonController();

            // Act
            queryExecutor.GetRetweets((ITweet)null);
        }

        [TestMethod]
        public void GetRetweets_FromTweet_ReturnsResponse()
        {
            // Arrange
            var queryExecutor = CreateTweetJsonController();

            var tweetDTO = A.Fake<ITweetDTO>();
            var tweet = A.Fake<ITweet>();
            tweet.CallsTo(x => x.TweetDTO).Returns(tweetDTO);

            var query = TestHelper.GenerateString();
            var expectedResult = TestHelper.GenerateString();

            _fakeTweetQueryGenerator.CallsTo(x => x.GetRetweetsQuery(tweetDTO)).Returns(query);
            _fakeTwitterAccessor.ArrangeExecuteJsonGETQuery(query, expectedResult);

            // Act
            var result = queryExecutor.GetRetweets(tweet);

            // Assert
            Assert.AreEqual(result, expectedResult);
        }

        [TestMethod]
        public void GetRetweets_FromTweetDTO_ReturnsResponse()
        {
            // Arrange
            var queryExecutor = CreateTweetJsonController();
            var tweetDTO = A.Fake<ITweetDTO>();
            var query = TestHelper.GenerateString();
            var expectedResult = TestHelper.GenerateString();

            _fakeTweetQueryGenerator.CallsTo(x => x.GetRetweetsQuery(tweetDTO)).Returns(query);
            _fakeTwitterAccessor.ArrangeExecuteJsonGETQuery(query, expectedResult);

            // Act
            var result = queryExecutor.GetRetweets(tweetDTO);

            // Assert
            Assert.AreEqual(result, expectedResult);
        }

        [TestMethod]
        public void GetRetweets_FromTweetId_ReturnsResponse()
        {
            // Arrange
            var queryExecutor = CreateTweetJsonController();
            var tweetId = TestHelper.GenerateRandomLong();
            var query = TestHelper.GenerateString();
            var expectedResult = TestHelper.GenerateString();

            _fakeTweetQueryGenerator.CallsTo(x => x.GetRetweetsQuery(tweetId)).Returns(query);
            _fakeTwitterAccessor.ArrangeExecuteJsonGETQuery(query, expectedResult);

            // Act
            var result = queryExecutor.GetRetweets(tweetId);

            // Assert
            Assert.AreEqual(result, expectedResult);
        }

        #endregion

        #region Destroy Tweet

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void DestroyTweet_WithNullTweet_ThrowsArgumentException()
        {
            // Arrange
            var queryExecutor = CreateTweetJsonController();

            // Act
            queryExecutor.DestroyTweet((ITweet)null);
        }

        [TestMethod]
        public void DestroyTweet_WithTweet_ReturnsResponse()
        {
            // Arrange
            var queryExecutor = CreateTweetJsonController();
            var tweetDTO = A.Fake<ITweetDTO>();
            var tweet = A.Fake<ITweet>();
            tweet.CallsTo(x => x.TweetDTO).Returns(tweetDTO);

            var query = TestHelper.GenerateString();
            var expectedResponse = TestHelper.GenerateString();

            _fakeTweetQueryGenerator.CallsTo(x => x.GetDestroyTweetQuery(tweetDTO)).Returns(query);
            _fakeTwitterAccessor.ArrangeExecuteJsonPOSTQuery(query, expectedResponse);

            // Act
            var result = queryExecutor.DestroyTweet(tweet);

            // Assert
            Assert.AreEqual(result, expectedResponse);
        }

        [TestMethod]
        public void DestroyTweet_WithTweetDTO_ReturnsResponse()
        {
            // Arrange
            var queryExecutor = CreateTweetJsonController();
            var tweetDTO = A.Fake<ITweetDTO>();
            var query = TestHelper.GenerateString();
            var expectedResponse = TestHelper.GenerateString();

            _fakeTweetQueryGenerator.CallsTo(x => x.GetDestroyTweetQuery(tweetDTO)).Returns(query);
            _fakeTwitterAccessor.ArrangeExecuteJsonPOSTQuery(query, expectedResponse);

            // Act
            var result = queryExecutor.DestroyTweet(tweetDTO);

            // Assert
            Assert.AreEqual(result, expectedResponse);
        }

        [TestMethod]
        public void DestroyTweet_WithTweetId_ReturnsResponse()
        {
            // Arrange
            var queryExecutor = CreateTweetJsonController();
            var tweetId = TestHelper.GenerateRandomLong();
            var query = TestHelper.GenerateString();
            var expectedResponse = TestHelper.GenerateString();

            _fakeTweetQueryGenerator.CallsTo(x => x.GetDestroyTweetQuery(tweetId)).Returns(query);
            _fakeTwitterAccessor.ArrangeExecuteJsonPOSTQuery(query, expectedResponse);

            // Act
            var result = queryExecutor.DestroyTweet(tweetId);

            // Assert
            Assert.AreEqual(result, expectedResponse);
        }

        #endregion

        #region Favourite Tweet

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void FavouriteTweet_WithNullTweet_ThrowsArgumentException()
        {
            // Arrange
            var queryExecutor = CreateTweetJsonController();

            // Act
            queryExecutor.FavouriteTweet((ITweet)null);
        }

        [TestMethod]
        public void FavouriteTweet_WithTweet_ReturnsResponse()
        {
            // Arrange
            var queryExecutor = CreateTweetJsonController();

            var tweetDTO = A.Fake<ITweetDTO>();
            var tweet = A.Fake<ITweet>();
            tweet.CallsTo(x => x.TweetDTO).Returns(tweetDTO);

            var query = TestHelper.GenerateString();
            var expectedResult = TestHelper.GenerateString();

            _fakeTweetQueryGenerator.CallsTo(x => x.GetFavouriteTweetQuery(tweetDTO)).Returns(query);
            _fakeTwitterAccessor.ArrangeExecuteJsonPOSTQuery(query, expectedResult);

            // Act
            var result = queryExecutor.FavouriteTweet(tweet);

            // Assert
            Assert.AreEqual(result, expectedResult);
        }

        [TestMethod]
        public void FavouriteTweet_WithTweetDTO_ReturnsResponse()
        {
            // Arrange
            var queryExecutor = CreateTweetJsonController();
            var tweetDTO = A.Fake<ITweetDTO>();
            var query = TestHelper.GenerateString();
            var expectedResult = TestHelper.GenerateString();

            _fakeTweetQueryGenerator.CallsTo(x => x.GetFavouriteTweetQuery(tweetDTO)).Returns(query);
            _fakeTwitterAccessor.ArrangeExecuteJsonPOSTQuery(query, expectedResult);

            // Act
            var result = queryExecutor.FavouriteTweet(tweetDTO);

            // Assert
            Assert.AreEqual(result, expectedResult);
        }

        [TestMethod]
        public void FavouriteTweet_WithTweetId_ReturnsResponse()
        {
            // Arrange
            var queryExecutor = CreateTweetJsonController();
            var tweetId = TestHelper.GenerateRandomLong();
            var query = TestHelper.GenerateString();
            var expectedResult = TestHelper.GenerateString();

            _fakeTweetQueryGenerator.CallsTo(x => x.GetFavouriteTweetQuery(tweetId)).Returns(query);
            _fakeTwitterAccessor.ArrangeExecuteJsonPOSTQuery(query, expectedResult);

            // Act
            var result = queryExecutor.FavouriteTweet(tweetId);

            // Assert
            Assert.AreEqual(result, expectedResult);
        }

        #endregion

        #region UnFavouriteTweet

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void UnFavouriteTweet_WithNullTweet_ThrowArgumentException()
        {
            // Arrange
            var queryExecutor = CreateTweetJsonController();

            // Act
            queryExecutor.UnFavouriteTweet((ITweet)null);
        }

        [TestMethod]
        public void UnFavouriteTweet_WithTweet_ReturnsResponse()
        {
            // Arrange
            var queryExecutor = CreateTweetJsonController();

            var tweetDTO = A.Fake<ITweetDTO>();
            var tweet = A.Fake<ITweet>();
            tweet.CallsTo(x => x.TweetDTO).Returns(tweetDTO);

            var query = TestHelper.GenerateString();
            var expectedResult = TestHelper.GenerateString();

            _fakeTweetQueryGenerator.CallsTo(x => x.GetUnFavouriteTweetQuery(tweetDTO)).Returns(query);
            _fakeTwitterAccessor.ArrangeExecuteJsonPOSTQuery(query, expectedResult);

            // Act
            var result = queryExecutor.UnFavouriteTweet(tweet);

            // Assert
            Assert.AreEqual(result, expectedResult);
        }

        [TestMethod]
        public void UnFavouriteTweet_WithTweetDTO_ReturnsResponse()
        {
            // Arrange
            var queryExecutor = CreateTweetJsonController();
            var tweetDTO = A.Fake<ITweetDTO>();
            var query = TestHelper.GenerateString();
            var expectedResult = TestHelper.GenerateString();

            _fakeTweetQueryGenerator.CallsTo(x => x.GetUnFavouriteTweetQuery(tweetDTO)).Returns(query);
            _fakeTwitterAccessor.ArrangeExecuteJsonPOSTQuery(query, expectedResult);

            // Act
            var result = queryExecutor.UnFavouriteTweet(tweetDTO);

            // Assert
            Assert.AreEqual(result, expectedResult);
        }

        [TestMethod]
        public void UnFavouriteTweet_WithTweetId_ReturnsResponse()
        {
            // Arrange
            var queryExecutor = CreateTweetJsonController();
            var tweetId = TestHelper.GenerateRandomLong();
            var query = TestHelper.GenerateString();
            var expectedResult = TestHelper.GenerateString();

            _fakeTweetQueryGenerator.CallsTo(x => x.GetUnFavouriteTweetQuery(tweetId)).Returns(query);
            _fakeTwitterAccessor.ArrangeExecuteJsonPOSTQuery(query, expectedResult);

            // Act
            var result = queryExecutor.UnFavouriteTweet(tweetId);

            // Assert
            Assert.AreEqual(result, expectedResult);
        }

        #endregion

        #region GenerateOEmbedTweet

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void GenerateOEmbedTweet_WithNullTweet_ThrowsArgumentException()
        {
            // Arrange
            var queryExecutor = CreateTweetJsonController();

            // Act
            queryExecutor.GenerateOEmbedTweet((ITweet)null);
        }

        [TestMethod]
        public void GenerateOEmbedTweet_WithTweet_ReturnsResponse()
        {
            // Arrange
            var queryExecutor = CreateTweetJsonController();

            var tweetDTO = A.Fake<ITweetDTO>();
            var tweet = A.Fake<ITweet>();
            tweet.CallsTo(x => x.TweetDTO).Returns(tweetDTO);

            var query = TestHelper.GenerateString();
            var expectedResult = TestHelper.GenerateString();

            _fakeTweetQueryGenerator.CallsTo(x => x.GetGenerateOEmbedTweetQuery(tweetDTO)).Returns(query);
            _fakeTwitterAccessor.ArrangeExecuteJsonGETQuery(query, expectedResult);

            // Act
            var result = queryExecutor.GenerateOEmbedTweet(tweet);

            // Assert
            Assert.AreEqual(result, expectedResult);
        }

        [TestMethod]
        public void GenerateOEmbedTweet_WithTweetDTO_ReturnsResponse()
        {
            // Arrange
            var queryExecutor = CreateTweetJsonController();
            var tweetDTO = A.Fake<ITweetDTO>();
            var query = TestHelper.GenerateString();
            var expectedResult = TestHelper.GenerateString();

            _fakeTweetQueryGenerator.CallsTo(x => x.GetGenerateOEmbedTweetQuery(tweetDTO)).Returns(query);
            _fakeTwitterAccessor.ArrangeExecuteJsonGETQuery(query, expectedResult);

            // Act
            var result = queryExecutor.GenerateOEmbedTweet(tweetDTO);

            // Assert
            Assert.AreEqual(result, expectedResult);
        }

        [TestMethod]
        public void GenerateOEmbedTweet_WithTweetId_ReturnsResponse()
        {
            // Arrange
            var queryExecutor = CreateTweetJsonController();
            var tweetId = TestHelper.GenerateRandomLong();
            var query = TestHelper.GenerateString();
            var expectedResult = TestHelper.GenerateString();

            _fakeTweetQueryGenerator.CallsTo(x => x.GetGenerateOEmbedTweetQuery(tweetId)).Returns(query);
            _fakeTwitterAccessor.ArrangeExecuteJsonGETQuery(query, expectedResult);

            // Act
            var result = queryExecutor.GenerateOEmbedTweet(tweetId);

            // Assert
            Assert.AreEqual(result, expectedResult);
        }

        #endregion

        public TweetJsonController CreateTweetJsonController()
        {
            return _fakeBuilder.GenerateClass();
        }
    }
}