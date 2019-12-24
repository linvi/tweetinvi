using System;
using System.Threading.Tasks;
using FakeItEasy;
using Tweetinvi;
using Tweetinvi.Core.Helpers;
using Tweetinvi.Core.Web;
using Tweetinvi.Models;
using Tweetinvi.Parameters.HelpClient;
using Xunit;
using Xunit.Abstractions;
using xUnitinvi.TestHelpers;
using TweetinviContainer = Tweetinvi.Injectinvi.TweetinviContainer;

namespace xUnitinvi.EndToEnd
{
    [Collection("EndToEndTests"), TestPriority(1)]
    [TestCaseOrderer()]
    public class RateLimitsEndToEndTests : TweetinviTest
    {
        public RateLimitsEndToEndTests(ITestOutputHelper logger) : base(logger)
        {
        }

        [Fact]
        public void Salut()
        {

        }

//        [Fact]
        public async Task GetRateLimits()
        {
            if (!EndToEndTestConfig.ShouldRunEndToEndTests)
                return;

            TwitterAccessorSpy twitterAccessorSpy = null;

            var container = new TweetinviContainer();
            container.BeforeRegistrationCompletes += (sender, args) =>
            {
                var twitterAccessor = Tweetinvi.TweetinviContainer.Resolve<ITwitterAccessor>();
                twitterAccessorSpy = new TwitterAccessorSpy(twitterAccessor);

                args.TweetinviContainer.RegisterInstance(typeof(ITwitterAccessor), twitterAccessorSpy);
            };
            container.Initialize();

            var client = new TwitterClient(EndToEndTestConfig.TweetinviTest.Credentials, new TwitterClientParameters
            {
                Container = container
            });

            // act
            var firstApplicationRateLimits = await client.RateLimits.GetRateLimits(RateLimitsSource.TwitterApiOnly);
            var rateLimits = await client.RateLimits.GetRateLimits();
            var fromCacheLimits = await client.RateLimits.GetRateLimits();

            // assert
            A.CallTo(() => twitterAccessorSpy.FakedObject.ExecuteRequest<ICredentialsRateLimits>(It.IsAny<ITwitterRequest>()))
                .MustHaveHappenedTwiceExactly();

            Assert.Equal(firstApplicationRateLimits.ApplicationRateLimitStatusLimit.Remaining, rateLimits.ApplicationRateLimitStatusLimit.Remaining + 1);
            Assert.Same(rateLimits, fromCacheLimits);
        }

//        [Fact]
        public async Task GetEndpointRateLimit()
        {
            if (!EndToEndTestConfig.ShouldRunEndToEndTests)
                return;

            TwitterAccessorSpy twitterAccessorSpy = null;

            var parameters = new TwitterClientParameters();

            parameters.BeforeRegistrationCompletes += (sender, args) =>
            {
                var container = args.TweetinviContainer;
                var twitterAccessor = args.TweetinviContainer.Resolve<ITwitterAccessor>();
                twitterAccessorSpy = new TwitterAccessorSpy(twitterAccessor);

                container.RegisterInstance(typeof(ITwitterAccessor), twitterAccessorSpy);
            };

            var client = new TwitterClient(EndToEndTestConfig.TweetinviTest.Credentials, parameters);

            client.ClientSettings.RateLimitTrackerMode = RateLimitTrackerMode.TrackOnly;

            // act
            var firstApplicationRateLimits = await client.RateLimits.GetEndpointRateLimit("https://api.twitter.com/1.1/statuses/home_timeline.json", RateLimitsSource.TwitterApiOnly);
            var rateLimits = await client.RateLimits.GetEndpointRateLimit("https://api.twitter.com/1.1/statuses/home_timeline.json");
            await client.Timeline.GetHomeTimelineIterator().MoveToNextPage();
            var fromCacheLimits = await client.RateLimits.GetEndpointRateLimit("https://api.twitter.com/1.1/statuses/home_timeline.json");

            // assert
            A.CallTo(() => twitterAccessorSpy.FakedObject.ExecuteRequest<ICredentialsRateLimits>(It.IsAny<ITwitterRequest>()))
                .MustHaveHappenedTwiceExactly();

            Assert.Equal(firstApplicationRateLimits.Remaining, fromCacheLimits.Remaining + 1);
            Assert.Same(rateLimits, fromCacheLimits);
        }

//        [Fact]
        public async Task RateLimitAwaiter()
        {
            if (!EndToEndTestConfig.ShouldRunEndToEndTests)
                return;

            var taskDelayer = A.Fake<ITaskDelayer>();

            var container = new TweetinviContainer();
            container.BeforeRegistrationCompletes += (sender, args) =>
            {
                container.RegisterInstance(typeof(ITaskDelayer), taskDelayer);
            };
            container.Initialize();

            var client = new TwitterClient(EndToEndTestConfig.TweetinviTest.Credentials, new TwitterClientParameters
            {
                Container = container
            });

            client.ClientSettings.RateLimitTrackerMode = RateLimitTrackerMode.TrackAndAwait;

            // act
            var rateLimits = await client.RateLimits.GetEndpointRateLimit("https://api.twitter.com/1.1/statuses/home_timeline.json").ConfigureAwait(false);
            var rateLimitsRemaining = rateLimits.Remaining;
            for (var i = 0; i < rateLimitsRemaining; ++i)
            {
                var timelineIterator = client.Timeline.GetHomeTimelineIterator();
                await timelineIterator.MoveToNextPage().ConfigureAwait(false);
            }

            A.CallTo(() => taskDelayer.Delay(It.IsAny<TimeSpan>())).MustNotHaveHappened();

            try
            {
                var timelineIterator = client.Timeline.GetHomeTimelineIterator();
                await timelineIterator.MoveToNextPage().ConfigureAwait(false);
            }
            // ReSharper disable once CC0004
            catch (Exception e)
            {
                // assert

                // we expect to throw as we are mocking the task delayer
                A.CallTo(() => taskDelayer.Delay(It.IsAny<TimeSpan>())).MustHaveHappened();
                return;
            }

            throw new InvalidOperationException("Should have failed ealier");
        }
    }
}