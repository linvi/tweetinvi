using FakeItEasy;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Testinvi.Helpers;
using Tweetinvi.Core.Credentials;
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
        private ITokenRateLimits _tokenRateLimits;
        private ITokenRateLimits _tokenRateLimits2; 

        [TestInitialize]
        public void Itinialize()
        {
            _fakeBuilder = new FakeClassBuilder<RateLimitCache>();

            _credentials = A.Fake<ITwitterCredentials>();
            _credentials2 = A.Fake<ITwitterCredentials>();
            _tokenRateLimits = A.Fake<ITokenRateLimits>();
            _tokenRateLimits2 = A.Fake<ITokenRateLimits>();
        }

        [TestMethod]
        public void GetTokenRateLimits_LimitsAreNotInCache_ReturnsNull()
        {
            // Arrange
            var rateLimitCache = CreateRateLimitCache();

            // Act
            var result = rateLimitCache.GetTokenRateLimits(_credentials);

            // Assert
            Assert.IsNull(result);
        }

        [TestMethod]
        public void GetTokenRateLimits_LimitsAreInCache_ReturnsLimits()
        {
            //// Arrange
            var rateLimitCache = CreateRateLimitCache();
            rateLimitCache.RefreshEntry(_credentials, _tokenRateLimits);
            rateLimitCache.RefreshEntry(_credentials2, _tokenRateLimits2);

            // Act
            var result = rateLimitCache.GetTokenRateLimits(_credentials);
            var result2 = rateLimitCache.GetTokenRateLimits(_credentials2);

            // Assert
            Assert.AreEqual(result, _tokenRateLimits);
            Assert.AreEqual(result2, _tokenRateLimits2);
        }

        [TestMethod]
        public void Refresh_EntryIsUpdated()
        {
            // Arrange
            var rateLimitCache = CreateRateLimitCache();
            rateLimitCache.RefreshEntry(_credentials, _tokenRateLimits);

            // Act
            rateLimitCache.RefreshEntry(_credentials, _tokenRateLimits2);

            // Assert
            var result = rateLimitCache.GetTokenRateLimits(_credentials);

            Assert.AreEqual(result, _tokenRateLimits2);
        }

        [TestMethod]
        public void Clear_LimitsIsRemoved()
        {

            // Arrange
            var rateLimitCache = CreateRateLimitCache();
            rateLimitCache.RefreshEntry(_credentials, _tokenRateLimits);
            rateLimitCache.RefreshEntry(_credentials2, _tokenRateLimits2);

            // Act
            rateLimitCache.Clear(_credentials);

            // Assert
            var result = rateLimitCache.GetTokenRateLimits(_credentials);
            var result2 = rateLimitCache.GetTokenRateLimits(_credentials2);

            Assert.IsNull(result);
            Assert.AreEqual(result2, _tokenRateLimits2);
        }

        [TestMethod]
        public void ClearAll_LimitsAreRemoved()
        {
            // Arrange
            var rateLimitCache = CreateRateLimitCache();
            rateLimitCache.RefreshEntry(_credentials, _tokenRateLimits);
            rateLimitCache.RefreshEntry(_credentials2, _tokenRateLimits2);

            // Act
            rateLimitCache.ClearAll();

            // Assert
            var result = rateLimitCache.GetTokenRateLimits(_credentials);
            var result2 = rateLimitCache.GetTokenRateLimits(_credentials2);

            Assert.IsNull(result);
            Assert.IsNull(result2);
        }

       
        private RateLimitCache CreateRateLimitCache()
        {
            return _fakeBuilder.GenerateClass();
        }
    }
}
