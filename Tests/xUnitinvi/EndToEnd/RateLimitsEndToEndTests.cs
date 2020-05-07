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
        public async Task GetRateLimitsAsync()
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
            var firstApplicationRateLimits = await client.RateLimits.GetRateLimitsAsync(RateLimitsSource.TwitterApiOnly);
            var rateLimits = await client.RateLimits.GetRateLimitsAsync();
            var fromCacheLimits = await client.RateLimits.GetRateLimitsAsync();

            // assert
            A.CallTo(() => twitterAccessorSpy.FakedObject.ExecuteRequestAsync<CredentialsRateLimitsDTO>(It.IsAny<ITwitterRequest>()))
                .MustHaveHappenedTwiceExactly();

            Assert.Equal(firstApplicationRateLimits.ApplicationRateLimitStatusLimit.Remaining, rateLimits.ApplicationRateLimitStatusLimit.Remaining + 1);
            Assert.Same(rateLimits, fromCacheLimits);
        }

        [Fact]
        public async Task GetEndpointRateLimitAsync()
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
            var firstApplicationRateLimits = await client.RateLimits.GetEndpointRateLimitAsync("https://api.twitter.com/1.1/statuses/home_timeline.json", RateLimitsSource.TwitterApiOnly);
            var rateLimits = await client.RateLimits.GetEndpointRateLimitAsync("https://api.twitter.com/1.1/statuses/home_timeline.json");
            await client.Timelines.GetHomeTimelineAsync();
            var fromCacheLimits = await client.RateLimits.GetEndpointRateLimitAsync("https://api.twitter.com/1.1/statuses/home_timeline.json");

            // assert
            A.CallTo(() => twitterAccessorSpy.FakedObject.ExecuteRequestAsync<CredentialsRateLimitsDTO>(It.IsAny<ITwitterRequest>()))
                .MustHaveHappenedTwiceExactly();

            Assert.Equal(firstApplicationRateLimits.Remaining, fromCacheLimits.Remaining + 1);
            Assert.Same(rateLimits, fromCacheLimits);

            // act
            await client.RateLimits.ClearRateLimitCacheAsync();
            await client.RateLimits.GetEndpointRateLimitAsync("https://api.twitter.com/1.1/statuses/home_timeline.json");

            A.CallTo(() => twitterAccessorSpy.FakedObject.ExecuteRequestAsync<CredentialsRateLimitsDTO>(It.IsAny<ITwitterRequest>()))
                .MustHaveHappened(3, Times.Exactly);
        }

        [Fact, Order(10)]
        public async Task RateLimitAwaiterAsync()
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
            var rateLimits = await client.RateLimits.GetEndpointRateLimitAsync(Resources.Timeline_GetHomeTimeline);
            var rateLimitsRemaining = rateLimits.Remaining;

            await client.RateLimits.WaitForQueryRateLimitAsync(Resources.Timeline_GetHomeTimeline);

            for (var i = 0; i < rateLimitsRemaining; ++i)
            {
                var timelineIterator = client.Timelines.GetHomeTimelineIterator();
                await timelineIterator.NextPageAsync();
            }

            A.CallTo(() => taskDelayer.Delay(It.IsAny<TimeSpan>())).MustNotHaveHappened();

            await client.RateLimits.WaitForQueryRateLimitAsync(Resources.Timeline_GetHomeTimeline);
            A.CallTo(() => taskDelayer.Delay(It.IsAny<TimeSpan>())).MustHaveHappenedOnceExactly();

            try
            {
                var timelineIterator = client.Timelines.GetHomeTimelineIterator();
                await timelineIterator.NextPageAsync();
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