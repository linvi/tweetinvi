using System;
using System.Collections.Generic;
using FakeItEasy;
using FakeItEasy.ExtensionSyntax.Full;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Testinvi.Helpers;
using Tweetinvi.Controllers.Tweet;
using Tweetinvi.Core.Interfaces;
using Tweetinvi.Core.Interfaces.DTO;
using Tweetinvi.Core.Interfaces.Factories;
using Tweetinvi.Core.Interfaces.Models;

namespace Testinvi.TweetinviControllers.TweetTests
{
    [TestClass]
    public class TweetControllerTests
    {
        private FakeClassBuilder<TweetController> _fakeBuilder;
        private Fake<ITweetQueryExecutor> _fakeTweetQueryExecutor;
        private Fake<ITweetFactory> _fakeTweetFactory;

        [TestInitialize]
        public void TestInitialize()
        {
            _fakeBuilder = new FakeClassBuilder<TweetController>();
            _fakeTweetQueryExecutor = _fakeBuilder.GetFake<ITweetQueryExecutor>();
            _fakeTweetFactory = _fakeBuilder.GetFake<ITweetFactory>();
        }

        #region Publish Tweet

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void PublishTweet_WithNullTweet_ThrowsArgumentException()
        {
            // Arrange
            var controller = CreateTweetController();

            // Act
            controller.PublishTweet((ITweet)null);
        }

        #endregion

        #region Publish Retweet

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void PublishRetweet_TweetIsNull_ThrowsArgumentException()
        {
            // Arrange
            var controller = CreateTweetController();

            // Act
            controller.PublishRetweet((ITweet)null);
        }

        [TestMethod]
        public void PublishRetweet_TweetWithNullTweetDTO_ReturnsTransformedDTOFromQueryExecutor()
        {
            // Arrange
            var controller = CreateTweetController();
            var tweet = GenerateTweet();
            var expectedTweetDTO = A.Fake<ITweetDTO>();
            var expectedTweet = A.Fake<ITweet>();

            _fakeTweetQueryExecutor.CallsTo(x => x.PublishRetweet(null)).Returns(expectedTweetDTO);
            _fakeTweetFactory.CallsTo(x => x.GenerateTweetFromDTO(expectedTweetDTO)).Returns(expectedTweet);

            // Act
            var result = controller.PublishRetweet(tweet);

            // Assert
            Assert.AreEqual(result, expectedTweet);
        }

        [TestMethod]
        public void PublishRetweet_TweetWithTweetDTO_ReturnsTransformedDTOFromQueryExecutor()
        {
            // Arrange
            var controller = CreateTweetController();
            var tweetDTO = A.Fake<ITweetDTO>();
            var tweet = GenerateTweet(tweetDTO);
            var expectedTweetDTO = A.Fake<ITweetDTO>();
            var expectedTweet = A.Fake<ITweet>();

            _fakeTweetQueryExecutor.CallsTo(x => x.PublishRetweet(tweetDTO)).Returns(expectedTweetDTO);
            _fakeTweetFactory.CallsTo(x => x.GenerateTweetFromDTO(expectedTweetDTO)).Returns(expectedTweet);

            // Act
            var result = controller.PublishRetweet(tweet);

            // Assert
            Assert.AreEqual(result, expectedTweet);
        }

        [TestMethod]
        public void PublishRetweet_TweetDTOIsNull_ReturnsTransformedDTOFromQueryExecutor()
        {
            // Arrange
            var controller = CreateTweetController();
            var expectedTweetDTO = A.Fake<ITweetDTO>();
            var expectedTweet = A.Fake<ITweet>();

            _fakeTweetQueryExecutor.CallsTo(x => x.PublishRetweet(null)).Returns(expectedTweetDTO);
            _fakeTweetFactory.CallsTo(x => x.GenerateTweetFromDTO(expectedTweetDTO)).Returns(expectedTweet);

            // Act
            var result = controller.PublishRetweet((ITweetDTO)null);

            // Assert
            Assert.AreEqual(result, expectedTweet);
        }

