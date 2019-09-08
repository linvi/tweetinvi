using System.Threading.Tasks;
using FakeItEasy;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Testinvi.Helpers;
using Testinvi.SetupHelpers;
using Tweetinvi.Controllers.User;
using Tweetinvi.Core.Parameters;
using Tweetinvi.Core.QueryGenerators;
using Tweetinvi.Core.Web;
using Tweetinvi.Models;

namespace Testinvi.TweetinviControllers.UserTests
{
    [TestClass]
    public class UserJsonControllerTests
    {
        private FakeClassBuilder<UserJsonController> _fakeBuilder;
        private Fake<IUserQueryGenerator> _fakeUserQueryGenerator;
        private Fake<ITwitterAccessor> _fakeTwitterAccessor;

        [TestInitialize]
        public void TestInitialize()
        {
            _fakeBuilder = new FakeClassBuilder<UserJsonController>();
            _fakeUserQueryGenerator = _fakeBuilder.GetFake<IUserQueryGenerator>();
            _fakeTwitterAccessor = _fakeBuilder.GetFake<ITwitterAccessor>();
        }

        #region FavouriteTweets

        [TestMethod]
        public async Task GetFavoriteTweetsWithUser_AnyData_ReturnsTwitterAccessorResult()
        {
            // Arrange
            var queryExecutor = CreateUserJsonController();
            var expectedQuery = TestHelper.GenerateString();
            var expectedResult = TestHelper.GenerateString();
            var parameters = It.IsAny<IGetUserFavoritesQueryParameters>();

            _fakeUserQueryGenerator.CallsTo(x => x.GetFavoriteTweetsQuery(parameters)).Returns(expectedQuery);
            _fakeTwitterAccessor.ArrangeExecuteJsonGETQuery(expectedQuery, expectedResult);

            // Act
            var result = await queryExecutor.GetFavoriteTweets(parameters);

            // Assert
            Assert.AreEqual(result, expectedResult);
        }

        #endregion

        private string[] GenerateExpectedCursorResults()
        {
            return new string[0];
        }

        private UserJsonController CreateUserJsonController()
        {
            return _fakeBuilder.GenerateClass();
        }
    }
}