using System;
using System.Globalization;
using Tweetinvi.Controllers.Properties;
using Tweetinvi.Core.Extensions;
using Tweetinvi.Core.Helpers;
using Tweetinvi.Models;
using Tweetinvi.Parameters;

namespace Tweetinvi.Controllers.Search
{
    public interface ISearchQueryParameterGenerator
    {
        ISearchTweetsParameters CreateSearchTweetParameter(string query);
        ISearchTweetsParameters CreateSearchTweetParameter(IGeoCode geoCode);
        ISearchTweetsParameters CreateSearchTweetParameter(ICoordinates coordinates, int radius, DistanceMeasure measure);
        ISearchTweetsParameters CreateSearchTweetParameter(double latitude, double longitude, int radius, DistanceMeasure measure);

        string GenerateSearchQueryParameter(string query);
        string GenerateSearchTypeParameter(SearchResultType? searchType);
        string GenerateSinceParameter(DateTime since);
        string GenerateUntilParameter(DateTime until);
        string GenerateLocaleParameter(string locale);
        string GenerateLangParameter(Language lang);
        string GenerateLangParameter(LanguageFilter? lang);
        string GenerateGeoCodeParameter(IGeoCode geoCode);

        ISearchUsersParameters CreateUserSearchParameters(string query);
    }

    public class SearchQueryParameterGenerator : ISearchQueryParameterGenerator
    {
        private readonly ISearchQueryValidator _searchQueryValidator;
        private readonly ITwitterStringFormatter _twitterStringFormatter;

        public SearchQueryParameterGenerator(
            ISearchQueryValidator searchQueryValidator,
            ITwitterStringFormatter twitterStringFormatter)
        {
            _searchQueryValidator = searchQueryValidator;
            _twitterStringFormatter = twitterStringFormatter;
        }

        public ISearchTweetsParameters CreateSearchTweetParameter(string query)
        {
            return new SearchTweetsParameters(query);
        }

        public ISearchTweetsParameters CreateSearchTweetParameter(IGeoCode geoCode)
        {
            return new SearchTweetsParameters(geoCode);
        }

        public ISearchTweetsParameters CreateSearchTweetParameter(ICoordinates coordinates, int radius, DistanceMeasure measure)
        {
            return new SearchTweetsParameters(coordinates, radius, measure);
        }

        public ISearchTweetsParameters CreateSearchTweetParameter(double latitude, double longitude, int radius, DistanceMeasure measure)
        {
            return new SearchTweetsParameters(latitude, longitude, radius, measure);
        }

        public string GenerateSearchQueryParameter(string searchQuery)
        {
            return _twitterStringFormatter.TwitterEncode(searchQuery);
        }

        public string GenerateSearchTypeParameter(SearchResultType? searchType)
        {
            if (searchType == null)
            {
                return string.Empty;
            }

            return string.Format(Resources.SearchParameter_ResultType, searchType.ToString().ToLowerInvariant());
        }

        public string GenerateSinceParameter(DateTime since)
        {
            if (!_searchQueryValidator.IsDateTimeDefined(since))
            {
                return string.Empty;
            }

            return string.Format(Resources.SearchParameter_Since, since.ToString("yyyy-MM-dd"));
        }

        public string GenerateUntilParameter(DateTime until)
        {
            if (!_searchQueryValidator.IsDateTimeDefined(until))
            {
                return string.Empty;
            }

            return string.Format(Resources.SearchParameter_Until, until.ToString("yyyy-MM-dd"));
        }

        public string GenerateLocaleParameter(string locale)
        {
            if (!_searchQueryValidator.IsLocaleParameterValid(locale))
            {
                return string.Empty;
            }

            return locale;
        }

        public string GenerateLangParameter(Language lang)
        {
            if (!_searchQueryValidator.IsLangDefined(lang))
            {
                return string.Empty;
            }

            return string.Format(Resources.SearchParameter_Lang, lang.GetLanguageCode());
        }

        public string GenerateLangParameter(LanguageFilter? lang)
        {
            if (lang == null)
            {
                return string.Empty;
            }

            return string.Format(Resources.SearchParameter_Lang, lang.GetLanguageCode());
        }

        public string GenerateGeoCodeParameter(IGeoCode geoCode)
        {
            if (!_searchQueryValidator.IsGeoCodeValid(geoCode))
            {
                return null;
            }

            var latitude = geoCode.Coordinates.Latitude.ToString(CultureInfo.InvariantCulture);
            var longitude = geoCode.Coordinates.Longitude.ToString(CultureInfo.InvariantCulture);
            var radius = geoCode.Radius.ToString(CultureInfo.InvariantCulture);
            var measure = geoCode.DistanceMeasure == DistanceMeasure.Kilometers ? "km" : "mi";

            return string.Format(Resources.SearchParameter_GeoCode, latitude, longitude, radius, measure, CultureInfo.InvariantCulture);
        }

        public ISearchUsersParameters CreateUserSearchParameters(string query)
        {
            return new SearchUsersParameters(query);
        }
    }
}