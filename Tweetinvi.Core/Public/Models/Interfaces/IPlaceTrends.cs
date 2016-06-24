using System;
using System.Collections.Generic;

namespace Tweetinvi.Models
{
    public interface IPlaceTrends
    {
        DateTime AsOf { get; set; }
        DateTime CreatedAt { get; set; }
        List<IWoeIdLocation> woeIdLocations { get; set; }
        List<ITrend> Trends { get; set; }
    }
}