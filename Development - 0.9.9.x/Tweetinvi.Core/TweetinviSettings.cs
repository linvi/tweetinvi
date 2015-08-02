namespace Tweetinvi.Core
{
    public enum RateLimitTrackerOptions
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

    public interface ITweetinviSettings
    {
        bool ShowDebug { get; set; }
        string ProxyURL { get; set; }
        int WebRequestTimeout { get; set; }
        RateLimitTrackerOptions RateLimitTrackerOption { get; set; }

        void InitialiseFrom(ITweetinviSettings other);
        ITweetinviSettings Clone();
    }

    public class TweetinviSettings : ITweetinviSettings
    {
        public const long DEFAULT_ID = -1;

        public bool ShowDebug { get; set; }
        public string ProxyURL { get; set; }
        public int WebRequestTimeout { get; set; }
        public RateLimitTrackerOptions RateLimitTrackerOption { get; set; }

        public ITweetinviSettings Clone()
        {
            var clone = new TweetinviSettings();
            clone.ShowDebug = ShowDebug;
            clone.ProxyURL = ProxyURL;
            clone.WebRequestTimeout = WebRequestTimeout;
            clone.RateLimitTrackerOption = RateLimitTrackerOption;
            return clone;
        }

        public void InitialiseFrom(ITweetinviSettings other)
        {
            ShowDebug = other.ShowDebug;
            ProxyURL = other.ProxyURL;
            WebRequestTimeout = other.WebRequestTimeout;
            RateLimitTrackerOption = other.RateLimitTrackerOption;
        }
    }
}