        [TestMethod]
        public void PublishRetweet_TweetDTO_ReturnsTransformedDTOFromQueryExecutor()
        {
            // Arrange
            var controller = CreateTweetController();
            var tweetDTO = A.Fake<ITweetDTO>();
            var expectedTweetDTO = A.Fake<ITweetDTO>();
            var expectedTweet = A.Fake<ITweet>();

            _fakeTweetQueryExecutor.CallsTo(x => x.PublishRetweet(tweetDTO)).Returns(expectedTweetDTO);
            _fakeTweetFactory.CallsTo(x => x.GenerateTweetFromDTO(expectedTweetDTO)).Returns(expectedTweet);

            // Act
            var result = controller.PublishRetweet(tweetDTO);

            // Assert
            Assert.AreEqual(result, expectedTweet);
        }

        [TestMethod]
        public void PublishRetweet_TweetId_ReturnsTransformedDTOFromQueryExecutor()
        {
            // Arrange
            var controller = CreateTweetController();
            var tweetId = TestHelper.GenerateRandomLong();
            var expectedTweetDTO = A.Fake<ITweetDTO>();
            var expectedTweet = A.Fake<ITweet>();

            _fakeTweetQueryExecutor.CallsTo(x => x.PublishRetweet(tweetId)).Returns(expectedTweetDTO);
            _fakeTweetFactory.CallsTo(x => x.GenerateTweetFromDTO(expectedTweetDTO)).Returns(expectedTweet);

            // Act
            var result = controller.PublishRetweet(tweetId);

            // Assert
            Assert.AreEqual(result, expectedTweet);
        }

        #endregion

        #region Get Retweets

        [TestMethod]
        public void GetRetweets_TweetDTO_ReturnsTransformedDTOFromQueryExecutor()
        {
            // Arrange
            var controller = CreateTweetController();
            var tweetDTO = A.Fake<ITweetDTO>();
            IEnumerable<ITweetDTO> expectedTweetsDTO = new List<ITweetDTO> { A.Fake<ITweetDTO>() };
            var expectedTweets = new ITweet [] { A.Fake<ITweet>() };
            var maxRetweetsToRetrieve = TestHelper.GenerateRandomInt();

            _fakeTweetQueryExecutor.CallsTo(x => x.GetRetweets(tweetDTO, maxRetweetsToRetrieve)).Returns(expectedTweetsDTO);
            _fakeTweetFactory.CallsTo(x => x.GenerateTweetsFromDTO(expectedTweetsDTO)).Returns(expectedTweets);

            // Act
            var result = controller.GetRetweets(tweetDTO, maxRetweetsToRetrieve);

            // Assert
            Assert.AreEqual(result, expectedTweets);
        }

        [TestMethod]
        public void GetRetweets_TweetId_ReturnsTransformedDTOFromQueryExecutor()
        {
            // Arrange
            var controller = CreateTweetController();
            var tweetId = TestHelper.GenerateRandomLong();
            var maxRetweetsToRetrieve = TestHelper.GenerateRandomInt();
            IEnumerable<ITweetDTO> expectedTweetsDTO = new List<ITweetDTO> { A.Fake<ITweetDTO>() };
            IEnumerable<ITweet> expectedTweets = new List<ITweet> { A.Fake<ITweet>() };

            _fakeTweetQueryExecutor.CallsTo(x => x.GetRetweets(A<ITweetIdentifier>.That.Matches(y => y.Id == tweetId), maxRetweetsToRetrieve)).Returns(expectedTweetsDTO);
            _fakeTweetFactory.CallsTo(x => x.GenerateTweetsFromDTO(expectedTweetsDTO)).Returns(expectedTweets);

            // Act
            var result = controller.GetRetweets(tweetId, maxRetweetsToRetrieve);

            // Assert
            Assert.AreEqual(result, expectedTweets);
        }

        #endregion

        #region Get Retweeters Ids

        [TestMethod]
        public void GetRetweetersIds_WithTweetIdentifier_ReturnsIdsListFromQueryExecutor()
        {
            // Arrange
            var controller = CreateTweetController();
            var tweetIdentifier = A.Fake<ITweetIdentifier>();
            var maxRetweetersToRetrieve = TestHelper.GenerateRandomInt();
            var retweeterIds = new[] { TestHelper.GenerateRandomLong() };

            _fakeTweetQueryExecutor.CallsTo(x => x.GetRetweetersIds(tweetIdentifier, maxRetweetersToRetrieve)).Returns(retweeterIds);

            // Act
            var result = controller.GetRetweetersIds(tweetIdentifier, maxRetweetersToRetrieve);

            // Assert
            Assert.AreEqual(result, retweeterIds);
        }

