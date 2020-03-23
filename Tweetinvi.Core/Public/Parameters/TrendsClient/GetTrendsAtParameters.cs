namespace Tweetinvi.Parameters.TrendsClient
{
    public enum GetTrendsExclude
    {
        Nothing,
        Hashtags
    }

    /// <summary>
    /// For more information read : https://developer.twitter.com/en/docs/trends/trends-for-location/api-reference/get-trends-place
    /// </summary>
    public interface IGetTrendsAtParameters : ICustomRequestParameters
    {
        /// <summary>
        /// The Yahoo! Where On Earth ID of the location to return trending information for.
        /// Global information is available by using 1 as the WOEID .
        /// </summary>
        long Woeid { get; set; }

        /// <summary>
        /// Setting this equal to hashtags will remove all hashtags from the trends list.
        /// </summary>
        GetTrendsExclude? Exclude { get; set; }
    }

    /// <inheritdoc />
    public class GetTrendsAtParameters : CustomRequestParameters, IGetTrendsAtParameters
    {
        public GetTrendsAtParameters(long woeid)
        {
            Woeid = woeid;
        }

        /// <inheritdoc />
        public long Woeid { get; set; }
        /// <inheritdoc />
        public GetTrendsExclude? Exclude { get; set; }
    }
}