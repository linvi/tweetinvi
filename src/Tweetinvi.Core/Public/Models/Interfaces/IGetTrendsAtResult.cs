using System;
using System.Collections.Generic;

namespace Tweetinvi.Models
{
    public interface IGetTrendsAtResult
    {
        DateTime AsOf { get; set; }
        DateTime CreatedAt { get; set; }
        IWoeIdLocation[] WoeIdLocations { get; set; }
        ITrend[] Trends { get; set; }
    }
}