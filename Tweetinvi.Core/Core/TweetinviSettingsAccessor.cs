using System;
using Tweetinvi.Core.Injectinvi;

namespace Tweetinvi.Core
{
    /// <summary>
    /// @Injectable : use in order to retrieve tweetinvi settings anywhere in the application.
    /// </summary>
    public interface ITweetinviSettingsAccessor
    {
        /// <summary>
        /// Current thread settings.
        /// </summary>
        ITweetinviSettings CurrentThreadSettings { get; set; }

        /// <summary>
        /// Application thread settings.
        /// </summary>
        ITweetinviSettings ApplicationSettings { get; set; }

        /// <summary>
        /// Proxy URL used by the current thread.
        /// </summary>
        IProxyConfig ProxyConfig { get; set; }

        /// <summary>
        /// Http requests timeout in the current thread.
        /// </summary>
        int HttpRequestTimeout { get; set; }

        /// <summary>
        /// Upload timeout in the current thread.
        /// </summary>
        int UploadTimeout { get; set; }

        /// <summary>
        /// Solution used to track the rate limits in the current thread.
        /// </summary>
        RateLimitTrackerMode RateLimitTrackerMode { get; set; }

        /// <summary>
        /// How much additional time to wait than should be strictly necessary for a new batch of Twitter rate limits
        /// to be available. Required to account for timing discrepancies both within Twitter's servers and between
        /// Twitter and us. 
        /// </summary>
        int RateLimitWaitFudgeMs { get; set; }
    }

    public class TweetinviSettingsAccessor : ITweetinviSettingsAccessor
    {
        private static ITweetinviSettings StaticTweetinviSettings { get; set; }

        public TweetinviSettingsAccessor()
        {
            var threadSettings = TweetinviCoreModule.TweetinviContainer.Resolve<ITweetinviSettings>();
            threadSettings.HttpRequestTimeout = 10000;
            threadSettings.UploadTimeout = 60000;
            threadSettings.RateLimitWaitFudgeMs = 5000;

            CurrentThreadSettings = threadSettings;
        }

        [ThreadStatic]
        private static ITweetinviSettings _currentThreadSettings;
        public ITweetinviSettings CurrentThreadSettings
        {
            get
            {
                if (_currentThreadSettings == null)
                {
                    InitialiseSettings();
                }

                return _currentThreadSettings;
            }
            set
            {
                _currentThreadSettings = value;

                if (!HasTheApplicationSettingsBeenInitialized() && _currentThreadSettings != null)
                {
                    StaticTweetinviSettings = value.Clone();
                }
            }
        }

        private void InitialiseSettings()
        {
            _currentThreadSettings = TweetinviCoreModule.TweetinviContainer.Resolve<ITweetinviSettings>();
            _currentThreadSettings.InitialiseFrom(StaticTweetinviSettings);
        }

        public ITweetinviSettings ApplicationSettings
        {
            get { return StaticTweetinviSettings; }
            set
            {
                StaticTweetinviSettings = value;

                if (_currentThreadSettings != null)
                {
                    _currentThreadSettings = value;
                }
            }
        }

        private bool HasTheApplicationSettingsBeenInitialized()
        {
            return StaticTweetinviSettings != null;
        }

        public IProxyConfig ProxyConfig
        {
            get { return CurrentThreadSettings.ProxyConfig; }
            set { CurrentThreadSettings.ProxyConfig = value; }
        }

        public int HttpRequestTimeout
        {
            get { return CurrentThreadSettings.HttpRequestTimeout; }
            set { CurrentThreadSettings.HttpRequestTimeout = value; }
        }

        public int UploadTimeout
        {
            get { return CurrentThreadSettings.UploadTimeout; }
            set { CurrentThreadSettings.UploadTimeout = value; }
        }

        public RateLimitTrackerMode RateLimitTrackerMode
        {
            get { return CurrentThreadSettings.RateLimitTrackerMode; }
            set { CurrentThreadSettings.RateLimitTrackerMode = value; }
        }

        public int RateLimitWaitFudgeMs
        {
            get => CurrentThreadSettings.RateLimitWaitFudgeMs;
            set => CurrentThreadSettings.RateLimitWaitFudgeMs = value;
        }
    }
}