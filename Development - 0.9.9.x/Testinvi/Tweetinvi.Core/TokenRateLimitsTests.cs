using Microsoft.VisualStudio.TestTools.UnitTesting;
using Testinvi.Helpers;
using Tweetinvi.Core.Helpers;
using Tweetinvi.Core.Interfaces.Models;

namespace Testinvi.Tweetinvi.Core
{
    [TestClass]
    public class TokenRateLimitsTests
    {
        private FakeClassBuilder<TokenRateLimits> _fakeBuilder;
        private IAttributeHelper _attributeHelper;

        [TestInitialize]
        public void TestInitialize()
        {
            _fakeBuilder = new FakeClassBuilder<TokenRateLimits>();
            _attributeHelper = new AttributeHelper();
        }

        public TokenRateLimits CreateTokenRateLimits()
        {
            return _fakeBuilder.GenerateClass();
        }
    }
}