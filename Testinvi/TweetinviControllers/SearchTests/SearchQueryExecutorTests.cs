using System.Linq;
using FakeItEasy;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json.Linq;
using Testinvi.Helpers;
using Testinvi.SetupHelpers;
using Tweetinvi.Controllers.Search;
using Tweetinvi.Controllers.Tweet;
using Tweetinvi.Core.Web;
using Tweetinvi.Models.DTO;
using Tweetinvi.Parameters;

namespace Testinvi.TweetinviControllers.SearchTests
{
    [TestClass]
    public class SearchQueryExecutorTests
    {
        private FakeClassBuilder<SearchQueryExecutor> _fakeBuilder;

        private ISearchQueryGenerator _searchQueryGenerator;
        private ITwitterAccessor _twitterAccessor;
        private ISearchQueryHelper _searchQueryHelper;
        private ITweetHelper _tweetHelper;
        private ISearchQueryParameterGenerator _searchQueryParameterGenerator;
        private ISearchResultsDTO _searchResultDTO;

        private string _searchQuery;
        private string _httpQuery;
        private string _statusesJson;
        private ITweetWithSearchMetadataDTO _originalTweetDTO;
        private ITweetWithSearchMetadataDTO _retweetDTO;
        private JObject _jObject;
        private ISearchTweetsParameters _searchTweetsParameter;
        private ITweetWithSearchMetadataDTO[] _tweetDTOs;

        [TestInitialize]
        public void TestInitialize()
        {
            _fakeBuilder = new FakeClassBuilder<SearchQueryExecutor>();
            _searchQueryGenerator = _fakeBuilder.GetFake<ISearchQueryGenerator>().FakedObject;
            _searchQueryHelper = _fakeBuilder.GetFake<ISearchQueryHelper>().FakedObject;
            _twitterAccessor = _fakeBuilder.GetFake<ITwitterAccessor>().FakedObject;
            _tweetHelper = _fakeBuilder.GetFake<ITweetHelper>().FakedObject;
            _searchQueryParameterGenerator = _fakeBuilder.GetFake<ISearchQueryParameterGenerator>().FakedObject;

            _searchQuery = TestHelper.GenerateString();
            _httpQuery = TestHelper.GenerateString();
            _statusesJson = TestHelper.GenerateString();
            _originalTweetDTO = GenerateTweetDTO(true);
            _retweetDTO = GenerateTweetDTO(false);

            _jObject = new JObject();
            _jObject["statuses"] = _statusesJson;
            _tweetDTOs = new[] { A.Fake<ITweetWithSearchMetadataDTO>() };
            _searchResultDTO = A.Fake<ISearchResultsDTO>();
            A.CallTo(() => _searchResultDTO.TweetDTOs).Returns(_tweetDTOs);

            _searchTweetsParameter = A.Fake<ISearchTweetsParameters>();
            A.CallTo(() => _searchQueryParameterGenerator.CreateSearchTweetParameter(_searchQuery))
                .Returns(_searchTweetsParameter);
        }

        #region Search Tweet

        [TestMethod]
        public void SearchTweet_BasedOnQuery_ReturnsTwitterAccessorStatuses()
        {
            // Arrange
            var queryExecutor = CreateSearchQueryExecutor();

            A.CallTo(() => _searchQueryGenerator.GetSearchTweetsQuery(_searchTweetsParameter)).Returns(_httpQuery);
            _twitterAccessor.ArrangeExecuteGETQuery(_httpQuery, _searchResultDTO);

            // Act
            var result = queryExecutor.SearchTweets(_searchQuery);

            // Assert
            Assert.IsTrue(result.ContainsAll(_tweetDTOs));
        }

        [TestMethod]
        public void SearchTweet_BasedOnSearchParameters_ReturnsTwitterAccessorStatuses()
        {
            // Arrange
            var queryExecutor = CreateSearchQueryExecutor();
            var searchQueryParameter = A.Fake<ISearchTweetsParameters>();

            A.CallTo(() => _searchQueryGenerator.GetSearchTweetsQuery(searchQueryParameter)).Returns(_httpQuery);
            _twitterAccessor.ArrangeExecuteGETQuery(_httpQuery, _searchResultDTO);

            // Act
            var result = queryExecutor.SearchTweets(searchQueryParameter);

            // Assert
            Assert.IsTrue(result.ContainsAll(_tweetDTOs));
        }

        [TestMethod]
        public void SearchTweet_FilterOriginal_FilterTheResults()
        {
            // Arrange
            var queryExecutor = CreateSearchQueryExecutor();
            var searchParameter = A.Fake<ISearchTweetsParameters>();
            A.CallTo(() => searchParameter.SearchQuery).Returns(_searchQuery);
            A.CallTo(() => searchParameter.TweetSearchType).Returns(TweetSearchType.OriginalTweetsOnly);

            var matchingTweetDTOs = new[]
            {
                _originalTweetDTO,
                _retweetDTO
            };

            A.CallTo(() => _searchResultDTO.TweetDTOs).Returns(matchingTweetDTOs);

            A.CallTo(() => _searchQueryGenerator.GetSearchTweetsQuery(searchParameter))
                .Returns(_httpQuery);
            _twitterAccessor.ArrangeExecuteGETQuery(_httpQuery, _searchResultDTO);

            // Act
            var result = queryExecutor.SearchTweets(searchParameter);

            // Assert
            Assert.IsTrue(result.Contains(_originalTweetDTO));
            Assert.IsFalse(result.Contains(_retweetDTO));
        }

        [TestMethod]
        public void SearchTweet_FilterRetweets_FilterTheResults()
        {
            // Arrange
            var queryExecutor = CreateSearchQueryExecutor();
            var searchParameter = A.Fake<ISearchTweetsParameters>();
            A.CallTo(() => searchParameter.SearchQuery).Returns(_searchQuery);
            A.CallTo(() => searchParameter.TweetSearchType).Returns(TweetSearchType.RetweetsOnly);

            var matchingTweetDTOs = new[]
            {
                _originalTweetDTO,
                _retweetDTO
            };

            A.CallTo(() => _searchResultDTO.TweetDTOs).Returns(matchingTweetDTOs);

            A.CallTo(() => _searchQueryGenerator.GetSearchTweetsQuery(searchParameter))
                .Returns(_httpQuery);
            _twitterAccessor.ArrangeExecuteGETQuery(_httpQuery, _searchResultDTO);

            // Act
            var result = queryExecutor.SearchTweets(searchParameter);

            // Assert
            Assert.IsFalse(result.Contains(_originalTweetDTO));
            Assert.IsTrue(result.Contains(_retweetDTO));
        }

        #endregion

        private ITweetWithSearchMetadataDTO GenerateTweetDTO(bool isOriginalTweet)
        {
            var tweetDTO = A.Fake<ITweetWithSearchMetadataDTO>();
            A.CallTo(() => tweetDTO.RetweetedTweetDTO)
                .Returns(isOriginalTweet ? null : A.Fake<ITweetWithSearchMetadataDTO>());
            return tweetDTO;
        }

        private SearchQueryExecutor CreateSearchQueryExecutor()
        {
            return _fakeBuilder.GenerateClass();
        }
    }
}