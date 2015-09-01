using System;
using System.Linq;
using FakeItEasy;
using FakeItEasy.ExtensionSyntax.Full;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using Testinvi.Helpers;
using Testinvi.SetupHelpers;
using Tweetinvi.Core.Credentials;
using Tweetinvi.Core.Helpers;
using Tweetinvi.Core.Interfaces.Controllers;
using Tweetinvi.Core.Interfaces.Credentials;
using Tweetinvi.Core.Interfaces.Models;
using Tweetinvi.Core.Interfaces.RateLimit;
using Tweetinvi.Core.Interfaces.WebLogic;
using Tweetinvi.Credentials.RateLimit;
using HttpMethod = Tweetinvi.Core.Enum.HttpMethod;

namespace Testinvi.Tweetinvi.Credentials.RateLimitTests
{
    [TestClass]
    public class RateLimitCacheManagerTests
    {
        private const string TEST_QUERY = "I like testing cache!";

        private FakeClassBuilder<RateLimitCacheManager> _fakeBuilder;
        private Fake<IRateLimitCache> _fakeRateLimitCache;
        private Fake<IRateLimitHelper> _fakeRateLimitHelper;
        private Fake<ITwitterRequester> _fakeTwitterRequester;
        private Fake<IHelpQueryGenerator> _fakeHelpQueryGenerator;
        private Fake<IJsonObjectConverter> _fakeJsonObjectConverter;
        private Fake<ICredentialsAccessor> _fakeCredentialsAccessor;
        private Fake<ITwitterQueryFactory> _fakeTwitterQueryFactory;

        private ITwitterQuery _twitterQuery;
        private ITokenRateLimits _tokenRateLimits;
        private ITokenRateLimit _tokenRateLimit;
        private ITokenRateLimits _refreshedTokenRateLimits;
        private ITokenRateLimit _refreshedTokenRateLimit;
        private ITwitterCredentials _credentials;
        private ITokenRateLimits _tokenRateLimits2; 

        [TestInitialize]
        public void TestInitialize()
        {
            _fakeBuilder = new FakeClassBuilder<RateLimitCacheManager>();
            _fakeRateLimitCache = _fakeBuilder.GetFake<IRateLimitCache>();
            _fakeRateLimitHelper = _fakeBuilder.GetFake<IRateLimitHelper>();
            _fakeTwitterRequester = _fakeBuilder.GetFake<ITwitterRequester>();
            _fakeHelpQueryGenerator = _fakeBuilder.GetFake<IHelpQueryGenerator>();
            _fakeJsonObjectConverter = _fakeBuilder.GetFake<IJsonObjectConverter>();
            _fakeCredentialsAccessor = _fakeBuilder.GetFake<ICredentialsAccessor>();
            _fakeTwitterQueryFactory = _fakeBuilder.GetFake<ITwitterQueryFactory>();

            InitializeData();

            _fakeRateLimitHelper.CallsTo(x => x.GetTokenRateLimitFromQuery(TEST_QUERY, _tokenRateLimits)).Returns(_tokenRateLimit);
            _fakeRateLimitCache.CallsTo(x => x.GetTokenRateLimits(_credentials)).Returns(_tokenRateLimits);

            _fakeRateLimitCache.CallsTo(x => x.RefreshEntry(_credentials, _tokenRateLimits)).Invokes(() =>
            {
                _fakeRateLimitCache.CallsTo(x => x.GetTokenRateLimits(_credentials)).Returns(_refreshedTokenRateLimits);
                _fakeRateLimitHelper.CallsTo(x => x.GetTokenRateLimitFromQuery(TEST_QUERY, _refreshedTokenRateLimits)).Returns(_refreshedTokenRateLimit);
            });

            _fakeCredentialsAccessor.SetupPassThrough<ITokenRateLimits>();

            _fakeHelpQueryGenerator.CallsTo(x => x.GetCredentialsLimitsQuery()).Returns(TEST_QUERY);

            _fakeTwitterRequester.CallsTo(x => x.ExecuteQuery(_twitterQuery, null)).Returns(TEST_QUERY);
            _fakeJsonObjectConverter.CallsTo(x => x.DeserializeObject<ITokenRateLimits>(TEST_QUERY, It.IsAny<JsonConverter[]>())).ReturnsNextFromSequence(_tokenRateLimits, _tokenRateLimits2);

            _fakeTwitterQueryFactory.CallsTo(x => x.Create(TEST_QUERY, It.IsAny<HttpMethod>(), It.IsAny<ITwitterCredentials>())).Returns(_twitterQuery);
        }

