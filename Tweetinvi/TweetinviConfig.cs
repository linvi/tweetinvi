using Tweetinvi.Core;

namespace Tweetinvi
{
    public static class TweetinviConfig
    {
        /// <summary>
        /// Default settings used when creating a new Thread
        /// </summary>
        public static ITweetinviSettings ApplicationSettings
        {
            get { return _currentSettingsAccessor.ApplicationSettings; }
        }

        /// <summary>
        /// Current Thread settings
        /// </summary>
        public static ITweetinviSettings CurrentSettings
        {
            get { return _currentSettingsAccessor.CurrentThreadSettings; }
        }

        private static readonly ITweetinviSettingsAccessor _currentSettingsAccessor;

        static TweetinviConfig()
        {
            _currentSettingsAccessor = TweetinviContainer.Resolve<ITweetinviSettingsAccessor>();
        }

        /// <summary>
        /// URL of the proxy to use when performing any request
        /// </summary>
        public static string APPLICATION_PROXY_URL
        {
            get { return ApplicationSettings.ProxyURL; }
            set { ApplicationSettings.ProxyURL = value; }
        }

        /// <summary>
        /// Value indicating whether some debug information will be printed in the console.
        /// By default it is enabled in debug and disabled in release.
        /// </summary>
        public static bool APPLICATION_SHOW_DEBUG
        {
            get { return ApplicationSettings.ShowDebug; }
            set { ApplicationSettings.ShowDebug = value; }
        }

        /// <summary>
        /// Duration in milliseconds before Tweetinvi considers that the WebRequest has failed to execute.
        /// A value of -1 indicates that the query will wait indifinitely for a response.
        /// </summary>
        public static int APPLICATION_WEB_REQUEST_TIMEOUT
        {
            get { return ApplicationSettings.WebRequestTimeout; }
            set { ApplicationSettings.WebRequestTimeout = value; }
        }

        /// <summary>
        /// When a query is performed Tweetinvi will wait for the rate limit to be available before executing the query
        /// </summary>
        public static RateLimitTrackerOptions APPLICATION_RATELIMIT_TRACKER_OPTION
        {
            get { return ApplicationSettings.RateLimitTrackerOption; }
            set { ApplicationSettings.RateLimitTrackerOption = value; }
        }

        /// <summary>
        /// URL of the proxy to use when performing any request
        /// </summary>
        public static string CURRENT_PROXY_URL
        {
            get { return CurrentSettings.ProxyURL; }
            set { CurrentSettings.ProxyURL = value; }
        }

        /// <summary>
        /// Value indicating whether some debug information will be printed in the console.
        /// By default it is enabled in debug and disabled in release.
        /// </summary>
        public static bool CURRENT_SHOW_DEBUG
        {
            get { return CurrentSettings.ShowDebug; }
            set { CurrentSettings.ShowDebug = value; }
        }

        /// <summary>
        /// Duration in milliseconds before Tweetinvi considers that the WebRequest has failed to execute.
        /// A value of -1 indicates that the query will wait indifinitely for a response.
        /// </summary>
        public static int CURRENT_WEB_REQUEST_TIMEOUT
        {
            get { return CurrentSettings.WebRequestTimeout; }
            set { CurrentSettings.WebRequestTimeout = value; }
        }

        /// <summary>
        /// When a query is performed Tweetinvi will wait for the rate limit to be available before executing the query
        /// </summary>
        public static RateLimitTrackerOptions CURRENT_RATELIMIT_TRACKER_OPTION
        {
            get { return CurrentSettings.RateLimitTrackerOption; }
            set { CurrentSettings.RateLimitTrackerOption = value; }
        }
    }
}