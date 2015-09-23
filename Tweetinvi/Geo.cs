using System;
using Tweetinvi.Core.Enum;
using Tweetinvi.Core.Interfaces.Controllers;
using Tweetinvi.Core.Interfaces.Factories;
using Tweetinvi.Core.Interfaces.Models;

namespace Tweetinvi
{
    public static class Geo
    {
        [ThreadStatic]
        private static IGeoFactory _geoFactory;
        public static IGeoFactory GeoFactory
        {
            get
            {
                if (_geoFactory == null)
                {
                    Initialize();
                }

                return _geoFactory;
            }
        }

        [ThreadStatic]
        private static IGeoController _geoController;
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
            _geoFactory = TweetinviContainer.Resolve<IGeoFactory>();
            _geoController = TweetinviContainer.Resolve<IGeoController>();
        }

        // Factory
        public static ICoordinates GenerateCoordinates(double longitude, double latitude)
        {
            return GeoFactory.GenerateCoordinates(longitude, latitude);
        }

        public static ILocation GenerateLocation(ICoordinates coordinates1, ICoordinates coordinates2)
        {
            return GeoFactory.GenerateLocation(coordinates1, coordinates2);
        }

        public static ILocation GenerateLocation(double longitude1, double latitude1,
                                                 double longitude2, double latitude2)
        {
            return GeoFactory.GenerateLocation(longitude1, latitude1, longitude2, latitude2);
        }

        public static IGeoCode GenerateGeoCode(ICoordinates coordinates, double radius, DistanceMeasure measure)
        {
            return GeoFactory.GenerateGeoCode(coordinates, radius, measure);
        }

        // Controller
        public static IPlace GetPlaceFromId(string placeId)
        {
            return GeoController.GetPlaceFromId(placeId);
        }
    }
}