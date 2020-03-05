using System;
using FakeItEasy;
using FakeItEasy.ExtensionSyntax.Full;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Testinvi.Helpers;
using Tweetinvi;
using Tweetinvi.Controllers.Properties;
using Tweetinvi.Controllers.Shared;
using Tweetinvi.Controllers.Tweet;
using Tweetinvi.Core.Client;
using Tweetinvi.Core.Helpers;
using Tweetinvi.Core.Injectinvi;
using Tweetinvi.Models;
using Tweetinvi.Models.DTO;
using Tweetinvi.Parameters;

namespace Testinvi.TweetinviControllers.TweetTests
{
    [TestClass]
    public class TweetQueryGeneratorTests
    {
        private FakeClassBuilder<TweetQueryGenerator> _fakeBuilder;
        private Fake<ITweetQueryValidator> _fakeTweetQueryValidator;
        private Fake<ITwitterStringFormatter> _fakeTwitterStringFormatter;
        private Fake<IQueryParameterGenerator> _fakeQueryParameterGenerator;

        private string _expectedTweetParameter;
        private string _expectedPlaceIdParameter;
        private ICoordinates _expectedCoordinatesParameter;

        [TestInitialize]
        public void TestInitialize()
        {
            _fakeBuilder = new FakeClassBuilder<TweetQueryGenerator>();
            _fakeTweetQueryValidator = _fakeBuilder.GetFake<ITweetQueryValidator>();
            _fakeTwitterStringFormatter = _fakeBuilder.GetFake<ITwitterStringFormatter>();
            _fakeQueryParameterGenerator = _fakeBuilder.GetFake<IQueryParameterGenerator>();

            QueryParameterGeneratorTestHelper.InitializeQueryParameterGenerator(_fakeQueryParameterGenerator);
        }

        #region Publish Tweet

        [TestMethod]
        public void GetPublishTweetInReplyToQuery_ReturnsExpectedResults()
        {
            VerifyPublishTweetInReplyToQuery(false, false, false, false);
            VerifyPublishTweetInReplyToQuery(false, false, false, true);
            VerifyPublishTweetInReplyToQuery(false, false, true, false);
            VerifyPublishTweetInReplyToQuery(false, false, true, true);

            VerifyPublishTweetInReplyToQuery(false, true, false, false);
            VerifyPublishTweetInReplyToQuery(false, true, false, true);
            VerifyPublishTweetInReplyToQuery(false, true, true, false);
            VerifyPublishTweetInReplyToQuery(false, true, true, true);

            VerifyPublishTweetInReplyToQuery(true, false, false, false);
            VerifyPublishTweetInReplyToQuery(true, false, false, true);
            VerifyPublishTweetInReplyToQuery(true, false, true, false);
            VerifyPublishTweetInReplyToQuery(true, false, true, true);

            VerifyPublishTweetInReplyToQuery(true, true, false, false);
            VerifyPublishTweetInReplyToQuery(true, true, false, true);
            VerifyPublishTweetInReplyToQuery(true, true, true, false);
            VerifyPublishTweetInReplyToQuery(true, true, true, true);
        }

        private void VerifyPublishTweetInReplyToQuery(
            bool canNewTweetBePublished,
            bool replyToTweetId,
            bool tweetContainsPlaceId,
            bool tweetContainsCoordinates)
        {
            // Arrange
            // Arrange
            var queryGenerator = CreateTweetQueryGenerator();
            var parameter = GeneratePublishTweetParameter(canNewTweetBePublished, tweetContainsPlaceId,
                tweetContainsCoordinates);
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
                    queryGenerator.GetPublishTweetQuery(parameter, TweetMode.Extended);
                }
                catch (ArgumentException)
                {
                    return;
                }
            }

            var result = queryGenerator.GetPublishTweetQuery(parameter, TweetMode.Extended);

