using System;
using System.Collections.Generic;
using Tweetinvi.Core.Interfaces.Controllers;
using Tweetinvi.Core.Interfaces.Models;
using Tweetinvi.Core.Parameters;

namespace Tweetinvi
{
    public static class Geo
    {
        [ThreadStatic]
        private static IGeoController _geoController;

        /// <summary>
        /// Controller handling any Geo request
        /// </summary>
        public static IGeoController GeoController
        {
            get
            {
                if (_geoController == null)
                {
                    Initialize();
                }

                return _geoController;
            }
        }

        static Geo()
        {
            Initialize();
        }

        private static void Initialize()
        {
            _geoController = TweetinviContainer.Resolve<IGeoController>();
        }

        // CONTROLLER

        /// <summary>
        /// Get a place information from place identifier.
        /// <see cref="https://dev.twitter.com/rest/reference/get/geo/id/%3Aplace_id">Learn More</see>
        /// </summary>
        public static IPlace GetPlaceFromId(string placeId)
        {
            return GeoController.GetPlaceFromId(placeId);
        }

        /// <summary>
        /// Search for places that can be attached to a statuses/update. Given a latitude and a longitude pair, an IP address, or a name.
        /// </summary>
        public static IEnumerable<IPlace> SearchGeo(IGeoSearchParameters parameters)
        {
            return GeoController.SearchGeo(parameters);
        }

        /// <summary>
        /// Given a latitude and a longitude, searches for up to 20 places that can be used as a place_id when updating a status.
        /// </summary>
        public static IEnumerable<IPlace> SearchGeoReverse(IGeoSearchReverseParameters parameters)
        {
            return GeoController.SearchGeoReverse(parameters);
        }
    }
}