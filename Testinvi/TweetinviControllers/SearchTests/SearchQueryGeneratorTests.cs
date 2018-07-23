using System;
using System.Net;
using FakeItEasy;
using FakeItEasy.ExtensionSyntax.Full;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Testinvi.Helpers;
using Tweetinvi.Controllers.Search;
using Tweetinvi.Controllers.Shared;
using Tweetinvi.Models;
using Tweetinvi.Parameters;

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

        private ISearchTweetsParameters _searchTweetsParameters;

        private string _searchQuery;
        private SearchResultType _searchResultType;
        private int _maximumNumberOfResults;
        private long _sinceId;
        private long _maxId;
        private DateTime _since;
        private DateTime _until;
        private string _locale;
        private LanguageFilter _lang;
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

            _fakeSearchQueryValidator.CallsTo(x => x.IsSearchQueryValid(It.IsAny<string>())).Returns(true);
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

            _lang = LanguageFilter.French;
            _languageParameter = TestHelper.GenerateString();

            _geoCode = A.Fake<IGeoCode>();
            _geoCodeParameter = TestHelper.GenerateString();

            _searchTweetsParameters = A.Fake<ISearchTweetsParameters>();
            _searchTweetsParameters.SearchQuery = _searchQuery;
            _searchTweetsParameters.SearchType = _searchResultType;
            _searchTweetsParameters.MaximumNumberOfResults = _maximumNumberOfResults;
            _searchTweetsParameters.SinceId = _sinceId;
            _searchTweetsParameters.MaxId = _maxId;
            _searchTweetsParameters.Since = _since;
            _searchTweetsParameters.Until = _until;
            _searchTweetsParameters.Locale = _locale;
            _searchTweetsParameters.Lang = _lang;
            _searchTweetsParameters.GeoCode = _geoCode;
        }

        [TestMethod]
        public void GetSearchTweetQuery_WithFilters_ReturnsExpectedQuery()
        {
            // Arrange
            var searchQueryGenerator = CreateSearchQueryGenerator();
            
            _fakeSearchQueryParameterGenerator.CallsTo(x => x.GenerateSearchQueryParameter(It.IsAny<string>())).ReturnsLazily((string a) => a);
            var tweetSearchParameters = A.Fake<ISearchTweetsParameters>();
            tweetSearchParameters.CallsTo(x => x.Filters).Returns(TweetSearchFilters.Videos);

            // Act
            var result = searchQueryGenerator.GetSearchTweetsQuery(tweetSearchParameters);

            // Assert
            Assert.IsTrue(WebUtility.UrlDecode(result).Contains(" filter:videos"));
            
            _fakeSearchQueryValidator.CallsTo(x => x.ThrowIfSearchParametersIsNotValid(tweetSearchParameters)).MustHaveHappened();
        }

        [TestMethod]
        public void GetSearchTweetQuery_WithTweetSearchParameter_QueryIsValid_ReturnsExpectedQuery()
        {
            // Arrange
            var searchQueryGenerator = CreateSearchQueryGenerator();

            // Act
            var result = searchQueryGenerator.GetSearchTweetsQuery(_searchTweetsParameters);

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

        private void VerifyResultContainsParameters(string result, params string[] expectedParameters)
        {
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