using System;
using Tweetinvi;
using Tweetinvi.Controllers.Shared;
using Tweetinvi.Controllers.Timeline;
using Tweetinvi.Core.Injectinvi;
using Tweetinvi.Core.QueryGenerators;
using Tweetinvi.Parameters;
using Xunit;
using xUnitinvi.TestHelpers;

namespace xUnitinvi.ClientActions.TimelineClient
{
    public class TimelineQueryGeneratorTests
    {
        public TimelineQueryGeneratorTests()
        {
            _fakeBuilder = new FakeClassBuilder<TimelineQueryGenerator>();
        }

        private readonly FakeClassBuilder<TimelineQueryGenerator> _fakeBuilder;

        private TimelineQueryGenerator CreateTimelineQueryGenerator()
        {
            return _fakeBuilder.GenerateClass(
                new ConstructorNamedParameter("queryParameterGenerator", TweetinviContainer.Resolve<IQueryParameterGenerator>()),
                new ConstructorNamedParameter("userQueryParameterGenerator", TweetinviContainer.Resolve<IUserQueryParameterGenerator>()));
        }

        [Fact]
        public void GetHomeTimelineQuery_ReturnsExpectedQuery()
        {
            // Arrange
            var queryGenerator = CreateTimelineQueryGenerator();

            var parameters = new GetGetHomeTimelineParameters
            {
                IncludeEntities = true,
                TrimUser = true,
                IncludeContributorDetails = true,
                ExcludeReplies = true,
                MaxId = 42,
                SinceId = 43,
                PageSize = 44,
                CustomQueryParameters = { new Tuple<string, string>("hello", "world") }
            };

            // Act
            var result = queryGenerator.GetHomeTimelineQuery(parameters, TweetMode.Extended);

            // Assert
            Assert.Equal(result, $"https://api.twitter.com/1.1/statuses/home_timeline.json?count=44&since_id=43&max_id=42" +
                                 $"&include_entities=true&trim_user=true&contributor_details=true&exclude_replies=true&tweet_mode=extended&hello=world");
        }

        [Fact]
        public void GetRetweetsOfMeTimelineQuery_ReturnsExpectedQuery()
        {
            // Arrange
            var queryGenerator = CreateTimelineQueryGenerator();

            var parameters = new GetRetweetsOfMeTimelineParameters
            {
                IncludeEntities = true,
                TrimUser = true,
                IncludeUserEntities = true,
                MaxId = 42,
                SinceId = 43,
                PageSize = 44,
                CustomQueryParameters = { new Tuple<string, string>("hello", "world") }
            };

            // Act
            var result = queryGenerator.GetRetweetsOfMeTimelineQuery(parameters, TweetMode.Extended);

            // Assert
            Assert.Equal(result, $"https://api.twitter.com/1.1/statuses/retweets_of_me.json?count=44&since_id=43&max_id=42" +
                                 $"&include_entities=true&trim_user=true&include_user_entities=true&tweet_mode=extended&hello=world");
        }
    }
}