using System;
using System.Threading.Tasks;
using FakeItEasy;
using Tweetinvi;
using Tweetinvi.Core.RateLimit;
using Tweetinvi.Core.Web;
using Tweetinvi.Injectinvi;
using Tweetinvi.Models;
using Tweetinvi.Parameters.HelpClient;
using Xunit;
using Xunit.Abstractions;
using xUnitinvi.TestHelpers;

namespace xUnitinvi.IntegrationTests
{
    public class RateLimitsIntegrationTests
    {
        private readonly ITestOutputHelper _logger;

        public RateLimitsIntegrationTests(ITestOutputHelper logger)
        {
            _logger = logger;
            _logger.WriteLine(DateTime.Now.ToLongTimeString());

            TweetinviEvents.QueryBeforeExecute += (sender, args) => { _logger.WriteLine(args.Url); };
        }

        [Fact]
        public async Task GetRateLimits()
        {
            TwitterAccessorStub twitterAccessorStub = null;

            var container = new AutofacContainer();
            container.BeforeRegistrationCompletes += (sender, args) =>
            {
                var twitterAccessor = TweetinviContainer.Resolve<ITwitterAccessor>();
                twitterAccessorStub = new TwitterAccessorStub(twitterAccessor);

                args.TweetinviContainer.RegisterInstance(typeof(ITwitterAccessor), twitterAccessorStub);
            };
            container.Initialize();

            var client = new TwitterClient(IntegrationTestConfig.TweetinviTest.Credentials, new TwitterClientParameters
            {
                Container = container
            });

            // act
            var firstApplicationRateLimits = await client.RateLimits.GetRateLimits(RateLimitsSource.TwitterApiOnly);
            var rateLimits = await client.RateLimits.GetRateLimits();
            var fromCacheLimits = await client.RateLimits.GetRateLimits();

            // assert
            A.CallTo(() => twitterAccessorStub.FakedObject.ExecuteRequest<ICredentialsRateLimits>(It.IsAny<ITwitterRequest>()))
                .MustHaveHappenedTwiceExactly();

            Assert.Equal(firstApplicationRateLimits.ApplicationRateLimitStatusLimit.Remaining, rateLimits.ApplicationRateLimitStatusLimit.Remaining + 1);
            Assert.Same(rateLimits, fromCacheLimits);
        }

        [Fact]
        public async Task GetRateLimits_FromCacheOnly_GetResultsFromCache()
        {
            var fakeRateLimitCache = A.Fake<IRateLimitCache>();
            var creds = A.Fake<ITwitterCredentials>();
            var expectedRateLimits = A.Fake<ICredentialsRateLimits>();
            var client = new TwitterClient(creds, new TwitterClientParameters
            {
                RateLimitCache = fakeRateLimitCache
            });

            A.CallTo(() => fakeRateLimitCache.GetCredentialsRateLimits(client.Credentials))
                .Returns(expectedRateLimits);

            // act
            var rateLimits = await client.RateLimits.GetRateLimits(RateLimitsSource.CacheOnly);

            // arrange
            Assert.Same(rateLimits, expectedRateLimits);
        }

        [Fact]
        public async Task GetRateLimits_FromTwitterApiOnly_GetsResultFromTwitterAccessor()
        {
            var expectedRateLimits = A.Fake<ICredentialsRateLimits>();
            var twitterAccessor = A.Fake<ITwitterAccessor>();

            var container = new AutofacContainer();
            container.BeforeRegistrationCompletes += (sender, args) =>
            {
                container.RegisterInstance(typeof(ITwitterAccessor), twitterAccessor);
            };
            container.Initialize();

            var client = new TwitterClient(IntegrationTestConfig.TweetinviTest.Credentials, new TwitterClientParameters
            {
                Container = container
            });

            A.CallTo(() => twitterAccessor.ExecuteRequest<ICredentialsRateLimits>(It.IsAny<ITwitterRequest>()))
                .Returns(new TwitterResult<ICredentialsRateLimits> { DataTransferObject = expectedRateLimits });

            // act
            var rateLimits = await client.RateLimits.GetRateLimits(RateLimitsSource.TwitterApiOnly);

            // arrange
            Assert.Same(rateLimits, expectedRateLimits);
        }

        [Fact]
        public async Task GetRateLimits_FromCacheOrTwitterApi_GetsFirstFromTwitterApiThenFromCache()
        {
            var fakeRateLimitCache = A.Fake<IRateLimitCache>();
            var twitterApiRateLimits = A.Fake<ICredentialsRateLimits>();
            var cacheRateLimits = A.Fake<ICredentialsRateLimits>();
            var twitterAccessor = A.Fake<ITwitterAccessor>();

            var container = new AutofacContainer();
            container.BeforeRegistrationCompletes += (sender, args) =>
            {
                container.RegisterInstance(typeof(ITwitterAccessor), twitterAccessor);
            };
            container.Initialize();


            var client = new TwitterClient(IntegrationTestConfig.TweetinviTest.Credentials, new TwitterClientParameters
            {
                Container = container,
                RateLimitCache = fakeRateLimitCache
            });

            A.CallTo(() => fakeRateLimitCache.GetCredentialsRateLimits(client.Credentials)).Returns(Task.FromResult<ICredentialsRateLimits>(null));
            A.CallTo(() => fakeRateLimitCache.RefreshEntry(client.Credentials, twitterApiRateLimits))
                .ReturnsLazily(() =>
                {
                    // we use sequence here as `RefreshCredentialsRateLimits` is calling GetCredentialsRateLimits right after having put it in the cache
                    A.CallTo(() => fakeRateLimitCache.GetCredentialsRateLimits(client.Credentials))
                       .ReturnsNextFromSequence(twitterApiRateLimits, cacheRateLimits);
                    return Task.CompletedTask;
                });

            A.CallTo(() => twitterAccessor.ExecuteRequest<ICredentialsRateLimits>(It.IsAny<ITwitterRequest>()))
                .Returns(new TwitterResult<ICredentialsRateLimits> { DataTransferObject = twitterApiRateLimits });

            // act
            var rateLimits = await client.RateLimits.GetRateLimits(RateLimitsSource.CacheOrTwitterApi);
            var rateLimitsThatShouldComeCache = await client.RateLimits.GetRateLimits(RateLimitsSource.CacheOrTwitterApi);

            // arrange
            Assert.Same(rateLimits, twitterApiRateLimits);
            Assert.Same(rateLimitsThatShouldComeCache, cacheRateLimits);
        }
    }
}