using System;
using FakeItEasy;
using FakeItEasy.ExtensionSyntax.Full;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Testinvi.Helpers;
using Tweetinvi.Controllers.Tweet;
using Tweetinvi.Models.DTO;
using Tweetinvi.Parameters;

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
        public void ThrowIfTweetCannotBePublished_ParameterIsNull_Throw()
        {
            // Arrange
            var queryValidator = CreateTweetQueryValidator();
            
            // Act
            try
            {
                queryValidator.ThrowIfTweetCannotBePublished(null);
            }
            catch (ArgumentException)
            {
            }
        }

        [TestMethod]
        public void ThrowIfTweetCannotBePublished_TextIsNull_Throw()
        {
            // Arrange
            var queryValidator = CreateTweetQueryValidator();

            var parameters = A.Fake<IPublishTweetParameters>();
            parameters.CallsTo(x => x.Text).Returns(null);

            // Act
            try
            {
                queryValidator.ThrowIfTweetCannotBePublished(parameters);
            }
            catch (ArgumentException)
            {
            }
        }

        [TestMethod]
        public void ThrowIfTweetCannotBePublished_TextIsEmpty_Throw()
        {
            // Arrange
            var queryValidator = CreateTweetQueryValidator();

            var parameters = A.Fake<IPublishTweetParameters>();
            parameters.CallsTo(x => x.Text).Returns(string.Empty);

            // Act
            try
            {
                queryValidator.ThrowIfTweetCannotBePublished(parameters);
            }
            catch (ArgumentException)
            {
            }
        }

        [TestMethod]
        public void ThrowIfTweetCannotBePublished_TextIsValid_Continue()
        {
            // Arrange
            var queryValidator = CreateTweetQueryValidator();
            var parameters = A.Fake<IPublishTweetParameters>();
            parameters.CallsTo(x => x.Text).Returns("hello");

            // Act - Assert
            queryValidator.ThrowIfTweetCannotBePublished(parameters);
        }

        #endregion

        #region CanTweetDTOBeDestroyed

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void CanTweetDTOBeDestroyed_TweetIsNull_ReturnsFalse()
        {
            // Arrange
            var queryValidator = CreateTweetQueryValidator();

            // Act
            queryValidator.ThrowIfTweetCannotBeDestroyed(null);
        }

        [TestMethod]
        public void CanTweetDTOBeDestroyed_TweetHasBeenPublished_ReturnsFalse()
        {
            // Arrange
            var queryValidator = CreateTweetQueryValidator();
            var tweet = A.Fake<ITweetDTO>();
            tweet.CallsTo(x => x.IsTweetPublished).Returns(true);

            // Act
            queryValidator.ThrowIfTweetCannotBeDestroyed(tweet);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        // This case is in theory impossible
        public void CanTweetDTOBeDestroyed_TweetIsHasNotBeenPublishedButDestroyed_ReturnsFalse()
        {
            // Arrange
            var queryValidator = CreateTweetQueryValidator();
            var tweet = A.Fake<ITweetDTO>();
            tweet.CallsTo(x => x.IsTweetPublished).Returns(false);
            tweet.CallsTo(x => x.IsTweetDestroyed).Returns(true);

            // Act
            queryValidator.ThrowIfTweetCannotBeDestroyed(tweet);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void CanTweetDTOBeDestroyed_TweetIsAlreadyDestroyed_ReturnsFalse()
        {
            // Arrange
            var queryValidator = CreateTweetQueryValidator();
            var tweet = A.Fake<ITweetDTO>();
            tweet.CallsTo(x => x.IsTweetPublished).Returns(true);
            tweet.CallsTo(x => x.IsTweetDestroyed).Returns(true);

            // Act
            queryValidator.ThrowIfTweetCannotBeDestroyed(tweet);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void CanTweetDTOBeDestroyed_TweetIsNew_ReturnsFalse()
        {
            // Arrange
            var queryValidator = CreateTweetQueryValidator();
            var tweet = A.Fake<ITweetDTO>();
            tweet.CallsTo(x => x.IsTweetPublished).Returns(false);
            tweet.CallsTo(x => x.IsTweetDestroyed).Returns(false);

            // Act
            queryValidator.ThrowIfTweetCannotBeDestroyed(tweet);
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