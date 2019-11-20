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
        private readonly ITwitterClient _tweetinviApiClient;
        private readonly ITwitterClient _tweetinviTestClient;

        public TimelineIntegrationTests(ITestOutputHelper logger)
        {
            _logger = logger;

            _logger.WriteLine(DateTime.Now.ToLongTimeString());

            _tweetinviApiClient = new TwitterClient(IntegrationTestConfig.TweetinviApiCredentials);
            _tweetinviTestClient = new TwitterClient(IntegrationTestConfig.TweetinviTestCredentials);

            TweetinviEvents.QueryBeforeExecute += (sender, args) => { _logger.WriteLine(args.Url); };
        }

        [Fact]
        public async Task RunAllAccountTests()
        {
            if (!IntegrationTestConfig.ShouldRunIntegrationTests)
                return;

            _logger.WriteLine($"Starting {nameof(RetweetsOfMeTimeline)}");
            await RetweetsOfMeTimeline().ConfigureAwait(false);
            _logger.WriteLine($"{nameof(RetweetsOfMeTimeline)} succeeded");
        }

        [Fact]
        public async Task RetweetsOfMeTimeline()
        {
            if (!IntegrationTestConfig.ShouldRunIntegrationTests)
                return;

            var tweet1 = await _tweetinviTestClient.Tweets.PublishTweet("tweet 1!");
            var tweet2 = await _tweetinviTestClient.Tweets.PublishTweet("tweet 2");

            await _tweetinviApiClient.Tweets.PublishRetweet(tweet1);
            await _tweetinviApiClient.Tweets.PublishRetweet(tweet2);

            var iterator = _tweetinviTestClient.Timeline.GetRetweetsOfMeTimelineIterator(new GetRetweetsOfMeTimelineParameters
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
            Assert.True(retweets.Select(x => x.Id).ToArray().ContainsSameObjectsAs(new[] { tweet1.Id, tweet2.Id }));
        }
    }
}