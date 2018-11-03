using System;
using FakeItEasy;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Testinvi.Helpers;
using Testinvi.SetupHelpers;
using Tweetinvi.Controllers.Account;
using Tweetinvi.Core.Web;

namespace Testinvi.TweetinviControllers.AccountTests
{
    [TestClass]
    public class AccountJsonControllerTests
    {
        private FakeClassBuilder<AccountJsonController> _fakeBuilder;

        private ITwitterAccessor _twitterAccessor;
        private IAccountQueryGenerator _accountQueryGenerator;

        [TestInitialize]
        public void TestInitialize()
        {
            _fakeBuilder = new FakeClassBuilder<AccountJsonController>();
            _twitterAccessor = _fakeBuilder.GetFake<ITwitterAccessor>().FakedObject;
            _accountQueryGenerator = _fakeBuilder.GetFake<IAccountQueryGenerator>().FakedObject;
        }

        [TestMethod]
        public void GetAuthenticatedUserSettingsJson_ReturnsAccessorJsonResult()
        {
            string query = Guid.NewGuid().ToString();
            string jsonResult = Guid.NewGuid().ToString();

            // Arrange
            var controller = CreateAccountJsonController();

            ArrangeGetAuthenticatedUserAccountSettingsQuery(query);
            _twitterAccessor.ArrangeExecuteJsonGETQuery(query, jsonResult);

            // Act
            var result = controller.GetAuthenticatedUserSettingsJson();

            // Assert
            Assert.AreEqual(result, jsonResult);
        }

        private void ArrangeGetAuthenticatedUserAccountSettingsQuery(string query)
        {
            A.CallTo(() => _accountQueryGenerator.GetAuthenticatedUserAccountSettingsQuery()).Returns(query);
        }

        public AccountJsonController CreateAccountJsonController()
        {
            return _fakeBuilder.GenerateClass();
        }
    }
}