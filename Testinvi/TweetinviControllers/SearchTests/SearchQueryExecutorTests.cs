using System.Linq;
using System.Threading.Tasks;
using FakeItEasy;
using FakeItEasy.ExtensionSyntax.Full;
using Microsoft.VisualStudio.TestTools.UnitTesting;
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
        private Fake<ISearchQueryGenerator> _fakeSearchQueryGenerator;
        private Fake<ITwitterAccessor> _fakeTwitterAccessor;
        private Fake<ISearchQueryParameterGenerator> _fakeSearchQueryParameterGenerator;

        private string _searchQuery;
        private string _httpQuery;
        private ITweetWithSearchMetadataDTO _originalTweetDTO;
        private ITweetWithSearchMetadataDTO _retweetDTO;
        private ISearchTweetsParameters _searchTweetsParameter;
        private ITweetWithSearchMetadataDTO[] _tweetDTOs;
        private ISearchResultsDTO _searchResultDTO;

        [TestInitialize]
        public void TestInitialize()
        {
            _fakeBuilder = new FakeClassBuilder<SearchQueryExecutor>();
            _fakeSearchQueryGenerator = _fakeBuilder.GetFake<ISearchQueryGenerator>();
            _fakeBuilder.GetFake<ISearchQueryHelper>();
            _fakeTwitterAccessor = _fakeBuilder.GetFake<ITwitterAccessor>();
            _fakeBuilder.GetFake<ITweetHelper>();
            _fakeSearchQueryParameterGenerator = _fakeBuilder.GetFake<ISearchQueryParameterGenerator>();

            _searchQuery = TestHelper.GenerateString();
            _httpQuery = TestHelper.GenerateString();
            TestHelper.GenerateString();
            _originalTweetDTO = GenerateTweetDTO(true);
            _retweetDTO = GenerateTweetDTO(false);

            _tweetDTOs = new[] { A.Fake<ITweetWithSearchMetadataDTO>() };
            _searchResultDTO = A.Fake<ISearchResultsDTO>();
            _searchResultDTO.CallsTo(x => x.TweetDTOs).Returns(_tweetDTOs);

            _searchTweetsParameter = A.Fake<ISearchTweetsParameters>();
            _fakeSearchQueryParameterGenerator.CallsTo(x => x.CreateSearchTweetParameter(_searchQuery)).Returns(_searchTweetsParameter);
        }

        #region Search Tweet

        [TestMethod]
        public async Task SearchTweet_BasedOnQuery_ReturnsTwitterAccessorStatuses()
        {
            // Arrange
            var queryExecutor = CreateSearchQueryExecutor();

            _fakeSearchQueryGenerator.CallsTo(x => x.GetSearchTweetsQuery(_searchTweetsParameter)).Returns(_httpQuery);
            _fakeTwitterAccessor.ArrangeExecuteGETQuery(_httpQuery, _searchResultDTO);

            // Act
            var result = await queryExecutor.SearchTweets(_searchQuery);

            // Assert
            Assert.IsTrue(result.ContainsAll(_tweetDTOs));
        }

        [TestMethod]
        public async Task SearchTweet_BasedOnSearchParameters_ReturnsTwitterAccessorStatuses()
        {
            // Arrange
            var queryExecutor = CreateSearchQueryExecutor();
            var searchQueryParameter = A.Fake<ISearchTweetsParameters>();

            _fakeSearchQueryGenerator.CallsTo(x => x.GetSearchTweetsQuery(searchQueryParameter)).Returns(_httpQuery);
            _fakeTwitterAccessor.ArrangeExecuteGETQuery(_httpQuery, _searchResultDTO);

            // Act
            var result = await queryExecutor.SearchTweets(searchQueryParameter);

            // Assert
            Assert.IsTrue(result.ContainsAll(_tweetDTOs));
        }

        [TestMethod]
        public async Task SearchTweet_FilterOriginal_FilterTheResults()
        {
            // Arrange
            var queryExecutor = CreateSearchQueryExecutor();
            var searchParameter = A.Fake<ISearchTweetsParameters>();
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
            var result = (await queryExecutor.SearchTweets(searchParameter)).ToArray();

            // Assert
            Assert.IsTrue(result.Contains(_originalTweetDTO));
            Assert.IsFalse(result.Contains(_retweetDTO));
        }

        [TestMethod]
        public async Task SearchTweet_FilterRetweets_FilterTheResults()
        {
            // Arrange
            var queryExecutor = CreateSearchQueryExecutor();
            var searchParameter = A.Fake<ISearchTweetsParameters>();
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
            var result = (await queryExecutor.SearchTweets(searchParameter)).ToArray();

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