        [TestMethod]
        public void GetRetweetersIds_WithTweetId_ReturnsIdsListFromQueryExecutor()
        {
            // Arrange
            var controller = CreateTweetController();
            var tweetId = TestHelper.GenerateRandomLong();
            var maxRetweetersToRetrieve = TestHelper.GenerateRandomInt();
            var retweeterIds = new[] { TestHelper.GenerateRandomLong() };

            _fakeTweetQueryExecutor.CallsTo(x => x.GetRetweetersIds(A<ITweetIdentifier>.That.Matches(y => y.Id == tweetId), maxRetweetersToRetrieve)).Returns(retweeterIds);

            // Act
            var result = controller.GetRetweetersIds(tweetId, maxRetweetersToRetrieve);

            // Assert
            Assert.AreEqual(result, retweeterIds);
        }
        
        #endregion

        #region Destroy Tweet

        // With Tweet
        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void DestroyTweet_WithNullTweet_ThrowArgumentException()
        {
            // Arrange
            var controller = CreateTweetController();

            // Act
            controller.DestroyTweet((ITweet)null);
        }

        [TestMethod]
        public void DestroyTweet_WithTweetButNullTweetDTOAndQueryExecutorReturnsTrue_ReturnsFalse()
        {
            // Arrange
            var controller = CreateTweetController();
            var tweet = GenerateTweet();

            _fakeTweetQueryExecutor.CallsTo(x => x.DestroyTweet(null)).Returns(true);

            // Act
            var result = controller.DestroyTweet(tweet);

            // Assert
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void DestroyTweet_WithTweetButNullTweetDTOAndQueryExecutorReturnsFalse_ReturnsFalse()
        {
            // Arrange
            var controller = CreateTweetController();
            var tweet = GenerateTweet();

            _fakeTweetQueryExecutor.CallsTo(x => x.DestroyTweet(null)).Returns(false);

            // Act
            var result = controller.DestroyTweet(tweet);

            // Assert
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void DestroyTweet_WithTweetAndQueryExecutorReturnsTrue_ReturnsTrue()
        {
            // Arrange
            var controller = CreateTweetController();
            var tweetDTO = A.Fake<ITweetDTO>();
            var tweet = GenerateTweet(tweetDTO);

            _fakeTweetQueryExecutor.CallsTo(x => x.DestroyTweet(tweetDTO)).Returns(true);

            // Act
            var result = controller.DestroyTweet(tweet);

            // Assert
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void DestroyTweet_WithTweetAndQueryExecutorReturnsFalse_ReturnsFalse()
        {
            // Arrange
            var controller = CreateTweetController();
            var tweetDTO = A.Fake<ITweetDTO>();
            var tweet = GenerateTweet(tweetDTO);

            _fakeTweetQueryExecutor.CallsTo(x => x.DestroyTweet(tweetDTO)).Returns(false);

            // Act
            var result = controller.DestroyTweet(tweet);

            // Assert
            Assert.IsFalse(result);
        }

        // With TweetDTO
        [TestMethod]
        public void DestroyTweet_WithNullTweetDTOAndQueryExecutorReturnsTrue_ReturnsTrue()
        {
            // Arrange
            var controller = CreateTweetController();

            _fakeTweetQueryExecutor.CallsTo(x => x.DestroyTweet(null)).Returns(true);

            // Act
            var result = controller.DestroyTweet((ITweetDTO)null);

            // Assert
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void DestroyTweet_WithNullTweetDTOAndQueryExecutorReturnsFalse_ReturnsFalse()
        {
            // Arrange
            var controller = CreateTweetController();

            _fakeTweetQueryExecutor.CallsTo(x => x.DestroyTweet(null)).Returns(false);

            // Act
            var result = controller.DestroyTweet((ITweetDTO)null);

            // Assert
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void DestroyTweet_WithTweetDTOAndQueryExecutorReturnsTrue_ReturnsTrue()
        {
            // Arrange
            var controller = CreateTweetController();
            var tweetDTO = A.Fake<ITweetDTO>();

            _fakeTweetQueryExecutor.CallsTo(x => x.DestroyTweet(tweetDTO)).Returns(true);

            // Act
            var result = controller.DestroyTweet(tweetDTO);

            // Assert
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void DestroyTweet_WithTweetDTOAndQueryExecutorReturnsFalse_ReturnsFalse()
        {
            // Arrange
            var controller = CreateTweetController();
            var tweetDTO = A.Fake<ITweetDTO>();

            _fakeTweetQueryExecutor.CallsTo(x => x.DestroyTweet(tweetDTO)).Returns(false);

            // Act
            var result = controller.DestroyTweet(tweetDTO);

            // Assert
            Assert.IsFalse(result);
        }

        // With TweetID
        [TestMethod]
        public void DestroyTweet_WithTweetIdAndQueryExecutorReturnsTrue_ReturnsTrue()
        {
            // Arrange
            var controller = CreateTweetController();
            var tweetId = TestHelper.GenerateRandomLong();

            _fakeTweetQueryExecutor.CallsTo(x => x.DestroyTweet(tweetId)).Returns(true);

            // Act
            var result = controller.DestroyTweet(tweetId);

            // Assert
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void DestroyTweet_WithTweetIdAndQueryExecutorReturnsFalse_ReturnsFalse()
        {
            // Arrange
            var controller = CreateTweetController();
            var tweetId = TestHelper.GenerateRandomLong();

            _fakeTweetQueryExecutor.CallsTo(x => x.DestroyTweet(tweetId)).Returns(false);

            // Act
            var result = controller.DestroyTweet(tweetId);

            // Assert
            Assert.IsFalse(result);
        }

        #endregion

        #region Favorite Tweet

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void FavoriteTweet_WithNullTweet_ThrowsArgumentException()
        {
            // Arrange
            var controller = CreateTweetController();

            // Act
            controller.FavoriteTweet((ITweet)null);
        }

        public void VerifyFavoriteTweet_WithTweet_ReturnsQueryExecutorResult(bool doesTweetDTOExists, bool expectedResult)
        {
            // Arrange
            var controller = CreateTweetController();
            var tweetDTO = doesTweetDTOExists ? A.Fake<ITweetDTO>() : null;
            var tweet = GenerateTweet(tweetDTO);

            _fakeTweetQueryExecutor.CallsTo(x => x.FavoriteTweet(tweetDTO)).Returns(expectedResult);

            // Act
            var result = controller.FavoriteTweet(tweet);

            // Assert
            Assert.AreEqual(result, expectedResult);
        }

        [TestMethod]
        public void FavoriteTweet_WithTweetDTO_ReturnsQueryExecutorResult()
        {
            VerifyFavoriteTweet_WithTweetDTO_ReturnsQueryExecutorResult(false, false);
            VerifyFavoriteTweet_WithTweetDTO_ReturnsQueryExecutorResult(false, false);
            VerifyFavoriteTweet_WithTweetDTO_ReturnsQueryExecutorResult(true, false);
            VerifyFavoriteTweet_WithTweetDTO_ReturnsQueryExecutorResult(true, true);
        }

        public void VerifyFavoriteTweet_WithTweetDTO_ReturnsQueryExecutorResult(bool tweetExists, bool expectedResult)
        {
            // Arrange
            var controller = CreateTweetController();
            var tweetDTO = tweetExists ? A.Fake<ITweetDTO>() : null;

            _fakeTweetQueryExecutor.CallsTo(x => x.FavoriteTweet(tweetDTO)).Returns(expectedResult);

            // Act
            var result = controller.FavoriteTweet(tweetDTO);

            // Assert
            Assert.AreEqual(result, expectedResult);
        }

        [TestMethod]
        public void FavoriteTweet_WithTweetId_ReturnsQueryExecutorResult()
        {
            VerifyFavoriteTweet_WithTweetId_ReturnsQueryExecutorResult(false);
            VerifyFavoriteTweet_WithTweetId_ReturnsQueryExecutorResult(true);
        }

        public void VerifyFavoriteTweet_WithTweetId_ReturnsQueryExecutorResult(bool expectedResult)
        {
            // Arrange
            var controller = CreateTweetController();
            var tweetId = TestHelper.GenerateRandomLong();

            _fakeTweetQueryExecutor.CallsTo(x => x.FavoriteTweet(tweetId)).Returns(expectedResult);

            // Act
            var result = controller.FavoriteTweet(tweetId);

            // Assert
            Assert.AreEqual(result, expectedResult);
        }

        #endregion

        #region UnFavorite Tweet

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void UnFavoriteTweet_WithNullTweet_ThrowsArgumentException()
        {
            // Arrange
            var controller = CreateTweetController();

            // Act
            controller.UnFavoriteTweet((ITweet)null);
        }

        [TestMethod]
        public void UnFavoriteTweet_WithTweet_ReturnsQueryExecutorResult()
        {
            VerifyUnFavoriteTweet_WithTweet_ReturnsQueryExecutorResult(false, false);
            VerifyUnFavoriteTweet_WithTweet_ReturnsQueryExecutorResult(false, true);
            VerifyUnFavoriteTweet_WithTweet_ReturnsQueryExecutorResult(true, false);
            VerifyUnFavoriteTweet_WithTweet_ReturnsQueryExecutorResult(true, true);
        }

        public void VerifyUnFavoriteTweet_WithTweet_ReturnsQueryExecutorResult(bool doesTweetDTOExists, bool expectedResult)
        {
            // Arrange
            var controller = CreateTweetController();
            var tweetDTO = doesTweetDTOExists ? A.Fake<ITweetDTO>() : null;
            var tweet = GenerateTweet(tweetDTO);

            _fakeTweetQueryExecutor.CallsTo(x => x.UnFavoriteTweet(tweetDTO)).Returns(expectedResult);

            // Act
            var result = controller.UnFavoriteTweet(tweet);

            // Assert
            Assert.AreEqual(result, expectedResult);
        }

        [TestMethod]
        public void UnFavoriteTweet_WithTweetDTO_ReturnsQueryExecutorResult()
        {
            VerifyUnFavoriteTweet_WithTweetDTO_ReturnsQueryExecutorResult(false, false);
            VerifyUnFavoriteTweet_WithTweetDTO_ReturnsQueryExecutorResult(false, true);
            VerifyUnFavoriteTweet_WithTweetDTO_ReturnsQueryExecutorResult(true, false);
            VerifyUnFavoriteTweet_WithTweetDTO_ReturnsQueryExecutorResult(true, true);
        }

        public void VerifyUnFavoriteTweet_WithTweetDTO_ReturnsQueryExecutorResult(bool tweetExists, bool expectedResult)
        {
            // Arrange
            var controller = CreateTweetController();
            var tweetDTO = tweetExists ? A.Fake<ITweetDTO>() : null;

            _fakeTweetQueryExecutor.CallsTo(x => x.UnFavoriteTweet(tweetDTO)).Returns(expectedResult);

            // Act
            var result = controller.UnFavoriteTweet(tweetDTO);

            // Assert
            Assert.AreEqual(result, expectedResult);
        }

        [TestMethod]
        public void UnFavoriteTweet_WithTweetId_ReturnsQueryExecutorResult()
        {
            VerifyUnFavoriteTweet_WithTweetId_ReturnsQueryExecutorResult(false);
            VerifyUnFavoriteTweet_WithTweetId_ReturnsQueryExecutorResult(true);
        }

        public void VerifyUnFavoriteTweet_WithTweetId_ReturnsQueryExecutorResult(bool expectedResult)
        {
            // Arrange
            var controller = CreateTweetController();
            var tweetId = TestHelper.GenerateRandomLong();

            _fakeTweetQueryExecutor.CallsTo(x => x.UnFavoriteTweet(tweetId)).Returns(expectedResult);

            // Act
            var result = controller.UnFavoriteTweet(tweetId);

            // Assert
            Assert.AreEqual(result, expectedResult);
        }

        #endregion

        #region Generate OEmbed Tweet

        // With Tweet
        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void GenerateOEmbedTweet_WithNullTweet_ThrowsArgumentException()
        {
            // Arrange
            var controller = CreateTweetController();

            // Act
            controller.GenerateOEmbedTweet((ITweet)null);
        }

        [TestMethod]
        public void GenerateOEmbedTweet_WithTweetButNullTweetDTO_ReturnsQueryExecutor()
        {
            // Arrange
            var controller = CreateTweetController();
            var tweet = GenerateTweet();
            var oembedTweetDTO = A.Fake<IOEmbedTweetDTO>();
            var oembedTweet = A.Fake<IOEmbedTweet>();

            _fakeTweetQueryExecutor.CallsTo(x => x.GenerateOEmbedTweet(null)).Returns(oembedTweetDTO);
            _fakeTweetFactory.CallsTo(x => x.GenerateOEmbedTweetFromDTO(oembedTweetDTO)).Returns(oembedTweet);

            // Act
            var result = controller.GenerateOEmbedTweet(tweet);

            // Assert
            Assert.AreEqual(result, oembedTweet);
        }

        [TestMethod]
        public void GenerateOEmbedTweet_WithTweet_ReturnsTrue()
        {
            // Arrange
            var controller = CreateTweetController();
            var tweetDTO = A.Fake<ITweetDTO>();
            var tweet = GenerateTweet(tweetDTO);
            var oembedTweetDTO = A.Fake<IOEmbedTweetDTO>();
            var oembedTweet = A.Fake<IOEmbedTweet>();

            _fakeTweetQueryExecutor.CallsTo(x => x.GenerateOEmbedTweet(tweetDTO)).Returns(oembedTweetDTO);
            _fakeTweetFactory.CallsTo(x => x.GenerateOEmbedTweetFromDTO(oembedTweetDTO)).Returns(oembedTweet);

            // Act
            var result = controller.GenerateOEmbedTweet(tweet);

            // Assert
            Assert.AreEqual(result, oembedTweet);
        }

        // With TweetDTO
        [TestMethod]
        public void GenerateOEmbedTweet_WithNullTweetDTO_ReturnsTrue()
        {
            // Arrange
            var controller = CreateTweetController();
            var oembedTweetDTO = A.Fake<IOEmbedTweetDTO>();
            var oembedTweet = A.Fake<IOEmbedTweet>();

            _fakeTweetQueryExecutor.CallsTo(x => x.GenerateOEmbedTweet(null)).Returns(oembedTweetDTO);
            _fakeTweetFactory.CallsTo(x => x.GenerateOEmbedTweetFromDTO(oembedTweetDTO)).Returns(oembedTweet);

            // Act
            var result = controller.GenerateOEmbedTweet((ITweetDTO)null);

            // Assert
            Assert.AreEqual(result, oembedTweet);
        }

        [TestMethod]
        public void GenerateOEmbedTweet_WithTweetDTO_ReturnsTrue()
        {
            // Arrange
            var controller = CreateTweetController();
            var tweetDTO = A.Fake<ITweetDTO>();
            var oembedTweetDTO = A.Fake<IOEmbedTweetDTO>();
            var oembedTweet = A.Fake<IOEmbedTweet>();

            _fakeTweetQueryExecutor.CallsTo(x => x.GenerateOEmbedTweet(tweetDTO)).Returns(oembedTweetDTO);
            _fakeTweetFactory.CallsTo(x => x.GenerateOEmbedTweetFromDTO(oembedTweetDTO)).Returns(oembedTweet);

            // Act
            var result = controller.GenerateOEmbedTweet(tweetDTO);

            // Assert
            Assert.AreEqual(result, oembedTweet);
        }

        // With TweetID
        [TestMethod]
        public void GenerateOEmbedTweet_WithTweetIdAndQueryExecutorReturnsTrue_ReturnsTrue()
        {
            // Arrange
            var controller = CreateTweetController();
            var tweetId = TestHelper.GenerateRandomLong();
            var oembedTweetDTO = A.Fake<IOEmbedTweetDTO>();
            var oembedTweet = A.Fake<IOEmbedTweet>();

            _fakeTweetQueryExecutor.CallsTo(x => x.GenerateOEmbedTweet(tweetId)).Returns(oembedTweetDTO);
            _fakeTweetFactory.CallsTo(x => x.GenerateOEmbedTweetFromDTO(oembedTweetDTO)).Returns(oembedTweet);

            // Act
            var result = controller.GenerateOEmbedTweet(tweetId);

            // Assert
            Assert.AreEqual(result, oembedTweet);
        }

        #endregion

        private ITweet GenerateTweet(ITweetDTO tweetDTO = null)
        {
            var tweet = A.Fake<ITweet>();
            tweet.TweetDTO = tweetDTO;
            tweet.CallsTo(x => x.IsTweetPublished).ReturnsLazily(() => tweet.TweetDTO != null && tweet.TweetDTO.IsTweetPublished);
            _fakeTweetFactory.CallsTo(x => x.GenerateTweetFromDTO(tweetDTO)).Returns(tweet);

            return tweet;
        }

        public TweetController CreateTweetController()
        {
            return _fakeBuilder.GenerateClass();
        }
    }
}