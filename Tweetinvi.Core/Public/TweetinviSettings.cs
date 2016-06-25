using System;

namespace Tweetinvi
{
    /// <summary>
    /// Provide a set of preconfigured solutions that you can use to track the Twitter rate limits.
    /// </summary>
    public enum RateLimitTrackerMode
    {
        /// <summary>
        /// By default Tweetinvi let you handle the RateLimits on your own
        /// </summary>
        None,

        /// <summary>
        /// This option will track the actions performed and update the internal RateLimits.
        /// If not enought RateLimits are available to perform the query, the current thread will
        /// await for the RateLimits to be available before continuing its process.
        /// </summary>
        TrackAndAwait,

        /// <summary>
        /// This option will only track the actions performed and update the internal RateLimits.
        /// This option won't pause a thread if you do not have enough RateLimits to perform a query.
        /// </summary>
        TrackOnly,
    }

    /// <summary>
    /// Specify whether you want your tweet to use Twitter extended mode.
    /// </summary>
    public enum TweetMode
    {
        Extended,
        Compat
    }

    public interface ITweetinviSettings
    {
        /// <summary>
        /// Proxy URL used to execute Http Requests.
        /// </summary>
        string ProxyURL { get; set; }

        /// <summary>
        /// Http Requests Timeout duration in milliseconds.
        /// </summary>
        int HttpRequestTimeout { get; set; }

        /// <summary>
        /// Upload Timeout duration in milliseconds.
        /// </summary>
        int UploadTimeout { get; set; }

        /// <summary>
        /// Solution used to track the RateLimits.
        /// </summary>
        RateLimitTrackerMode RateLimitTrackerMode { get; set; }

        /// <summary>
        /// Specify whether you want your tweet to use the extended mode.
        /// </summary>
        TweetMode? TweetMode { get; set; }

        /// <summary>
        /// A method allowing developers to specify how to retrieve the current DateTime.
        /// The DateTime must be valid for the HttpRequest signature to be accepted by Twitter.
        /// </summary>
        Func<DateTime> GetUtcDateTime { get; set; }

        /// <summary>
        /// Initialize a setting from another one.
        /// </summary>
        void InitialiseFrom(ITweetinviSettings other);

        /// <summary>
        /// Clone settings.
        /// </summary>
        ITweetinviSettings Clone();
    }

    public class TweetinviSettings : ITweetinviSettings
    {
        public const long DEFAULT_ID = -1;

        public string ProxyURL { get; set; }
        public int HttpRequestTimeout { get; set; }
        public RateLimitTrackerMode RateLimitTrackerMode { get; set; }
        public TweetMode? TweetMode { get; set; }
        public int UploadTimeout { get; set; }
        public Func<DateTime> GetUtcDateTime { get; set; }

        public TweetinviSettings()
        {
            GetUtcDateTime = () => DateTime.UtcNow;
        }

        public ITweetinviSettings Clone()
        {
            var clone = new TweetinviSettings();

            clone.ProxyURL = ProxyURL;
            clone.HttpRequestTimeout = HttpRequestTimeout;
            clone.UploadTimeout = UploadTimeout;
            clone.RateLimitTrackerMode = RateLimitTrackerMode;
            clone.TweetMode = TweetMode;
            clone.GetUtcDateTime = GetUtcDateTime;

            return clone;
        }

        public void InitialiseFrom(ITweetinviSettings other)
        {
            ProxyURL = other.ProxyURL;
            HttpRequestTimeout = other.HttpRequestTimeout;
            UploadTimeout = other.UploadTimeout;
            RateLimitTrackerMode = other.RateLimitTrackerMode;
            TweetMode = other.TweetMode;
            GetUtcDateTime = other.GetUtcDateTime;
        }
    }
}