            // Assert
            {
                var baseQuery = "https://api.twitter.com/1.1/statuses/update.json?";

                Assert.IsTrue(result.StartsWith(baseQuery));
                Assert.IsTrue(result.Contains($"status={_expectedTweetParameter}"));

                Assert.AreEqual(tweetContainsPlaceId, result.Contains($"place_id={_expectedPlaceIdParameter}"));

                if (tweetContainsCoordinates)
                {
                    Assert.IsTrue(result.Contains($"lat={_expectedCoordinatesParameter.Latitude}"));
                    Assert.IsTrue(result.Contains($"long={_expectedCoordinatesParameter.Longitude}"));
                }
                else
                {
                    Assert.IsFalse(result.Contains("lat="));
                    Assert.IsFalse(result.Contains("long="));
                }
            }
        }

        [TestMethod]
        public void GetPublishTweetInReplyToIdQuery_ReturnsExpectedResults()
        {
            VerifyPublishTweetInReplyToIdQuery(false, false, false);
            VerifyPublishTweetInReplyToIdQuery(false, false, true);
            VerifyPublishTweetInReplyToIdQuery(false, true, false);
            VerifyPublishTweetInReplyToIdQuery(false, true, true);

            VerifyPublishTweetInReplyToIdQuery(true, false, false);
            VerifyPublishTweetInReplyToIdQuery(true, false, true);
            VerifyPublishTweetInReplyToIdQuery(true, true, false);
            VerifyPublishTweetInReplyToIdQuery(true, true, true);
        }

        private void VerifyPublishTweetInReplyToIdQuery(
            bool canNewTweetBePublished,
            bool tweetContainsPlaceId,
            bool tweetContainsCoordinates)
        {
            // Arrange
            var queryGenerator = CreateTweetQueryGenerator();
            var parameter = GeneratePublishTweetParameter(canNewTweetBePublished, tweetContainsPlaceId, tweetContainsCoordinates);

            // Act
            if (!canNewTweetBePublished)
            {
                try
                {
                    queryGenerator.GetPublishTweetQuery(parameter, TweetMode.Extended);
                }
                catch (ArgumentException)
                {
                    return;
                }
            }

            var result = queryGenerator.GetPublishTweetQuery(parameter, TweetMode.Extended);

            // Assert
            var baseQuery = "https://api.twitter.com/1.1/statuses/update.json?";

            Assert.IsTrue(result.StartsWith(baseQuery));
            Assert.IsTrue(result.Contains($"status={_expectedTweetParameter}"));

            Assert.AreEqual(tweetContainsPlaceId, result.Contains($"place_id={_expectedPlaceIdParameter}"));

            if (tweetContainsCoordinates)
            {
                Assert.IsTrue(result.Contains($"lat={_expectedCoordinatesParameter.Latitude}"));
                Assert.IsTrue(result.Contains($"long={_expectedCoordinatesParameter.Longitude}"));
            }
            else
            {
                Assert.IsFalse(result.Contains("lat="));
                Assert.IsFalse(result.Contains("long="));
            }
        }

        #endregion

        #region GetPublishRetweetQuery

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
            var result = queryGenerator.GetPublishRetweetQuery(tweetToRetweet, TweetMode.Extended);

            // Assert
            var expectedResult = string.Format(Resources.Tweet_Retweet_Publish, tweetToRetweetId);
            Assert.AreEqual(result, expectedResult);
        }

        [TestMethod]
        public void GetPublishRetweetQuery_WithTweetId_ReturnsExpectedQuery()
        {
            // Arrange
            var queryGenerator = CreateTweetQueryGenerator();
            var tweetToRetweetId = new TweetIdentifier(TestHelper.GenerateRandomLong());

            // Act
            var result = queryGenerator.GetPublishRetweetQuery(tweetToRetweetId, TweetMode.Extended);

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
            var maxRetweetsToRetrieve = TestHelper.GenerateRandomInt(100);
            tweetToRetweet.CallsTo(x => x.Id).Returns(tweetToRetweetId);

            var executionContext = new TwitterExecutionContext();

            _fakeTweetQueryValidator.CallsTo(x => x.IsTweetPublished(tweetToRetweet)).Returns(true);

            // Act
            var result = queryGenerator.GetRetweetsQuery(tweetToRetweet, maxRetweetsToRetrieve, executionContext);

            // Assert
            var expectedResult = $"https://api.twitter.com/1.1/statuses/retweets/{tweetToRetweetId}.json?count={maxRetweetsToRetrieve}";

            Assert.AreEqual(result, expectedResult);
        }

        #endregion

        #region Get Retweeter Ids Query

        [TestMethod]
        public void GetRetweeterIdsQuery_WithInValidTweetIdentifier_ReturnsExpectedQuery()
        {
            // Arrange
            var queryGenerator = CreateTweetQueryGenerator();
            var tweetIdentifier = new TweetIdentifier(42);
            var maxRetweetersToRetrieve = 43;

            // Act
            var result = queryGenerator.GetRetweeterIdsQuery(tweetIdentifier, maxRetweetersToRetrieve);

            // Assert
            var expectedResult = $"https://api.twitter.com/1.1/statuses/retweeters/ids.json?id=42&count=43";
            Assert.AreEqual(result, expectedResult);

            _fakeTweetQueryValidator.CallsTo(x => x.ThrowIfTweetCannotBeUsed(tweetIdentifier)).MustHaveHappened();
        }

        #endregion

        #region GetDestroyTweetQuery

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

            _fakeTweetQueryValidator.CallsTo(x => x.ThrowIfTweetCannotBeUsed(tweetToFavourite)).MustHaveHappened();
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

            _fakeTweetQueryValidator.CallsTo(x => x.ThrowIfTweetCannotBeUsed(tweetToFavouriteId)).MustHaveHappened();
        }

        #endregion

        #region GetUnfavoriteTweetQuery

        [TestMethod]
        public void GetUnfavoriteTweetQuery_TweetPublished_ReturnsExpectedQuery()
        {
            // Arrange
            var queryGenerator = CreateTweetQueryGenerator();
            var tweetToUnfavoriteId = TestHelper.GenerateRandomLong();
            var tweetToUnfavorite = A.Fake<ITweetDTO>();
            tweetToUnfavorite.CallsTo(x => x.Id).Returns(tweetToUnfavoriteId);

            _fakeTweetQueryValidator.CallsTo(x => x.IsTweetPublished(tweetToUnfavorite)).Returns(true);

            // Act
            var result = queryGenerator.GetUnfavoriteTweetQuery(tweetToUnfavorite);

            // Assert
            var expectedResult = string.Format(Resources.Tweet_Favorite_Destroy, tweetToUnfavoriteId);
            Assert.AreEqual(result, expectedResult);

            _fakeTweetQueryValidator.CallsTo(x => x.ThrowIfTweetCannotBeUsed(tweetToUnfavorite)).MustHaveHappened();
        }

        [TestMethod]
        public void GetUnfavoriteTweetQuery_WithTweetId_ReturnsExpectedQuery()
        {
            // Arrange
            var queryGenerator = CreateTweetQueryGenerator();
            var tweetToUnfavoriteId = TestHelper.GenerateRandomLong();

            // Act
            var result = queryGenerator.GetUnfavoriteTweetQuery(tweetToUnfavoriteId);

            // Assert
            var expectedResult = string.Format(Resources.Tweet_Favorite_Destroy, tweetToUnfavoriteId);
            Assert.AreEqual(result, expectedResult);

            _fakeTweetQueryValidator.CallsTo(x => x.ThrowIfTweetCannotBeUsed(tweetToUnfavoriteId)).MustHaveHappened();
        }

        #endregion

        #region GetGenerateOEmbedTweetQuery


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

            _fakeTweetQueryValidator.CallsTo(x => x.ThrowIfTweetCannotBeUsed(tweet)).MustHaveHappened();
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

            _fakeTweetQueryValidator.CallsTo(x => x.ThrowIfTweetCannotBeUsed(tweetToRetweetId)).MustHaveHappened();
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

        private TweetQueryGenerator CreateTweetQueryGenerator()
        {
            return _fakeBuilder.GenerateClass();
        }
    }
}