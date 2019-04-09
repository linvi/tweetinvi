using System;
using System.Threading.Tasks;
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

        public static Task<string> GetTrendsAt(long woeid)
        {
            return TrendsJsonController.GetPlaceTrendsAt(woeid);
        }

        public static Task<string> GetTrendsAt(IWoeIdLocation woeIdLocation)
        {
            return TrendsJsonController.GetPlaceTrendsAt(woeIdLocation);
        }
    }
}
