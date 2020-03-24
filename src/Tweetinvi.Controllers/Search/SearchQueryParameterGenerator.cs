using System;
using System.Globalization;
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

        string GenerateSinceParameter(DateTime? since);
        string GenerateUntilParameter(DateTime? until);
        string GenerateGeoCodeParameter(IGeoCode geoCode);

        ISearchUsersParameters CreateUserSearchParameters(string query);
    }

    public class SearchQueryParameterGenerator : ISearchQueryParameterGenerator
    {
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

        public string GenerateSinceParameter(DateTime? since)
        {
            if (since == null)
            {
                return string.Empty;
            }

            return $"since={since.Value.ToString("yyyy-MM-dd")}";
        }

        public string GenerateUntilParameter(DateTime? until)
        {
            if (until == null)
            {
                return string.Empty;
            }

            return $"until={until.Value.ToString("yyyy-MM-dd")}";
        }

        public string GenerateGeoCodeParameter(IGeoCode geoCode)
        {
            if (geoCode?.Coordinates == null)
            {
                return null;
            }

            var latitude = geoCode.Coordinates.Latitude.ToString(CultureInfo.InvariantCulture);
            var longitude = geoCode.Coordinates.Longitude.ToString(CultureInfo.InvariantCulture);
            var radius = geoCode.Radius.ToString(CultureInfo.InvariantCulture);
            var measure = geoCode.DistanceMeasure == DistanceMeasure.Kilometers ? "km" : "mi";

            return $"{latitude},{longitude},{radius}{measure}";
        }

        public ISearchUsersParameters CreateUserSearchParameters(string query)
        {
            return new SearchUsersParameters(query);
        }
    }
}