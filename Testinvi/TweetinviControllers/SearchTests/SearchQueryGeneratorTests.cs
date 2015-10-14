using System;
using FakeItEasy;
using FakeItEasy.ExtensionSyntax.Full;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Testinvi.Helpers;
using Tweetinvi.Controllers.Search;
using Tweetinvi.Controllers.Shared;
using Tweetinvi.Core.Enum;
using Tweetinvi.Core.Extensions;
using Tweetinvi.Core.Interfaces.Models;
using Tweetinvi.Core.Parameters;

namespace Testinvi.TweetinviControllers.SearchTests
{
    [TestClass]
    public class SearchQueryGeneratorTests
    {
        // GC stands for Geo Code
        // LA stands for Language
        // MI stands for Max Id
        // MNOR stands for Maximum Number Of Objects
        // SI stands for Since Id
        // U stands for Until
        
        private FakeClassBuilder<SearchQueryGenerator> _fakeBuilder;
        private Fake<ISearchQueryValidator> _fakeSearchQueryValidator;
        private Fake<ISearchQueryParameterGenerator> _fakeSearchQueryParameterGenerator;
        private Fake<IQueryParameterGenerator> _fakeQueryParameterGenerator;

        private ITweetSearchParameters _tweetSearchParameters;

        private string _searchQuery;
        private SearchResultType _searchResultType;
        private int _maximumNumberOfResults;
        private long _sinceId;
        private long _maxId;
        private DateTime _since;
        private DateTime _until;
        private string _locale;
        private Language _lang;
        private IGeoCode _geoCode;

        private string _searchQueryParameter;
        private string _searchTypeParameter;
        private string _maximumNumberOfResultsParameter;
        private string _sinceIdParameter;
        private string _maxIdParameter;
        private string _sinceParameter;
        private string _untilParameter;
        private string _localeParameter;
        private string _languageParameter;
        private string _geoCodeParameter;

        [TestInitialize]
        public void TestInitialize()
        {
            _fakeBuilder = new FakeClassBuilder<SearchQueryGenerator>();
            _fakeSearchQueryValidator = _fakeBuilder.GetFake<ISearchQueryValidator>();
            _fakeSearchQueryParameterGenerator = _fakeBuilder.GetFake<ISearchQueryParameterGenerator>();
            _fakeQueryParameterGenerator = _fakeBuilder.GetFake<IQueryParameterGenerator>();

            InitData();

            _fakeSearchQueryParameterGenerator.CallsTo(x => x.GenerateSearchQueryParameter(_searchQuery)).Returns(_searchQueryParameter);
            _fakeSearchQueryParameterGenerator.CallsTo(x => x.GenerateSearchTypeParameter(_searchResultType)).Returns(_searchTypeParameter);
            _fakeSearchQueryParameterGenerator.CallsTo(x => x.GenerateSinceParameter(_since)).Returns(_sinceParameter);
            _fakeSearchQueryParameterGenerator.CallsTo(x => x.GenerateUntilParameter(_until)).Returns(_untilParameter);
            _fakeSearchQueryParameterGenerator.CallsTo(x => x.GenerateLocaleParameter(_locale)).Returns(_localeParameter);
            _fakeSearchQueryParameterGenerator.CallsTo(x => x.GenerateLangParameter(_lang)).Returns(_languageParameter);
            _fakeSearchQueryParameterGenerator.CallsTo(x => x.GenerateGeoCodeParameter(_geoCode)).Returns(_geoCodeParameter);

            _fakeQueryParameterGenerator.CallsTo(x => x.GenerateCountParameter(_maximumNumberOfResults)).Returns(_maximumNumberOfResultsParameter);
            _fakeQueryParameterGenerator.CallsTo(x => x.GenerateSinceIdParameter(_sinceId)).Returns(_sinceIdParameter);
            _fakeQueryParameterGenerator.CallsTo(x => x.GenerateMaxIdParameter(_maxId)).Returns(_maxIdParameter);

            _fakeSearchQueryValidator.CallsTo(x => x.IsSearchParameterValid(It.IsAny<ITweetSearchParameters>())).Returns(true);
            _fakeSearchQueryValidator.CallsTo(x => x.IsSearchTweetsQueryValid(It.IsAny<string>())).Returns(true);
        }

