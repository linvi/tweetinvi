using System;
using System.Linq;
using System.Linq.Expressions;
using FakeItEasy;
using FakeItEasy.ExtensionSyntax.Full;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Testinvi.Helpers;
using Tweetinvi.Core;
using Tweetinvi.Core.Attributes;
using Tweetinvi.Core.Helpers;
using Tweetinvi.Core.Injectinvi;
using Tweetinvi.Core.Interfaces.Credentials;
using Tweetinvi.Core.Interfaces.RateLimit;
using Tweetinvi.Credentials.RateLimit;
using Tweetinvi.WebLogic;

namespace Testinvi.Tweetinvi.Credentials.RateLimitTests
{
    [TestClass]
    public class RateLimitHelperTests
    {
        private FakeClassBuilder<RateLimitHelper> _fakeBuilder;

        private ICredentialsRateLimits _credentialsRateLimits;
        private IWebHelper _webHelper;
        private IAttributeHelper _attributeHelper;

        [TestInitialize]
        public void Initialize()
        {
            _fakeBuilder = new FakeClassBuilder<RateLimitHelper>();
            _credentialsRateLimits = A.Fake<ICredentialsRateLimits>();

            _webHelper = new WebHelper(A.Fake<ITweetinviSettingsAccessor>());
            _attributeHelper = new AttributeHelper();
        }

        [TestMethod]
        public void GetTokenRateLimitFromQuery_NoEndpointAssociated_ReturnsNull()
        {
            // Arrange
            var rateLimitHelper = CreateRateLimitHelper();

            // Act
            var result = rateLimitHelper.GetEndpointRateLimitFromQuery("UNEXPECTED QUERY", _credentialsRateLimits);

            // Assert
            Assert.IsNull(result);
        }

        [TestMethod]
        public void GetTokenRateLimitFromQuery_AllEnpointsAssociatedCorrectly()
        {
            var rateLimitHelper = CreateRateLimitHelper();
            GetTokenRateLimitFromQuery_EndpointAssociatedCorrectly(x => x.AccountSettingsLimit, ACCOUNT_SETTINGS_QUERY, rateLimitHelper);
        }

        [TestMethod]
        public void IsQueryAssociatedWithTokenRateLimit_ReturnsTrue()
        {
            // Arrange
            var rateLimitHelper = CreateRateLimitHelper();

            // Act
            var isQueryAssociatedToARateLimit = rateLimitHelper.IsQueryAssociatedWithEndpointRateLimit(ACCOUNT_SETTINGS_QUERY, _credentialsRateLimits);

            // Assert
            Assert.IsTrue(isQueryAssociatedToARateLimit);
        }

        [TestMethod]
        public void IsQueryAssociatedWithTokenRateLimit_RateLimitsIsNull_ReturnsFalse()
        {
            // Arrange
            var rateLimitHelper = CreateRateLimitHelper();

            // Act
            var isQueryAssociatedToARateLimit = rateLimitHelper.IsQueryAssociatedWithEndpointRateLimit(ACCOUNT_SETTINGS_QUERY, null);

            // Assert
            Assert.IsFalse(isQueryAssociatedToARateLimit);
        }

        [TestMethod]
        public void IsQueryAssociatedWithTokenRateLimit_QueryIsAnURLButDoesNotMatch_ReturnsFalse()
        {
            // Arrange
            var rateLimitHelper = CreateRateLimitHelper();

            // Act
            var isQueryAssociatedToARateLimit = rateLimitHelper.IsQueryAssociatedWithEndpointRateLimit("http://tweetinvi.codeplex.com", _credentialsRateLimits);

            // Assert
            Assert.IsFalse(isQueryAssociatedToARateLimit);
        }

        [TestMethod]
        public void IsQueryAssociatedWithTokenRateLimit_QueryIsNotAnURL_ReturnsFalse()
        {
            // Arrange
            var rateLimitHelper = CreateRateLimitHelper();

            // Act
            var isQueryAssociatedToARateLimit = rateLimitHelper.IsQueryAssociatedWithEndpointRateLimit("Test me a river!", _credentialsRateLimits);

            // Assert
            Assert.IsFalse(isQueryAssociatedToARateLimit);
        }

        [CustomTwitterEndpoint("https://api.twitter.com/1.1/account/settings.json")]
        private void MatchingTestMethod() { }

        [TestMethod]
        public void GetTokenRateLimitsFromMethod_Matching_ReturnsTokenRateLimits()
        {
            // Arrange
            var rateLimitHelper = CreateRateLimitHelper();

            // Act
            var tokenRateLimits = rateLimitHelper.GetTokenRateLimitsFromMethod(() => MatchingTestMethod(), _credentialsRateLimits);

            // Assert
            Assert.IsTrue(tokenRateLimits.First() == _credentialsRateLimits.AccountSettingsLimit);
        }

        [TestMethod]
        public void GetTokenRateLimitsFromMethod_ExpressionIsNull_ReturnsNull()
        {
            // Arrange
            var rateLimitHelper = CreateRateLimitHelper();

            // Act
            var tokenRateLimits = rateLimitHelper.GetTokenRateLimitsFromMethod(null, _credentialsRateLimits);

            // Assert
            Assert.IsNull(tokenRateLimits);
        }

        [TestMethod]
        public void GetTokenRateLimitsFromMethod_ExpressionIsNotMethod_ReturnsNull()
        {
            // Arrange
            var rateLimitHelper = CreateRateLimitHelper();

            // Act
            Expression<Action> exp = Expression<Action>.Lambda<Action>(Expression.Empty());
            var tokenRateLimits = rateLimitHelper.GetTokenRateLimitsFromMethod(exp, _credentialsRateLimits);

            // Assert
            Assert.IsNull(tokenRateLimits);
        }

        private void GetTokenRateLimitFromQuery_EndpointAssociatedCorrectly(Expression<Func<ICredentialsRateLimits, IEndpointRateLimit>> rateLimit, string associatedURL, IRateLimitHelper rateLimitHelper)
        {
            // Arrange
            var fakeTokenRateLimit = A.Fake<IEndpointRateLimit>();
            _credentialsRateLimits.CallsTo(rateLimit).Returns(fakeTokenRateLimit);

            // Act
            var tokenRateLimit = rateLimitHelper.GetEndpointRateLimitFromQuery(associatedURL, _credentialsRateLimits);

            // Assert
            Assert.AreEqual(tokenRateLimit, fakeTokenRateLimit);
        }

        public RateLimitHelper CreateRateLimitHelper()
        {
            return _fakeBuilder.GenerateClass
            (
                new ConstructorNamedParameter("webHelper", _webHelper),
                new ConstructorNamedParameter("attributeHelper", _attributeHelper)
            );
        }

        private const string ACCOUNT_SETTINGS_QUERY = "https://api.twitter.com/1.1/account/settings.json?plop=15";
    }
}
