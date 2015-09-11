using System;
using System.Globalization;
using Tweetinvi.Controllers.Properties;
using Tweetinvi.Core.Interfaces.Models;

namespace Tweetinvi.Controllers.Geo
{
    public interface IGeoQueryGenerator
    {
        string GetPlaceFromIdQuery(string placeId);
        string GeneratePlaceIdParameter(string placeId);
        string GenerateGeoParameter(ICoordinates coordinates);
    }

    public class GeoQueryGenerator : IGeoQueryGenerator
    {
        public string GeneratePlaceIdParameter(string placeId)
        {
            if (String.IsNullOrEmpty(placeId))
            {
                return null;
            }

            return String.Format(Resources.Geo_PlaceIdParameter, placeId);
        }

        public string GenerateGeoParameter(ICoordinates coordinates)
        {
            if (coordinates == null)
            {
                return null;
            }

            string latitudeValue = coordinates.Latitude.ToString(CultureInfo.InvariantCulture);
            string longitudeValue = coordinates.Longitude.ToString(CultureInfo.InvariantCulture);

            return String.Format(Resources.Geo_CoordinatesParameter, longitudeValue, latitudeValue);
        }

        public string GetPlaceFromIdQuery(string placeId)
        {
            if (placeId == null)
            {
                return null;
            }

            return String.Format(Resources.Geo_GetPlaceFromId, placeId);
        }
    }
}