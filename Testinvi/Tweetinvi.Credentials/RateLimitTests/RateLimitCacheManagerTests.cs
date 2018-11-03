using System;
using System.Linq;
using FakeItEasy;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using Testinvi.Helpers;
using Testinvi.SetupHelpers;
using Tweetinvi.Core.Credentials;
using Tweetinvi.Core.Helpers;
using Tweetinvi.Core.QueryGenerators;
using Tweetinvi.Core.RateLimit;
using Tweetinvi.Core.Web;
using Tweetinvi.Credentials.RateLimit;
using Tweetinvi.Models;
using HttpMethod = Tweetinvi.Models.HttpMethod;

namespace Testinvi.Tweetinvi.Credentials.RateLimitTests
{
    [TestClass]
    public class RateLimitCacheManagerTests
    {
        private const string TEST_QUERY = "I like testing cache!";

        private FakeClassBuilder<RateLimitCacheManager> _fakeBuilder;

        private IRateLimitCache _rateLimitCache;
        private IRateLimitHelper _rateLimitHelper;
        private IWebRequestExecutor _webRequestExecutor;
        private IHelpQueryGenerator _helpQueryGenerator;
        private IJsonObjectConverter _jsonObjectConverter;
        private ICredentialsAccessor _credentialsAccessor;
        private ITwitterQueryFactory _twitterQueryFactory;
        private ITwitterQuery _twitterQuery;
        private IEndpointRateLimit _endpointRateLimit;
        private ICredentialsRateLimits _credentialsRateLimits;
        private ICredentialsRateLimits _refreshedCredentialsRateLimits;
        private IEndpointRateLimit _refreshedEndpointRateLimit;
        private ITwitterCredentials _credentials;
        private ICredentialsRateLimits _credentialsRateLimits2;
        private IWebRequestResult _webRequestResult;

        [TestInitialize]
        public void TestInitialize()
        {
            _fakeBuilder = new FakeClassBuilder<RateLimitCacheManager>();
            _rateLimitCache = _fakeBuilder.GetFake<IRateLimitCache>().FakedObject;
            _rateLimitHelper = _fakeBuilder.GetFake<IRateLimitHelper>().FakedObject;
            _webRequestExecutor = _fakeBuilder.GetFake<IWebRequestExecutor>().FakedObject;
            _helpQueryGenerator = _fakeBuilder.GetFake<IHelpQueryGenerator>().FakedObject;
            _jsonObjectConverter = _fakeBuilder.GetFake<IJsonObjectConverter>().FakedObject;
            _credentialsAccessor = _fakeBuilder.GetFake<ICredentialsAccessor>().FakedObject;
            _twitterQueryFactory = _fakeBuilder.GetFake<ITwitterQueryFactory>().FakedObject;

            InitializeData();

            A.CallTo(() => _rateLimitHelper.GetEndpointRateLimitFromQuery(TEST_QUERY, _credentialsRateLimits, false))
                .Returns(_endpointRateLimit);
            A.CallTo(() => _rateLimitCache.GetCredentialsRateLimits(_credentials)).Returns(_credentialsRateLimits);

            A.CallTo(() => _rateLimitCache.RefreshEntry(_credentials, _credentialsRateLimits)).Invokes(() =>
            {
                A.CallTo(() => _rateLimitCache.GetCredentialsRateLimits(_credentials))
                    .Returns(_refreshedCredentialsRateLimits);
                A.CallTo(() =>
                        _rateLimitHelper.GetEndpointRateLimitFromQuery(TEST_QUERY, _refreshedCredentialsRateLimits,
                            false))
                    .Returns(_refreshedEndpointRateLimit);
            });

            _credentialsAccessor.SetupPassThrough<ICredentialsRateLimits>();

            A.CallTo(() => _helpQueryGenerator.GetCredentialsLimitsQuery()).Returns(TEST_QUERY);

            _webRequestResult = A.Fake<IWebRequestResult>();
            _webRequestResult.Text = TEST_QUERY;

            A.CallTo(() => _webRequestExecutor.ExecuteQuery(_twitterQuery, null)).Returns(_webRequestResult);
            A.CallTo(() =>
                    _jsonObjectConverter.DeserializeObject<ICredentialsRateLimits>(TEST_QUERY,
                        It.IsAny<JsonConverter[]>()))
                .ReturnsNextFromSequence(_credentialsRateLimits, _credentialsRateLimits2);

            A.CallTo(() =>
                    _twitterQueryFactory.Create(TEST_QUERY, It.IsAny<HttpMethod>(), It.IsAny<ITwitterCredentials>()))
                .Returns(_twitterQuery);
        }

        [TestMethod]
        public void GetQueryRateLimit_TokenRateLimitsIsNull_Refreshes()
        {
            // Arrange
            var cacheManager = CreateRateLimitCacheManager();

            A.CallTo(() => _rateLimitCache.GetCredentialsRateLimits(_credentials)).Returns(null);

            // Act
            var result = cacheManager.GetQueryRateLimit(TEST_QUERY, _credentials);

            // Assert
            Assert.AreEqual(result, _refreshedEndpointRateLimit);
            A.CallTo(() => _rateLimitCache.RefreshEntry(It.IsAny<ITwitterCredentials>(), It.IsAny<ICredentialsRateLimits>())).MustHaveHappened(Repeated.Exactly.Once);
        }

