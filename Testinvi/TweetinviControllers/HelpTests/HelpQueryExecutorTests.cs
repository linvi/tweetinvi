using System;
using FakeItEasy;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json.Linq;
using Testinvi.Helpers;
using Testinvi.SetupHelpers;
using Tweetinvi.Controllers.Help;
using Tweetinvi.Core.Interfaces.Controllers;
using Tweetinvi.Core.Interfaces.Credentials;

namespace Testinvi.TweetinviControllers.HelpTests
{
    [TestClass]
    public class HelpQueryExecutorTests
    {
        private FakeClassBuilder<HelpQueryExecutor> _fakeBuilder;
        private Fake<IHelpQueryGenerator> _fakeHelpQueryGenerator;
        private Fake<ITwitterAccessor> _fakeTwitterAccessor;

        [TestInitialize]
        public void TestInitialize()
        {
            _fakeBuilder = new FakeClassBuilder<HelpQueryExecutor>();
            _fakeHelpQueryGenerator = _fakeBuilder.GetFake<IHelpQueryGenerator>();
            _fakeTwitterAccessor = _fakeBuilder.GetFake<ITwitterAccessor>();
        }

        [TestMethod]
        public void GetTokenRateLimits_ReturnsJsonTwitterAccessor()
        {
            var expectedResult = A.Fake<ICredentialsRateLimits>();

            // Arrange
            var queryExecutor = CreateHelpQueryExecutor();
            string query = Guid.NewGuid().ToString();

            ArrangeQueryGeneratorGetTokenRateLimits(query);
            _fakeTwitterAccessor.ArrangeExecuteGETQuery(query, expectedResult);

            // Act
            var result = queryExecutor.GetCurrentCredentialsRateLimits();

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
        public void GetTwitterPrivacyPolicy_ReturnsJsonTwitterAccessor()
        {
            var expectedResult = Guid.NewGuid().ToString();

            // Arrange
            var queryExecutor = CreateHelpQueryExecutor();
            string query = Guid.NewGuid().ToString();

            var expectedJObject = new JObject();
            expectedJObject["privacy"] = expectedResult;

            ArrangeQueryGeneratorGetTwitterPrivacyPolicy(query);
            _fakeTwitterAccessor.ArrangeExecuteGETQuery(query, expectedJObject);

            // Act
            var result = queryExecutor.GetTwitterPrivacyPolicy();

            // Assert
            Assert.AreEqual(result, expectedResult);
        }

        private void ArrangeQueryGeneratorGetTwitterPrivacyPolicy(string query)
        {
            _fakeHelpQueryGenerator
                .CallsTo(x => x.GetTwitterPrivacyPolicyQuery())
                .Returns(query);
        }

        public HelpQueryExecutor CreateHelpQueryExecutor()
        {
            return _fakeBuilder.GenerateClass();
        }
    }
}