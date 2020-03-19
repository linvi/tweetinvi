using System;
using Tweetinvi.Models;
using Tweetinvi.Parameters.Enum;

namespace Tweetinvi.Parameters
{
    /// <summary>
    /// For more information read : https://developer.twitter.com/en/docs/tweets/search/api-reference/get-search-tweets
    /// <para>Learn more about query here : https://developer.twitter.com/en/docs/tweets/search/guides/standard-operators</para>
    /// </summary>
    public interface ISearchTweetsParameters : IMinMaxQueryParameters
    {
        /// <summary>
        /// Query to search tweets.
        /// </summary>
        string Query { get; set; }

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
        /// Search will only return tweets published after this date.
        /// </summary>
        DateTime? Since { get; set; }

        /// <summary>
        /// Search will only return tweets published before this date.
        /// </summary>
        DateTime? Until { get; set; }

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
        /// Include tweet entities.
        /// </summary>
        bool? IncludeEntities { get; set; }

        /// <summary>
        /// Define whether you want to use the Tweet extended or compatibility mode
        /// </summary>
        TweetMode? TweetMode { get; set; }
    }

    /// <summary>
    /// https://dev.twitter.com/rest/reference/get/search/tweets
    /// </summary>
    public class SearchTweetsParameters : MinMaxQueryParameters, ISearchTweetsParameters
    {
        private SearchTweetsParameters()
        {
            Filters = TweetSearchFilters.None;
            PageSize = TwitterLimits.DEFAULTS.SEARCH_MAX_PAGE_SIZE;
        }

        public SearchTweetsParameters(string searchQuery) : this()
        {
            Query = searchQuery;
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

        public SearchTweetsParameters(ISearchTweetsParameters source) : base(source)
        {
            if (source == null)
            {
                return;
            }

            Query = source.Query;
            Locale = source.Locale;
            Lang = source.Lang;
            GeoCode = new GeoCode(source.GeoCode);
            SearchType = source.SearchType;
            Since = source.Since;
            Until = source.Until;
            Filters = source.Filters;
            IncludeEntities = source.IncludeEntities;
            TweetMode = source.TweetMode;
        }

        public string Query { get; set; }
        public string Locale { get; set; }

        public LanguageFilter? Lang { get; set; }
        public IGeoCode GeoCode { get; set; }
        public SearchResultType? SearchType { get; set; }

        public DateTime? Since { get; set; }
        public DateTime? Until { get; set; }

        public TweetSearchFilters Filters { get; set; }

        public void SetGeoCode(ICoordinates coordinates, double radius, DistanceMeasure measure)
        {
            GeoCode = new GeoCode(coordinates, radius, measure);
        }

        public void SetGeoCode(double latitude, double longitude, double radius, DistanceMeasure measure)
        {
            GeoCode = new GeoCode(latitude, longitude, radius, measure);
        }

        public bool? IncludeEntities { get; set; }

        public TweetMode? TweetMode { get; set; }
    }
}