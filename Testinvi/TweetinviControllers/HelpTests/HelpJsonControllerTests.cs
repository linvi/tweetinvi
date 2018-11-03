using System;
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
        private IHelpQueryGenerator _helpQueryGenerator;
        private ITwitterAccessor _twitterAccessor;

        [TestInitialize]
        public void TestInitialize()
        {
            _fakeBuilder = new FakeClassBuilder<HelpJsonController>();
            _helpQueryGenerator = _fakeBuilder.GetFake<IHelpQueryGenerator>().FakedObject;
            _twitterAccessor = _fakeBuilder.GetFake<ITwitterAccessor>().FakedObject;
        }

        [TestMethod]
        public void GetTokenRateLimits_ReturnsJsonTwitterAccessor()
        {
            string expectedResult = Guid.NewGuid().ToString();

            // Arrange
            var jsonController = CreateHelpJsonController();
            string query = Guid.NewGuid().ToString();

            ArrangeQueryGeneratorGetTokenRateLimits(query);
            _twitterAccessor.ArrangeExecuteJsonGETQuery(query, expectedResult);

            // Act
            var result = jsonController.GetCredentialsRateLimits();

            // Assert
            Assert.AreEqual(result, expectedResult);
        }

        private void ArrangeQueryGeneratorGetTokenRateLimits(string query)
        {
            A.CallTo(() => _helpQueryGenerator.GetCredentialsLimitsQuery()).Returns(query);
        }

        [TestMethod]
        public void GetTwitterPrivacyPolicy_ReturnsJsonTwitterAccessor()
        {
            string expectedResult = Guid.NewGuid().ToString();

            // Arrange
            var jsonController = CreateHelpJsonController();
            string query = Guid.NewGuid().ToString();

            ArrangeQueryGeneratorGetTwitterPrivacyPolicy(query);
            _twitterAccessor.ArrangeExecuteJsonGETQuery(query, expectedResult);

            // Act
            var result = jsonController.GetTwitterPrivacyPolicy();

            // Assert
            Assert.AreEqual(result, expectedResult);
        }

        private void ArrangeQueryGeneratorGetTwitterPrivacyPolicy(string query)
        {
            A.CallTo(() => _helpQueryGenerator.GetTwitterPrivacyPolicyQuery()).Returns(query);
        }

        public HelpJsonController CreateHelpJsonController()
        {
            return _fakeBuilder.GenerateClass();
        }
    }
}