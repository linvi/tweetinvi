using System;
using System.Globalization;
using System.Text;
using Tweetinvi.Controllers.Properties;
using Tweetinvi.Core.Extensions;
using Tweetinvi.Core.QueryGenerators;
using Tweetinvi.Models;
using Tweetinvi.Parameters;

namespace Tweetinvi.Controllers.Help
{
    public class HelpQueryGenerator : IHelpQueryGenerator
    {
        public string GetRateLimitsQuery(IGetRateLimitsParameters parameters)
        {
            var query = new StringBuilder(Resources.Help_GetRateLimit);
            query.AddFormattedParameterToQuery(parameters.FormattedCustomQueryParameters);
            return query.ToString();
        }

        public string GetTwitterConfigurationQuery(IGetTwitterConfigurationParameters parameters)
        {
            var query = new StringBuilder(Resources.Help_GetTwitterConfiguration);
            query.AddFormattedParameterToQuery(parameters.FormattedCustomQueryParameters);
            return query.ToString();
        }

        public string GetSupportedLanguagesQuery(IGetSupportedLanguagesParameters parameters)
        {
            var query = new StringBuilder(Resources.Help_GetSupportedLanguages);
            query.AddFormattedParameterToQuery(parameters.FormattedCustomQueryParameters);
            return query.ToString();
        }

        public string GetPlaceQuery(IGetPlaceParameters parameters)
        {
            var query = new StringBuilder(string.Format(Resources.Geo_GetPlaceFromId, parameters.PlaceId));
            query.AddFormattedParameterToQuery(parameters.FormattedCustomQueryParameters);
            return query.ToString();
        }

        public string GenerateGeoParameter(ICoordinates coordinates)
        {
            if (coordinates == null)
            {
                throw new ArgumentNullException(nameof(coordinates));
            }

            string latitudeValue = coordinates.Latitude.ToString(CultureInfo.InvariantCulture);
            string longitudeValue = coordinates.Longitude.ToString(CultureInfo.InvariantCulture);

            return string.Format(Resources.Geo_CoordinatesParameter, longitudeValue, latitudeValue);
        }

        public string GetSearchGeoQuery(IGeoSearchParameters parameters)
        {
            if (string.IsNullOrEmpty(parameters.Query) &&
                string.IsNullOrEmpty(parameters.Ip) &&
                parameters.Coordinates == null &&
                parameters.Attributes.IsNullOrEmpty())
            {
                throw new ArgumentException("You must provide valid coordinates, Ip address, query, or attributes.");
            }

            var query = new StringBuilder(Resources.Geo_SearchGeo);

            query.AddParameterToQuery("query", parameters.Query);
            query.AddParameterToQuery("ip", parameters.Ip);

            if (parameters.Coordinates != null)
            {
                query.AddParameterToQuery("lat", parameters.Coordinates.Latitude);
                query.AddParameterToQuery("long", parameters.Coordinates.Longitude);
            }

            foreach (var attribute in parameters.Attributes)
            {
                query.AddParameterToQuery(string.Format("attribute:{0}", attribute.Key), attribute.Value);
            }

            if (parameters.Granularity != Granularity.Undefined)
            {
                query.AddParameterToQuery("granularity", parameters.Granularity.ToString().ToLowerInvariant());
            }

            if (parameters.Accuracy != null)
            {
                var accuracyMeasure = parameters.AccuracyMeasure == AccuracyMeasure.Feets ? "ft" : "m";
                query.AddParameterToQuery("accuracy", $"{parameters.Accuracy}{accuracyMeasure}");
            }

            query.AddParameterToQuery("max_results", parameters.MaximumNumberOfResults);
            query.AddParameterToQuery("contained_within", parameters.ContainedWithin);
            query.AddParameterToQuery("callback", parameters.Callback);

            query.AddFormattedParameterToQuery(parameters.FormattedCustomQueryParameters);

            return query.ToString();
        }

        public string GetSearchGeoReverseQuery(IGeoSearchReverseParameters parameters)
        {
            if (parameters.Coordinates == null)
            {
                throw new ArgumentException("You must provide valid coordinates.");
            }

            var query = new StringBuilder(Resources.Geo_SearchGeoReverse);

            if (parameters.Coordinates != null)
            {
                query.AddParameterToQuery("lat", parameters.Coordinates.Latitude);
                query.AddParameterToQuery("long", parameters.Coordinates.Longitude);
            }

            if (parameters.Granularity != Granularity.Undefined)
            {
                query.AddParameterToQuery("granularity", parameters.Granularity.ToString().ToLowerInvariant());
            }

            query.AddParameterToQuery("accuracy", parameters.Accuracy);
            query.AddParameterToQuery("max_results", parameters.MaximumNumberOfResults);
            query.AddParameterToQuery("callback", parameters.Callback);

            query.AddFormattedParameterToQuery(parameters.FormattedCustomQueryParameters);

            return query.ToString();
        }

        public string GetPlaceFromIdQuery(string placeId)
        {
            if (placeId == null)
            {
                throw new ArgumentNullException(nameof(placeId));
            }

            if (placeId == "")
            {
                throw new ArgumentException("Cannot be empty", nameof(placeId));
            }

            return string.Format(Resources.Geo_GetPlaceFromId, placeId);
        }
    }
}