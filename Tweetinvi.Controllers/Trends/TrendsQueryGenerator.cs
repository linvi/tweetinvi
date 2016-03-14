using System;
using System.Text;
using Tweetinvi.Controllers.Properties;
using Tweetinvi.Controllers.Shared;
using Tweetinvi.Core.Extensions;
using Tweetinvi.Core.Interfaces.Models;

namespace Tweetinvi.Controllers.Trends
{
    public interface ITrendsQueryGenerator
    {
        string GetPlaceTrendsAtQuery(long woeid);
        string GetPlaceTrendsAtQuery(IWoeIdLocation woeIdLocation);
        string GetAvailableTrendLocationsQuery();
        string GetClosestTrendLocationsQuery(ICoordinates coordinates);
    }

    public class TrendsQueryGenerator : ITrendsQueryGenerator
    {
        private readonly IQueryParameterGenerator _queryParameterGenerator;

        public TrendsQueryGenerator(IQueryParameterGenerator queryParameterGenerator)
        {
            _queryParameterGenerator = queryParameterGenerator;
        }

        public string GetPlaceTrendsAtQuery(long woeid)
        {
            return string.Format(Resources.Trends_GetTrendsFromWoeId, woeid);
        }

        public string GetPlaceTrendsAtQuery(IWoeIdLocation woeIdLocation)
        {
            if (woeIdLocation == null)
            {
                throw new ArgumentException("WoeId cannot be null");
            }

            return GetPlaceTrendsAtQuery(woeIdLocation.WoeId);
        }

        public string GetAvailableTrendLocationsQuery()
        {
            return Resources.Trends_GetAvailableTrendsLocations;
        }

        public string GetClosestTrendLocationsQuery(ICoordinates coordinates)
        {
            var query = new StringBuilder(Resources.Trends_GetClosestTrendsLocations);

            query.AddParameterToQuery("lat", coordinates.Latitude);
            query.AddParameterToQuery("long", coordinates.Longitude);

            return query.ToString();
        }
    }
}