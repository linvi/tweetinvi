using System.Collections.Generic;
using FakeItEasy;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Testinvi.Helpers;
using Tweetinvi.Controllers.Search;
using Tweetinvi.Core.Interfaces;
using Tweetinvi.Core.Interfaces.DTO;
using Tweetinvi.Core.Interfaces.Factories;
using Tweetinvi.Core.Parameters;

namespace Testinvi.TweetinviControllers.SearchTests
{
    [TestClass]
    public class SearchControllerTests
    {
        private FakeClassBuilder<SearchController> _fakeBuilder;
        private Fake<ISearchQueryExecutor> _fakeSearchQueryExecutor;
        private Fake<ITweetFactory> _fakeTweetFactory;

        [TestInitialize]
        public void TestInitialize()
        {
            _fakeBuilder = new FakeClassBuilder<SearchController>();
            _fakeSearchQueryExecutor = _fakeBuilder.GetFake<ISearchQueryExecutor>();
            _fakeTweetFactory = _fakeBuilder.GetFake<ITweetFactory>();
        }

        [TestMethod]
        public void SearchTweets_WithSearchQuery_ReturnsQueryExecutorDTOTransformed()
        {
            // Arrange
            var controller = CreateSearchController();
            var searchQuery = TestHelper.GenerateString();
            var searchDTOResult = new List<ITweetDTO>();
            var searchResult = new List<ITweet>();

            _fakeSearchQueryExecutor.CallsTo(x => x.SearchTweets(searchQuery)).Returns(searchDTOResult);
            _fakeTweetFactory.CallsTo(x => x.GenerateTweetsFromDTO(searchDTOResult)).Returns(searchResult);

            // Act
            var result = controller.SearchTweets(searchQuery);

            // Assert
            Assert.AreEqual(result, searchResult);
        }

        [TestMethod]
        public void SearchTweets_WithSearchTweetParameter_ReturnsQueryExecutorDTOTransformed()
        {
            // Arrange
            var controller = CreateSearchController();
            var searchParameter = A.Fake<ITweetSearchParameters>();
            var searchDTOResult = new List<ITweetDTO>();
            var searchResult = new List<ITweet>();

            _fakeSearchQueryExecutor.CallsTo(x => x.SearchTweets(searchParameter)).Returns(searchDTOResult);
            _fakeTweetFactory.CallsTo(x => x.GenerateTweetsFromDTO(searchDTOResult)).Returns(searchResult);

            // Act
            var result = controller.SearchTweets(searchParameter);

            // Assert
            Assert.AreEqual(result, searchResult);
        }

        public SearchController CreateSearchController()
        {
            return _fakeBuilder.GenerateClass();
        }
    }
}