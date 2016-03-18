using Tweetinvi.Core;

namespace Tweetinvi
{
    public static class TweetinviConfig
    {
        private static readonly ITweetinviSettingsAccessor _currentSettingsAccessor;

        static TweetinviConfig()
        {
            _currentSettingsAccessor = TweetinviContainer.Resolve<ITweetinviSettingsAccessor>();
        }

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
        public static ITweetinviSettings CurrentThreadSettings
        {
            get { return _currentSettingsAccessor.CurrentThreadSettings; }
        }
    }
}