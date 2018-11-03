using System;
using FakeItEasy;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Testinvi.Helpers;
using Testinvi.SetupHelpers;
using Tweetinvi.Controllers.Account;
using Tweetinvi.Core.Web;
using Tweetinvi.Models.DTO;

namespace Testinvi.TweetinviControllers.AccountTests
{
    [TestClass]
    public class AccountQueryExecutorTests
    {
        private FakeClassBuilder<AccountQueryExecutor> _fakeBuilder;
        private ITwitterAccessor _twitterAccessor;
        private IAccountQueryGenerator _accountQueryGenerator;

        [TestInitialize]
        public void TestInitialize()
        {
            _fakeBuilder = new FakeClassBuilder<AccountQueryExecutor>();
            _twitterAccessor = _fakeBuilder.GetFake<ITwitterAccessor>().FakedObject;
            _accountQueryGenerator = _fakeBuilder.GetFake<IAccountQueryGenerator>().FakedObject;
        }

        [TestMethod]
        public void GetAuthenticatedUserSettings_ReturnsAccessorJsonResult()
        {
            string query = Guid.NewGuid().ToString();
            var queryResult = A.Fake<IAccountSettingsDTO>();

            // Arrange
            var controller = CreateAccountQueryExecutor();

            ArrangeGetAuthenticatedUserAccountSettingsQuery(query);
            _twitterAccessor.ArrangeExecuteGETQuery(query, queryResult);

            // Act
            var result = controller.GetAuthenticatedUserAccountSettings();

            // Assert
            Assert.AreEqual(result, queryResult);
        }

        private void ArrangeGetAuthenticatedUserAccountSettingsQuery(string query)
        {
            A.CallTo(() => _accountQueryGenerator.GetAuthenticatedUserAccountSettingsQuery()).Returns(query);
        }

        public AccountQueryExecutor CreateAccountQueryExecutor()
        {
            return _fakeBuilder.GenerateClass();
        }
    }
}