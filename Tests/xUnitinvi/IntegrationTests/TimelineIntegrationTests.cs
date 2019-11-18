using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Tweetinvi;
using Tweetinvi.Core.Extensions;
using Tweetinvi.Models;
using Tweetinvi.Parameters;
using Xunit;
using Xunit.Abstractions;

namespace xUnitinvi.IntegrationTests
{
    public class TimelineIntegrationTests
    {
        private readonly ITestOutputHelper _logger;
        private ITwitterClient TweetinviApiClient { get; }
        private ITwitterClient TweetinviTestClient { get; }

        public TimelineIntegrationTests(ITestOutputHelper logger)
        {
            _logger = logger;

            _logger.WriteLine(DateTime.Now.ToLongTimeString());

            TweetinviApiClient = new TwitterClient(IntegrationTestConfig.TweetinviApiCredentials);
            TweetinviTestClient = new TwitterClient(IntegrationTestConfig.TweetinviTestCredentials);

            TweetinviEvents.QueryBeforeExecute += (sender, args) => { _logger.WriteLine(args.Url); };
        }

        [Fact]
//        [Fact(Skip = "IntegrationTests")]
        public async Task RunAllAccountTests()
        {
            if (!IntegrationTestConfig.ShouldRunIntegrationTests)
            {
                return;
            }

            _logger.WriteLine($"Starting {nameof(RetweetsOfMeTimeline)}");
            await RetweetsOfMeTimeline().ConfigureAwait(false);
            _logger.WriteLine($"{nameof(RetweetsOfMeTimeline)} succeeded");
        }

        private async Task RetweetsOfMeTimeline()
        {
            var tweet1 = await TweetinviTestClient.Tweets.PublishTweet("tweet 1!");
            var tweet2 = await TweetinviTestClient.Tweets.PublishTweet("tweet 2");

            await TweetinviApiClient.Tweets.PublishRetweet(tweet1);
            await TweetinviApiClient.Tweets.PublishRetweet(tweet2);

            var iterator = TweetinviTestClient.Timeline.GetRetweetsOfMeTimelineIterator(new GetRetweetsOfMeTimelineParameters
            {
                PageSize = 1,
                SinceId = tweet1.Id - 1
            });

            var retweets = new List<ITweet>();
            while (!iterator.Completed)
            {
                var retweetsPage = await iterator.MoveToNextPage();
                retweets.AddRange(retweetsPage);
            }

            await tweet1.Destroy();
            await tweet2.Destroy();

            // assert
            Assert.True(retweets.Select(x => x.Id).ContainsSameObjectsAs(new[] { tweet1.Id, tweet2.Id }));
        }
    }
}