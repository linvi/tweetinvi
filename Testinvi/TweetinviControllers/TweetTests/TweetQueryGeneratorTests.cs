using System;
using FakeItEasy;
using FakeItEasy.ExtensionSyntax.Full;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Testinvi.Helpers;
using Tweetinvi.Controllers.Properties;
using Tweetinvi.Controllers.Tweet;
using Tweetinvi.Core.Helpers;
using Tweetinvi.Core.Interfaces.DTO;
using Tweetinvi.Core.Interfaces.Models;
using Tweetinvi.Core.Parameters;
using Tweetinvi.Core.Parameters.QueryParameters;

namespace Testinvi.TweetinviControllers.TweetTests
{
    [TestClass]
    public class TweetQueryGeneratorTests
    {
        private FakeClassBuilder<TweetQueryGenerator> _fakeBuilder;
        private Fake<ITweetQueryValidator> _fakeTweetQueryValidator;
        private Fake<ITwitterStringFormatter> _fakeTwitterStringFormatter;

        private string _expectedTweetParameter;
        private string _expectedPlaceIdParameter;
        private ICoordinates _expectedCoordinatesParameter;

        [TestInitialize]
        public void TestInitialize()
        {
            _fakeBuilder = new FakeClassBuilder<TweetQueryGenerator>();
            _fakeTweetQueryValidator = _fakeBuilder.GetFake<ITweetQueryValidator>();
            _fakeTwitterStringFormatter = _fakeBuilder.GetFake<ITwitterStringFormatter>();
        }

        #region Publish Tweet
        
        [TestMethod]
        public void GetPublishTweetInReplyToQuery_ReturnsExpectedResults()
        {
            VerifyPublishTweetInReplyToQuery(false, false, false, false, false);
            VerifyPublishTweetInReplyToQuery(false, false, false, true, false);
            VerifyPublishTweetInReplyToQuery(false, false, true, false, false);
            VerifyPublishTweetInReplyToQuery(false, false, true, true, false);

            VerifyPublishTweetInReplyToQuery(false, true, false, false, false);
            VerifyPublishTweetInReplyToQuery(false, true, false, true, false);
            VerifyPublishTweetInReplyToQuery(false, true, true, false, false);
            VerifyPublishTweetInReplyToQuery(false, true, true, true, false);

            VerifyPublishTweetInReplyToQuery(true, false, false, false, true);
            VerifyPublishTweetInReplyToQuery(true, false, false, true, true);
            VerifyPublishTweetInReplyToQuery(true, false, true, false, true);
            VerifyPublishTweetInReplyToQuery(true, false, true, true, true);

            VerifyPublishTweetInReplyToQuery(true, true, false, false, true);
            VerifyPublishTweetInReplyToQuery(true, true, false, true, true);
            VerifyPublishTweetInReplyToQuery(true, true, true, false, true);
            VerifyPublishTweetInReplyToQuery(true, true, true, true, true);
        }

        public void VerifyPublishTweetInReplyToQuery(
            bool canNewTweetBePublished,
            bool replyToTweetId,
            bool tweetContainsPlaceId,
            bool tweetContainsCoordinates,
            bool isValid)
        {
            // Arrange
            // Arrange
            var queryGenerator = CreateTweetQueryGenerator();
            var parameter = GeneratePublishTweetParameter(canNewTweetBePublished, tweetContainsPlaceId, tweetContainsCoordinates);
            var tweetId = TestHelper.GenerateRandomLong();

            if (replyToTweetId)
            {
                var tweetIdentifier = A.Fake<ITweetIdentifier>();
                tweetIdentifier.CallsTo(x => x.Id).Returns(tweetId);

                parameter.CallsTo(x => x.InReplyToTweet).Returns(tweetIdentifier);
            }
            else
            {
                parameter.CallsTo(x => x.InReplyToTweet).Returns(null);
            }

            // Act
            if (!canNewTweetBePublished)
            {
                try
                {
                    queryGenerator.GetPublishTweetQuery(parameter);
                }
                catch (ArgumentException)
                {
                    return;
                }
            }

            var result = queryGenerator.GetPublishTweetQuery(parameter);

            // Assert
            if (isValid)
            {
                var baseQuery = "https://api.twitter.com/1.1/statuses/update.json?";

                Assert.IsTrue(result.StartsWith(baseQuery));
                Assert.IsTrue(result.Contains(string.Format("status={0}", _expectedTweetParameter)));

                Assert.AreEqual(tweetContainsPlaceId, result.Contains(string.Format("place_id={0}", _expectedPlaceIdParameter)));

                if (tweetContainsCoordinates)
                {
                    Assert.IsTrue(result.Contains(string.Format("lat={0}", _expectedCoordinatesParameter.Latitude)));
                    Assert.IsTrue(result.Contains(string.Format("long={0}", _expectedCoordinatesParameter.Longitude)));
                }
                else
                {
                    Assert.IsFalse(result.Contains("lat="));
                    Assert.IsFalse(result.Contains("long="));
                }
            }
            else
            {
                Assert.IsNull(result);
            }
        }

