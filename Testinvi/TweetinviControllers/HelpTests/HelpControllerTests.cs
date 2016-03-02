using System;
using FakeItEasy;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Testinvi.Helpers;
using Tweetinvi.Controllers.Help;
using Tweetinvi.Core.Interfaces.Credentials;

namespace Testinvi.TweetinviControllers.HelpTests
{
    [TestClass]
    public class HelpControllerTests
    {
        private FakeClassBuilder<HelpController> _fakeBuilder;
        private Fake<IHelpQueryExecutor> _fakeHelpQueryExecutor;

        [TestInitialize]
        public void TestInitialize()
        {
            _fakeBuilder = new FakeClassBuilder<HelpController>();
            _fakeHelpQueryExecutor = _fakeBuilder.GetFake<IHelpQueryExecutor>();
        }

        [TestMethod]
        public void GetTokenRateLimits_ReturnsQueryExecutor()
        {
            var expectedResult = A.Fake<ICredentialsRateLimits>();

            // Arrange
            var helpController = CreateHelpControllerTests();
            ArrangeQueryExecutorGetTokenRateLimits(expectedResult);

            // Act
            var result = helpController.GetCurrentCredentialsRateLimits();

            // Assert
            Assert.AreEqual(result, expectedResult);
        }

        private void ArrangeQueryExecutorGetTokenRateLimits(ICredentialsRateLimits result)
        {
            _fakeHelpQueryExecutor
                .CallsTo(x => x.GetCurrentCredentialsRateLimits())
                .Returns(result);
        }

        [TestMethod]
        public void GetTwitterPrivacyPolicy_ReturnsQueryExecutor()
        {
            var expectedResult = Guid.NewGuid().ToString();

            // Arrange
            var helpController = CreateHelpControllerTests();
            ArrangeQueryExecutorGetTwitterPrivacyPolicy(expectedResult);

            // Act
            var result = helpController.GetTwitterPrivacyPolicy();

            // Assert
            Assert.AreEqual(result, expectedResult);
        }

        private void ArrangeQueryExecutorGetTwitterPrivacyPolicy(string result)
        {
            _fakeHelpQueryExecutor
                .CallsTo(x => x.GetTwitterPrivacyPolicy())
                .Returns(result);
        }

        public HelpController CreateHelpControllerTests()
        {
            return _fakeBuilder.GenerateClass();
        }
    }
}