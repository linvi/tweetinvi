using Microsoft.VisualStudio.TestTools.UnitTesting;
using Testinvi.Helpers;
using Tweetinvi.Controllers.Account;
using Tweetinvi.Controllers.Properties;

using Tweetinvi.Core.Enum;

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

        [TestMethod]
        public void GetAccountUpdateDeliveryDeviceQuery_RequiredParamsOnly()
        {
            // Arrange
            var queryGenerator = CreateAccountQueryGenerator();

            // Act
            var result = queryGenerator.GetAccountUpdateDeliveryDeviceQuery(UpdateDeliveryDeviceType.SMS, null);

            // Assert
            var expextedResult = string.Format(Resources.Account_UpdateDeliveryDevice, "device=sms");
            Assert.AreEqual(result, expextedResult);
        }

        [TestMethod]
        public void GetAccountUpdateDeliveryDeviceQuery_WithOptionalParams()
        {
            // Arrange
            var queryGenerator = CreateAccountQueryGenerator();

            // Act
            var result = queryGenerator.GetAccountUpdateDeliveryDeviceQuery(UpdateDeliveryDeviceType.SMS, false);

            // Assert
            var expextedResult = string.Format(Resources.Account_UpdateDeliveryDevice, "device=sms&include_entities=False");
            Assert.AreEqual(result, expextedResult);
        }

        public AccountQueryGenerator CreateAccountQueryGenerator()
        {
            return _fakeBuilder.GenerateClass();
        }
    }
}