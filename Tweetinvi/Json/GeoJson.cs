using System;
using System.Threading.Tasks;
using Tweetinvi.Controllers.Geo;

namespace Tweetinvi.Json
{
    public static class GeoJson
    {
        [ThreadStatic]
        private static IGeoJsonController _geoJsonController;
        public static IGeoJsonController GeoJsonController
        {
            get
            {
                if (_geoJsonController == null)
                {
                    Initialize();
                }

                return _geoJsonController;
            }
        }

        static GeoJson()
        {
            Initialize();
        }
        
        private static void Initialize()
        {
            _geoJsonController = TweetinviContainer.Resolve<IGeoJsonController>();
        }

        public static Task<string> GetPlaceFromId(string placeId)
        {
            return GeoJsonController.GetPlaceFromId(placeId);
        }
    }
}