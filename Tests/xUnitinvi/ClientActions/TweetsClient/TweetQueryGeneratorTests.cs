using System;
using System.Collections.Generic;
using FakeItEasy;
using Tweetinvi;
using Tweetinvi.Controllers.Shared;
using Tweetinvi.Controllers.Tweet;
using Tweetinvi.Core.Helpers;
using Tweetinvi.Core.Injectinvi;
using Tweetinvi.Core.QueryGenerators;
using Tweetinvi.Models;
using Tweetinvi.Parameters;
using Xunit;
using xUnitinvi.TestHelpers;

namespace xUnitinvi.ClientActions.TweetsClient
{
    public class TweetQueryGeneratorTests
    {
        public TweetQueryGeneratorTests()
        {
            _fakeBuilder = new FakeClassBuilder<TweetQueryGenerator>();
        }

        private readonly FakeClassBuilder<TweetQueryGenerator> _fakeBuilder;

        private TweetQueryGenerator CreateUserQueryGenerator()
        {
            return _fakeBuilder.GenerateClass(
                new ConstructorNamedParameter("queryParameterGenerator", TweetinviContainer.Resolve<IQueryParameterGenerator>()),
                new ConstructorNamedParameter("userQueryParameterGenerator", TweetinviContainer.Resolve<IUserQueryParameterGenerator>()));
        }

        [Fact]
        public void GetTweetQuery_ReturnsExpectedQuery()
        {
            // Arrange
            var queryGenerator = CreateUserQueryGenerator();

            var parameters = new GetTweetParameters(42)
            {
                IncludeEntities = true,
                IncludeCardUri = true,
                IncludeMyRetweet = true,
                IncludeExtAltText = true,
                TrimUser = true,
                CustomQueryParameters = { new Tuple<string, string>("hello", "world") }
            };

            // Act
            var result = queryGenerator.GetTweetQuery(parameters, TweetMode.Extended);

            // Assert
            Assert.Equal(result,
                $"https://api.twitter.com/1.1/statuses/show.json?id=42&include_card_uri=true&include_entities=true&include_ext_alt_text=true&include_my_retweet=true&trim_user=true&tweet_mode=extended&hello=world");
        }
        
        [Fact]
        public void SimplePublishTweetQuery_ReturnsExpectedQuery()
        {
            // Arrange
            var queryGenerator = CreateUserQueryGenerator();
            var parameters = new PublishTweetParameters("hello");

            // Act
            var result = queryGenerator.GetPublishTweetQuery(parameters, null);

            // Assert
            Assert.Equal(result, $"https://api.twitter.com/1.1/statuses/update.json?status=hello");
        }

        [Fact]
        public void PublishTweetQuery_ReturnsExpectedQuery()
        {
            // Arrange
            var queryGenerator = CreateUserQueryGenerator();

            var quotedAuthor = A.Fake<IUser>();
            A.CallTo(() => quotedAuthor.ScreenName).Returns("quoted_author");
            var quotedTweet = A.Fake<ITweet>();
            A.CallTo(() => quotedTweet.Id).Returns(48);
            A.CallTo(() => quotedTweet.CreatedBy).Returns(quotedAuthor);

            var parameters = new PublishTweetParameters("hello")
            {
                Coordinates = new Coordinates(42, 43),
                MediaIds = new List<long> { 44, 51 },
                PlaceId = "place",
                PossiblySensitive = false,
                DisplayExactCoordinates = true,
                AutoPopulateReplyMetadata = true,
                ExcludeReplyUserIds = new long[] { 45, 50 },
                InReplyToTweetId = 46,
                QuotedTweet = quotedTweet,
                TrimUser = true,
                CardUri = "cardUri",
                CustomQueryParameters = { new Tuple<string, string>("hello", "world") }
            };

            // Act
            var result = queryGenerator.GetPublishTweetQuery(parameters, TweetMode.Extended);

            // Assert
            var url = "https://api.twitter.com/1.1/statuses/update.json?status=hello&auto_populate_reply_metadata=true&" +
                $"attachment_url={Uri.EscapeDataString("https://twitter.com/quoted_author/status/48")}&" +
                "card_uri=cardUri&display_coordinates=true&exclude_reply_user_ids=45%2C50&" +
                "in_reply_to_status_id=46&lat=42&long=43&media_ids=44%2C51&" +
                "place_id=place&possibly_sensitive=false&trim_user=true&tweet_mode=extended&hello=world";
            Assert.Equal(result, url);
        }

        [Fact]
        public void GetFavoriteTweetsQuery_ReturnsExpectedQuery()
        {
            // Arrange
            var queryGenerator = CreateUserQueryGenerator();
            var user = new UserIdentifier(42);

            var parameters = new GetFavoriteTweetsParameters(user)
            {
                IncludeEntities = true,
                MaxId = 42,
                SinceId = 43,
                PageSize = 12,
                CustomQueryParameters = { new Tuple<string, string>("hello", "world") }
            };

            // Act
            var result = queryGenerator.GetFavoriteTweetsQuery(parameters, TweetMode.Extended);

            // Assert
            Assert.Equal(result, $"https://api.twitter.com/1.1/favorites/list.json?user_id=42&include_entities=true&since_id=43&max_id=42&count=12&tweet_mode=extended&hello=world");
        }
        
        [Fact]
        public void GetRetweetsQuery_ReturnsExpectedQuery()
        {
            // Arrange
            var queryGenerator = CreateUserQueryGenerator();

            var parameters = new GetRetweetsParameters(42)
            {
                TrimUser = true,
                PageSize = 12,
                CustomQueryParameters = { new Tuple<string, string>("hello", "world") }
            };

            // Act
            var result = queryGenerator.GetRetweetsQuery(parameters, TweetMode.Extended);

            // Assert
            Assert.Equal(result, $"https://api.twitter.com/1.1/statuses/retweets/42.json?count=12&trim_user=true&tweet_mode=extended&hello=world");
        }
    }
}