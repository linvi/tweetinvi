using FakeItEasy;
using FakeItEasy.ExtensionSyntax.Full;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Testinvi.Helpers;
using Tweetinvi.Controllers.Tweet;
using Tweetinvi.Core.Interfaces.DTO;

namespace Testinvi.TweetinviControllers.TweetTests
{
    [TestClass]
    public class TweetQueryValidatorTests
    {
        private FakeClassBuilder<TweetQueryValidator> _fakeBuilder;

        [TestInitialize]
        public void TestInitialize()
        {
            _fakeBuilder = new FakeClassBuilder<TweetQueryValidator>();
        }

        #region CanTweetDTOBePublished

        [TestMethod]
        public void CanTweetDTOBePublished_TweetIsNull_ReturnsFalse()
        {
            // Arrange
            var queryValidator = CreateTweetQueryValidator();
            
            // Act
            var result = queryValidator.CanTweetDTOBePublished(null);

            // Assert
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void CanTweetDTOBePublished_TweetIsAlreadyPublished_ReturnsFalse()
        {
            // Arrange
            var queryValidator = CreateTweetQueryValidator();
            var tweet = A.Fake<ITweetDTO>();
            tweet.CallsTo(x => x.IsTweetPublished).Returns(true);

            // Act
            var result = queryValidator.CanTweetDTOBePublished(tweet);

            // Assert
            Assert.IsFalse(result);
        }

        [TestMethod]
        // This case is in theory impossible
        public void CanTweetDTOBePublished_TweetIsNotAlreadyPublishedButDestroyed_ReturnsFalse()
        {
            // Arrange
            var queryValidator = CreateTweetQueryValidator();
            var tweet = A.Fake<ITweetDTO>();
            tweet.CallsTo(x => x.IsTweetPublished).Returns(false);
            tweet.CallsTo(x => x.IsTweetDestroyed).Returns(true);

            // Act
            var result = queryValidator.CanTweetDTOBePublished(tweet);

            // Assert
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void CanTweetDTOBePublished_TweetIsDestroyed_ReturnsFalse()
        {
            // Arrange
            var queryValidator = CreateTweetQueryValidator();
            var tweet = A.Fake<ITweetDTO>();
            tweet.CallsTo(x => x.IsTweetPublished).Returns(true);
            tweet.CallsTo(x => x.IsTweetDestroyed).Returns(true);

            // Act
            var result = queryValidator.CanTweetDTOBePublished(tweet);

            // Assert
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void CanTweetDTOBePublished_TweetIsNew_ReturnsTrue()
        {
            // Arrange
            var queryValidator = CreateTweetQueryValidator();
            var tweet = A.Fake<ITweetDTO>();
            tweet.CallsTo(x => x.IsTweetPublished).Returns(false);
            tweet.CallsTo(x => x.IsTweetDestroyed).Returns(false);

            // Act
            var result = queryValidator.CanTweetDTOBePublished(tweet);

            // Assert
            Assert.IsTrue(result);
        }

        #endregion

        #region CanTweetDTOBeDestroyed

        [TestMethod]
        public void CanTweetDTOBeDestroyed_TweetIsNull_ReturnsFalse()
        {
            // Arrange
            var queryValidator = CreateTweetQueryValidator();

            // Act
            var result = queryValidator.CanTweetDTOBeDestroyed(null);

            // Assert
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void CanTweetDTOBeDestroyed_TweetHasBeenPublished_ReturnsFalse()
        {
            // Arrange
            var queryValidator = CreateTweetQueryValidator();
            var tweet = A.Fake<ITweetDTO>();
            tweet.CallsTo(x => x.IsTweetPublished).Returns(true);

            // Act
            var result = queryValidator.CanTweetDTOBeDestroyed(tweet);

            // Assert
            Assert.IsTrue(result);
        }

        [TestMethod]
        // This case is in theory impossible
        public void CanTweetDTOBeDestroyed_TweetIsHasNotBeenPublishedButDestroyed_ReturnsFalse()
        {
            // Arrange
            var queryValidator = CreateTweetQueryValidator();
            var tweet = A.Fake<ITweetDTO>();
            tweet.CallsTo(x => x.IsTweetPublished).Returns(false);
            tweet.CallsTo(x => x.IsTweetDestroyed).Returns(true);

            // Act
            var result = queryValidator.CanTweetDTOBeDestroyed(tweet);

            // Assert
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void CanTweetDTOBeDestroyed_TweetIsAlreadyDestroyed_ReturnsFalse()
        {
            // Arrange
            var queryValidator = CreateTweetQueryValidator();
            var tweet = A.Fake<ITweetDTO>();
            tweet.CallsTo(x => x.IsTweetPublished).Returns(true);
            tweet.CallsTo(x => x.IsTweetDestroyed).Returns(true);

            // Act
            var result = queryValidator.CanTweetDTOBeDestroyed(tweet);

            // Assert
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void CanTweetDTOBeDestroyed_TweetIsNew_ReturnsFalse()
        {
            // Arrange
            var queryValidator = CreateTweetQueryValidator();
            var tweet = A.Fake<ITweetDTO>();
            tweet.CallsTo(x => x.IsTweetPublished).Returns(false);
            tweet.CallsTo(x => x.IsTweetDestroyed).Returns(false);

            // Act
            var result = queryValidator.CanTweetDTOBeDestroyed(tweet);

            // Assert
            Assert.IsFalse(result);
        }

        #endregion

        #region IsTweetPublished

        [TestMethod]
        public void IsTweetPublished_TweetIsNull_ReturnsFalse()
        {
            // Arrange
            var queryValidator = CreateTweetQueryValidator();

            // Act
            var result = queryValidator.IsTweetPublished(null);

            // Assert
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void IsTweetPublished_TweetHasBeenPublished_ReturnsFalse()
        {
            // Arrange
            var queryValidator = CreateTweetQueryValidator();
            var tweet = A.Fake<ITweetDTO>();
            tweet.CallsTo(x => x.IsTweetPublished).Returns(true);

            // Act
            var result = queryValidator.IsTweetPublished(tweet);

            // Assert
            Assert.IsTrue(result);
        }

        [TestMethod]
        // This case is in theory impossible
        public void IsTweetPublished_TweetIsHasNotBeenPublishedButDestroyed_ReturnsFalse()
        {
            // Arrange
            var queryValidator = CreateTweetQueryValidator();
            var tweet = A.Fake<ITweetDTO>();
            tweet.CallsTo(x => x.IsTweetPublished).Returns(false);
            tweet.CallsTo(x => x.IsTweetDestroyed).Returns(true);

            // Act
            var result = queryValidator.IsTweetPublished(tweet);

            // Assert
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void IsTweetPublished_TweetIsAlreadyDestroyed_ReturnsFalse()
        {
            // Arrange
            var queryValidator = CreateTweetQueryValidator();
            var tweet = A.Fake<ITweetDTO>();
            tweet.CallsTo(x => x.IsTweetPublished).Returns(true);
            tweet.CallsTo(x => x.IsTweetDestroyed).Returns(true);

            // Act
            var result = queryValidator.IsTweetPublished(tweet);

            // Assert
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void IsTweetPublished_TweetIsNew_ReturnsFalse()
        {
            // Arrange
            var queryValidator = CreateTweetQueryValidator();
            var tweet = A.Fake<ITweetDTO>();
            tweet.CallsTo(x => x.IsTweetPublished).Returns(false);
            tweet.CallsTo(x => x.IsTweetDestroyed).Returns(false);

            // Act
            var result = queryValidator.IsTweetPublished(tweet);

            // Assert
            Assert.IsFalse(result);
        }

        #endregion

        public TweetQueryValidator CreateTweetQueryValidator()
        {
            return _fakeBuilder.GenerateClass();
        }
    }
}