        [TestMethod]
        public void GetPublishTweetInReplyToIdQuery_ReturnsExpectedResults()
        {
            VerifyPublishTweetInReplyToIdQuery(false, false, false, false);
            VerifyPublishTweetInReplyToIdQuery(false, false, true, false);
            VerifyPublishTweetInReplyToIdQuery(false, true, false, false);
            VerifyPublishTweetInReplyToIdQuery(false, true, true, false);

            VerifyPublishTweetInReplyToIdQuery(true, false, false, true);
            VerifyPublishTweetInReplyToIdQuery(true, false, true, true);
            VerifyPublishTweetInReplyToIdQuery(true, true, false, true);
            VerifyPublishTweetInReplyToIdQuery(true, true, true, true);
        }

        public void VerifyPublishTweetInReplyToIdQuery(
            bool canNewTweetBePublished,
            bool tweetContainsPlaceId,
            bool tweetContainsCoordinates,
            bool isValid)
        {
            // Arrange
            var queryGenerator = CreateTweetQueryGenerator();
            var parameter = GeneratePublishTweetParameter(canNewTweetBePublished, tweetContainsPlaceId, tweetContainsCoordinates);

            // Act
            if (!canNewTweetBePublished)
            {
                try
                {
                    queryGenerator.GetPublishTweetQuery(parameter);
                }
                catch (ArgumentException)
                {
                    return;
                }
            }

            var result = queryGenerator.GetPublishTweetQuery(parameter);

            // Assert
            if (isValid)
            {
                var baseQuery = "https://api.twitter.com/1.1/statuses/update.json?";

                Assert.IsTrue(result.StartsWith(baseQuery));
                Assert.IsTrue(result.Contains(string.Format("status={0}", _expectedTweetParameter)));

                Assert.AreEqual(tweetContainsPlaceId, result.Contains(string.Format("place_id={0}", _expectedPlaceIdParameter)));

                if (tweetContainsCoordinates)
                {
                    Assert.IsTrue(result.Contains(string.Format("lat={0}", _expectedCoordinatesParameter.Latitude)));
                    Assert.IsTrue(result.Contains(string.Format("long={0}", _expectedCoordinatesParameter.Longitude)));
                }
                else
                {
                    Assert.IsFalse(result.Contains("lat="));
                    Assert.IsFalse(result.Contains("long="));
                }
            }
            else
            {
                Assert.IsNull(result);
            }
        }
        
        #endregion

        #region GetPublishRetweetQuery

        [TestMethod]
        public void GetPublishRetweetQuery_RetweetingTweetUnpublished_ReturnsNull()
        {
            // Arrange
            var queryGenerator = CreateTweetQueryGenerator();
            var tweetToRetweet = A.Fake<ITweetDTO>();

            _fakeTweetQueryValidator.CallsTo(x => x.IsTweetPublished(tweetToRetweet)).Returns(false);

            // Act
            var result = queryGenerator.GetPublishRetweetQuery(tweetToRetweet);

            // Assert
            Assert.AreEqual(result, null);
        }

        [TestMethod]
        public void GetPublishRetweetQuery_RetweetingTweetPublished_ReturnsExpectedQuery()
        {
            // Arrange
            var queryGenerator = CreateTweetQueryGenerator();
            var tweetToRetweetId = TestHelper.GenerateRandomLong();
            var tweetToRetweet = A.Fake<ITweetDTO>();
            tweetToRetweet.CallsTo(x => x.Id).Returns(tweetToRetweetId);

            _fakeTweetQueryValidator.CallsTo(x => x.IsTweetPublished(tweetToRetweet)).Returns(true);

            // Act
            var result = queryGenerator.GetPublishRetweetQuery(tweetToRetweet);

            // Assert
            var expectedResult = string.Format(Resources.Tweet_Retweet_Publish, tweetToRetweetId);
            Assert.AreEqual(result, expectedResult);
        }

