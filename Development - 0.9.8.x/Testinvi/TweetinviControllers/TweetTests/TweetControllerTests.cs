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

        [TestMethod]
        public void PublishTweet_WithTweet_ReturnsTrue()
        {
            // Arrange
            var controller = CreateTweetController();

            var tweetDTO = A.Fake<ITweetDTO>();
            var tweet = GenerateTweet(tweetDTO);

            var publishedTweetDTO = A.Fake<ITweetDTO>();
            publishedTweetDTO.CallsTo(x => x.IsTweetPublished).Returns(true);

            _fakeTweetQueryExecutor.CallsTo(x => x.PublishTweet(tweetDTO)).Returns(publishedTweetDTO);

            // Act
            var result = controller.PublishTweet(tweet);

            // Assert
            Assert.AreEqual(result, true);
            Assert.AreEqual(tweet.TweetDTO, publishedTweetDTO);
        }

        [TestMethod]
        public void PublishTweet_WithNullTweetDTO_ReturnsTweetFromQueryExecutorDTO()
        {
            // Arrange
            var controller = CreateTweetController();

            var publishedTweetDTO = A.Fake<ITweetDTO>();
            var publishedTweet = GenerateTweet();

            _fakeTweetQueryExecutor.CallsTo(x => x.PublishTweet(null)).Returns(publishedTweetDTO);
            _fakeTweetFactory.CallsTo(x => x.GenerateTweetFromDTO(publishedTweetDTO)).Returns(publishedTweet);

            // Act
            var result = controller.PublishTweet((ITweetDTO)null);

            // Assert
            Assert.AreEqual(result, publishedTweet);
        }

        [TestMethod]
        public void PublishTweet_WithTweetDTO_ReturnsTweetFromQueryExecutorDTO()
        {
            // Arrange
            var controller = CreateTweetController();

            var tweetDTO = A.Fake<ITweetDTO>();

            var publishedTweetDTO = A.Fake<ITweetDTO>();
            var publishedTweet = GenerateTweet();

            _fakeTweetQueryExecutor.CallsTo(x => x.PublishTweet(tweetDTO)).Returns(publishedTweetDTO);
            _fakeTweetFactory.CallsTo(x => x.GenerateTweetFromDTO(publishedTweetDTO)).Returns(publishedTweet);

            // Act
            var result = controller.PublishTweet(tweetDTO);

            // Assert
            Assert.AreEqual(result, publishedTweet);
        }

        #endregion

        #region Publish Tweet In Reply To

        // Publish with 2 tweets as parameters
        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void PublishTweetInReplyToTweet_TweetToPublishIsNull_ThrowsArgumentException()
        {
            // Arrange
            var controller = CreateTweetController();
            var tweetToReplyTo = A.Fake<ITweet>();

            // Act
            controller.PublishTweetInReplyTo(null, tweetToReplyTo);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void PublishTweetInReplyToTweet_TweetToReplyToIsNull_ThrowsArgumentException()
        {
            // Arrange
            var controller = CreateTweetController();
            var tweetToPublish = A.Fake<ITweet>();

            // Act
            controller.PublishTweetInReplyTo(tweetToPublish, null);
        }

        [TestMethod]
        public void PublishTweetInReplyToTweet_TweetsDTOsAreNull_UpdateTweetReturnsIsTweetPublished_True()
        {
            const bool isTweetPublished = true;

            // Arrange
            var controller = CreateTweetController();
            var tweetToPublish = GenerateTweet();
            var tweetToReplyTo = GenerateTweet();
            var publishedTweetDTO = A.Fake<ITweetDTO>();
            publishedTweetDTO.CallsTo(x => x.IsTweetPublished).Returns(isTweetPublished);

            _fakeTweetQueryExecutor.CallsTo(x => x.PublishTweetInReplyTo(null, null)).Returns(publishedTweetDTO);

            // Act
            var result = controller.PublishTweetInReplyTo(tweetToPublish, tweetToReplyTo);

            // Assert
            Assert.AreEqual(result, isTweetPublished);
            Assert.AreEqual(tweetToPublish.TweetDTO, publishedTweetDTO);
        }

        [TestMethod]
        public void PublishTweetInReplyToTweet_TweetsDTOsAreNull_sReturnsIsTweetPublished_False()
        {
            const bool isTweetPublished = false;

            // Arrange
            var controller = CreateTweetController();
            var tweetToPublish = GenerateTweet();
            var tweetToReplyTo = GenerateTweet();
            var publishedTweetDTO = A.Fake<ITweetDTO>();
            publishedTweetDTO.CallsTo(x => x.IsTweetPublished).Returns(isTweetPublished);

            _fakeTweetQueryExecutor.CallsTo(x => x.PublishTweetInReplyTo(null, null)).Returns(publishedTweetDTO);

            // Act
            var result = controller.PublishTweetInReplyTo(tweetToPublish, tweetToReplyTo);

            // Assert
            Assert.AreEqual(result, isTweetPublished);
            Assert.AreNotEqual(tweetToPublish.TweetDTO, publishedTweetDTO);
        }

        [TestMethod]
        public void PublishTweetInReplyToTweet_TweetDTOToReplyToIsNull_UpdateTweetReturnsIsTweetPublished_True()
        {
            const bool isTweetPublished = true;

            // Arrange
            var controller = CreateTweetController();
            var tweetToPublishDTO = A.Fake<ITweetDTO>();
            var tweetToPublish = GenerateTweet(tweetToPublishDTO);
            var tweetToReplyTo = GenerateTweet();
            var publishedTweetDTO = A.Fake<ITweetDTO>();
            publishedTweetDTO.CallsTo(x => x.IsTweetPublished).Returns(isTweetPublished);

            _fakeTweetQueryExecutor.CallsTo(x => x.PublishTweetInReplyTo(tweetToPublishDTO, null)).Returns(publishedTweetDTO);

            // Act
            var result = controller.PublishTweetInReplyTo(tweetToPublish, tweetToReplyTo);

            // Assert
            Assert.AreEqual(result, isTweetPublished);
            Assert.AreEqual(tweetToPublish.TweetDTO, publishedTweetDTO);
        }

        [TestMethod]
        public void PublishTweetInReplyToTweet_TweetDTOToReplyToIsNull_ReturnsIsTweetPublished_False()
        {
            const bool isTweetPublished = false;

            // Arrange
            var controller = CreateTweetController();
            var tweetToPublishDTO = A.Fake<ITweetDTO>();
            var tweetToPublish = GenerateTweet(tweetToPublishDTO);
            var tweetToReplyTo = GenerateTweet();
            var publishedTweetDTO = A.Fake<ITweetDTO>();
            publishedTweetDTO.CallsTo(x => x.IsTweetPublished).Returns(isTweetPublished);

            _fakeTweetQueryExecutor.CallsTo(x => x.PublishTweetInReplyTo(tweetToPublishDTO, null)).Returns(publishedTweetDTO);

            // Act
            var result = controller.PublishTweetInReplyTo(tweetToPublish, tweetToReplyTo);

            // Assert
            Assert.AreEqual(result, isTweetPublished);
            Assert.AreNotEqual(tweetToPublish.TweetDTO, publishedTweetDTO);
        }

        [TestMethod]
        public void PublishTweetInReplyToTweet_TweetDTOToPublishIsNull_TweetUpdatedReturnsIsTweetPublished_True()
        {
            const bool isTweetPublished = true;

            // Arrange
            var controller = CreateTweetController();
            var tweetToPublish = GenerateTweet();
            var tweetToReplyToDTO = A.Fake<ITweetDTO>();
            var tweetToReplyTo = GenerateTweet(tweetToReplyToDTO);
            var publishedTweetDTO = A.Fake<ITweetDTO>();
            publishedTweetDTO.CallsTo(x => x.IsTweetPublished).Returns(isTweetPublished);

            _fakeTweetQueryExecutor.CallsTo(x => x.PublishTweetInReplyTo(null, tweetToReplyToDTO)).Returns(publishedTweetDTO);

            // Act
            var result = controller.PublishTweetInReplyTo(tweetToPublish, tweetToReplyTo);

            // Assert
            Assert.AreEqual(result, isTweetPublished);
            Assert.AreEqual(tweetToPublish.TweetDTO, publishedTweetDTO);
        }

        [TestMethod]
        public void PublishTweetInReplyToTweet_TweetDTOToPublishIsNull_ReturnsIsTweetPublished_False()
        {
            const bool isTweetPublished = false;

            // Arrange
            var controller = CreateTweetController();
            var tweetToPublish = GenerateTweet();
            var tweetToReplyToDTO = A.Fake<ITweetDTO>();
            var tweetToReplyTo = GenerateTweet(tweetToReplyToDTO);
            var publishedTweetDTO = A.Fake<ITweetDTO>();
            publishedTweetDTO.CallsTo(x => x.IsTweetPublished).Returns(isTweetPublished);

            _fakeTweetQueryExecutor.CallsTo(x => x.PublishTweetInReplyTo(null, tweetToReplyToDTO)).Returns(publishedTweetDTO);

            // Act
            var result = controller.PublishTweetInReplyTo(tweetToPublish, tweetToReplyTo);

            // Assert
            Assert.AreEqual(result, isTweetPublished);
            Assert.AreNotEqual(tweetToPublish.TweetDTO, publishedTweetDTO);
        }

        [TestMethod]
        public void PublishTweetInReplyToTweet_TweetDTOAreNotNull_UpdateDTOReturnsGeneratedTweetFromDTO_True()
        {
            const bool isTweetPublished = true;

            // Arrange
            var controller = CreateTweetController();
            var tweetToPublishDTO = A.Fake<ITweetDTO>();
            var tweetToPublish = GenerateTweet(tweetToPublishDTO);
            var tweetToReplyToDTO = A.Fake<ITweetDTO>();
            var tweetToReplyTo = GenerateTweet(tweetToReplyToDTO);
            var publishedTweetDTO = A.Fake<ITweetDTO>();
            publishedTweetDTO.CallsTo(x => x.IsTweetPublished).Returns(isTweetPublished);

            _fakeTweetQueryExecutor.CallsTo(x => x.PublishTweetInReplyTo(tweetToPublishDTO, tweetToReplyToDTO)).Returns(publishedTweetDTO);

            // Act
            var result = controller.PublishTweetInReplyTo(tweetToPublish, tweetToReplyTo);

            // Assert
            Assert.AreEqual(result, isTweetPublished);
            Assert.AreEqual(tweetToPublish.TweetDTO, publishedTweetDTO);
        }

        [TestMethod]
        public void PublishTweetInReplyToTweet_TweetDTOAreNotNull_ReturnsGeneratedTweetFromDTO_False()
        {
            const bool isTweetPublished = false;

            // Arrange
            var controller = CreateTweetController();
            var tweetToPublishDTO = A.Fake<ITweetDTO>();
            var tweetToPublish = GenerateTweet(tweetToPublishDTO);
            var tweetToReplyToDTO = A.Fake<ITweetDTO>();
            var tweetToReplyTo = GenerateTweet(tweetToReplyToDTO);
            var publishedTweetDTO = A.Fake<ITweetDTO>();
            publishedTweetDTO.CallsTo(x => x.IsTweetPublished).Returns(isTweetPublished);

            _fakeTweetQueryExecutor.CallsTo(x => x.PublishTweetInReplyTo(tweetToPublishDTO, tweetToReplyToDTO)).Returns(publishedTweetDTO);

            // Act
            var result = controller.PublishTweetInReplyTo(tweetToPublish, tweetToReplyTo);

            // Assert
            Assert.AreEqual(result, isTweetPublished);
            Assert.AreNotEqual(tweetToPublish.TweetDTO, publishedTweetDTO);
        }

        // Publish with 2 tweets DTO as parameters
        [TestMethod]
        public void PublishTweetDTOInReplyToTweetDTO_TweetsDTOsAreNull_ReturnsGeneratedTweetFromDTO()
        {
            // Arrange
            var controller = CreateTweetController();
            var publishedTweetDTO = A.Fake<ITweetDTO>();
            var publishedTweet = A.Fake<ITweet>();

            _fakeTweetQueryExecutor.CallsTo(x => x.PublishTweetInReplyTo(null, null)).Returns(publishedTweetDTO);
            _fakeTweetFactory.CallsTo(x => x.GenerateTweetFromDTO(publishedTweetDTO)).Returns(publishedTweet);

            // Act
            var result = controller.PublishTweetInReplyTo((ITweetDTO)null, null);

            // Assert
            Assert.AreEqual(result, publishedTweet);
        }

        [TestMethod]
        public void PublishTweetDTOInReplyToTweetDTO_TweetDTOToReplyToIsNull_ReturnsGeneratedTweetFromDTO()
        {
            // Arrange
            var controller = CreateTweetController();
            var tweetDTOToPublish = A.Fake<ITweetDTO>();
            var publishedTweetDTO = A.Fake<ITweetDTO>();
            var publishedTweet = A.Fake<ITweet>();

            _fakeTweetQueryExecutor.CallsTo(x => x.PublishTweetInReplyTo(tweetDTOToPublish, null)).Returns(publishedTweetDTO);
            _fakeTweetFactory.CallsTo(x => x.GenerateTweetFromDTO(publishedTweetDTO)).Returns(publishedTweet);

            // Act
            var result = controller.PublishTweetInReplyTo(tweetDTOToPublish, null);

            // Assert
            Assert.AreEqual(result, publishedTweet);
        }

        [TestMethod]
        public void PublishTweetDTOInReplyToTweetDTO_TweetDTOToPublishIsNull_ReturnsGeneratedTweetFromDTO()
        {
            // Arrange
            var controller = CreateTweetController();
            var tweetToReplyToDTO = A.Fake<ITweetDTO>();
            var publishedTweetDTO = A.Fake<ITweetDTO>();
            var publishedTweet = A.Fake<ITweet>();

            _fakeTweetQueryExecutor.CallsTo(x => x.PublishTweetInReplyTo(null, tweetToReplyToDTO)).Returns(publishedTweetDTO);
            _fakeTweetFactory.CallsTo(x => x.GenerateTweetFromDTO(publishedTweetDTO)).Returns(publishedTweet);

            // Act
            var result = controller.PublishTweetInReplyTo(null, tweetToReplyToDTO);

            // Assert
            Assert.AreEqual(result, publishedTweet);
        }

        [TestMethod]
        public void PublishTweetDTOInReplyToTweetDTO_TweetDTOAreNotNull_ReturnsGeneratedTweetFromDTO()
        {
            // Arrange
            var controller = CreateTweetController();
            var tweetToPublishDTO = A.Fake<ITweetDTO>();
            var tweetToReplyToDTO = A.Fake<ITweetDTO>();
            var publishedTweetDTO = A.Fake<ITweetDTO>();
            var publishedTweet = A.Fake<ITweet>();

            _fakeTweetQueryExecutor.CallsTo(x => x.PublishTweetInReplyTo(tweetToPublishDTO, tweetToReplyToDTO)).Returns(publishedTweetDTO);
            _fakeTweetFactory.CallsTo(x => x.GenerateTweetFromDTO(publishedTweetDTO)).Returns(publishedTweet);

            // Act
            var result = controller.PublishTweetInReplyTo(tweetToPublishDTO, tweetToReplyToDTO);

            // Assert
            Assert.AreEqual(result, publishedTweet);
        }

        // Publish with tweet and tweet id
        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void PublishTweetInReplyToTweetId_TweetToPublishIsNull_ThrowsArgumentException()
        {
            // Arrange
            var controller = CreateTweetController();
            var tweetIdToReplyTo = TestHelper.GenerateRandomLong();

            // Act
            controller.PublishTweetInReplyTo((ITweet)null, tweetIdToReplyTo);
        }

        [TestMethod]
        public void PublishTweetInReplyToTweetId_TweetDTOIsNull_ReturnsIsTweetPublished_False()
        {
            // Arrange
            var controller = CreateTweetController();
            var tweetIdToReplyTo = TestHelper.GenerateRandomLong();
            var tweetToPublish = GenerateTweet();
            var publishedTweetDTO = A.Fake<ITweetDTO>();
            var publishedTweet = TestHelper.GenerateTweet(publishedTweetDTO);
            publishedTweet.CallsTo(x => x.IsTweetPublished).Returns(false);

            _fakeTweetQueryExecutor.CallsTo(x => x.PublishTweetInReplyTo(null, tweetIdToReplyTo)).Returns(publishedTweetDTO);
            _fakeTweetFactory.CallsTo(x => x.GenerateTweetFromDTO(publishedTweetDTO)).Returns(publishedTweet);

            // Act
            var result = controller.PublishTweetInReplyTo(tweetToPublish, tweetIdToReplyTo);

            // Assert
            Assert.AreEqual(result, false);
        }

        [TestMethod]
        public void PublishTweetInReplyToTweetId_TweetDTOIsNull_ReturnsIsTweetPublished_True()
        {
            // Arrange
            var controller = CreateTweetController();
            var tweetIdToReplyTo = TestHelper.GenerateRandomLong();
            var tweetToPublish = GenerateTweet();

            var publishedTweetDTO = A.Fake<ITweetDTO>();
            publishedTweetDTO.CallsTo(x => x.IsTweetPublished).Returns(false);

            var publishedTweet = TestHelper.GenerateTweet(publishedTweetDTO);

            _fakeTweetQueryExecutor.CallsTo(x => x.PublishTweetInReplyTo(null, tweetIdToReplyTo)).Returns(publishedTweetDTO);
            _fakeTweetFactory.CallsTo(x => x.GenerateTweetFromDTO(publishedTweetDTO)).Returns(publishedTweet);

            // Act
            var result = controller.PublishTweetInReplyTo(tweetToPublish, tweetIdToReplyTo);

            // Assert
            Assert.AreEqual(result, false);
            Assert.AreNotEqual(tweetToPublish.TweetDTO, publishedTweetDTO);
        }

        [TestMethod]
        public void PublishTweetInReplyToTweetId_TweetDTOIsValid_UpdatedTweetDTOAndReturnsIsPublished_False()
        {
            // Arrange
            var controller = CreateTweetController();
            var tweetIdToReplyTo = TestHelper.GenerateRandomLong();
            var tweetToReplyToDTO = A.Fake<ITweetDTO>();
            var tweetToPublish = TestHelper.GenerateTweet(tweetToReplyToDTO);

            var publishedTweetDTO = A.Fake<ITweetDTO>();
            publishedTweetDTO.CallsTo(x => x.IsTweetPublished).Returns(false);

            var publishedTweet = TestHelper.GenerateTweet(publishedTweetDTO);
            publishedTweet.CallsTo(x => x.IsTweetPublished).Returns(false);

            _fakeTweetQueryExecutor.CallsTo(x => x.PublishTweetInReplyTo(tweetToReplyToDTO, tweetIdToReplyTo)).Returns(publishedTweetDTO);
            _fakeTweetFactory.CallsTo(x => x.GenerateTweetFromDTO(publishedTweetDTO)).Returns(publishedTweet);

            // Act
            var result = controller.PublishTweetInReplyTo(tweetToPublish, tweetIdToReplyTo);

            // Assert
            Assert.AreEqual(result, false);

        }

        // Publish with tweet DTO and tweet id
        [TestMethod]
        public void PublishTweetDTOInReplyToTweetId_TweetToPublishIsNull_ReturnsNull()
        {
            // Arrange
            var controller = CreateTweetController();
            var tweetIdToReplyTo = TestHelper.GenerateRandomLong();

            var publishedTweetDTO = A.Fake<ITweetDTO>();
            var publishedTweet = A.Fake<ITweet>();

            _fakeTweetQueryExecutor.CallsTo(x => x.PublishTweetInReplyTo(null, tweetIdToReplyTo)).Returns(publishedTweetDTO);
            _fakeTweetFactory.CallsTo(x => x.GenerateTweetFromDTO(publishedTweetDTO)).Returns(publishedTweet);

            // Act
            var result = controller.PublishTweetInReplyTo((ITweetDTO)null, tweetIdToReplyTo);

            // Assert
            Assert.AreEqual(result, publishedTweet);
        }

        [TestMethod]
        public void PublishTweetDTOInReplyToTweetId_TweetDTOIsNull_ReturnsGeneratedTweetFromDTO()
        {
            // Arrange
            var controller = CreateTweetController();
            var tweetIdToReplyTo = TestHelper.GenerateRandomLong();
            var tweetDTOToPublish = A.Fake<ITweetDTO>();
            var publishedTweetDTO = A.Fake<ITweetDTO>();
            var publishedTweet = A.Fake<ITweet>();

            _fakeTweetQueryExecutor.CallsTo(x => x.PublishTweetInReplyTo(tweetDTOToPublish, tweetIdToReplyTo)).Returns(publishedTweetDTO);
            _fakeTweetFactory.CallsTo(x => x.GenerateTweetFromDTO(publishedTweetDTO)).Returns(publishedTweet);

            // Act
            var result = controller.PublishTweetInReplyTo(tweetDTOToPublish, tweetIdToReplyTo);

            // Assert
            Assert.AreEqual(result, publishedTweet);
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
        [ExpectedException(typeof(ArgumentException))]
        public void GetRetweets_TweetIsNull_ThrowsArgumentException()
        {
            // Arrange
            var controller = CreateTweetController();

            // Act
            controller.GetRetweets((ITweet)null);
        }

        [TestMethod]
        public void GetRetweets_TweetWithNullTweetDTO_ReturnsTransformedDTOFromQueryExecutor()
        {
            // Arrange
            var controller = CreateTweetController();
            var tweet = GenerateTweet();
            IEnumerable<ITweetDTO> expectedTweetsDTO = new List<ITweetDTO> { A.Fake<ITweetDTO>() };
            IEnumerable<ITweet> expectedTweets = new List<ITweet> { A.Fake<ITweet>() };

            _fakeTweetQueryExecutor.CallsTo(x => x.GetRetweets(null)).Returns(expectedTweetsDTO);
            _fakeTweetFactory.CallsTo(x => x.GenerateTweetsFromDTO(expectedTweetsDTO)).Returns(expectedTweets);

            // Act
            var result = controller.GetRetweets(tweet);

            // Assert
            Assert.AreEqual(result, expectedTweets);
        }

        [TestMethod]
        public void GetRetweets_TweetWithTweetDTO_ReturnsTransformedDTOFromQueryExecutor()
        {
            // Arrange
            var controller = CreateTweetController();
            var tweetDTO = A.Fake<ITweetDTO>();
            var tweet = GenerateTweet(tweetDTO);
            IEnumerable<ITweetDTO> expectedTweetsDTO = new List<ITweetDTO> { A.Fake<ITweetDTO>() };
            IEnumerable<ITweet> expectedTweets = new List<ITweet> { A.Fake<ITweet>() };

            _fakeTweetQueryExecutor.CallsTo(x => x.GetRetweets(tweetDTO)).Returns(expectedTweetsDTO);
            _fakeTweetFactory.CallsTo(x => x.GenerateTweetsFromDTO(expectedTweetsDTO)).Returns(expectedTweets);

            // Act
            var result = controller.GetRetweets(tweet);

            // Assert
            Assert.AreEqual(result, expectedTweets);
        }

        [TestMethod]
        public void GetRetweets_TweetDTOIsNull_ReturnsTransformedDTOFromQueryExecutor()
        {
            // Arrange
            var controller = CreateTweetController();
            IEnumerable<ITweetDTO> expectedTweetsDTO = new List<ITweetDTO> { A.Fake<ITweetDTO>() };
            IEnumerable<ITweet> expectedTweets = new List<ITweet> { A.Fake<ITweet>() };

            _fakeTweetQueryExecutor.CallsTo(x => x.GetRetweets(null)).Returns(expectedTweetsDTO);
            _fakeTweetFactory.CallsTo(x => x.GenerateTweetsFromDTO(expectedTweetsDTO)).Returns(expectedTweets);

            // Act
            var result = controller.GetRetweets((ITweetDTO)null);

            // Assert
            Assert.AreEqual(result, expectedTweets);
        }

        [TestMethod]
        public void GetRetweets_TweetDTO_ReturnsTransformedDTOFromQueryExecutor()
        {
            // Arrange
            var controller = CreateTweetController();
            var tweetDTO = A.Fake<ITweetDTO>();
            IEnumerable<ITweetDTO> expectedTweetsDTO = new List<ITweetDTO> { A.Fake<ITweetDTO>() };
            IEnumerable<ITweet> expectedTweets = new List<ITweet> { A.Fake<ITweet>() };

            _fakeTweetQueryExecutor.CallsTo(x => x.GetRetweets(tweetDTO)).Returns(expectedTweetsDTO);
            _fakeTweetFactory.CallsTo(x => x.GenerateTweetsFromDTO(expectedTweetsDTO)).Returns(expectedTweets);

            // Act
            var result = controller.GetRetweets(tweetDTO);

            // Assert
            Assert.AreEqual(result, expectedTweets);
        }

        [TestMethod]
        public void GetRetweets_TweetId_ReturnsTransformedDTOFromQueryExecutor()
        {
            // Arrange
            var controller = CreateTweetController();
            var tweetId = TestHelper.GenerateRandomLong();
            IEnumerable<ITweetDTO> expectedTweetsDTO = new List<ITweetDTO> { A.Fake<ITweetDTO>() };
            IEnumerable<ITweet> expectedTweets = new List<ITweet> { A.Fake<ITweet>() };

            _fakeTweetQueryExecutor.CallsTo(x => x.GetRetweets(tweetId)).Returns(expectedTweetsDTO);
            _fakeTweetFactory.CallsTo(x => x.GenerateTweetsFromDTO(expectedTweetsDTO)).Returns(expectedTweets);

            // Act
            var result = controller.GetRetweets(tweetId);

            // Assert
            Assert.AreEqual(result, expectedTweets);
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

            _fakeTweetQueryExecutor.CallsTo(x => x.FavouriteTweet(tweetDTO)).Returns(expectedResult);

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

            _fakeTweetQueryExecutor.CallsTo(x => x.FavouriteTweet(tweetDTO)).Returns(expectedResult);

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

            _fakeTweetQueryExecutor.CallsTo(x => x.FavouriteTweet(tweetId)).Returns(expectedResult);

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

            _fakeTweetQueryExecutor.CallsTo(x => x.UnFavouriteTweet(tweetDTO)).Returns(expectedResult);

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

            _fakeTweetQueryExecutor.CallsTo(x => x.UnFavouriteTweet(tweetDTO)).Returns(expectedResult);

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

            _fakeTweetQueryExecutor.CallsTo(x => x.UnFavouriteTweet(tweetId)).Returns(expectedResult);

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