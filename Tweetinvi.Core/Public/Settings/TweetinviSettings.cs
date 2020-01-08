using System;
using Newtonsoft.Json;

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
        Extended = 0,
        Compat = 1
    }

    public interface ITweetinviSettings
    {
        /// <summary>
        /// Proxy used to execute Http Requests.
        /// </summary>
        IProxyConfig ProxyConfig { get; set; }

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
        /// How much additional time to wait than should be strictly necessary for a new batch of Twitter rate limits
        /// to be available. Required to account for timing discrepancies both within Twitter's servers and between
        /// Twitter and us.
        /// </summary>
        int RateLimitWaitFudgeMs { get; set; }

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
        /// Converters used by Tweetinvi to transform json received from Twitter
        /// into models understandable by Tweetinvi.
        /// </summary>
        JsonConverter[] Converters { get; set; }

        /// <summary>
        /// Limits that Tweetinvi will use to communicate with Twitter
        /// </summary>
        TwitterLimits Limits { get; set; }

        /// <summary>
        /// Initialize a setting from another one.
        /// </summary>
        void InitialiseFrom(ITweetinviSettings other);
    }

    public class TweetinviSettings : ITweetinviSettings
    {
        public const long DEFAULT_ID = -1;

        public IProxyConfig ProxyConfig { get; set; }
        public int HttpRequestTimeout { get; set; }
        public RateLimitTrackerMode RateLimitTrackerMode { get; set; }
        public int RateLimitWaitFudgeMs { get; set; }
        public TweetMode? TweetMode { get; set; }
        public int UploadTimeout { get; set; }
        public Func<DateTime> GetUtcDateTime { get; set; }
        public TwitterLimits Limits { get; set; }

        public TweetinviSettings()
        {
            GetUtcDateTime = () => DateTime.UtcNow;
            Limits = new TwitterLimits();
        }

        public TweetinviSettings(ITweetinviSettings source) : this()
        {
            if (source == null)
            {
                return;
            }

            ProxyConfig = source.ProxyConfig == null || source.ProxyConfig.Address == null ? null : new ProxyConfig(source.ProxyConfig);;
            HttpRequestTimeout = source.HttpRequestTimeout;
            UploadTimeout = source.UploadTimeout;
            RateLimitTrackerMode = source.RateLimitTrackerMode;
            RateLimitWaitFudgeMs = source.RateLimitWaitFudgeMs;
            TweetMode = source.TweetMode;
            GetUtcDateTime = source.GetUtcDateTime;
            Converters = source.Converters;
            Limits = new TwitterLimits(source.Limits);
        }

        public JsonConverter[] Converters { get; set; }

        public void InitialiseFrom(ITweetinviSettings other)
        {
            ProxyConfig = other.ProxyConfig;
            HttpRequestTimeout = other.HttpRequestTimeout;
            UploadTimeout = other.UploadTimeout;
            RateLimitTrackerMode = other.RateLimitTrackerMode;
            RateLimitWaitFudgeMs = other.RateLimitWaitFudgeMs;
            TweetMode = other.TweetMode;
            GetUtcDateTime = other.GetUtcDateTime;
            Converters = other.Converters;
            Limits = new TwitterLimits(other.Limits);
        }
    }
}