        [TestMethod]
        public void GetPublishRetweetQuery_WithTweetId_ReturnsExpectedQuery()
        {
            // Arrange
            var queryGenerator = CreateTweetQueryGenerator();
            var tweetToRetweetId = TestHelper.GenerateRandomLong();

            // Act
            var result = queryGenerator.GetPublishRetweetQuery(tweetToRetweetId);

            // Assert
            var expectedResult = string.Format(Resources.Tweet_Retweet_Publish, tweetToRetweetId);
            Assert.AreEqual(result, expectedResult);
        }

        #endregion

        #region GetRetweetsQuery

        [TestMethod]
        public void GetRetweetsQuery_WithValidTweetDTO_ReturnsExpectedQuery()
        {
            // Arrange
            var queryGenerator = CreateTweetQueryGenerator();
            var tweetToRetweetId = TestHelper.GenerateRandomLong();
            var tweetToRetweet = A.Fake<ITweetDTO>();
            var maxRetweetsToRetrieve = TestHelper.GenerateRandomInt();
            tweetToRetweet.CallsTo(x => x.Id).Returns(tweetToRetweetId);

            _fakeTweetQueryValidator.CallsTo(x => x.IsTweetPublished(tweetToRetweet)).Returns(true);

            // Act
            var result = queryGenerator.GetRetweetsQuery(tweetToRetweet, maxRetweetsToRetrieve);

            // Assert
            var expectedResult = string.Format("https://api.twitter.com/1.1/statuses/retweets/{0}.json?count={1}", tweetToRetweetId, maxRetweetsToRetrieve);

            Assert.AreEqual(result, expectedResult);
        }

        #endregion

        #region Get Retweeter Ids Query

        [TestMethod]
        public void GetRetweeterIdsQuery_WithInValidTweetIdentifier_ReturnsExpectedQuery()
        {
            // Arrange
            var queryGenerator = CreateTweetQueryGenerator();
            var tweetIdentifier = A.Fake<ITweetIdentifier>();
            var maxRetweetersToRetrieve = TestHelper.GenerateRandomInt();
            _fakeTweetQueryValidator.CallsTo(x => x.IsValidTweetIdentifier(tweetIdentifier)).Returns(true);

            // Act
            var result = queryGenerator.GetRetweeterIdsQuery(tweetIdentifier, maxRetweetersToRetrieve);

            // Assert
            var expectedResult = string.Format("https://api.twitter.com/1.1/statuses/retweeters/ids.json?id={0}&count={1}", tweetIdentifier.Id, maxRetweetersToRetrieve);
            Assert.AreEqual(result, expectedResult);
        }
        
        #endregion

        #region GetDestroyTweetQuery

        [TestMethod]
        public void GetDestroyTweetQuery_TweetCannotBeDestroyed_ReturnsNull()
        {
            // Arrange
            var queryGenerator = CreateTweetQueryGenerator();
            var tweetToDestroy = A.Fake<ITweetDTO>();

            _fakeTweetQueryValidator.CallsTo(x => x.CanTweetDTOBeDestroyed(tweetToDestroy)).Returns(false);

            // Act
            var result = queryGenerator.GetDestroyTweetQuery(tweetToDestroy);

            // Assert
            Assert.AreEqual(result, null);
        }

        [TestMethod]
        public void GetDestroyTweetQuery_TweetCanBeDestroyed_ReturnsExpectedQuery()
        {
            // Arrange
            var queryGenerator = CreateTweetQueryGenerator();
            var tweetToDestroyId = TestHelper.GenerateRandomLong();
            var tweetToDestroy = A.Fake<ITweetDTO>();
            tweetToDestroy.CallsTo(x => x.Id).Returns(tweetToDestroyId);

            _fakeTweetQueryValidator.CallsTo(x => x.CanTweetDTOBeDestroyed(tweetToDestroy)).Returns(true);

            // Act
            var result = queryGenerator.GetDestroyTweetQuery(tweetToDestroy);

            // Assert
            var expectedResult = string.Format(Resources.Tweet_Destroy, tweetToDestroyId);
            Assert.AreEqual(result, expectedResult);
        }

        [TestMethod]
        public void GetDestroyTweetQuery_WithTweetId_ReturnsExpectedQuery()
        {
            // Arrange
            var queryGenerator = CreateTweetQueryGenerator();
            var tweetToDestroyId = TestHelper.GenerateRandomLong();

            // Act
            var result = queryGenerator.GetDestroyTweetQuery(tweetToDestroyId);

            // Assert
            var expectedResult = string.Format(Resources.Tweet_Destroy, tweetToDestroyId);
            Assert.AreEqual(result, expectedResult);
        }

