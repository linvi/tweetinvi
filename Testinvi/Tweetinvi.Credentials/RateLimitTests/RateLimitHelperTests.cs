using System;
using System.Linq.Expressions;
using FakeItEasy;
using FakeItEasy.ExtensionSyntax.Full;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Testinvi.Helpers;
using Tweetinvi.Core;
using Tweetinvi.Core.Helpers;
using Tweetinvi.Core.Injectinvi;
using Tweetinvi.Core.RateLimit;
using Tweetinvi.Credentials.RateLimit;
using Tweetinvi.Models;
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
            var result = rateLimitHelper.GetEndpointRateLimitFromQuery("UNEXPECTED QUERY", _credentialsRateLimits, true);

            // Assert
            Assert.IsNull(result);
        }

        [TestMethod]
        public void GetTokenRateLimitFromQuery_AllEnpointsAssociatedCorrectly()
        {
            var rateLimitHelper = CreateRateLimitHelper();
            GetTokenRateLimitFromQuery_EndpointAssociatedCorrectly(x => x.AccountSettingsLimit, ACCOUNT_SETTINGS_QUERY, rateLimitHelper);
        }
        
        private void GetTokenRateLimitFromQuery_EndpointAssociatedCorrectly(Expression<Func<ICredentialsRateLimits, IEndpointRateLimit>> rateLimit, string associatedURL, IRateLimitHelper rateLimitHelper)
        {
            // Arrange
            var fakeTokenRateLimit = A.Fake<IEndpointRateLimit>();
            _credentialsRateLimits.CallsTo(rateLimit).Returns(fakeTokenRateLimit);

            // Act
            var tokenRateLimit = rateLimitHelper.GetEndpointRateLimitFromQuery(associatedURL, _credentialsRateLimits, true);

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
