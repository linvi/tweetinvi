namespace Tweetinvi.Parameters
{
    public enum RateLimitsSource
    {
        /// <summary>
        /// Gets the rate limits from the cache if they exists.
        /// If they do not exists, get from Twitter api and save in cache.
        /// </summary>
        CacheOrTwitterApi,

        /// <summary>
        /// Gets the rate limits from the cache only.
        /// If the cache does not have such rate limits, will return null.
        /// </summary>
        CacheOnly,

        /// <summary>
        /// Gets the rate limits from Twitter api.
        /// This does not try to get the rate limits from the cache nor does it save them there.
        /// </summary>
        TwitterApiOnly
    }

    /// <summary>
    /// For more information visit : https://developer.twitter.com/en/docs/developer-utilities/rate-limit-status/api-reference/get-application-rate_limit_status
    /// </summary>
    public interface IGetRateLimitsParameters : ICustomRequestParameters
    {
        /// <summary>
        /// How you want the rate limits to be retrieved.
        /// This parameter is not a parameter applied to the Twitter Api request.
        /// </summary>
        RateLimitsSource From { get; set; }

        /// <summary>
        /// Defines how the requests will be tracked
        /// </summary>
        RateLimitTrackerMode? TrackerMode { get; set; }
    }

    /// <inheritdoc/>
    public class GetRateLimitsParameters : CustomRequestParameters, IGetRateLimitsParameters
    {
        public GetRateLimitsParameters()
        {
        }

        public GetRateLimitsParameters(IGetRateLimitsParameters source)
        {
            if (source == null)
            {
                return;
            }

            From = source.From;
            TrackerMode = source.TrackerMode;
        }

        /// <inheritdoc/>
        public RateLimitsSource From { get; set; }

        /// <inheritdoc/>
        public RateLimitTrackerMode? TrackerMode { get; set; }
    }
}