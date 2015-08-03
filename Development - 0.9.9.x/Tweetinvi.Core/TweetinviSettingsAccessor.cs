using System;
using System.Diagnostics;

namespace Tweetinvi.Core
{
    public interface ITweetinviSettingsAccessor
    {
        ITweetinviSettings CurrentThreadSettings { get; set; }
        ITweetinviSettings ApplicationSettings { get; set; }

        bool ShowDebug { get; set; }
        string ProxyURL { get; set; }
        int WebRequestTimeout { get; set; }
        RateLimitTrackerOptions RateLimitTrackerOption { get; set; }
    }

    public class TweetinviSettingsAccessor : ITweetinviSettingsAccessor
    {
        private static ITweetinviSettings StaticTweetinviSettings { get; set; }

        public TweetinviSettingsAccessor()
        {
            var threadSettings = TweetinviCoreModule.TweetinviContainer.Resolve<ITweetinviSettings>();
            threadSettings.WebRequestTimeout = 10000;

            CurrentThreadSettings = threadSettings;


# if DEBUG
            CurrentThreadSettings.ShowDebug = true;
#endif
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

        public bool ShowDebug
        {
            get { return CurrentThreadSettings.ShowDebug; }
            set { CurrentThreadSettings.ShowDebug = value; }
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

        public RateLimitTrackerOptions RateLimitTrackerOption
        {
            get { return CurrentThreadSettings.RateLimitTrackerOption; }
            set { CurrentThreadSettings.RateLimitTrackerOption = value; }
        }
    }
}