using System;
using System.Net;
using FakeItEasy;
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

        private ISearchQueryValidator _searchQueryValidator;
        private ISearchQueryParameterGenerator _searchQueryParameterGenerator;
        private IQueryParameterGenerator _queryParameterGenerator;
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
            _searchQueryValidator = _fakeBuilder.GetFake<ISearchQueryValidator>().FakedObject;
            _searchQueryParameterGenerator = _fakeBuilder.GetFake<ISearchQueryParameterGenerator>().FakedObject;
            _queryParameterGenerator = _fakeBuilder.GetFake<IQueryParameterGenerator>().FakedObject;

            InitData();

            A.CallTo(() => _searchQueryParameterGenerator.GenerateSearchQueryParameter(_searchQuery))
                .Returns(_searchQueryParameter);
            A.CallTo(() => _searchQueryParameterGenerator.GenerateSearchTypeParameter(_searchResultType))
                .Returns(_searchTypeParameter);
            A.CallTo(() => _searchQueryParameterGenerator.GenerateSinceParameter(_since)).Returns(_sinceParameter);
            A.CallTo(() => _searchQueryParameterGenerator.GenerateUntilParameter(_until)).Returns(_untilParameter);
            A.CallTo(() => _searchQueryParameterGenerator.GenerateLocaleParameter(_locale)).Returns(_localeParameter);
            A.CallTo(() => _searchQueryParameterGenerator.GenerateLangParameter(_lang)).Returns(_languageParameter);
            A.CallTo(() => _searchQueryParameterGenerator.GenerateGeoCodeParameter(_geoCode))
                .Returns(_geoCodeParameter);

            A.CallTo(() => _queryParameterGenerator.GenerateCountParameter(_maximumNumberOfResults))
                .Returns(_maximumNumberOfResultsParameter);
            A.CallTo(() => _queryParameterGenerator.GenerateSinceIdParameter(_sinceId)).Returns(_sinceIdParameter);
            A.CallTo(() => _queryParameterGenerator.GenerateMaxIdParameter(_maxId)).Returns(_maxIdParameter);

            A.CallTo(() => _searchQueryValidator.IsSearchQueryValid(It.IsAny<string>())).Returns(true);
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
            
            A.CallTo(() => _searchQueryParameterGenerator.GenerateSearchQueryParameter(It.IsAny<string>()))
                .ReturnsLazily((string a) => a);
            var tweetSearchParameters = A.Fake<ISearchTweetsParameters>();
            A.CallTo(() => tweetSearchParameters.Filters).Returns(TweetSearchFilters.Videos);

            // Act
            var result = searchQueryGenerator.GetSearchTweetsQuery(tweetSearchParameters);

            // Assert
            Assert.IsTrue(WebUtility.UrlDecode(result).Contains(" filter:videos"));
            
            A.CallTo(() => _searchQueryValidator.ThrowIfSearchParametersIsNotValid(tweetSearchParameters))
                .MustHaveHappened();
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