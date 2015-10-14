using System;
using Tweetinvi.Controllers.Properties;
using Tweetinvi.Core.Interfaces.Models;

namespace Tweetinvi.Controllers.Trends
{
    public interface ITrendsQueryGenerator
    {
        string GetPlaceTrendsAtQuery(long woeid);
        string GetPlaceTrendsAtQuery(IWoeIdLocation woeIdLocation);
    }

    public class TrendsQueryGenerator : ITrendsQueryGenerator
    {
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
    }
}