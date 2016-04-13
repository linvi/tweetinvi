using System;
using Tweetinvi.Core.Enum;
using Tweetinvi.Core.Interfaces.Models;

namespace Tweetinvi.Core.Parameters
{
    /// <summary>
    /// https://dev.twitter.com/rest/reference/get/search/tweets
    /// </summary>
    public interface ITweetSearchParameters : ICustomRequestParameters
    {
        /// <summary>
        /// Query to search tweets.
        /// </summary>
        string SearchQuery { get; set; }

        /// <summary>
        /// Specify the language of the query you are sending (only ja is currently effective). 
        /// This is intended for language-specific consumers and the default should work in the majority of cases.
        /// </summary>
        string Locale { get; set; }

        /// <summary>
        /// Language identified for the tweet.
        /// </summary>
        Language Lang { get; set; }

        /// <summary>
        /// Restrict your query to a given location.
        /// </summary>
        IGeoCode GeoCode { get; set; }

        /// <summary>
        /// Choose if the result set will be represented by recent or popular Tweets, or even a mix of both.
        /// </summary>
        SearchResultType SearchType { get; set; }

        /// <summary>
        /// Maximum number of tweets the search will return.
        /// </summary>
        int MaximumNumberOfResults { get; set; }

        /// <summary>
        /// Search will only return tweets published after this date.
        /// </summary>
        DateTime Since { get; set; }

        /// <summary>
        /// Search will only return tweets published before this date.
        /// </summary>
        DateTime Until { get; set; }

        /// <summary>
        /// Returns tweets with an ID greater than the specified value.
        /// </summary>
        long SinceId { get; set; }

        /// <summary>
        /// Returns tweets with an ID lower than the specified value.
        /// </summary>
        long MaxId { get; set; }

        /// <summary>
        /// Filter to distinguish retweets from new tweets.
        /// </summary>
        TweetSearchType TweetSearchType { get; set; }

        /// <summary>
        /// Filters tweets based on metadata.
        /// </summary>
        TweetSearchFilters Filters { get; set; }

        /// <summary>
        /// Set the geo location where the search have to be performed.
        /// </summary>
        void SetGeoCode(ICoordinates coordinates, double radius, DistanceMeasure measure);

        /// <summary>
        /// Set the geo location where the search have to be performed.
        /// </summary>
        void SetGeoCode(double longitude, double latitude, double radius, DistanceMeasure measure);
    }

    /// <summary>
    /// https://dev.twitter.com/rest/reference/get/search/tweets
    /// </summary>
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