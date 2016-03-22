using System;
using System.Globalization;
using System.Text;
using Tweetinvi.Controllers.Properties;
using Tweetinvi.Core.Extensions;
using Tweetinvi.Core.Interfaces.Models;
using Tweetinvi.Core.Parameters;

namespace Tweetinvi.Controllers.Geo
{
    public interface IGeoQueryGenerator
    {
        string GetPlaceFromIdQuery(string placeId);
        string GeneratePlaceIdParameter(string placeId);
        string GenerateGeoParameter(ICoordinates coordinates);
        string GetSearchGeoQuery(IGeoSearchParameters parameters);
        string GetSearchGeoReverseQuery(IGeoSearchReverseParameters parameters);
    }

    public class GeoQueryGenerator : IGeoQueryGenerator
    {
        public string GeneratePlaceIdParameter(string placeId)
        {
            if (String.IsNullOrEmpty(placeId))
            {
                return null;
            }

            return string.Format(Resources.Geo_PlaceIdParameter, placeId);
        }

        public string GenerateGeoParameter(ICoordinates coordinates)
        {
            if (coordinates == null)
            {
                return null;
            }

            string latitudeValue = coordinates.Latitude.ToString(CultureInfo.InvariantCulture);
            string longitudeValue = coordinates.Longitude.ToString(CultureInfo.InvariantCulture);

            return string.Format(Resources.Geo_CoordinatesParameter, longitudeValue, latitudeValue);
        }

        public string GetSearchGeoQuery(IGeoSearchParameters parameters)
        {
            if (string.IsNullOrEmpty(parameters.Query) &&
                string.IsNullOrEmpty(parameters.IP) &&
                parameters.Coordinates == null &&
                parameters.Attributes.IsNullOrEmpty())
            {
                throw new ArgumentException("You must provide valid coordinates, IP address, query, or attributes.");
            }

            var query = new StringBuilder(Resources.Geo_SearchGeo);

            query.AddParameterToQuery("query", parameters.Query);
            query.AddParameterToQuery("ip", parameters.IP);

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

            query.AddParameterToQuery("accuracy", parameters.Accuracy);
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
                return null;
            }

            return string.Format(Resources.Geo_GetPlaceFromId, placeId);
        }
    }
}