        #endregion

        #region GetFavouriteTweetQuery

        [TestMethod]
        public void GetFavouriteTweetQuery_TweetNotPublished_ReturnsNull()
        {
            // Arrange
            var queryGenerator = CreateTweetQueryGenerator();
            var tweetToRetweet = A.Fake<ITweetDTO>();

            _fakeTweetQueryValidator.CallsTo(x => x.IsTweetPublished(tweetToRetweet)).Returns(false);

            // Act
            var result = queryGenerator.GetFavoriteTweetQuery(tweetToRetweet);

            // Assert
            Assert.AreEqual(result, null);
        }

        [TestMethod]
        public void GetFavouriteTweetQuery_TweetPublished_ReturnsExpectedQuery()
        {
            // Arrange
            var queryGenerator = CreateTweetQueryGenerator();
            var tweetToFavouriteId = TestHelper.GenerateRandomLong();
            var tweetToFavourite = A.Fake<ITweetDTO>();
            tweetToFavourite.CallsTo(x => x.Id).Returns(tweetToFavouriteId);

            _fakeTweetQueryValidator.CallsTo(x => x.IsTweetPublished(tweetToFavourite)).Returns(true);

            // Act
            var result = queryGenerator.GetFavoriteTweetQuery(tweetToFavourite);

            // Assert
            var expectedResult = string.Format(Resources.Tweet_Favorite_Create, tweetToFavouriteId);
            Assert.AreEqual(result, expectedResult);
        }

        [TestMethod]
        public void GetFavouriteTweetQuery_WithTweetId_ReturnsExpectedQuery()
        {
            // Arrange
            var queryGenerator = CreateTweetQueryGenerator();
            var tweetToFavouriteId = TestHelper.GenerateRandomLong();

            // Act
            var result = queryGenerator.GetFavoriteTweetQuery(tweetToFavouriteId);

            // Assert
            var expectedResult = string.Format(Resources.Tweet_Favorite_Create, tweetToFavouriteId);
            Assert.AreEqual(result, expectedResult);
        }

        #endregion

        #region GetUnFavoriteTweetQuery

        [TestMethod]
        public void GetUnFavouriteTweetQuery_TweetNotPublished_ReturnsNull()
        {
            // Arrange
            var queryGenerator = CreateTweetQueryGenerator();
            var tweetToRetweet = A.Fake<ITweetDTO>();

            _fakeTweetQueryValidator.CallsTo(x => x.IsTweetPublished(tweetToRetweet)).Returns(false);

            // Act
            var result = queryGenerator.GetUnFavoriteTweetQuery(tweetToRetweet);

            // Assert
            Assert.AreEqual(result, null);
        }

        [TestMethod]
        public void GetUnFavouriteTweetQuery_TweetPublished_ReturnsExpectedQuery()
        {
            // Arrange
            var queryGenerator = CreateTweetQueryGenerator();
            var tweetToUnFavouriteId = TestHelper.GenerateRandomLong();
            var tweetToUnFavourite = A.Fake<ITweetDTO>();
            tweetToUnFavourite.CallsTo(x => x.Id).Returns(tweetToUnFavouriteId);

            _fakeTweetQueryValidator.CallsTo(x => x.IsTweetPublished(tweetToUnFavourite)).Returns(true);

            // Act
            var result = queryGenerator.GetUnFavoriteTweetQuery(tweetToUnFavourite);

            // Assert
            var expectedResult = string.Format(Resources.Tweet_Favorite_Destroy, tweetToUnFavouriteId);
            Assert.AreEqual(result, expectedResult);
        }

        [TestMethod]
        public void GetUnFavouriteTweetQuery_WithTweetId_ReturnsExpectedQuery()
        {
            // Arrange
            var queryGenerator = CreateTweetQueryGenerator();
            var tweetToUnFavouriteId = TestHelper.GenerateRandomLong();

            // Act
            var result = queryGenerator.GetUnFavoriteTweetQuery(tweetToUnFavouriteId);

            // Assert
            var expectedResult = string.Format(Resources.Tweet_Favorite_Destroy, tweetToUnFavouriteId);
            Assert.AreEqual(result, expectedResult);
        }

        #endregion

        #region GetGenerateOEmbedTweetQuery