        [TestMethod]
        public void GetQueryRateLimit_TokenRateLimitsIsNull_Refreshes()
        {
            // Arrange
            var cacheManager = CreateRateLimitCacheManager();

            _fakeRateLimitCache.CallsTo(x => x.GetTokenRateLimits(_credentials)).Returns(null);

            // Act
            var result = cacheManager.GetQueryRateLimit(TEST_QUERY, _credentials);

            // Assert
            Assert.AreEqual(result, _refreshedTokenRateLimit);
            _fakeRateLimitCache.CallsTo(x => x.RefreshEntry(It.IsAny<ITwitterCredentials>(), It.IsAny<ITokenRateLimits>())).MustHaveHappened(Repeated.Exactly.Once);
        }

        [TestMethod]
        public void GetQueryRateLimit_QueryCannotBeMatched_ReturnsNull()
        {
            // Arrange
            var cacheManager = CreateRateLimitCacheManager();

            _fakeRateLimitHelper.CallsTo(x => x.GetTokenRateLimitFromQuery(TEST_QUERY, _tokenRateLimits)).Returns(null);

            // Act
            var result = cacheManager.GetQueryRateLimit(TEST_QUERY, _credentials);

            // Assert
            Assert.IsNull(result);
            _fakeRateLimitCache.CallsTo(x => x.RefreshEntry(It.IsAny<ITwitterCredentials>(), It.IsAny<ITokenRateLimits>())).MustNotHaveHappened();
        }

        [TestMethod]
        public void GetQueryRateLimit_QueryReturnedValidTokenRateLimit_NoRefreshAndReturnsTheRateLimit()
        {
            // Arrange
            var cacheManager = CreateRateLimitCacheManager();

            // Act
            var result = cacheManager.GetQueryRateLimit(TEST_QUERY, _credentials);

            // Assert
            Assert.AreEqual(result, _tokenRateLimit);
            _fakeRateLimitCache.CallsTo(x => x.RefreshEntry(It.IsAny<ITwitterCredentials>(), It.IsAny<ITokenRateLimits>())).MustNotHaveHappened();
        }

        [TestMethod]
        public void GetQueryRateLimit_QueryReturnedOutOfDateTokenRateLimit_RefreshNotPerformed()
        {
            // Arrange
            var cacheManager = CreateRateLimitCacheManager();
            _tokenRateLimit.CallsTo(x => x.ResetDateTime).Returns(DateTime.Now.AddMinutes(1));

            // Act
            cacheManager.GetQueryRateLimit(TEST_QUERY, _credentials);

            // Assert
            _fakeRateLimitCache.CallsTo(x => x.RefreshEntry(It.IsAny<ITwitterCredentials>(), It.IsAny<ITokenRateLimits>())).MustNotHaveHappened();
        }

        [TestMethod]
        public void GetQueryRateLimit_QueryReturnedOutOfDateTokenRateLimit_RefreshPerformed()
        {
            // Arrange
            var cacheManager = CreateRateLimitCacheManager();
            _tokenRateLimit.CallsTo(x => x.ResetDateTime).Returns(DateTime.Now.AddMinutes(-1));

            // Act
            var result = cacheManager.GetQueryRateLimit(TEST_QUERY, _credentials);

            // Assert
            Assert.AreEqual(result, _refreshedTokenRateLimit);
            _fakeRateLimitCache.CallsTo(x => x.RefreshEntry(It.IsAny<ITwitterCredentials>(), It.IsAny<ITokenRateLimits>())).MustHaveHappened(Repeated.Exactly.Once);
        }

