using System.Linq;
using FakeItEasy;
using FakeItEasy.ExtensionSyntax.Full;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json.Linq;
using Testinvi.Helpers;
using Testinvi.SetupHelpers;
using Tweetinvi.Controllers.Search;
using Tweetinvi.Controllers.Tweet;
using Tweetinvi.Core.Interfaces.Credentials;
using Tweetinvi.Core.Interfaces.DTO;
using Tweetinvi.Core.Parameters;

namespace Testinvi.TweetinviControllers.SearchTests
{
    [TestClass]
    public class SearchQueryExecutorTests
    {
        private FakeClassBuilder<SearchQueryExecutor> _fakeBuilder;
        private Fake<ISearchQueryGenerator> _fakeSearchQueryGenerator;
        private Fake<ITwitterAccessor> _fakeTwitterAccessor;
        private Fake<ISearchQueryHelper> _fakeSearchQueryHelper;
        private Fake<ITweetHelper> _fakeTweetHelper;
        private Fake<ISearchQueryParameterGenerator> _fakeSearchQueryParameterGenerator;

        private string _searchQuery;
        private string _httpQuery;
        private string _statusesJson;
        private ITweetWithSearchMetadataDTO _originalTweetDTO;
        private ITweetWithSearchMetadataDTO _retweetDTO;
        private JObject _jObject;
        private ITweetSearchParameters _tweetSearchParameter;
        private ITweetWithSearchMetadataDTO[] _tweetDTOs;
        private ISearchResultsDTO _searchResultDTO;

        [TestInitialize]
        public void TestInitialize()
        {
            _fakeBuilder = new FakeClassBuilder<SearchQueryExecutor>();
            _fakeSearchQueryGenerator = _fakeBuilder.GetFake<ISearchQueryGenerator>();
            _fakeSearchQueryHelper = _fakeBuilder.GetFake<ISearchQueryHelper>();
            _fakeTwitterAccessor = _fakeBuilder.GetFake<ITwitterAccessor>();
            _fakeTweetHelper = _fakeBuilder.GetFake<ITweetHelper>();
            _fakeSearchQueryParameterGenerator = _fakeBuilder.GetFake<ISearchQueryParameterGenerator>();

            _searchQuery = TestHelper.GenerateString();
            _httpQuery = TestHelper.GenerateString();
            _statusesJson = TestHelper.GenerateString();
            _originalTweetDTO = GenerateTweetDTO(true);
            _retweetDTO = GenerateTweetDTO(false);

            _jObject = new JObject();
            _jObject["statuses"] = _statusesJson;
            _tweetDTOs = new[] { A.Fake<ITweetWithSearchMetadataDTO>() };
            _searchResultDTO = A.Fake<ISearchResultsDTO>();
            _searchResultDTO.CallsTo(x => x.TweetDTOs).Returns(_tweetDTOs);

            _tweetSearchParameter = A.Fake<ITweetSearchParameters>();
            _fakeSearchQueryParameterGenerator.CallsTo(x => x.CreateSearchTweetParameter(_searchQuery)).Returns(_tweetSearchParameter);
        }

        #region Search Tweet

        [TestMethod]
        public void SearchTweet_BasedOnQuery_ReturnsTwitterAccessorStatuses()
        {
            // Arrange
            var queryExecutor = CreateSearchQueryExecutor();

            _fakeSearchQueryGenerator.CallsTo(x => x.GetSearchTweetsQuery(_tweetSearchParameter)).Returns(_httpQuery);
            _fakeTwitterAccessor.ArrangeExecuteGETQuery(_httpQuery, _searchResultDTO);

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
            var searchQueryParameter = A.Fake<ITweetSearchParameters>();

            _fakeSearchQueryGenerator.CallsTo(x => x.GetSearchTweetsQuery(searchQueryParameter)).Returns(_httpQuery);
            _fakeTwitterAccessor.ArrangeExecuteGETQuery(_httpQuery, _searchResultDTO);

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
            var searchParameter = A.Fake<ITweetSearchParameters>();
            searchParameter.CallsTo(x => x.SearchQuery).Returns(_searchQuery);
            searchParameter.CallsTo(x => x.TweetSearchType).Returns(TweetSearchType.OriginalTweetsOnly);

            var matchingTweetDTOs = new[]
            {
                _originalTweetDTO,
                _retweetDTO
            };

            _searchResultDTO.CallsTo(x => x.TweetDTOs).Returns(matchingTweetDTOs);

            _fakeSearchQueryGenerator.CallsTo(x => x.GetSearchTweetsQuery(searchParameter)).Returns(_httpQuery);
            _fakeTwitterAccessor.ArrangeExecuteGETQuery(_httpQuery, _searchResultDTO);

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
            var searchParameter = A.Fake<ITweetSearchParameters>();
            searchParameter.CallsTo(x => x.SearchQuery).Returns(_searchQuery);
            searchParameter.CallsTo(x => x.TweetSearchType).Returns(TweetSearchType.RetweetsOnly);

            var matchingTweetDTOs = new[]
            {
                _originalTweetDTO,
                _retweetDTO
            };

            _searchResultDTO.CallsTo(x => x.TweetDTOs).Returns(matchingTweetDTOs);

            _fakeSearchQueryGenerator.CallsTo(x => x.GetSearchTweetsQuery(searchParameter)).Returns(_httpQuery);
            _fakeTwitterAccessor.ArrangeExecuteGETQuery(_httpQuery, _searchResultDTO);

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
            tweetDTO.CallsTo(x => x.RetweetedTweetDTO).Returns(isOriginalTweet ? null : A.Fake<ITweetWithSearchMetadataDTO>());
            return tweetDTO;
        }

        private SearchQueryExecutor CreateSearchQueryExecutor()
        {
            return _fakeBuilder.GenerateClass();
        }
    }
}