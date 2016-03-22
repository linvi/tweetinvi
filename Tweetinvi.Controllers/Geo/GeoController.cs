using System.Collections.Generic;
using Tweetinvi.Core.Interfaces.Controllers;
using Tweetinvi.Core.Interfaces.Models;
using Tweetinvi.Core.Parameters;

namespace Tweetinvi.Controllers.Geo
{
    public class GeoController : IGeoController
    {
        private readonly IGeoQueryExecutor _geoQueryExecutor;

        public GeoController(IGeoQueryExecutor geoQueryExecutor)
        {
            _geoQueryExecutor = geoQueryExecutor;
        }

        /// <summary>
        /// A place in the world. These IDs can be retrieved from geo/reverse_geocode.
        /// </summary>
        public IPlace GetPlaceFromId(string placeId)
        {
            return _geoQueryExecutor.GetPlaceFromId(placeId);
        }

        /// <summary>
        /// Search for places that can be attached to a statuses/update. Given a latitude and a longitude pair, an IP address, or a name.
        /// </summary>
        public IEnumerable<IPlace> SearchGeo(IGeoSearchParameters parameters)
        {
            return _geoQueryExecutor.SearchGeo(parameters);
        }

        public IEnumerable<IPlace> SearchGeoReverse(IGeoSearchReverseParameters parameters)
        {
            return _geoQueryExecutor.SearchGeoReverse(parameters);
        }
    }
}