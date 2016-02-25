using Microsoft.VisualStudio.TestTools.UnitTesting;
using Testinvi.Helpers;
using Tweetinvi.Controllers.Account;
using Tweetinvi.Controllers.Properties;

namespace Testinvi.TweetinviControllers.AccountTests
{
    [TestClass]
    public class AccountQueryGeneratorTests
    {
        private FakeClassBuilder<AccountQueryGenerator> _fakeBuilder;

        [TestInitialize]
        public void TestInitialize()
        {
            _fakeBuilder = new FakeClassBuilder<AccountQueryGenerator>();
        }

        [TestMethod]
        public void GetAuthenticatedUserAccountSettingsQuery_ReturnsFixedResource()
        {
            // Arrange
            var queryGenerator = CreateAccountQueryGenerator();

            // Act
            var result = queryGenerator.GetAuthenticatedUserAccountSettingsQuery();

            // Assert
            Assert.AreEqual(result, Resources.Account_GetSettings);
        }

        public AccountQueryGenerator CreateAccountQueryGenerator()
        {
            return _fakeBuilder.GenerateClass();
        }
    }
}