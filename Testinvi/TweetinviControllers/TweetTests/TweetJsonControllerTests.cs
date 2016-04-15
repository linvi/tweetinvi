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
using Tweetinvi.Core.Interfaces.Models;
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
        public void GetRetweets_FromTweet_ReturnsResponse()
        {
            // Arrange
            var queryExecutor = CreateTweetJsonController();

            var tweetIdentifier = A.Fake<ITweetIdentifier>();

            var query = TestHelper.GenerateString();
            var expectedResult = TestHelper.GenerateString();
            var maxRetweetsToRetrieve = TestHelper.GenerateRandomInt();

            _fakeTweetQueryGenerator.CallsTo(x => x.GetRetweetsQuery(tweetIdentifier, maxRetweetsToRetrieve)).Returns(query);
            _fakeTwitterAccessor.ArrangeExecuteJsonGETQuery(query, expectedResult);

            // Act
            var result = queryExecutor.GetRetweets(tweetIdentifier, maxRetweetsToRetrieve);

            // Assert
            Assert.AreEqual(result, expectedResult);
        }

        [TestMethod]
        public void GetRetweets_FromTweetIdentifier_ReturnsResponse()
        {
            // Arrange
            var queryExecutor = CreateTweetJsonController();
            var tweetIdentifier = A.Fake<ITweetIdentifier>();
            var query = TestHelper.GenerateString();
            var expectedResult = TestHelper.GenerateString();
            var maxRetweetsToRetrieve = TestHelper.GenerateRandomInt();

            _fakeTweetQueryGenerator.CallsTo(x => x.GetRetweetsQuery(tweetIdentifier, maxRetweetsToRetrieve)).Returns(query);
            _fakeTwitterAccessor.ArrangeExecuteJsonGETQuery(query, expectedResult);

            // Act
            var result = queryExecutor.GetRetweets(tweetIdentifier, maxRetweetsToRetrieve);

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
            queryExecutor.FavoriteTweet((ITweet)null);
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

            _fakeTweetQueryGenerator.CallsTo(x => x.GetFavoriteTweetQuery(tweetDTO)).Returns(query);
            _fakeTwitterAccessor.ArrangeExecuteJsonPOSTQuery(query, expectedResult);

            // Act
            var result = queryExecutor.FavoriteTweet(tweet);

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

            _fakeTweetQueryGenerator.CallsTo(x => x.GetFavoriteTweetQuery(tweetDTO)).Returns(query);
            _fakeTwitterAccessor.ArrangeExecuteJsonPOSTQuery(query, expectedResult);

            // Act
            var result = queryExecutor.FavoriteTweet(tweetDTO);

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

            _fakeTweetQueryGenerator.CallsTo(x => x.GetFavoriteTweetQuery(tweetId)).Returns(query);
            _fakeTwitterAccessor.ArrangeExecuteJsonPOSTQuery(query, expectedResult);

            // Act
            var result = queryExecutor.FavoriteTweet(tweetId);

            // Assert
            Assert.AreEqual(result, expectedResult);
        }

        #endregion

        #region UnFavoriteTweet

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void UnFavouriteTweet_WithNullTweet_ThrowArgumentException()
        {
            // Arrange
            var queryExecutor = CreateTweetJsonController();

            // Act
            queryExecutor.UnFavoriteTweet((ITweet)null);
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

            _fakeTweetQueryGenerator.CallsTo(x => x.GetUnFavoriteTweetQuery(tweetDTO)).Returns(query);
            _fakeTwitterAccessor.ArrangeExecuteJsonPOSTQuery(query, expectedResult);

            // Act
            var result = queryExecutor.UnFavoriteTweet(tweet);

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

            _fakeTweetQueryGenerator.CallsTo(x => x.GetUnFavoriteTweetQuery(tweetDTO)).Returns(query);
            _fakeTwitterAccessor.ArrangeExecuteJsonPOSTQuery(query, expectedResult);

            // Act
            var result = queryExecutor.UnFavoriteTweet(tweetDTO);

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

            _fakeTweetQueryGenerator.CallsTo(x => x.GetUnFavoriteTweetQuery(tweetId)).Returns(query);
            _fakeTwitterAccessor.ArrangeExecuteJsonPOSTQuery(query, expectedResult);

            // Act
            var result = queryExecutor.UnFavoriteTweet(tweetId);

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