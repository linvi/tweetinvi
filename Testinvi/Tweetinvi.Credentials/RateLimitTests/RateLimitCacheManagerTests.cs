using System;
using System.Linq;
using FakeItEasy;
using FakeItEasy.ExtensionSyntax.Full;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using Testinvi.Helpers;
using Testinvi.SetupHelpers;
using Tweetinvi.Core.Authentication;
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
        private ICredentialsRateLimits _credentialsRateLimits;
        private IEndpointRateLimit _endpointRateLimit;
        private ICredentialsRateLimits _refreshedCredentialsRateLimits;
        private IEndpointRateLimit _refreshedEndpointRateLimit;
        private ITwitterCredentials _credentials;
        private ICredentialsRateLimits _credentialsRateLimits2; 

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

            _fakeRateLimitHelper.CallsTo(x => x.GetEndpointRateLimitFromQuery(TEST_QUERY, _credentialsRateLimits)).Returns(_endpointRateLimit);
            _fakeRateLimitCache.CallsTo(x => x.GetCredentialsRateLimits(_credentials)).Returns(_credentialsRateLimits);

            _fakeRateLimitCache.CallsTo(x => x.RefreshEntry(_credentials, _credentialsRateLimits)).Invokes(() =>
            {
                _fakeRateLimitCache.CallsTo(x => x.GetCredentialsRateLimits(_credentials)).Returns(_refreshedCredentialsRateLimits);
                _fakeRateLimitHelper.CallsTo(x => x.GetEndpointRateLimitFromQuery(TEST_QUERY, _refreshedCredentialsRateLimits)).Returns(_refreshedEndpointRateLimit);
            });

            _fakeCredentialsAccessor.SetupPassThrough<ICredentialsRateLimits>();

            _fakeHelpQueryGenerator.CallsTo(x => x.GetCredentialsLimitsQuery()).Returns(TEST_QUERY);

            _fakeTwitterRequester.CallsTo(x => x.ExecuteQuery(_twitterQuery, null)).Returns(TEST_QUERY);
            _fakeJsonObjectConverter.CallsTo(x => x.DeserializeObject<ICredentialsRateLimits>(TEST_QUERY, It.IsAny<JsonConverter[]>())).ReturnsNextFromSequence(_credentialsRateLimits, _credentialsRateLimits2);

            _fakeTwitterQueryFactory.CallsTo(x => x.Create(TEST_QUERY, It.IsAny<HttpMethod>(), It.IsAny<ITwitterCredentials>())).Returns(_twitterQuery);
        }

        [TestMethod]
        public void GetQueryRateLimit_TokenRateLimitsIsNull_Refreshes()
        {
            // Arrange
            var cacheManager = CreateRateLimitCacheManager();

            _fakeRateLimitCache.CallsTo(x => x.GetCredentialsRateLimits(_credentials)).Returns(null);

            // Act
            var result = cacheManager.GetQueryRateLimit(TEST_QUERY, _credentials);

            // Assert
            Assert.AreEqual(result, _refreshedEndpointRateLimit);
            _fakeRateLimitCache.CallsTo(x => x.RefreshEntry(It.IsAny<ITwitterCredentials>(), It.IsAny<ICredentialsRateLimits>())).MustHaveHappened(Repeated.Exactly.Once);
        }

        [TestMethod]
        public void GetQueryRateLimit_QueryCannotBeMatched_ReturnsNull()
        {
            // Arrange
            var cacheManager = CreateRateLimitCacheManager();

            _fakeRateLimitHelper.CallsTo(x => x.GetEndpointRateLimitFromQuery(TEST_QUERY, _credentialsRateLimits)).Returns(null);

            // Act
            var result = cacheManager.GetQueryRateLimit(TEST_QUERY, _credentials);

            // Assert
            Assert.IsNull(result);
            _fakeRateLimitCache.CallsTo(x => x.RefreshEntry(It.IsAny<ITwitterCredentials>(), It.IsAny<ICredentialsRateLimits>())).MustNotHaveHappened();
        }

        [TestMethod]
        public void GetQueryRateLimit_QueryReturnedValidTokenRateLimit_NoRefreshAndReturnsTheRateLimit()
        {
            // Arrange
            var cacheManager = CreateRateLimitCacheManager();

            // Act
            var result = cacheManager.GetQueryRateLimit(TEST_QUERY, _credentials);

            // Assert
            Assert.AreEqual(result, _endpointRateLimit);
            _fakeRateLimitCache.CallsTo(x => x.RefreshEntry(It.IsAny<ITwitterCredentials>(), It.IsAny<ICredentialsRateLimits>())).MustNotHaveHappened();
        }

        [TestMethod]
        public void GetQueryRateLimit_QueryReturnedOutOfDateTokenRateLimit_RefreshNotPerformed()
        {
            // Arrange
            var cacheManager = CreateRateLimitCacheManager();
            _endpointRateLimit.CallsTo(x => x.ResetDateTime).Returns(DateTime.Now.AddMinutes(1));

            // Act
            cacheManager.GetQueryRateLimit(TEST_QUERY, _credentials);

            // Assert
            _fakeRateLimitCache.CallsTo(x => x.RefreshEntry(It.IsAny<ITwitterCredentials>(), It.IsAny<ICredentialsRateLimits>())).MustNotHaveHappened();
        }

        [TestMethod]
        public void GetQueryRateLimit_QueryReturnedOutOfDateTokenRateLimit_RefreshPerformed()
        {
            // Arrange
            var cacheManager = CreateRateLimitCacheManager();
            _endpointRateLimit.CallsTo(x => x.ResetDateTime).Returns(DateTime.Now.AddMinutes(-1));

            // Act
            var result = cacheManager.GetQueryRateLimit(TEST_QUERY, _credentials);

            // Assert
            Assert.AreEqual(result, _refreshedEndpointRateLimit);
            _fakeRateLimitCache.CallsTo(x => x.RefreshEntry(It.IsAny<ITwitterCredentials>(), It.IsAny<ICredentialsRateLimits>())).MustHaveHappened(Repeated.Exactly.Once);
        }

        [TestMethod]
        public void GetTokenRateLimits_AlreadyInCache_ReturnsCacheValueWithoutRefresh()
        {
            // Arrange
            var cacheManager = CreateRateLimitCacheManager();

            // Act
            var rateLimits = cacheManager.GetCredentialsRateLimits(_credentials);

            // Assert
            Assert.AreEqual(rateLimits, _credentialsRateLimits);
            _fakeRateLimitCache.CallsTo(x => x.RefreshEntry(It.IsAny<ITwitterCredentials>(), It.IsAny<ICredentialsRateLimits>())).MustNotHaveHappened();
        }

        [TestMethod]
        public void GetTokenRateLimits_NotInCache_ReturnsCacheValueAfterRefresh()
        {
            // Arrange
            var cacheManager = CreateRateLimitCacheManager();
            var refreshedTokenRateLimits = A.Fake<ICredentialsRateLimits>();
            
            _fakeRateLimitCache.CallsTo(x => x.GetCredentialsRateLimits(_credentials)).Returns(null);
            _fakeRateLimitCache.CallsTo(x => x.RefreshEntry(_credentials, _credentialsRateLimits)).Invokes(() =>
            {
                _fakeRateLimitCache.CallsTo(x => x.GetCredentialsRateLimits(_credentials)).Returns(refreshedTokenRateLimits);
            });

            // Act
            var rateLimits = cacheManager.GetCredentialsRateLimits(_credentials);

            // Assert
            Assert.AreEqual(rateLimits, refreshedTokenRateLimits);
            _fakeRateLimitCache.CallsTo(x => x.RefreshEntry(_credentials, _credentialsRateLimits)).MustHaveHappened(Repeated.Exactly.Once);
        }

        

        [TestMethod]
        public void UpdateTokenRateLimits_UpdateRateLimitCache()
        {
            // Arrange
            var rateLimitCacheManager = CreateRateLimitCacheManager();

            // Act
            rateLimitCacheManager.UpdateCredentialsRateLimits(_credentials, _credentialsRateLimits);

            // Assert
            _fakeRateLimitCache.CallsTo(x => x.RefreshEntry(_credentials, _credentialsRateLimits)).MustHaveHappened(Repeated.Exactly.Once);
        }

        private void InitializeData()
        {
            _credentials = A.Fake<ITwitterCredentials>();
            _credentials.AccessToken = TestHelper.GenerateString();
            _credentials.AccessTokenSecret = TestHelper.GenerateString();

            _credentialsRateLimits = A.Fake<ICredentialsRateLimits>();
            _endpointRateLimit = A.Fake<IEndpointRateLimit>();
            _credentialsRateLimits2 = A.Fake<ICredentialsRateLimits>();

            _refreshedCredentialsRateLimits = A.Fake<ICredentialsRateLimits>();
            _refreshedEndpointRateLimit = A.Fake<IEndpointRateLimit>();

            _endpointRateLimit.CallsTo(x => x.Remaining).Returns(0);
            _endpointRateLimit.CallsTo(x => x.ResetDateTime).Returns(DateTime.Now.AddMinutes(1));

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