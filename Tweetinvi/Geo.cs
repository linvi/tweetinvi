using System;
using Tweetinvi.Core.Interfaces.Controllers;
using Tweetinvi.Core.Interfaces.Models;

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
    }
}