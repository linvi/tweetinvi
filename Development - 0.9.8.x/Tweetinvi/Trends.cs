using System;
using Tweetinvi.Core.Interfaces.Controllers;
using Tweetinvi.Core.Interfaces.Models;

namespace Tweetinvi
{
    public static class Trends
    {
        [ThreadStatic]
        private static ITrendsController _trendsController;

        static Trends()
        {
            Initialize();
        }

        public static ITrendsController TrendsController
        {
            get
            {
                if (_trendsController == null)
                {
                    Initialize();
                }

                return _trendsController;
            }
        }

        private static void Initialize()
        {
            _trendsController = TweetinviContainer.Resolve<ITrendsController>();
        }

        public static IPlaceTrends GetTrendsAt(long woeid)
        {
            return TrendsController.GetPlaceTrendsAt(woeid);
        }

        public static IPlaceTrends GetTrendsAt(IWoeIdLocation woeIdLocation)
        {
            return TrendsController.GetPlaceTrendsAt(woeIdLocation);
        }
    }
}