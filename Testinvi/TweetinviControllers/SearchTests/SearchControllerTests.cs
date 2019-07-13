using System.Collections.Generic;
using System.Threading.Tasks;
using FakeItEasy;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Testinvi.Helpers;
using Tweetinvi.Controllers.Search;
using Tweetinvi.Core.Factories;
using Tweetinvi.Models;
using Tweetinvi.Models.DTO;
using Tweetinvi.Parameters;
// ReSharper disable CollectionNeverUpdated.Local

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
        public async Task  SearchTweets_WithSearchQuery_ReturnsQueryExecutorDTOTransformed()
        {
            // Arrange
            var controller = CreateSearchController();
            var searchQuery = TestHelper.GenerateString();
            var searchDTOResult = new List<ITweetDTO>();
            var searchResult = new List<ITweet>();

            _fakeSearchQueryExecutor.CallsTo(x => x.SearchTweets(searchQuery)).Returns(searchDTOResult);
            _fakeTweetFactory.CallsTo(x => x.GenerateTweetsFromDTO(searchDTOResult, null)).Returns(searchResult);

            // Act
            var result = await controller.SearchTweets(searchQuery);

            // Assert
            Assert.AreEqual(result, searchResult);
        }

        [TestMethod]
        public async Task SearchTweets_WithSearchTweetParameter_ReturnsQueryExecutorDTOTransformed()
        {
            // Arrange
            var controller = CreateSearchController();
            var searchParameter = A.Fake<ISearchTweetsParameters>();
            var searchDTOResult = new List<ITweetDTO>();
            var searchResult = new List<ITweet>();

            _fakeSearchQueryExecutor.CallsTo(x => x.SearchTweets(searchParameter)).Returns(searchDTOResult);
            _fakeTweetFactory.CallsTo(x => x.GenerateTweetsFromDTO(searchDTOResult, null)).Returns(searchResult);

            // Act
            var result = await controller.SearchTweets(searchParameter);

            // Assert
            Assert.AreEqual(result, searchResult);
        }

        private SearchController CreateSearchController()
        {
            return _fakeBuilder.GenerateClass();
        }
    }
}