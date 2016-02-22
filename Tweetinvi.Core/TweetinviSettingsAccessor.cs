using System;
using System.Diagnostics;

namespace Tweetinvi.Core
{
    public interface ITweetinviSettingsAccessor
    {
        ITweetinviSettings CurrentThreadSettings { get; set; }
        ITweetinviSettings ApplicationSettings { get; set; }

        string ProxyURL { get; set; }
        int WebRequestTimeout { get; set; }
        RateLimitTrackerMode RateLimitTrackerMode { get; set; }
    }

    public class TweetinviSettingsAccessor : ITweetinviSettingsAccessor
    {
        private static ITweetinviSettings StaticTweetinviSettings { get; set; }

        public TweetinviSettingsAccessor()
        {
            var threadSettings = TweetinviCoreModule.TweetinviContainer.Resolve<ITweetinviSettings>();
            threadSettings.WebRequestTimeout = 10000;

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

        public string ProxyURL
        {
            get { return CurrentThreadSettings.ProxyURL; }
            set { CurrentThreadSettings.ProxyURL = value; }
        }

        public int WebRequestTimeout
        {
            get { return CurrentThreadSettings.WebRequestTimeout; }
            set { CurrentThreadSettings.WebRequestTimeout = value; }
        }

        public RateLimitTrackerMode RateLimitTrackerMode
        {
            get { return CurrentThreadSettings.RateLimitTrackerMode; }
            set { CurrentThreadSettings.RateLimitTrackerMode = value; }
        }
    }
}