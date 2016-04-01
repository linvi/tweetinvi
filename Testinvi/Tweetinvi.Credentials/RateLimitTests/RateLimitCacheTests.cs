using FakeItEasy;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Testinvi.Helpers;
using Tweetinvi.Core.Authentication;
using Tweetinvi.Core.Interfaces.Credentials;
using Tweetinvi.Core.Interfaces.WebLogic;
using Tweetinvi.Credentials.RateLimit;

namespace Testinvi.Tweetinvi.Credentials.RateLimitTests
{
    [TestClass]
    public class RateLimitCacheTests
    {
        private const string TEST_QUERY = "I like cranes!";

        private FakeClassBuilder<RateLimitCache> _fakeBuilder;

        private ITwitterCredentials _credentials;
        private ITwitterCredentials _credentials2;
        private ICredentialsRateLimits _credentialsRateLimits;
        private ICredentialsRateLimits _credentialsRateLimits2; 

        [TestInitialize]
        public void Itinialize()
        {
            _fakeBuilder = new FakeClassBuilder<RateLimitCache>();

            _credentials = A.Fake<ITwitterCredentials>();
            _credentials2 = A.Fake<ITwitterCredentials>();
            _credentialsRateLimits = A.Fake<ICredentialsRateLimits>();
            _credentialsRateLimits2 = A.Fake<ICredentialsRateLimits>();
        }

        [TestMethod]
        public void GetTokenRateLimits_LimitsAreNotInCache_ReturnsNull()
        {
            // Arrange
            var rateLimitCache = CreateRateLimitCache();

            // Act
            var result = rateLimitCache.GetCredentialsRateLimits(_credentials);

            // Assert
            Assert.IsNull(result);
        }

        [TestMethod]
        public void GetTokenRateLimits_LimitsAreInCache_ReturnsLimits()
        {
            //// Arrange
            var rateLimitCache = CreateRateLimitCache();
            rateLimitCache.RefreshEntry(_credentials, _credentialsRateLimits);
            rateLimitCache.RefreshEntry(_credentials2, _credentialsRateLimits2);

            // Act
            var result = rateLimitCache.GetCredentialsRateLimits(_credentials);
            var result2 = rateLimitCache.GetCredentialsRateLimits(_credentials2);

            // Assert
            Assert.AreEqual(result, _credentialsRateLimits);
            Assert.AreEqual(result2, _credentialsRateLimits2);
        }

        [TestMethod]
        public void Refresh_EntryIsUpdated()
        {
            // Arrange
            var rateLimitCache = CreateRateLimitCache();
            rateLimitCache.RefreshEntry(_credentials, _credentialsRateLimits);

            // Act
            rateLimitCache.RefreshEntry(_credentials, _credentialsRateLimits2);

            // Assert
            var result = rateLimitCache.GetCredentialsRateLimits(_credentials);

            Assert.AreEqual(result, _credentialsRateLimits2);
        }

        [TestMethod]
        public void Clear_LimitsIsRemoved()
        {

            // Arrange
            var rateLimitCache = CreateRateLimitCache();
            rateLimitCache.RefreshEntry(_credentials, _credentialsRateLimits);
            rateLimitCache.RefreshEntry(_credentials2, _credentialsRateLimits2);

            // Act
            rateLimitCache.Clear(_credentials);

            // Assert
            var result = rateLimitCache.GetCredentialsRateLimits(_credentials);
            var result2 = rateLimitCache.GetCredentialsRateLimits(_credentials2);

            Assert.IsNull(result);
            Assert.AreEqual(result2, _credentialsRateLimits2);
        }

        [TestMethod]
        public void ClearAll_LimitsAreRemoved()
        {
            // Arrange
            var rateLimitCache = CreateRateLimitCache();
            rateLimitCache.RefreshEntry(_credentials, _credentialsRateLimits);
            rateLimitCache.RefreshEntry(_credentials2, _credentialsRateLimits2);

            // Act
            rateLimitCache.ClearAll();

            // Assert
            var result = rateLimitCache.GetCredentialsRateLimits(_credentials);
            var result2 = rateLimitCache.GetCredentialsRateLimits(_credentials2);

            Assert.IsNull(result);
            Assert.IsNull(result2);
        }

       
        private RateLimitCache CreateRateLimitCache()
        {
            return _fakeBuilder.GenerateClass();
        }
    }
}
