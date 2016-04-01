using FakeItEasy;
using FakeItEasy.ExtensionSyntax.Full;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Testinvi.Helpers;
using Tweetinvi.Core.Authentication;
using Tweetinvi.Core.Interfaces.Credentials;
using Tweetinvi.Core.Interfaces.RateLimit;
using Tweetinvi.Core.Interfaces.WebLogic;
using Tweetinvi.Credentials.RateLimit;

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
        public void QueryExecuted_QueryCannotBeFound_DoesNothing()
        {
            // Arrange
            var cacheUpdater = CreateRateLimitUpdater();
            _fakeRateLimitCacheManager.CallsTo(x => x.GetQueryRateLimit(TEST_QUERY, _credentials)).Returns(null);

            // Act
            cacheUpdater.QueryExecuted(TEST_QUERY);

            // Assert
            Assert.AreEqual(_endpointRateLimit.Remaining, 5);
        }

        [TestMethod]
        public void QueryExecutedWithCredentials_QueryCannotBeFound_DoesNothing()
        {
            // Arrange
            var cacheUpdater = CreateRateLimitUpdater();
            _fakeRateLimitCacheManager.CallsTo(x => x.GetQueryRateLimit(TEST_QUERY, _credentials)).Returns(null);

            // Act
            cacheUpdater.QueryExecuted(TEST_QUERY, _credentials);

            // Assert
            Assert.AreEqual(_endpointRateLimit.Remaining, 5);
        }

        [TestMethod]
        public void QueryExecutedWithCredentials_QueryFoundAndRemainingGreaterThan0_Substract1()
        {
            // Arrange
            var cacheUpdater = CreateRateLimitUpdater();

            // Act
            cacheUpdater.QueryExecuted(TEST_QUERY, _credentials);

            // Assert
            Assert.AreEqual(_endpointRateLimit.Remaining, 4);
        }

        [TestMethod]
        public void QueryExecutedWithCredentials_QueryFoundAndRemainingGreaterThan3_NumberOfRequestIs3_Substract3()
        {
            // Arrange
            var cacheUpdater = CreateRateLimitUpdater();

            // Act
            cacheUpdater.QueryExecuted(TEST_QUERY, _credentials, 3);

            // Assert
            Assert.AreEqual(_endpointRateLimit.Remaining, 2);
        }

        [TestMethod]
        public void QueryExecutedWithCredentials_QueryFoundAndRemainingIs2_NumberOfRequestIs3_Substract2()
        {
            // Arrange
            var cacheUpdater = CreateRateLimitUpdater();
            _endpointRateLimit.CallsTo(x => x.Remaining).Returns(2);

            // Act
            cacheUpdater.QueryExecuted(TEST_QUERY, _credentials, 3);

            // Assert
            Assert.AreEqual(_endpointRateLimit.Remaining, 0);
        }

        [TestMethod]
        public void QueryExecutedWithCredentials_QueryFoundAndRemainingIs0_DoesNothing()
        {
            // Arrange
            var cacheUpdater = CreateRateLimitUpdater();
            _endpointRateLimit.CallsTo(x => x.Remaining).Returns(0);

            // Act
            cacheUpdater.QueryExecuted(TEST_QUERY, _credentials);

            // Assert
            Assert.AreEqual(_endpointRateLimit.Remaining, 0);
        }

        [TestMethod]
        public void ClearRateLimitsForQuery_RemainingRateLimitsIsNowEmpty()
        {
            // Arrange
            var rateLimitUpdater = CreateRateLimitUpdater();

            // Act
            rateLimitUpdater.ClearRateLimitsForQuery(TEST_QUERY);

            // Assert
            Assert.AreEqual(_endpointRateLimit.Remaining, 0);
        }

        private void InitializeData()
        {
            _credentials = A.Fake<ITwitterCredentials>();
            _endpointRateLimit = A.Fake<IEndpointRateLimit>();
            _endpointRateLimit.CallsTo(x => x.Remaining).Returns(5);
        }

        public RateLimitUpdater CreateRateLimitUpdater()
        {
            return _fakeBuilder.GenerateClass();
        }
    }
}