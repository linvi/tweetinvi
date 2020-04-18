using System;
using System.Threading.Tasks;
using FakeItEasy;
using Tweetinvi;
using Tweetinvi.Controllers.Properties;
using Tweetinvi.Core.DTO;
using Tweetinvi.Core.Helpers;
using Tweetinvi.Core.Web;
using Tweetinvi.Models;
using Tweetinvi.Parameters;
using Xunit;
using Xunit.Abstractions;
using Xunit.Extensions.Ordering;
using xUnitinvi.TestHelpers;
using TweetinviContainer = Tweetinvi.Injectinvi.TweetinviContainer;

namespace xUnitinvi.EndToEnd
{
    [Collection("EndToEndTests"), Order(10)]
    public class RateLimitsEndToEndTests : TweetinviTest
    {
        public RateLimitsEndToEndTests(ITestOutputHelper logger) : base(logger)
        {
        }

        [Fact]
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

            TweetinviEvents.SubscribeToClientEvents(client);

            // act
            var firstApplicationRateLimits = await client.RateLimits.GetRateLimits(RateLimitsSource.TwitterApiOnly);
            var rateLimits = await client.RateLimits.GetRateLimits();
            var fromCacheLimits = await client.RateLimits.GetRateLimits();

            // assert
            A.CallTo(() => twitterAccessorSpy.FakedObject.ExecuteRequest<CredentialsRateLimitsDTO>(It.IsAny<ITwitterRequest>()))
                .MustHaveHappenedTwiceExactly();

            Assert.Equal(firstApplicationRateLimits.ApplicationRateLimitStatusLimit.Remaining, rateLimits.ApplicationRateLimitStatusLimit.Remaining + 1);
            Assert.Same(rateLimits, fromCacheLimits);
        }

        [Fact]
        public async Task GetEndpointRateLimit()
        {
            if (!EndToEndTestConfig.ShouldRunEndToEndTests)
                return;

            var parameters = new TwitterClientParameters();

            parameters.BeforeRegistrationCompletes += (sender, args) =>
            {
                args.TweetinviContainer.RegisterDecorator<TwitterAccessorSpy, ITwitterAccessor>();
            };

            var client = new TwitterClient(EndToEndTestConfig.TweetinviTest.Credentials, parameters);
            TweetinviEvents.SubscribeToClientEvents(client);

            var twitterAccessorSpy = client.CreateTwitterExecutionContext().Container.Resolve<ITwitterAccessor>() as TwitterAccessorSpy;
            client.ClientSettings.RateLimitTrackerMode = RateLimitTrackerMode.TrackOnly;

            // act
            var firstApplicationRateLimits = await client.RateLimits.GetEndpointRateLimit("https://api.twitter.com/1.1/statuses/home_timeline.json", RateLimitsSource.TwitterApiOnly);
            var rateLimits = await client.RateLimits.GetEndpointRateLimit("https://api.twitter.com/1.1/statuses/home_timeline.json");
            await client.Timelines.GetHomeTimeline();
            var fromCacheLimits = await client.RateLimits.GetEndpointRateLimit("https://api.twitter.com/1.1/statuses/home_timeline.json");

            // assert
            A.CallTo(() => twitterAccessorSpy.FakedObject.ExecuteRequest<CredentialsRateLimitsDTO>(It.IsAny<ITwitterRequest>()))
                .MustHaveHappenedTwiceExactly();

            Assert.Equal(firstApplicationRateLimits.Remaining, fromCacheLimits.Remaining + 1);
            Assert.Same(rateLimits, fromCacheLimits);

            // act
            await client.RateLimits.ClearRateLimitCache();
            await client.RateLimits.GetEndpointRateLimit("https://api.twitter.com/1.1/statuses/home_timeline.json");

            A.CallTo(() => twitterAccessorSpy.FakedObject.ExecuteRequest<CredentialsRateLimitsDTO>(It.IsAny<ITwitterRequest>()))
                .MustHaveHappened(3, Times.Exactly);
        }

        [Fact, Order(10)]
        public async Task RateLimitAwaiter()
        {
            if (!EndToEndTestConfig.ShouldRunEndToEndTests || !EndToEndTestConfig.ShouldRunRateLimitHungryTests)
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
            TweetinviEvents.SubscribeToClientEvents(client);

            client.ClientSettings.RateLimitTrackerMode = RateLimitTrackerMode.TrackAndAwait;

            // act - assert
            var rateLimits = await client.RateLimits.GetEndpointRateLimit(Resources.Timeline_GetHomeTimeline);
            var rateLimitsRemaining = rateLimits.Remaining;

            await client.RateLimits.WaitForQueryRateLimit(Resources.Timeline_GetHomeTimeline);

            for (var i = 0; i < rateLimitsRemaining; ++i)
            {
                var timelineIterator = client.Timelines.GetHomeTimelineIterator();
                await timelineIterator.NextPage();
            }

            A.CallTo(() => taskDelayer.Delay(It.IsAny<TimeSpan>())).MustNotHaveHappened();

            await client.RateLimits.WaitForQueryRateLimit(Resources.Timeline_GetHomeTimeline);
            A.CallTo(() => taskDelayer.Delay(It.IsAny<TimeSpan>())).MustHaveHappenedOnceExactly();

            try
            {
                var timelineIterator = client.Timelines.GetHomeTimelineIterator();
                await timelineIterator.NextPage();
            }
            // ReSharper disable once CC0004
            catch (Exception)
            {
                // we expect to throw as we are mocking the task delayer
                A.CallTo(() => taskDelayer.Delay(It.IsAny<TimeSpan>())).MustHaveHappenedTwiceExactly();
                return;
            }

            throw new InvalidOperationException("Should have failed ealier");
        }
    }
}