using System;
using System.Collections.Generic;

namespace Tweetinvi.Models
{
    public interface IGetTrendsAtResult
    {
        DateTimeOffset AsOf { get; set; }
        DateTimeOffset CreatedAt { get; set; }
        IWoeIdLocation[] WoeIdLocations { get; set; }
        ITrend[] Trends { get; set; }
    }
}