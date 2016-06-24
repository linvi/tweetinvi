using System;
using Tweetinvi.Controllers.Trends;
using Tweetinvi.Models;

namespace Tweetinvi.Json
{
    public static class TrendsJson
    {
        [ThreadStatic]
        private static ITrendsJsonController _trendsJsonController;
        public static ITrendsJsonController TrendsJsonController
        {
            get
            {
                if (_trendsJsonController == null)
                {
                    Initialize();
                }

                return _trendsJsonController;
            }
        }

        static TrendsJson()
        {
            Initialize();
        }

        private static void Initialize()
        {
            _trendsJsonController = TweetinviContainer.Resolve<ITrendsJsonController>();
        }

        public static string GetTrendsAt(long woeid)
        {
            return TrendsJsonController.GetPlaceTrendsAt(woeid);
        }

        public static string GetTrendsAt(IWoeIdLocation woeIdLocation)
        {
            return TrendsJsonController.GetPlaceTrendsAt(woeIdLocation);
        }
    }
}
