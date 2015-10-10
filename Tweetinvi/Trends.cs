using System;
using Tweetinvi.Core.Interfaces.Controllers;
using Tweetinvi.Core.Interfaces.Models;

namespace Tweetinvi
{
    public static class Trends
    {
        [ThreadStatic]
        private static ITrendsController _trendsController;

        /// <summary>
        /// Controller handling any Trends request
        /// </summary>
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

        static Trends()
        {
            Initialize();
        }

        private static void Initialize()
        {
            _trendsController = TweetinviContainer.Resolve<ITrendsController>();
        }

        /// <summary>
        /// Get the trends at a specific location
        /// </summary>
        public static IPlaceTrends GetTrendsAt(long woeid)
        {
            return TrendsController.GetPlaceTrendsAt(woeid);
        }

        /// <summary>
        /// Get the trends at a specific location
        /// </summary>
        public static IPlaceTrends GetTrendsAt(IWoeIdLocation woeIdLocation)
        {
            return TrendsController.GetPlaceTrendsAt(woeIdLocation);
        }
    }
}