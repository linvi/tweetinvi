using System.Threading.Tasks;
using FakeItEasy;
using Tweetinvi;
using Tweetinvi.Core.DTO;
using Tweetinvi.Core.Models;
using Tweetinvi.Core.RateLimit;
using Tweetinvi.Core.Web;
using Tweetinvi.Models;
using Tweetinvi.Parameters;
using Xunit;
using Xunit.Abstractions;
using xUnitinvi.TestHelpers;
using TweetinviContainer = Tweetinvi.Injectinvi.TweetinviContainer;

// ReSharper disable AccessToDisposedClosure

namespace xUnitinvi.Integration
{
    public class RateLimitIntegrationTests : TweetinviTest
    {
        public RateLimitIntegrationTests(ITestOutputHelper logger) : base(logger)
        {
        }

        [Fact]
        public async Task GetRateLimits_FromCacheOnly_GetResultsFromCacheAsync()
        {
            var fakeRateLimitCache = A.Fake<IRateLimitCache>();
            var creds = A.Fake<ITwitterCredentials>();
            var expectedRateLimits = A.Fake<ICredentialsRateLimits>();

            var client = new TwitterClient(creds, new TwitterClientParameters
            {
                RateLimitCache = fakeRateLimitCache
            });

            A.CallTo(() => fakeRateLimitCache.GetCredentialsRateLimitsAsync(client.Credentials))
                .Returns(expectedRateLimits);

            // act
            var rateLimits = await client.RateLimits.GetRateLimitsAsync(RateLimitsSource.CacheOnly);

            // arrange
            Assert.Same(rateLimits, expectedRateLimits);
        }

        [Fact]
        public async Task GetRateLimits_FromTwitterApiOnly_GetsResultFromTwitterAccessorAsync()
        {
            var expectedRateLimits = A.Fake<CredentialsRateLimitsDTO>();
            var twitterAccessor = A.Fake<ITwitterAccessor>();

            var container = new TweetinviContainer();
            container.BeforeRegistrationCompletes += (sender, args) => { container.RegisterInstance(typeof(ITwitterAccessor), twitterAccessor); };
            container.Initialize();

            var fakeCredentials = new TwitterCredentials("consumerKey", "consumerSecret", "accessToken", "accessTokenSecret");
            var client = new TwitterClient(fakeCredentials, new TwitterClientParameters
            {
                Container = container
            });
            A.CallTo(() => twitterAccessor.ExecuteRequestAsync<CredentialsRateLimitsDTO>(It.IsAny<ITwitterRequest>()))
                .Returns(new TwitterResult<CredentialsRateLimitsDTO> { DataTransferObject = expectedRateLimits });

            // act
            var rateLimits = await client.RateLimits.GetRateLimitsAsync(RateLimitsSource.TwitterApiOnly);

            // arrange
            Assert.Same(rateLimits.CredentialsRateLimitsDTO, expectedRateLimits);
        }

        [Fact]
        public async Task GetRateLimits_FromCacheOrTwitterApi_GetsFirstFromTwitterApiThenFromCacheAsync()
        {
            var fakeRateLimitCache = A.Fake<IRateLimitCache>();
            var twitterApiRateLimitsDTO = A.Fake<CredentialsRateLimitsDTO>();
            var twitterApiRateLimits = new CredentialsRateLimits(twitterApiRateLimitsDTO);
            var cacheRateLimits = A.Fake<ICredentialsRateLimits>();
            var twitterAccessor = A.Fake<ITwitterAccessor>();


            var container = new TweetinviContainer();
            container.BeforeRegistrationCompletes += (sender, args) => { container.RegisterInstance(typeof(ITwitterAccessor), twitterAccessor); };
            container.Initialize();

            var fakeCredentials = new TwitterCredentials("consumerKey", "consumerSecret", "accessToken", "accessTokenSecret");
            var client = new TwitterClient(fakeCredentials, new TwitterClientParameters
            {
                Container = container,
                RateLimitCache = fakeRateLimitCache
            });

            A.CallTo(() => fakeRateLimitCache.GetCredentialsRateLimitsAsync(client.Credentials)).Returns(Task.FromResult<ICredentialsRateLimits>(null));
            A.CallTo(() => fakeRateLimitCache.RefreshEntryAsync(client.Credentials, A<ICredentialsRateLimits>.That.Matches(x => x.CredentialsRateLimitsDTO == twitterApiRateLimitsDTO)))
                .ReturnsLazily(() =>
                {
                    // we use sequence here as `RefreshCredentialsRateLimits` is calling GetCredentialsRateLimits right after having put it in the cache
                    A.CallTo(() => fakeRateLimitCache.GetCredentialsRateLimitsAsync(client.Credentials))
                        .ReturnsNextFromSequence(twitterApiRateLimits, cacheRateLimits);
                    return Task.CompletedTask;
                });

            A.CallTo(() => twitterAccessor.ExecuteRequestAsync<CredentialsRateLimitsDTO>(It.IsAny<ITwitterRequest>()))
                .Returns(new TwitterResult<CredentialsRateLimitsDTO> { DataTransferObject = twitterApiRateLimitsDTO });

            // act
            var rateLimits = await client.RateLimits.GetRateLimitsAsync(RateLimitsSource.CacheOrTwitterApi);
            var rateLimitsThatShouldComeCache = await client.RateLimits.GetRateLimitsAsync(RateLimitsSource.CacheOrTwitterApi);

            // arrange
            Assert.Same(rateLimits.CredentialsRateLimitsDTO, twitterApiRateLimitsDTO);
            Assert.Same(rateLimitsThatShouldComeCache, cacheRateLimits);
        }
    }
}