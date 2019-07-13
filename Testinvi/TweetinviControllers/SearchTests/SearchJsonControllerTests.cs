using System.Linq;
using System.Threading.Tasks;
using FakeItEasy;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Testinvi.Helpers;
using Testinvi.SetupHelpers;
using Tweetinvi.Controllers.Search;
using Tweetinvi.Core.Web;
using Tweetinvi.Parameters;

namespace Testinvi.TweetinviControllers.SearchTests
{
    [TestClass]
    public class SearchJsonControllerTests
    {
        private FakeClassBuilder<SearchJsonController> _fakeBuilder;
        private Fake<ITwitterAccessor> _fakeTwitterAccessor;
        private Fake<ISearchQueryGenerator> _fakeSearchQueryGenerator;

        [TestInitialize]
        public void TestInitialize()
        {
            _fakeBuilder = new FakeClassBuilder<SearchJsonController>();
            _fakeTwitterAccessor = _fakeBuilder.GetFake<ITwitterAccessor>();
            _fakeSearchQueryGenerator = _fakeBuilder.GetFake<ISearchQueryGenerator>();
        }

        [TestMethod]
        public async Task  SearchTweet_BasedOnQuery_ReturnsTwitterAccessorStatuses()
        {
            // Arrange
            var queryExecutor = CreateSearchJsonController();
            var httpQuery = TestHelper.GenerateString();
            var searchQuery = TestHelper.GenerateString();
            var jsonResult = TestHelper.GenerateString();

            _fakeSearchQueryGenerator.CallsTo(x => x.GetSearchTweetsQuery(searchQuery)).Returns(httpQuery);
            _fakeTwitterAccessor.ArrangeExecuteJsonGETQuery(httpQuery, jsonResult);

            // Act
            var result = await queryExecutor.SearchTweets(searchQuery);

            // Assert
            Assert.AreEqual(result, jsonResult);
        }

        [TestMethod]
        public async Task SearchTweet_BasedOnSearchParameters_ReturnsTwitterAccessorStatuses()
        {
            // Arrange
            var queryExecutor = CreateSearchJsonController();
            var httpQuery = TestHelper.GenerateString();
            var searchQueryParameter = A.Fake<ISearchTweetsParameters>();
            var jsonResult = TestHelper.GenerateString();

            _fakeSearchQueryGenerator.CallsTo(x => x.GetSearchTweetsQuery(searchQueryParameter)).Returns(httpQuery);
            _fakeTwitterAccessor.ArrangeExecuteJsonGETQuery(httpQuery, jsonResult);

            // Act
            var result = await queryExecutor.SearchTweets(searchQueryParameter);

            // Assert
            Assert.AreEqual(result.Length, 1);
            Assert.IsTrue(result.Contains(jsonResult));
        }

        private SearchJsonController CreateSearchJsonController()
        {
            return _fakeBuilder.GenerateClass();
        }
    }
}