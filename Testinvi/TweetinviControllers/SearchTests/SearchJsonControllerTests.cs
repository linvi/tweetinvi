using System.Linq;
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
        private ITwitterAccessor _twitterAccessor;
        private ISearchQueryGenerator _searchQueryGenerator;

        [TestInitialize]
        public void TestInitialize()
        {
            _fakeBuilder = new FakeClassBuilder<SearchJsonController>();
            _twitterAccessor = _fakeBuilder.GetFake<ITwitterAccessor>().FakedObject;
            _searchQueryGenerator = _fakeBuilder.GetFake<ISearchQueryGenerator>().FakedObject;
        }

        [TestMethod]
        public void SearchTweet_BasedOnQuery_ReturnsTwitterAccessorStatuses()
        {
            // Arrange
            var queryExecutor = CreateSearchJsonController();
            var httpQuery = TestHelper.GenerateString();
            var searchQuery = TestHelper.GenerateString();
            var jsonResult = TestHelper.GenerateString();

            A.CallTo(() => _searchQueryGenerator.GetSearchTweetsQuery(searchQuery)).Returns(httpQuery);
            _twitterAccessor.ArrangeExecuteJsonGETQuery(httpQuery, jsonResult);

            // Act
            var result = queryExecutor.SearchTweets(searchQuery);

            // Assert
            Assert.AreEqual(result, jsonResult);
        }

        [TestMethod]
        public void SearchTweet_BasedOnSearchParameters_ReturnsTwitterAccessorStatuses()
        {
            // Arrange
            var queryExecutor = CreateSearchJsonController();
            var httpQuery = TestHelper.GenerateString();
            var searchQueryParameter = A.Fake<ISearchTweetsParameters>();
            var jsonResult = TestHelper.GenerateString();

            A.CallTo(() => _searchQueryGenerator.GetSearchTweetsQuery(searchQueryParameter)).Returns(httpQuery);
            _twitterAccessor.ArrangeExecuteJsonGETQuery(httpQuery, jsonResult);

            // Act
            var result = queryExecutor.SearchTweets(searchQueryParameter);

            // Assert
            Assert.AreEqual(result.Count(), 1);
            Assert.IsTrue(result.Contains(jsonResult));
        }

        public SearchJsonController CreateSearchJsonController()
        {
            return _fakeBuilder.GenerateClass();
        }
    }
}