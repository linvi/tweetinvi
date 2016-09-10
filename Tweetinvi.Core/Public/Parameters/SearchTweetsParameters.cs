using System;
using Tweetinvi.Models;

namespace Tweetinvi.Parameters
{
    /// <summary>
    /// https://dev.twitter.com/rest/reference/get/search/tweets
    /// </summary>
    public interface ISearchTweetsParameters : ICustomRequestParameters
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
        LanguageFilter? Lang { get; set; }

        /// <summary>
        /// Restrict your query to a given location.
        /// </summary>
        IGeoCode GeoCode { get; set; }

        /// <summary>
        /// Choose if the result set will be represented by recent or popular Tweets, or even a mix of both.
        /// </summary>
        SearchResultType? SearchType { get; set; }

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
        void SetGeoCode(double latitude, double longitude, double radius, DistanceMeasure measure);

        /// <summary>
        /// Ensure that tweets returned by Twitter search returns tweet with geo information.
        /// This is necessary for people who wants to make sure that the Tweet was posted from the location requested.
        /// To learn more : https://github.com/linvi/tweetinvi/issues/333
        /// </summary>
        bool FilterTweetsNotContainingGeoInformation { get; set; }
    }

    /// <summary>
    /// https://dev.twitter.com/rest/reference/get/search/tweets
    /// </summary>
    public class SearchTweetsParameters : CustomRequestParameters, ISearchTweetsParameters
    {
        private SearchTweetsParameters()
        {
            MaximumNumberOfResults = TweetinviConsts.SEARCH_TWEETS_COUNT;
            
            SinceId = -1;
            MaxId = -1;

            TweetSearchType = TweetSearchType.All;
            Filters = TweetSearchFilters.None;
            FilterTweetsNotContainingGeoInformation = false;
        }

        public SearchTweetsParameters(string searchQuery) : this()
        {
            SearchQuery = searchQuery;
        }

        public SearchTweetsParameters(IGeoCode geoCode) : this()
        {
            GeoCode = geoCode;
        }

        public SearchTweetsParameters(ICoordinates coordinates, int radius, DistanceMeasure measure) : this()
        {
            GeoCode = new GeoCode(coordinates, radius, measure);
        }

        public SearchTweetsParameters(double latitude, double longitude, int radius, DistanceMeasure measure) : this()
        {
            GeoCode = new GeoCode(latitude, longitude, radius, measure);
        }

        public string SearchQuery { get; set; }
        public string Locale { get; set; }
        public int MaximumNumberOfResults { get; set; }

        public LanguageFilter? Lang { get; set; }
        public IGeoCode GeoCode { get; set; }
        public SearchResultType? SearchType { get; set; }

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

        public void SetGeoCode(double latitude, double longitude, double radius, DistanceMeasure measure)
        {
            GeoCode = new GeoCode(latitude, longitude, radius, measure);
        }

        public bool FilterTweetsNotContainingGeoInformation { get; set; }
    }
}