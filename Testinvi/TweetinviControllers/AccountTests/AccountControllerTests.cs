using FakeItEasy;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Testinvi.Helpers;
using Testinvi.SetupHelpers;
using Tweetinvi.Controllers.Account;
using Tweetinvi.Core.Controllers;
using Tweetinvi.Core.Injectinvi;
using Tweetinvi.Models;
using Tweetinvi.Models.DTO;

namespace Testinvi.TweetinviControllers.AccountTests
{
    [TestClass]
    public class AccountControllerTests
    {
        private FakeClassBuilder<AccountController> _fakeBuilder;

        private IFactory<IAccountSettings> _accountSettingsFactory;
        private IAccountQueryExecutor _accountQueryExecutor;

        [TestInitialize]
        public void TestInitialize()
        {
            _fakeBuilder = new FakeClassBuilder<AccountController>();
            _accountSettingsFactory = _fakeBuilder.GetFake<IFactory<IAccountSettings>>().FakedObject;
            _accountQueryExecutor = _fakeBuilder.GetFake<IAccountQueryExecutor>().FakedObject;
        }

        [TestMethod]
        public void GetAuthenticatedUserSettings_ExecuteQuery()
        {
            var fakeAccountSettingsDTO = A.Fake<IAccountSettingsDTO>();
            var fakeAccountSettings = A.Fake<IAccountSettings>();
            
            // Arrange
            var controller = CreateAccountController();

            ArrangeGetAuthenticatedUserAccountSettings(fakeAccountSettingsDTO);

            _accountSettingsFactory.ArrangeGenerateParameterOverride<IAccountSettingsDTO, IAccountSettings>();
            A.CallTo(() => _accountSettingsFactory.Create(A<IConstructorNamedParameter>
                .That.Matches(p => p.Name == "accountSettingsDTO" &&
                                   p.Value == fakeAccountSettingsDTO))).Returns(fakeAccountSettings);

            // Act
            var result = controller.GetAuthenticatedUserSettings();

            // Assert
            Assert.AreEqual(result, fakeAccountSettings);
        }

        private void ArrangeGetAuthenticatedUserAccountSettings(IAccountSettingsDTO result)
        {
            A.CallTo(() => _accountQueryExecutor.GetAuthenticatedUserAccountSettings()).Returns(result);
        }

        private IAccountController CreateAccountController()
        {
            return _fakeBuilder.GenerateClass();
        }
    }
}