        [TestMethod]
        public void GetTokenRateLimits_AlreadyInCache_ReturnsCacheValueWithoutRefresh()
        {
            // Arrange
            var cacheManager = CreateRateLimitCacheManager();

            // Act
            var rateLimits = cacheManager.GetTokenRateLimits(_credentials);

            // Assert
            Assert.AreEqual(rateLimits, _tokenRateLimits);
            _fakeRateLimitCache.CallsTo(x => x.RefreshEntry(It.IsAny<ITwitterCredentials>(), It.IsAny<ITokenRateLimits>())).MustNotHaveHappened();
        }

        [TestMethod]
        public void GetTokenRateLimits_NotInCache_ReturnsCacheValueAfterRefresh()
        {
            // Arrange
            var cacheManager = CreateRateLimitCacheManager();
            var refreshedTokenRateLimits = A.Fake<ITokenRateLimits>();
            
            _fakeRateLimitCache.CallsTo(x => x.GetTokenRateLimits(_credentials)).Returns(null);
            _fakeRateLimitCache.CallsTo(x => x.RefreshEntry(_credentials, _tokenRateLimits)).Invokes(() =>
            {
                _fakeRateLimitCache.CallsTo(x => x.GetTokenRateLimits(_credentials)).Returns(refreshedTokenRateLimits);
            });

            // Act
            var rateLimits = cacheManager.GetTokenRateLimits(_credentials);

            // Assert
            Assert.AreEqual(rateLimits, refreshedTokenRateLimits);
            _fakeRateLimitCache.CallsTo(x => x.RefreshEntry(_credentials, _tokenRateLimits)).MustHaveHappened(Repeated.Exactly.Once);
        }

        

        [TestMethod]
        public void UpdateTokenRateLimits_UpdateRateLimitCache()
        {
            // Arrange
            var rateLimitCacheManager = CreateRateLimitCacheManager();

            // Act
            rateLimitCacheManager.UpdateTokenRateLimits(_credentials, _tokenRateLimits);

            // Assert
            _fakeRateLimitCache.CallsTo(x => x.RefreshEntry(_credentials, _tokenRateLimits)).MustHaveHappened(Repeated.Exactly.Once);
        }

        private void InitializeData()
        {
            _credentials = A.Fake<ITwitterCredentials>();
            _credentials.AccessToken = TestHelper.GenerateString();
            _credentials.AccessTokenSecret = TestHelper.GenerateString();

            _tokenRateLimits = A.Fake<ITokenRateLimits>();
            _tokenRateLimit = A.Fake<ITokenRateLimit>();
            _tokenRateLimits2 = A.Fake<ITokenRateLimits>();

            _refreshedTokenRateLimits = A.Fake<ITokenRateLimits>();
            _refreshedTokenRateLimit = A.Fake<ITokenRateLimit>();

            _tokenRateLimit.CallsTo(x => x.Remaining).Returns(0);
            _tokenRateLimit.CallsTo(x => x.ResetDateTime).Returns(DateTime.Now.AddMinutes(1));

            _twitterQuery = A.Fake<ITwitterQuery>();
            _twitterQuery.CallsTo(x => x.QueryURL).Returns(TEST_QUERY);
            _twitterQuery.CallsTo(x => x.HttpMethod).Returns(HttpMethod.GET);
            _twitterQuery.CallsTo(x => x.QueryParameters).Returns(Enumerable.Empty<IOAuthQueryParameter>());
        }

        private RateLimitCacheManager CreateRateLimitCacheManager()
        {
            return _fakeBuilder.GenerateClass();
        }
    }
}