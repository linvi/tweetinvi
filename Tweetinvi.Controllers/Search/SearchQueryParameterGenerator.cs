using System;
using System.Globalization;
using Tweetinvi.Controllers.Properties;
using Tweetinvi.Core.Enum;
using Tweetinvi.Core.Extensions;
using Tweetinvi.Core.Helpers;
using Tweetinvi.Core.Interfaces.Models;
using Tweetinvi.Core.Parameters;

namespace Tweetinvi.Controllers.Search
{
    public interface ISearchQueryParameterGenerator
    {
        ITweetSearchParameters CreateSearchTweetParameter(string query);
        ITweetSearchParameters CreateSearchTweetParameter(IGeoCode geoCode);
        ITweetSearchParameters CreateSearchTweetParameter(ICoordinates coordinates, int radius, DistanceMeasure measure);
        ITweetSearchParameters CreateSearchTweetParameter(double longitude, double latitude, int radius, DistanceMeasure measure);

        string GenerateSearchQueryParameter(string query);
        string GenerateSearchTypeParameter(SearchResultType searchType);
        string GenerateSinceParameter(DateTime since);
        string GenerateUntilParameter(DateTime until);
        string GenerateLocaleParameter(string locale);
        string GenerateLangParameter(Language lang);
        string GenerateGeoCodeParameter(IGeoCode geoCode);

        IUserSearchParameters CreateUserSearchParameters(string query);
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

        public ITweetSearchParameters CreateSearchTweetParameter(string query)
        {
            return new TweetSearchParameters(query);
        }

        public ITweetSearchParameters CreateSearchTweetParameter(IGeoCode geoCode)
        {
            return new TweetSearchParameters(geoCode);
        }

        public ITweetSearchParameters CreateSearchTweetParameter(ICoordinates coordinates, int radius, DistanceMeasure measure)
        {
            return new TweetSearchParameters(coordinates, radius, measure);
        }

        public ITweetSearchParameters CreateSearchTweetParameter(double longitude, double latitude, int radius, DistanceMeasure measure)
        {
            return new TweetSearchParameters(longitude, latitude, radius, measure);
        }

        public string GenerateSearchQueryParameter(string searchQuery)
        {
            return _twitterStringFormatter.TwitterEncode(searchQuery);
        }

        public string GenerateSearchTypeParameter(SearchResultType searchType)
        {
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

        public string GenerateGeoCodeParameter(IGeoCode geoCode)
        {
            if (!_searchQueryValidator.IsGeoCodeValid(geoCode))
            {
                return string.Empty;
            }

            string latitude = geoCode.Coordinates.Latitude.ToString(CultureInfo.InvariantCulture);
            string longitude = geoCode.Coordinates.Longitude.ToString(CultureInfo.InvariantCulture);
            string radius = geoCode.Radius.ToString(CultureInfo.InvariantCulture);
            string measure = geoCode.DistanceMeasure == DistanceMeasure.Kilometers ? "km" : "mi";
            return string.Format(Resources.SearchParameter_GeoCode, latitude, longitude, radius, measure, CultureInfo.InvariantCulture);
        }

        public IUserSearchParameters CreateUserSearchParameters(string query)
        {
            return new UserSearchParameters(query);
        }
    }
}