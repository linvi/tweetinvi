using System;
using Tweetinvi.Core.Enum;
using Tweetinvi.Core.Interfaces.Models;
using Tweetinvi.Core.Interfaces.Parameters;

namespace Tweetinvi.Core.Parameters
{
    public class TweetSearchParameters : CustomRequestParameters, ITweetSearchParameters
    {
        private TweetSearchParameters()
        {
            MaximumNumberOfResults = TweetinviConsts.SEARCH_TWEETS_COUNT;
            
            SinceId = -1;
            MaxId = -1;

            TweetSearchType = TweetSearchType.All;
            Filters = TweetSearchFilters.None;
        }

        public TweetSearchParameters(string searchQuery) : this()
        {
            SearchQuery = searchQuery;
        }

        public TweetSearchParameters(IGeoCode geoCode) : this()
        {
            GeoCode = geoCode;
        }

        public TweetSearchParameters(ICoordinates coordinates, int radius, DistanceMeasure measure) : this()
        {
            GeoCode = new GeoCode(coordinates, radius, measure);
        }

        public TweetSearchParameters(double longitude, double latitude, int radius, DistanceMeasure measure) : this()
        {
            GeoCode = new GeoCode(longitude, latitude, radius, measure);
        }

        public string SearchQuery { get; set; }
        public string Locale { get; set; }
        public int MaximumNumberOfResults { get; set; }

        public Language Lang { get; set; }
        public IGeoCode GeoCode { get; set; }
        public SearchResultType SearchType { get; set; }

        public DateTime Since { get; set; }
        public DateTime Until { get; set; }

        public long SinceId { get; set; }
        public long MaxId { get; set; }

        public TweetSearchType TweetSearchType { get; set; }
        public TweetSearchFilters Filters { get; set; }

        public void SetGeoCode(ICoordinates coordinates, double radius, DistanceMeasure measure)
        {
            GeoCode = new GeoCode(coordinates, radius, measure);
        }

        public void SetGeoCode(double longitude, double latitude, double radius, DistanceMeasure measure)
        {
            GeoCode = new GeoCode(longitude, latitude, radius, measure);
        }
    }
}