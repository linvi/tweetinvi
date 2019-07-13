using System.Threading.Tasks;
using FakeItEasy;
using FakeItEasy.ExtensionSyntax.Full;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Testinvi.Helpers;
using Tweetinvi.Core.Credentials;
using Tweetinvi.Core.RateLimit;
using Tweetinvi.Credentials.RateLimit;
using Tweetinvi.Models;

namespace Testinvi.Tweetinvi.Credentials.RateLimitTests
{
    [TestClass]
    public class RateLimitUpdaterTests
    {
        private const string TEST_QUERY = "This is a test!";

        private FakeClassBuilder<RateLimitUpdater> _fakeBuilder;
        private Fake<IRateLimitCacheManager> _fakeRateLimitCacheManager;
        private Fake<ICredentialsAccessor> _fakeCredentialsAccessor;

        private IEndpointRateLimit _endpointRateLimit;
        private ITwitterCredentials _credentials;

        [TestInitialize]
         public void TestInitialize()
        {
            _fakeBuilder = new FakeClassBuilder<RateLimitUpdater>();
            _fakeRateLimitCacheManager = _fakeBuilder.GetFake<IRateLimitCacheManager>();
            _fakeCredentialsAccessor = _fakeBuilder.GetFake<ICredentialsAccessor>();

            InitializeData();

            _fakeRateLimitCacheManager.CallsTo(x => x.GetQueryRateLimit(TEST_QUERY, _credentials)).Returns(_endpointRateLimit);
            _fakeCredentialsAccessor.CallsTo(x => x.CurrentThreadCredentials).Returns(_credentials);
        }

        [TestMethod]
         public async Task QueryExecuted_QueryCannotBeFound_DoesNothing()
        {
            // Arrange
            var cacheUpdater = CreateRateLimitUpdater();
            _fakeRateLimitCacheManager.CallsTo(x => x.GetQueryRateLimit(TEST_QUERY, _credentials)).Returns(Task.FromResult((IEndpointRateLimit)null));

            // Act
            await cacheUpdater.QueryExecuted(TEST_QUERY);

            // Assert
            Assert.AreEqual(_endpointRateLimit.Remaining, 5);
        }

        [TestMethod]
         public async Task QueryExecutedWithCredentials_QueryCannotBeFound_DoesNothing()
        {
            // Arrange
            var cacheUpdater = CreateRateLimitUpdater();
            _fakeRateLimitCacheManager.CallsTo(x => x.GetQueryRateLimit(TEST_QUERY, _credentials)).Returns(Task.FromResult((IEndpointRateLimit)null));

            // Act
            await cacheUpdater.QueryExecuted(TEST_QUERY, _credentials);

            // Assert
            Assert.AreEqual(_endpointRateLimit.Remaining, 5);
        }

        [TestMethod]
         public async Task QueryExecutedWithCredentials_QueryFoundAndRemainingGreaterThan0_Subtract1()
        {
            // Arrange
            var cacheUpdater = CreateRateLimitUpdater();

            // Act
            await cacheUpdater.QueryExecuted(TEST_QUERY, _credentials);

            // Assert
            Assert.AreEqual(_endpointRateLimit.Remaining, 4);
        }

        [TestMethod]
         public async Task QueryExecutedWithCredentials_QueryFoundAndRemainingGreaterThan3_NumberOfRequestIs3_Subtract3()
        {
            // Arrange
            var cacheUpdater = CreateRateLimitUpdater();

            // Act
            await cacheUpdater.QueryExecuted(TEST_QUERY, _credentials, 3);

            // Assert
            Assert.AreEqual(_endpointRateLimit.Remaining, 2);
        }

        [TestMethod]
         public async Task QueryExecutedWithCredentials_QueryFoundAndRemainingIs2_NumberOfRequestIs3_Subtract2()
        {
            // Arrange
            var cacheUpdater = CreateRateLimitUpdater();
            _endpointRateLimit.CallsTo(x => x.Remaining).Returns(2);

            // Act
            await cacheUpdater.QueryExecuted(TEST_QUERY, _credentials, 3);

            // Assert
            Assert.AreEqual(_endpointRateLimit.Remaining, 0);
        }

        [TestMethod]
         public async Task QueryExecutedWithCredentials_QueryFoundAndRemainingIs0_DoesNothing()
        {
            // Arrange
            var cacheUpdater = CreateRateLimitUpdater();
            _endpointRateLimit.CallsTo(x => x.Remaining).Returns(0);

            // Act
            await cacheUpdater.QueryExecuted(TEST_QUERY, _credentials);

            // Assert
            Assert.AreEqual(_endpointRateLimit.Remaining, 0);
        }

        [TestMethod]
         public async Task ClearRateLimitsForQuery_RemainingRateLimitsIsNowEmpty()
        {
            // Arrange
            var rateLimitUpdater = CreateRateLimitUpdater();

            // Act
            await rateLimitUpdater.ClearRateLimitsForQuery(TEST_QUERY);

            // Assert
            Assert.AreEqual(_endpointRateLimit.Remaining, 0);
        }

        private void InitializeData()
        {
            _credentials = A.Fake<ITwitterCredentials>();
            _endpointRateLimit = A.Fake<IEndpointRateLimit>();
            _endpointRateLimit.CallsTo(x => x.Remaining).Returns(5);
        }

        private RateLimitUpdater CreateRateLimitUpdater()
        {
            return _fakeBuilder.GenerateClass();
        }
    }
}