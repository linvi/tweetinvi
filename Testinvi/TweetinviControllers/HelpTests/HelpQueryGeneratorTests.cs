using Microsoft.VisualStudio.TestTools.UnitTesting;
using Testinvi.Helpers;
using Tweetinvi.Controllers.Help;
using Tweetinvi.Controllers.Properties;

namespace Testinvi.TweetinviControllers.HelpTests
{
    [TestClass]
    public class HelpQueryGeneratorTests
    {
        private FakeClassBuilder<HelpQueryGenerator> _fakeBuilder;

        [TestInitialize]
        public void TestInitialize()
        {
            _fakeBuilder = new FakeClassBuilder<HelpQueryGenerator>();
        }

        [TestMethod]
        public void GetTokenRateLimitsQuery_ReturnResource()
        {
            // Arrange
            var queryGenerator = CreateHelpQueryGenerator();

            // Act
            string result = queryGenerator.GetCredentialsLimitsQuery();

            // Assert
            Assert.AreEqual(result, Resources.Help_GetRateLimit);
        }

        [TestMethod]
        public void GetTwitterPrivacyPolicyQuery_ReturnResource()
        {
            // Arrange
            var queryGenerator = CreateHelpQueryGenerator();

            // Act
            string result = queryGenerator.GetTwitterPrivacyPolicyQuery();

            // Assert
            Assert.AreEqual(result, Resources.Help_GetTwitterPrivacyPolicy);
        }

        public HelpQueryGenerator CreateHelpQueryGenerator()
        {
            return _fakeBuilder.GenerateClass();
        }
    }
}