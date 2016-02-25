using FakeItEasy;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Testinvi.Helpers;
using Testinvi.SetupHelpers;
using Tweetinvi.Controllers.Account;
using Tweetinvi.Core.Injectinvi;
using Tweetinvi.Core.Interfaces.Controllers;
using Tweetinvi.Core.Interfaces.DTO;
using Tweetinvi.Core.Interfaces.Models;

namespace Testinvi.TweetinviControllers.AccountTests
{
    [TestClass]
    public class AccountControllerTests
    {
        private FakeClassBuilder<AccountController> _fakeBuilder;
        private Fake<IAccountQueryExecutor> _fakeAccountQueryExecutor;
        private Fake<IFactory<IAccountSettings>> _fakeAccountSettingsFactory;

        [TestInitialize]
        public void TestInitialize()
        {
            _fakeBuilder = new FakeClassBuilder<AccountController>();
            _fakeAccountQueryExecutor = _fakeBuilder.GetFake<IAccountQueryExecutor>();
            _fakeAccountSettingsFactory = _fakeBuilder.GetFake<IFactory<IAccountSettings>>();
        }

        [TestMethod]
        public void GetAuthenticatedUserSettings_ExecuteQuery()
        {
            var fakeAccountSettingsDTO = A.Fake<IAccountSettingsDTO>();
            var fakeAccountSettings = A.Fake<IAccountSettings>();
            
            // Arrange
            var controller = CreateAccountController();

            ArrangeGetAuthenticatedUserAccountSettings(fakeAccountSettingsDTO);

            _fakeAccountSettingsFactory.ArrangeGenerateParameterOverride<IAccountSettingsDTO, IAccountSettings>();
            _fakeAccountSettingsFactory
                .CallsTo(x => x.Create(
                    A<IConstructorNamedParameter>
                        .That.Matches(p => p.Name == "accountSettingsDTO" &&
                                           p.Value == fakeAccountSettingsDTO)))
                .Returns(fakeAccountSettings);

            // Act
            var result = controller.GetAuthenticatedUserSettings();

            // Assert
            Assert.AreEqual(result, fakeAccountSettings);
        }

        private void ArrangeGetAuthenticatedUserAccountSettings(IAccountSettingsDTO result)
        {
            _fakeAccountQueryExecutor
                .CallsTo(x => x.GetAuthenticatedUserAccountSettings())
                .Returns(result);
        }

        private IAccountController CreateAccountController()
        {
            return _fakeBuilder.GenerateClass();
        }
    }
}