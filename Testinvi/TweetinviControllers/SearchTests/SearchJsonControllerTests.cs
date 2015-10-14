using System.Linq;
using FakeItEasy;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Testinvi.Helpers;
using Testinvi.SetupHelpers;
using Tweetinvi.Controllers.Search;
using Tweetinvi.Core.Interfaces.Credentials;
using Tweetinvi.Core.Parameters;

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
        public void SearchTweet_BasedOnQuery_ReturnsTwitterAccessorStatuses()
        {
            // Arrange
            var queryExecutor = CreateSearchJsonController();
            var httpQuery = TestHelper.GenerateString();
            var searchQuery = TestHelper.GenerateString();
            var jsonResult = TestHelper.GenerateString();

            _fakeSearchQueryGenerator.CallsTo(x => x.GetSearchTweetsQuery(searchQuery)).Returns(httpQuery);
            _fakeTwitterAccessor.ArrangeExecuteJsonGETQuery(httpQuery, jsonResult);

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
            var searchQueryParameter = A.Fake<ITweetSearchParameters>();
            var jsonResult = TestHelper.GenerateString();

            _fakeSearchQueryGenerator.CallsTo(x => x.GetSearchTweetsQuery(searchQueryParameter)).Returns(httpQuery);
            _fakeTwitterAccessor.ArrangeExecuteJsonGETQuery(httpQuery, jsonResult);

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