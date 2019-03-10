using System.Threading;
using Tweetinvi.Core.ExecutionContext;

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
        ITweetinviSettings ApplicationSettings { get; }

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
        /// Specify whether you want exceptions coming from Twitter are returning null instead of throwing
        /// </summary>
        bool OnTwitterExceptionReturnNull { get; set; }
    }

    public class TweetinviSettingsAccessor : ITweetinviSettingsAccessor, IAsyncContextPreparable
    {
        private static ITweetinviSettings StaticTweetinviSettings { get; }

        private static readonly AsyncLocal<ITweetinviSettings> _executionContextTweetinviSettings;
        private static readonly ThreadLocal<ITweetinviSettings> _threadSettings = new ThreadLocal<ITweetinviSettings>(() => null);
        private static readonly ThreadLocal<bool> _fromAwait = new ThreadLocal<bool>(() => false);

        static TweetinviSettingsAccessor()
        {
            StaticTweetinviSettings = new TweetinviSettings();
            _executionContextTweetinviSettings = new AsyncLocal<ITweetinviSettings>();
        }

        public TweetinviSettingsAccessor()
        {
            _fromAwait.Value = true;
            CurrentThreadSettings = new TweetinviSettings();
        }

        public ITweetinviSettings CurrentThreadSettings
        {
            get
            {
                InitializeThreadExecutionContext();
                return _threadSettings.Value ?? _executionContextTweetinviSettings.Value;
            }
            set
            {
                if (_fromAwait.Value)
                {
                    _executionContextTweetinviSettings.Value = value;
                }
                else
                {
                    _threadSettings.Value = value;
                }
            }
        }

        public ITweetinviSettings ApplicationSettings
        {
            get => StaticTweetinviSettings;
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

        public bool OnTwitterExceptionReturnNull
        {
            get => CurrentThreadSettings.OnTwitterExceptionReturnNull;
            set => CurrentThreadSettings.OnTwitterExceptionReturnNull = value;
        }

        public void InitializeThreadExecutionContext()
        {
            var isToLevelExecutionContext = _executionContextTweetinviSettings.Value == null;
            if (isToLevelExecutionContext)
            {
                _fromAwait.Value = true;
                _executionContextTweetinviSettings.Value = StaticTweetinviSettings.Clone();
                return;
            }

            if (!_fromAwait.Value && _threadSettings.Value == null)
            {
                //Debug.WriteLine("Duplicating Application value");
                _threadSettings.Value = _executionContextTweetinviSettings.Value.Clone();
            }
        }

        public void InitializeFromParentAsyncContext()
        {
            //Debug.WriteLine("-> InitializeFromParentAsyncContext");

            //if (_executionContextTweetinviSettings.Value == null && StaticTweetinviSettings != null)
            //{
            //    Debug.WriteLine("Duplicating Application value");
            //    _executionContextTweetinviSettings.Value = StaticTweetinviSettings.Clone();
            //}
        }

        public void InitializeFromChildAsyncContext()
        {
            _fromAwait.Value = true;
            //Debug.WriteLine("Initializing AsyncLocal<TweetinviSettings>");

            //Debug.WriteLine("Credentials InitializeFromParentAsyncContext");

            if (_executionContextTweetinviSettings.Value != null)
            {
                //Debug.WriteLine("Cloning current AsyncLocal<TweetinviSettings>");
                _executionContextTweetinviSettings.Value = _executionContextTweetinviSettings.Value.Clone();
                //as clone outside the await the change the parent asyncContext with the clone
            }
        }
    }
}