        [TestMethod]
        public void GetGenerateOEmbedTweetQuery_TweetNotPublished_ReturnsNull()
        {
            // Arrange
            var queryGenerator = CreateTweetQueryGenerator();
            var tweetToRetweet = A.Fake<ITweetDTO>();

            _fakeTweetQueryValidator.CallsTo(x => x.IsTweetPublished(tweetToRetweet)).Returns(false);

            // Act
            var result = queryGenerator.GetGenerateOEmbedTweetQuery(tweetToRetweet);

            // Assert
            Assert.AreEqual(result, null);
        }

        [TestMethod]
        public void GetGenerateOEmbedTweetQuery_TweetPublished_ReturnsExpectedQuery()
        {
            // Arrange
            var queryGenerator = CreateTweetQueryGenerator();
            var tweetId = TestHelper.GenerateRandomLong();
            var tweet = A.Fake<ITweetDTO>();
            tweet.CallsTo(x => x.Id).Returns(tweetId);

            _fakeTweetQueryValidator.CallsTo(x => x.IsTweetPublished(tweet)).Returns(true);

            // Act
            var result = queryGenerator.GetGenerateOEmbedTweetQuery(tweet);

            // Assert
            var expectedResult = string.Format(Resources.Tweet_GenerateOEmbed, tweetId);
            Assert.AreEqual(result, expectedResult);
        }

        [TestMethod]
        public void GetGenerateOEmbedTweetQuery_WithTweetId_ReturnsExpectedQuery()
        {
            // Arrange
            var queryGenerator = CreateTweetQueryGenerator();
            var tweetToRetweetId = TestHelper.GenerateRandomLong();

            // Act
            var result = queryGenerator.GetGenerateOEmbedTweetQuery(tweetToRetweetId);

            // Assert
            var expectedResult = string.Format(Resources.Tweet_GenerateOEmbed, tweetToRetweetId);
            Assert.AreEqual(result, expectedResult);
        }

        #endregion

        private IPublishTweetParameters GeneratePublishTweetParameter(
            bool canTweetBePublished,
            bool hasPlaceIdParameter,
            bool hasCoordinatesParameter)
        {
            var publishTweetParameters = A.Fake<IPublishTweetParameters>();
            var text = TestHelper.GenerateString();
            _expectedPlaceIdParameter = TestHelper.GenerateString();
            _expectedCoordinatesParameter = A.Fake<ICoordinates>();

            publishTweetParameters.CallsTo(x => x.Text).Returns(text);
            publishTweetParameters.CallsTo(x => x.QuotedTweet).Returns(null);

            if (hasPlaceIdParameter)
            {
                publishTweetParameters.CallsTo(x => x.PlaceId).Returns(_expectedPlaceIdParameter);
            }

            if (hasCoordinatesParameter)
            {
                _expectedCoordinatesParameter.CallsTo(x => x.Latitude).Returns(TestHelper.GenerateRandomInt());
                _expectedCoordinatesParameter.CallsTo(x => x.Longitude).Returns(TestHelper.GenerateRandomInt());

                publishTweetParameters.CallsTo(x => x.Coordinates).Returns(_expectedCoordinatesParameter);
            }
            else
            {
                publishTweetParameters.CallsTo(x => x.Coordinates).Returns(null);
            }

            _expectedTweetParameter = TestHelper.GenerateString();
            _fakeTwitterStringFormatter.CallsTo(x => x.TwitterEncode(text)).Returns(_expectedTweetParameter);

            _fakeTweetQueryValidator.CallsTo(x => x.ThrowIfTweetCannotBePublished(publishTweetParameters)).Invokes(x =>
            {
                if (!canTweetBePublished)
                {
                    throw new ArgumentException();
                }
            });

            return publishTweetParameters;
        }

        private ITweetDTO GeneratePublishedTweet(bool hasTweetBeenPublished = false)
        {
            var tweet = A.Fake<ITweetDTO>();
            var tweetId = TestHelper.GenerateRandomLong();

            tweet.CallsTo(x => x.Id).Returns(tweetId);

            _fakeTweetQueryValidator.CallsTo(x => x.IsTweetPublished(tweet)).Returns(hasTweetBeenPublished);

            return tweet;
        }

        private string GeneratePlaceIdParameter()
        {
            return string.Format("&{0}", _expectedPlaceIdParameter);
        }

        private string GenerateCoordinatesParameter()
        {
            return string.Format("&{0}", _expectedCoordinatesParameter);
        }

        public TweetQueryGenerator CreateTweetQueryGenerator()
        {
            return _fakeBuilder.GenerateClass();
        }
    }
}