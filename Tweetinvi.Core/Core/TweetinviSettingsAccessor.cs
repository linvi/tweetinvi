using System;
using System.Threading;
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
    }

    public class TweetinviSettingsAccessor : ITweetinviSettingsAccessor
    {
        private static ITweetinviSettings StaticTweetinviSettings { get; set; }

        private static readonly AsyncLocal<ITweetinviSettings> _currentThreadSettings =
            new AsyncLocal<ITweetinviSettings>();

        public TweetinviSettingsAccessor()
        {
            var threadSettings = TweetinviCoreModule.TweetinviContainer.Resolve<ITweetinviSettings>();
            threadSettings.HttpRequestTimeout = 10000;
            threadSettings.UploadTimeout = 60000;

            CurrentThreadSettings = threadSettings;
        }

        public ITweetinviSettings CurrentThreadSettings
        {
            get
            {
                if (_currentThreadSettings.Value == null)
                {
                    _currentThreadSettings.Value = TweetinviCoreModule.TweetinviContainer.Resolve<ITweetinviSettings>();
                    _currentThreadSettings.Value.InitialiseFrom(StaticTweetinviSettings);
                }

                return _currentThreadSettings.Value;
            }
            set
            {
                _currentThreadSettings.Value = value;

                if (!HasTheApplicationSettingsBeenInitialized() && value != null)
                {
                    StaticTweetinviSettings = value.Clone();
                }
            }
        }

        public ITweetinviSettings ApplicationSettings
        {
            get => StaticTweetinviSettings;
            set
            {
                StaticTweetinviSettings = value;

                if (_currentThreadSettings.Value != null)
                {
                    _currentThreadSettings.Value = value;
                }
            }
        }

        private bool HasTheApplicationSettingsBeenInitialized()
        {
            return StaticTweetinviSettings != null;
        }

        public IProxyConfig ProxyConfig
        {
            get => CurrentThreadSettings.ProxyConfig;
            set => CurrentThreadSettings.ProxyConfig = value;
        }

        public int HttpRequestTimeout
        {
            get => CurrentThreadSettings.HttpRequestTimeout;
            set => CurrentThreadSettings.HttpRequestTimeout = value;
        }

        public int UploadTimeout
        {
            get => CurrentThreadSettings.UploadTimeout;
            set => CurrentThreadSettings.UploadTimeout = value;
        }

        public RateLimitTrackerMode RateLimitTrackerMode
        {
            get => CurrentThreadSettings.RateLimitTrackerMode;
            set => CurrentThreadSettings.RateLimitTrackerMode = value;
        }
    }
}