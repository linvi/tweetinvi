using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FakeItEasy;
using FakeItEasy.ExtensionSyntax.Full;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Testinvi.Helpers;
using Tweetinvi.Controllers.Tweet;
using Tweetinvi.Core.Factories;
using Tweetinvi.Core.Web;
using Tweetinvi.Models;
using Tweetinvi.Models.DTO;
using Tweetinvi.Models.Interfaces;

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

        #region Publish Retweet

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public async Task PublishRetweet_TweetIsNull_ThrowsArgumentException()
        {
            // Arrange
            var controller = CreateTweetController();

            // Act
            await controller.PublishRetweet((ITweet)null);
        }

        [TestMethod]
        public async Task PublishRetweet_TweetWithNullTweetDTO_ReturnsTransformedDTOFromQueryExecutor()
        {
            // Arrange
            var controller = CreateTweetController();
            var tweet = GenerateTweet();
            var expectedTweetDTO = A.Fake<ITweetDTO>();
            var expectedTweet = A.Fake<ITweet>();

            _fakeTweetQueryExecutor.CallsTo(x => x.PublishRetweet(null)).Returns(expectedTweetDTO);
            _fakeTweetFactory.CallsTo(x => x.GenerateTweetFromDTO(expectedTweetDTO, null)).Returns(expectedTweet);

            // Act
            var result = await controller.PublishRetweet(tweet);

            // Assert
            Assert.AreEqual(result, expectedTweet);
        }

        [TestMethod]
        public async Task PublishRetweet_TweetWithTweetDTO_ReturnsTransformedDTOFromQueryExecutor()
        {
            // Arrange
            var controller = CreateTweetController();
            var tweetDTO = A.Fake<ITweetDTO>();
            var tweet = GenerateTweet(tweetDTO);
            var expectedTweetDTO = A.Fake<ITweetDTO>();
            var expectedTweet = A.Fake<ITweet>();

            _fakeTweetQueryExecutor.CallsTo(x => x.PublishRetweet(tweetDTO)).Returns(expectedTweetDTO);
            _fakeTweetFactory.CallsTo(x => x.GenerateTweetFromDTO(expectedTweetDTO, null)).Returns(expectedTweet);

            // Act
            var result = await controller.PublishRetweet(tweet);

            // Assert
            Assert.AreEqual(result, expectedTweet);
        }

        [TestMethod]
        public async Task PublishRetweet_TweetDTOIsNull_ReturnsTransformedDTOFromQueryExecutor()
        {
            // Arrange
            var controller = CreateTweetController();
            var expectedTweetDTO = A.Fake<ITweetDTO>();
            var expectedTweet = A.Fake<ITweet>();

            _fakeTweetQueryExecutor.CallsTo(x => x.PublishRetweet(null)).Returns(expectedTweetDTO);
            _fakeTweetFactory.CallsTo(x => x.GenerateTweetFromDTO(expectedTweetDTO, null)).Returns(expectedTweet);

            // Act
            var result = await controller.PublishRetweet((ITweetDTO)null);

            // Assert
            Assert.AreEqual(result, expectedTweet);
        }

        [TestMethod]
        public async Task PublishRetweet_TweetDTO_ReturnsTransformedDTOFromQueryExecutor()
        {
            // Arrange
            var controller = CreateTweetController();
            var tweetDTO = A.Fake<ITweetDTO>();
            var expectedTweetDTO = A.Fake<ITweetDTO>();
            var expectedTweet = A.Fake<ITweet>();

            _fakeTweetQueryExecutor.CallsTo(x => x.PublishRetweet(tweetDTO)).Returns(expectedTweetDTO);
            _fakeTweetFactory.CallsTo(x => x.GenerateTweetFromDTO(expectedTweetDTO, null)).Returns(expectedTweet);

            // Act
            var result = await controller.PublishRetweet(tweetDTO);

            // Assert
            Assert.AreEqual(result, expectedTweet);
        }

        [TestMethod]
        public async Task PublishRetweet_TweetId_ReturnsTransformedDTOFromQueryExecutor()
        {
            // Arrange
            var controller = CreateTweetController();
            var tweetId = TestHelper.GenerateRandomLong();
            var expectedTweetDTO = A.Fake<ITweetDTO>();
            var expectedTweet = A.Fake<ITweet>();

            _fakeTweetQueryExecutor.CallsTo(x => x.PublishRetweet(tweetId)).Returns(expectedTweetDTO);
            _fakeTweetFactory.CallsTo(x => x.GenerateTweetFromDTO(expectedTweetDTO, null)).Returns(expectedTweet);

            // Act
            var result = await controller.PublishRetweet(tweetId);

            // Assert
            Assert.AreEqual(result, expectedTweet);
        }

        #endregion

        #region Get Retweets

        [TestMethod]
        public async Task GetRetweets_TweetDTO_ReturnsTransformedDTOFromQueryExecutor()
        {
            // Arrange
            var controller = CreateTweetController();
            var tweetDTO = A.Fake<ITweetDTO>();
            IEnumerable<ITweetDTO> expectedTweetsDTO = new List<ITweetDTO> { A.Fake<ITweetDTO>() };
            var expectedTweets = new[] { A.Fake<ITweet>() };
            var maxRetweetsToRetrieve = TestHelper.GenerateRandomInt();

            _fakeTweetQueryExecutor.CallsTo(x => x.GetRetweets(tweetDTO, maxRetweetsToRetrieve)).Returns(expectedTweetsDTO);
            _fakeTweetFactory.CallsTo(x => x.GenerateTweetsFromDTO(expectedTweetsDTO, null)).Returns(expectedTweets);

            // Act
            var result = await controller.GetRetweets(tweetDTO, maxRetweetsToRetrieve);

            // Assert
            Assert.AreEqual(result, expectedTweets);
        }

        [TestMethod]
        public async Task GetRetweets_TweetId_ReturnsTransformedDTOFromQueryExecutor()
        {
            // Arrange
            var controller = CreateTweetController();
            var tweetId = TestHelper.GenerateRandomLong();
            var maxRetweetsToRetrieve = TestHelper.GenerateRandomInt();
            IEnumerable<ITweetDTO> expectedTweetsDTO = new List<ITweetDTO> { A.Fake<ITweetDTO>() };
            IEnumerable<ITweet> expectedTweets = new List<ITweet> { A.Fake<ITweet>() };

            _fakeTweetQueryExecutor.CallsTo(x => x.GetRetweets(A<ITweetIdentifier>.That.Matches(y => y.Id == tweetId), maxRetweetsToRetrieve)).Returns(expectedTweetsDTO);
            _fakeTweetFactory.CallsTo(x => x.GenerateTweetsFromDTO(expectedTweetsDTO, null)).Returns(expectedTweets);

            // Act
            var result = await controller.GetRetweets(tweetId, maxRetweetsToRetrieve);

            // Assert
            Assert.AreEqual(result, expectedTweets);
        }

        #endregion

        #region Get Retweeters Ids

        [TestMethod]
        public async Task GetRetweetersIds_WithTweetIdentifier_ReturnsIdsListFromQueryExecutor()
        {
            // Arrange
            var controller = CreateTweetController();
            var tweetIdentifier = A.Fake<ITweetIdentifier>();
            var maxRetweetersToRetrieve = TestHelper.GenerateRandomInt();
            var retweeterIds = new[] { TestHelper.GenerateRandomLong() };

            _fakeTweetQueryExecutor.CallsTo(x => x.GetRetweetersIds(tweetIdentifier, maxRetweetersToRetrieve)).Returns(retweeterIds);

            // Act
            var result = await controller.GetRetweetersIds(tweetIdentifier, maxRetweetersToRetrieve);

            // Assert
            Assert.AreEqual(result, retweeterIds);
        }

        [TestMethod]
        public async Task GetRetweetersIds_WithTweetId_ReturnsIdsListFromQueryExecutor()
        {
            // Arrange
            var controller = CreateTweetController();
            var tweetId = TestHelper.GenerateRandomLong();
            var maxRetweetersToRetrieve = TestHelper.GenerateRandomInt();
            var retweeterIds = new[] { TestHelper.GenerateRandomLong() };

            _fakeTweetQueryExecutor.CallsTo(x => x.GetRetweetersIds(A<ITweetIdentifier>.That.Matches(y => y.Id == tweetId), maxRetweetersToRetrieve)).Returns(retweeterIds);

            // Act
            var result = await controller.GetRetweetersIds(tweetId, maxRetweetersToRetrieve);

            // Assert
            Assert.AreEqual(result, retweeterIds);
        }
        
        #endregion

        #region Destroy Tweet

        // With Tweet
        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public async Task DestroyTweet_WithNullTweet_ThrowArgumentException()
        {
            // Arrange
            var controller = CreateTweetController();

            // Act
            await controller.DestroyTweet((ITweet)null);
        }

        [TestMethod]
        public async Task DestroyTweet_WithTweetButNullTweetDTOAndQueryExecutorReturnsTrue_ReturnsFalse()
        {
            // Arrange
            var controller = CreateTweetController();
            var tweet = GenerateTweet();

            _fakeTweetQueryExecutor.CallsTo(x => x.DestroyTweet(null)).Returns(true);

            // Act
            var result = await controller.DestroyTweet(tweet);

            // Assert
            Assert.IsFalse(result);
        }

        [TestMethod]
        public async Task DestroyTweet_WithTweetButNullTweetDTOAndQueryExecutorReturnsFalse_ReturnsFalse()
        {
            // Arrange
            var controller = CreateTweetController();
            var tweet = GenerateTweet();

            _fakeTweetQueryExecutor.CallsTo(x => x.DestroyTweet(null)).Returns(false);

            // Act
            var result = await controller.DestroyTweet(tweet);

            // Assert
            Assert.IsFalse(result);
        }

        [TestMethod]
        public async Task DestroyTweet_WithTweetAndQueryExecutorReturnsTrue_ReturnsTrue()
        {
            // Arrange
            var controller = CreateTweetController();
            var tweetDTO = A.Fake<ITweetDTO>();
            var tweet = GenerateTweet(tweetDTO);

            _fakeTweetQueryExecutor.CallsTo(x => x.DestroyTweet(tweetDTO)).Returns(true);

            // Act
            var result = await controller.DestroyTweet(tweet);

            // Assert
            Assert.IsTrue(result);
        }

        [TestMethod]
        public async Task DestroyTweet_WithTweetAndQueryExecutorReturnsFalse_ReturnsFalse()
        {
            // Arrange
            var controller = CreateTweetController();
            var tweetDTO = A.Fake<ITweetDTO>();
            var tweet = GenerateTweet(tweetDTO);

            _fakeTweetQueryExecutor.CallsTo(x => x.DestroyTweet(tweetDTO)).Returns(false);

            // Act
            var result = await controller.DestroyTweet(tweet);

            // Assert
            Assert.IsFalse(result);
        }

        // With TweetDTO
        [TestMethod]
        public async Task DestroyTweet_WithNullTweetDTOAndQueryExecutorReturnsTrue_ReturnsTrue()
        {
            // Arrange
            var controller = CreateTweetController();

            _fakeTweetQueryExecutor.CallsTo(x => x.DestroyTweet(null)).Returns(true);

            // Act
            var result = await controller.DestroyTweet((ITweetDTO)null);

            // Assert
            Assert.IsFalse(result);
        }

        [TestMethod]
        public async Task DestroyTweet_WithNullTweetDTOAndQueryExecutorReturnsFalse_ReturnsFalse()
        {
            // Arrange
            var controller = CreateTweetController();

            _fakeTweetQueryExecutor.CallsTo(x => x.DestroyTweet(null)).Returns(false);

            // Act
            var result = await controller.DestroyTweet((ITweetDTO)null);

            // Assert
            Assert.IsFalse(result);
        }

        [TestMethod]
        public async Task DestroyTweet_WithTweetDTOAndQueryExecutorReturnsTrue_ReturnsTrue()
        {
            // Arrange
            var controller = CreateTweetController();
            var tweetDTO = A.Fake<ITweetDTO>();

            _fakeTweetQueryExecutor.CallsTo(x => x.DestroyTweet(tweetDTO)).Returns(true);

            // Act
            var result = await controller.DestroyTweet(tweetDTO);

            // Assert
            Assert.IsTrue(result);
        }

        [TestMethod]
        public async Task  DestroyTweet_WithTweetDTOAndQueryExecutorReturnsFalse_ReturnsFalse()
        {
            // Arrange
            var controller = CreateTweetController();
            var tweetDTO = A.Fake<ITweetDTO>();

            _fakeTweetQueryExecutor.CallsTo(x => x.DestroyTweet(tweetDTO)).Returns(false);

            // Act
            var result = await controller.DestroyTweet(tweetDTO);

            // Assert
            Assert.IsFalse(result);
        }

        // With TweetID
        [TestMethod]
        public async Task DestroyTweet_WithTweetIdAndQueryExecutor_ReturnsTwitterResult()
        {
            // Arrange
            var controller = CreateTweetController();
            var tweetId = TestHelper.GenerateRandomLong();
            var twitterResult = A.Fake<ITwitterResult>();

            _fakeTweetQueryExecutor.CallsTo(x => x.DestroyTweet(tweetId, It.IsAny<ITwitterRequest>())).ReturnsLazily(() => twitterResult);

            // Act
            var result = await controller.DestroyTweet(tweetId, A.Fake<ITwitterRequest>());

            // Assert
            Assert.AreEqual(result, twitterResult);
        }

        #endregion

        #region Favorite Tweet

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public async Task FavoriteTweet_WithNullTweet_ThrowsArgumentException()
        {
            // Arrange
            var controller = CreateTweetController();

            // Act
            await controller.FavoriteTweet((ITweet)null);
        }

        [TestMethod]
        public async Task FavoriteTweet_WithTweetDTO_ReturnsQueryExecutorResult()
        {
            await VerifyFavoriteTweet_WithTweetDTO_ReturnsQueryExecutorResult(false, false);
            await VerifyFavoriteTweet_WithTweetDTO_ReturnsQueryExecutorResult(false, false);
            await VerifyFavoriteTweet_WithTweetDTO_ReturnsQueryExecutorResult(true, false);
            await VerifyFavoriteTweet_WithTweetDTO_ReturnsQueryExecutorResult(true, true);
        }

        private async Task VerifyFavoriteTweet_WithTweetDTO_ReturnsQueryExecutorResult(bool tweetExists, bool expectedResult)
        {
            // Arrange
            var controller = CreateTweetController();
            var tweetDTO = tweetExists ? A.Fake<ITweetDTO>() : null;

            _fakeTweetQueryExecutor.CallsTo(x => x.FavoriteTweet(tweetDTO)).Returns(expectedResult);

            // Act
            var result = await controller.FavoriteTweet(tweetDTO);

            // Assert
            Assert.AreEqual(result, expectedResult);
        }

        [TestMethod]
        public async Task FavoriteTweet_WithTweetId_ReturnsQueryExecutorResult()
        {
            await VerifyFavoriteTweet_WithTweetId_ReturnsQueryExecutorResult(false);
            await VerifyFavoriteTweet_WithTweetId_ReturnsQueryExecutorResult(true);
        }

        private async Task VerifyFavoriteTweet_WithTweetId_ReturnsQueryExecutorResult(bool expectedResult)
        {
            // Arrange
            var controller = CreateTweetController();
            var tweetId = TestHelper.GenerateRandomLong();

            _fakeTweetQueryExecutor.CallsTo(x => x.FavoriteTweet(tweetId)).Returns(expectedResult);

            // Act
            var result = await controller.FavoriteTweet(tweetId);

            // Assert
            Assert.AreEqual(result, expectedResult);
        }

        #endregion

        #region UnFavorite Tweet

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public async Task UnFavoriteTweet_WithNullTweet_ThrowsArgumentException()
        {
            // Arrange
            var controller = CreateTweetController();

            // Act
            await controller.UnFavoriteTweet((ITweet)null);
        }

        [TestMethod]
        public async Task UnFavoriteTweet_WithTweet_ReturnsQueryExecutorResult()
        {
            await VerifyUnFavoriteTweet_WithTweet_ReturnsQueryExecutorResult(false, false);
            await VerifyUnFavoriteTweet_WithTweet_ReturnsQueryExecutorResult(false, true);
            await VerifyUnFavoriteTweet_WithTweet_ReturnsQueryExecutorResult(true, false);
            await VerifyUnFavoriteTweet_WithTweet_ReturnsQueryExecutorResult(true, true);
        }

        private async Task VerifyUnFavoriteTweet_WithTweet_ReturnsQueryExecutorResult(bool doesTweetDTOExists, bool expectedResult)
        {
            // Arrange
            var controller = CreateTweetController();
            var tweetDTO = doesTweetDTOExists ? A.Fake<ITweetDTO>() : null;
            var tweet = GenerateTweet(tweetDTO);

            _fakeTweetQueryExecutor.CallsTo(x => x.UnFavoriteTweet(tweetDTO)).Returns(expectedResult);

            // Act
            var result = await controller.UnFavoriteTweet(tweet);

            // Assert
            Assert.AreEqual(result, expectedResult);
        }

        [TestMethod]
        public async Task UnFavoriteTweet_WithTweetDTO_ReturnsQueryExecutorResult()
        {
            await VerifyUnFavoriteTweet_WithTweetDTO_ReturnsQueryExecutorResult(false, false);
            await VerifyUnFavoriteTweet_WithTweetDTO_ReturnsQueryExecutorResult(false, true);
            await VerifyUnFavoriteTweet_WithTweetDTO_ReturnsQueryExecutorResult(true, false);
            await VerifyUnFavoriteTweet_WithTweetDTO_ReturnsQueryExecutorResult(true, true);
        }

        private async Task VerifyUnFavoriteTweet_WithTweetDTO_ReturnsQueryExecutorResult(bool tweetExists, bool expectedResult)
        {
            // Arrange
            var controller = CreateTweetController();
            var tweetDTO = tweetExists ? A.Fake<ITweetDTO>() : null;

            _fakeTweetQueryExecutor.CallsTo(x => x.UnFavoriteTweet(tweetDTO)).Returns(expectedResult);

            // Act
            var result = await controller.UnFavoriteTweet(tweetDTO);

            // Assert
            Assert.AreEqual(result, expectedResult);
        }

        [TestMethod]
        public async Task UnFavoriteTweet_WithTweetId_ReturnsQueryExecutorResult()
        {
            await VerifyUnFavoriteTweet_WithTweetId_ReturnsQueryExecutorResult(false);
            await VerifyUnFavoriteTweet_WithTweetId_ReturnsQueryExecutorResult(true);
        }

        private async Task VerifyUnFavoriteTweet_WithTweetId_ReturnsQueryExecutorResult(bool expectedResult)
        {
            // Arrange
            var controller = CreateTweetController();
            var tweetId = TestHelper.GenerateRandomLong();

            _fakeTweetQueryExecutor.CallsTo(x => x.UnFavoriteTweet(tweetId)).Returns(expectedResult);

            // Act
            var result = await controller.UnFavoriteTweet(tweetId);

            // Assert
            Assert.AreEqual(result, expectedResult);
        }

        #endregion

        #region Generate OEmbed Tweet

        // With Tweet
        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public async Task GenerateOEmbedTweet_WithNullTweet_ThrowsArgumentException()
        {
            // Arrange
            var controller = CreateTweetController();

            // Act
            await controller.GenerateOEmbedTweet((ITweet)null);
        }

        [TestMethod]
        public async Task GenerateOEmbedTweet_WithTweetButNullTweetDTO_ReturnsQueryExecutor()
        {
            // Arrange
            var controller = CreateTweetController();
            var tweet = GenerateTweet();
            var oembedTweetDTO = A.Fake<IOEmbedTweetDTO>();
            var oembedTweet = A.Fake<IOEmbedTweet>();

            _fakeTweetQueryExecutor.CallsTo(x => x.GenerateOEmbedTweet(null)).Returns(oembedTweetDTO);
            _fakeTweetFactory.CallsTo(x => x.GenerateOEmbedTweetFromDTO(oembedTweetDTO)).Returns(oembedTweet);

            // Act
            var result = await controller.GenerateOEmbedTweet(tweet);

            // Assert
            Assert.AreEqual(result, oembedTweet);
        }

        [TestMethod]
        public async Task GenerateOEmbedTweet_WithTweet_ReturnsTrue()
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
            var result = await controller.GenerateOEmbedTweet(tweet);

            // Assert
            Assert.AreEqual(result, oembedTweet);
        }

        // With TweetDTO
        [TestMethod]
        public async Task GenerateOEmbedTweet_WithNullTweetDTO_ReturnsTrue()
        {
            // Arrange
            var controller = CreateTweetController();
            var oembedTweetDTO = A.Fake<IOEmbedTweetDTO>();
            var oembedTweet = A.Fake<IOEmbedTweet>();

            _fakeTweetQueryExecutor.CallsTo(x => x.GenerateOEmbedTweet(null)).Returns(oembedTweetDTO);
            _fakeTweetFactory.CallsTo(x => x.GenerateOEmbedTweetFromDTO(oembedTweetDTO)).Returns(oembedTweet);

            // Act
            var result = await controller.GenerateOEmbedTweet((ITweetDTO)null);

            // Assert
            Assert.AreEqual(result, oembedTweet);
        }

        [TestMethod]
        public async Task GenerateOEmbedTweet_WithTweetDTO_ReturnsTrue()
        {
            // Arrange
            var controller = CreateTweetController();
            var tweetDTO = A.Fake<ITweetDTO>();
            var oembedTweetDTO = A.Fake<IOEmbedTweetDTO>();
            var oembedTweet = A.Fake<IOEmbedTweet>();

            _fakeTweetQueryExecutor.CallsTo(x => x.GenerateOEmbedTweet(tweetDTO)).Returns(oembedTweetDTO);
            _fakeTweetFactory.CallsTo(x => x.GenerateOEmbedTweetFromDTO(oembedTweetDTO)).Returns(oembedTweet);

            // Act
            var result = await controller.GenerateOEmbedTweet(tweetDTO);

            // Assert
            Assert.AreEqual(result, oembedTweet);
        }

        // With TweetID
        [TestMethod]
        public async Task GenerateOEmbedTweet_WithTweetIdAndQueryExecutorReturnsTrue_ReturnsTrue()
        {
            // Arrange
            var controller = CreateTweetController();
            var tweetId = TestHelper.GenerateRandomLong();
            var oembedTweetDTO = A.Fake<IOEmbedTweetDTO>();
            var oembedTweet = A.Fake<IOEmbedTweet>();

            _fakeTweetQueryExecutor.CallsTo(x => x.GenerateOEmbedTweet(tweetId)).Returns(oembedTweetDTO);
            _fakeTweetFactory.CallsTo(x => x.GenerateOEmbedTweetFromDTO(oembedTweetDTO)).Returns(oembedTweet);

            // Act
            var result = await controller.GenerateOEmbedTweet(tweetId);

            // Assert
            Assert.AreEqual(result, oembedTweet);
        }

        #endregion

        private ITweet GenerateTweet(ITweetDTO tweetDTO = null)
        {
            var tweet = A.Fake<ITweet>();
            tweet.TweetDTO = tweetDTO;
            tweet.CallsTo(x => x.IsTweetPublished).ReturnsLazily(() => tweet.TweetDTO != null && tweet.TweetDTO.IsTweetPublished);
            _fakeTweetFactory.CallsTo(x => x.GenerateTweetFromDTO(tweetDTO, null)).Returns(tweet);

            return tweet;
        }

        private TweetController CreateTweetController()
        {
            return _fakeBuilder.GenerateClass();
        }
    }
}