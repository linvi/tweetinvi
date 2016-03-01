namespace Tweetinvi.Core.Interfaces.Models
{
    public interface ITrend
    {
        /// <summary>
        /// Name/Title of the trend.
        /// </summary>
        string Name { get; set; }

        /// <summary>
        /// Search URL returning the tweets related with this trend.
        /// </summary>
        string URL { get; set; }

        /// <summary>
        /// Only the query part of the search URL.
        /// </summary>
        string Query { get; set; }

        // TODO : ADD THE FIELD DESCRIPTION
        string PromotedContent { get; set; }

        /// <summary>
        /// Number of tweet matching the trend that have been posted for the last 24 hours.
        /// </summary>
        int? TweetVolume { get; set; }
    }
}