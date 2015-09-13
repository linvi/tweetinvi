using System;
using Tweetinvi.Core.Enum;
using Tweetinvi.Core.Interfaces.Models;

namespace Tweetinvi.Core.Interfaces.Parameters
{
    public enum TweetSearchType
    {
        All,
        OriginalTweetsOnly,
        RetweetsOnly,
    }

    [Flags]
    public enum TweetSearchFilters
    {
        None = 1,
        Hashtags = 2,
        Links = 4,
        Images = 8,
        News = 16,
        Replies = 32,
        Verified = 64,
        Videos = 128
    }

    public interface ITweetSearchParameters : ICustomRequestParameters
    {
        string SearchQuery { get; set; }
    
        string Locale { get; set; }
        Language Lang { get; set; }
        IGeoCode GeoCode { get; set; }
        SearchResultType SearchType { get; set; }

        int MaximumNumberOfResults { get; set; }
        
        DateTime Since { get; set; }
        DateTime Until { get; set; }

        long SinceId { get; set; }
        long MaxId { get; set; }

        TweetSearchType TweetSearchType { get; set; }
        TweetSearchFilters Filters { get; set; }

        void SetGeoCode(ICoordinates coordinates, double radius, DistanceMeasure measure);
        void SetGeoCode(double longitude, double latitude, double radius, DistanceMeasure measure);
    }
}