        [TestMethod]
        public void GetQueryRateLimit_QueryCannotBeMatched_ReturnsNull()
        {
            // Arrange
            var cacheManager = CreateRateLimitCacheManager();

            A.CallTo(() => _rateLimitHelper.GetEndpointRateLimitFromQuery(TEST_QUERY, _credentialsRateLimits, false))
                .Returns(null);

            // Act
            var result = cacheManager.GetQueryRateLimit(TEST_QUERY, _credentials);

            // Assert
            Assert.IsNull(result);
            A.CallTo(() =>
                    _rateLimitCache.RefreshEntry(It.IsAny<ITwitterCredentials>(), It.IsAny<ICredentialsRateLimits>()))
                .MustNotHaveHappened();
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
            A.CallTo(() =>
                    _rateLimitCache.RefreshEntry(It.IsAny<ITwitterCredentials>(), It.IsAny<ICredentialsRateLimits>()))
                .MustNotHaveHappened();
        }

        [TestMethod]
        public void GetQueryRateLimit_QueryReturnedOutOfDateTokenRateLimit_RefreshNotPerformed()
        {
            // Arrange
            var cacheManager = CreateRateLimitCacheManager();
            A.CallTo(() => _endpointRateLimit.ResetDateTime).Returns(DateTime.Now.AddMinutes(1));

            // Act
            cacheManager.GetQueryRateLimit(TEST_QUERY, _credentials);

            // Assert
            A.CallTo(() =>
                    _rateLimitCache.RefreshEntry(It.IsAny<ITwitterCredentials>(), It.IsAny<ICredentialsRateLimits>()))
                .MustNotHaveHappened();
        }

        [TestMethod]
        public void GetQueryRateLimit_QueryReturnedOutOfDateTokenRateLimit_RefreshPerformed()
        {
            // Arrange
            var cacheManager = CreateRateLimitCacheManager();
            A.CallTo(() => _endpointRateLimit.ResetDateTime).Returns(DateTime.Now.AddMinutes(-1));

            // Act
            var result = cacheManager.GetQueryRateLimit(TEST_QUERY, _credentials);

            // Assert
            Assert.AreEqual(result, _refreshedEndpointRateLimit);
            A.CallTo(() =>
                    _rateLimitCache.RefreshEntry(It.IsAny<ITwitterCredentials>(), It.IsAny<ICredentialsRateLimits>()))
                .MustHaveHappened(Repeated.Exactly.Once);
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
            A.CallTo(() =>
                    _rateLimitCache.RefreshEntry(It.IsAny<ITwitterCredentials>(), It.IsAny<ICredentialsRateLimits>()))
                .MustNotHaveHappened();
        }

        [TestMethod]
        public void GetTokenRateLimits_NotInCache_ReturnsCacheValueAfterRefresh()
        {
            // Arrange
            var cacheManager = CreateRateLimitCacheManager();
            var refreshedTokenRateLimits = A.Fake<ICredentialsRateLimits>();
            
            A.CallTo(() => _rateLimitCache.GetCredentialsRateLimits(_credentials)).Returns(null);
            A.CallTo(() => _rateLimitCache.RefreshEntry(_credentials, _credentialsRateLimits)).Invokes(() =>
            {
                A.CallTo(() => _rateLimitCache.GetCredentialsRateLimits(_credentials))
                    .Returns(refreshedTokenRateLimits);
            });

            // Act
            var rateLimits = cacheManager.GetCredentialsRateLimits(_credentials);

            // Assert
            Assert.AreEqual(rateLimits, refreshedTokenRateLimits);
            A.CallTo(() => _rateLimitCache.RefreshEntry(_credentials, _credentialsRateLimits))
                .MustHaveHappened(Repeated.Exactly.Once);
        }

        

        [TestMethod]
        public void UpdateTokenRateLimits_UpdateRateLimitCache()
        {
            // Arrange
            var rateLimitCacheManager = CreateRateLimitCacheManager();

            // Act
            rateLimitCacheManager.UpdateCredentialsRateLimits(_credentials, _credentialsRateLimits);

            // Assert
            A.CallTo(() => _rateLimitCache.RefreshEntry(_credentials, _credentialsRateLimits))
                .MustHaveHappened(Repeated.Exactly.Once);
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

            A.CallTo(() => _endpointRateLimit.Remaining).Returns(0);
            A.CallTo(() => _endpointRateLimit.ResetDateTime).Returns(DateTime.Now.AddMinutes(1));

            _twitterQuery = A.Fake<ITwitterQuery>();
            A.CallTo(() => _twitterQuery.QueryURL).Returns(TEST_QUERY);
            A.CallTo(() => _twitterQuery.HttpMethod).Returns(HttpMethod.GET);
            A.CallTo(() => _twitterQuery.QueryParameters).Returns(Enumerable.Empty<IOAuthQueryParameter>());
        }

        private RateLimitCacheManager CreateRateLimitCacheManager()
        {
            return _fakeBuilder.GenerateClass();
        }
    }
}