        private void InitData()
        {
            _searchQuery = TestHelper.GenerateString();
            _searchQueryParameter = TestHelper.GenerateString();

            _searchResultType = SearchResultType.Mixed;
            _searchTypeParameter = TestHelper.GenerateString();

            _maximumNumberOfResults = TestHelper.GenerateRandomInt();
            _maximumNumberOfResultsParameter = TestHelper.GenerateString();

            _sinceId = TestHelper.GenerateRandomLong();
            _sinceIdParameter = TestHelper.GenerateString();

            _maxId = TestHelper.GenerateRandomLong();
            _maxIdParameter = TestHelper.GenerateString();

            _since = DateTime.Now.AddMinutes(TestHelper.GenerateRandomInt());
            _sinceParameter = TestHelper.GenerateString();

            _until = DateTime.Now.AddMinutes(TestHelper.GenerateRandomInt());
            _untilParameter = TestHelper.GenerateString();

            _locale = TestHelper.GenerateString();
            _localeParameter = TestHelper.GenerateString();

            _lang = Language.Afrikaans;
            _languageParameter = TestHelper.GenerateString();

            _geoCode = A.Fake<IGeoCode>();
            _geoCodeParameter = TestHelper.GenerateString();

            _tweetSearchParameters = A.Fake<ITweetSearchParameters>();
            _tweetSearchParameters.SearchQuery = _searchQuery;
            _tweetSearchParameters.SearchType = _searchResultType;
            _tweetSearchParameters.MaximumNumberOfResults = _maximumNumberOfResults;
            _tweetSearchParameters.SinceId = _sinceId;
            _tweetSearchParameters.MaxId = _maxId;
            _tweetSearchParameters.Since = _since;
            _tweetSearchParameters.Until = _until;
            _tweetSearchParameters.Locale = _locale;
            _tweetSearchParameters.Lang = _lang;
            _tweetSearchParameters.GeoCode = _geoCode;
        }

        [TestMethod]
        public void GetSearchTweetQuery_WithTweetSearchParameter_QueryIsValid_ReturnsExpectedQuery()
        {
            // Arrange
            var searchQueryGenerator = CreateSearchQueryGenerator();

            // Act
            var result = searchQueryGenerator.GetSearchTweetsQuery(_tweetSearchParameters);

            // Assert
            VerifyResultContainsParameters(result, _searchQueryParameter,
                                                   _searchTypeParameter,
                                                   _maximumNumberOfResultsParameter,
                                                   _sinceIdParameter,
                                                   _maxIdParameter,
                                                   _sinceParameter,
                                                   _untilParameter,
                                                   _localeParameter,
                                                   _languageParameter,
                                                   _geoCodeParameter);
        }

        [TestMethod]
        public void GetSearchTweetQuery_WithFilters_ReturnsExpectedQuery()
        {
            // Arrange
            var searchQueryGenerator = CreateSearchQueryGenerator();
            
            _fakeSearchQueryParameterGenerator.CallsTo(x => x.GenerateSearchQueryParameter(It.IsAny<string>())).ReturnsLazily((string a) => a);
            var tweetSearchParameters = A.Fake<ITweetSearchParameters>();
            tweetSearchParameters.CallsTo(x => x.Filters).Returns(TweetSearchFilters.Videos);

            // Act
            var result = searchQueryGenerator.GetSearchTweetsQuery(tweetSearchParameters);

            // Assert
            Assert.IsTrue(result.Contains(" filter:videos"));
        }

        [TestMethod]
        public void GetSearchTweetQuery_WithTweetSearchParameter_SearchParameterIsInvalid_ReturnsNull()
        {
            // Arrange
            var searchQueryGenerator = CreateSearchQueryGenerator();
            _fakeSearchQueryValidator.CallsTo(x => x.IsSearchParameterValid(_tweetSearchParameters)).Returns(false);

            // Act
            var result = searchQueryGenerator.GetSearchTweetsQuery(_tweetSearchParameters);

            // Assert
            Assert.IsNull(result);
        }

        [TestMethod]
        public void GetSearchTweetQuery_WithTweetSearchParameter_SearchQueryIsInvalid_ReturnsNull()
        {
            // Arrange
            var searchQueryGenerator = CreateSearchQueryGenerator();
            _fakeSearchQueryValidator.CallsTo(x => x.IsSearchTweetsQueryValid(_searchQuery)).Returns(false);

            // Act
            var result = searchQueryGenerator.GetSearchTweetsQuery(_tweetSearchParameters);

            // Assert
            Assert.IsNull(result);
        }

        private void VerifyResultContainsParameters(string result, params string[] expectedParameters)
        {
            int totalLength = 0;
            expectedParameters.ForEach(x => totalLength += x.Length);

            Assert.AreEqual(result.Length, totalLength);
            foreach (var expectedParameter in expectedParameters)
            {
                Assert.IsTrue(result.Contains(expectedParameter));
            }
        }

        public SearchQueryGenerator CreateSearchQueryGenerator()
        {
            return _fakeBuilder.GenerateClass();
        }
    }
}