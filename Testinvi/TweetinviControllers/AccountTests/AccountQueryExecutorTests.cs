using System;
using System.Threading.Tasks;
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
        private Fake<ITwitterAccessor> _fakeTwitterAccessor;
        private Fake<IAccountQueryGenerator> _fakeAccountQueryGenerator;

        [TestInitialize]
        public void TestInitialize()
        {
            _fakeBuilder = new FakeClassBuilder<AccountQueryExecutor>();
            _fakeTwitterAccessor = _fakeBuilder.GetFake<ITwitterAccessor>();
            _fakeAccountQueryGenerator = _fakeBuilder.GetFake<IAccountQueryGenerator>();
        }

        [TestMethod]
        public async Task GetAuthenticatedUserSettings_ReturnsAccessorJsonResult()
        {
            string query = Guid.NewGuid().ToString();
            var queryResult = A.Fake<IAccountSettingsDTO>();

            // Arrange
            var controller = CreateAccountQueryExecutor();

            ArrangeGetAuthenticatedUserAccountSettingsQuery(query);
            _fakeTwitterAccessor.ArrangeExecuteGETQuery(query, queryResult);

            // Act
            var result = await controller.GetAuthenticatedUserAccountSettings();

            // Assert
            Assert.AreEqual(result, queryResult);
        }

        private void ArrangeGetAuthenticatedUserAccountSettingsQuery(string query)
        {
            _fakeAccountQueryGenerator
                .CallsTo(x => x.GetAuthenticatedUserAccountSettingsQuery())
                .Returns(query);
        }

        private AccountQueryExecutor CreateAccountQueryExecutor()
        {
            return _fakeBuilder.GenerateClass();
        }
    }
}