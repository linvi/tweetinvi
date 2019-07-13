using System;
using System.Threading.Tasks;
using FakeItEasy;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Testinvi.Helpers;
using Testinvi.SetupHelpers;
using Tweetinvi.Controllers.Help;
using Tweetinvi.Core.QueryGenerators;
using Tweetinvi.Core.Web;

namespace Testinvi.TweetinviControllers.HelpTests
{
    [TestClass]
    public class HelpJsonControllerTests
    {
        private FakeClassBuilder<HelpJsonController> _fakeBuilder;
        private Fake<IHelpQueryGenerator> _fakeHelpQueryGenerator;
        private Fake<ITwitterAccessor> _fakeTwitterAccessor;

        [TestInitialize]
        public void TestInitialize()
        {
            _fakeBuilder = new FakeClassBuilder<HelpJsonController>();
            _fakeHelpQueryGenerator = _fakeBuilder.GetFake<IHelpQueryGenerator>();
            _fakeTwitterAccessor = _fakeBuilder.GetFake<ITwitterAccessor>();
        }

        [TestMethod]
        public async Task GetTokenRateLimits_ReturnsJsonTwitterAccessor()
        {
            string expectedResult = Guid.NewGuid().ToString();

            // Arrange
            var jsonController = CreateHelpJsonController();
            string query = Guid.NewGuid().ToString();

            ArrangeQueryGeneratorGetTokenRateLimits(query);
            _fakeTwitterAccessor.ArrangeExecuteJsonGETQuery(query, expectedResult);

            // Act
            var result = await jsonController.GetCredentialsRateLimits();

            // Assert
            Assert.AreEqual(result, expectedResult);
        }

        private void ArrangeQueryGeneratorGetTokenRateLimits(string query)
        {
            _fakeHelpQueryGenerator
                .CallsTo(x => x.GetCredentialsLimitsQuery())
                .Returns(query);
        }

        [TestMethod]
        public async Task GetTwitterPrivacyPolicy_ReturnsJsonTwitterAccessor()
        {
            string expectedResult = Guid.NewGuid().ToString();

            // Arrange
            var jsonController = CreateHelpJsonController();
            string query = Guid.NewGuid().ToString();

            ArrangeQueryGeneratorGetTwitterPrivacyPolicy(query);
            _fakeTwitterAccessor.ArrangeExecuteJsonGETQuery(query, expectedResult);

            // Act
            var result = await jsonController.GetTwitterPrivacyPolicy();

            // Assert
            Assert.AreEqual(result, expectedResult);
        }

        private void ArrangeQueryGeneratorGetTwitterPrivacyPolicy(string query)
        {
            _fakeHelpQueryGenerator
                .CallsTo(x => x.GetTwitterPrivacyPolicyQuery())
                .Returns(query);
        }

        private HelpJsonController CreateHelpJsonController()
        {
            return _fakeBuilder.GenerateClass();
        }
    }
}