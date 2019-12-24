using System;
using Tweetinvi.Core.Controllers;
using Tweetinvi.Core.RateLimit;
using Tweetinvi.Events;
using Tweetinvi.Models;

namespace Tweetinvi
{
    /// <summary>
    /// Handle the RateLimits restricting the Twitter API.
    /// </summary>
    public static class RateLimit
    {
        public static IHelpController HelpController { get; private set; }

        private static IRateLimitCacheManager RateLimitCacheManager { get; set; }
        private static IRateLimitAwaiter RateLimitAwaiter { get; set; }

        private static readonly IRateLimitCache _rateLimitCache;

        static RateLimit()
        {
            Initialize();

            _rateLimitCache = TweetinviContainer.Resolve<IRateLimitCache>();
        }

        static void Initialize()
        {
            HelpController = TweetinviContainer.Resolve<IHelpController>();
            RateLimitCacheManager = TweetinviContainer.Resolve<IRateLimitCacheManager>();
            RateLimitAwaiter = TweetinviContainer.Resolve<IRateLimitAwaiter>();
        }

        /// <summary>
        /// Notify that a query is awaiting for RateLimits to become available in order to continue
        /// </summary>
        public static event EventHandler<QueryAwaitingEventArgs> QueryAwaitingForRateLimit
        {
            add { RateLimitAwaiter.QueryAwaitingForRateLimit += value; }
            remove { RateLimitAwaiter.QueryAwaitingForRateLimit -= value; }
        }

        /// <summary>
        /// Configure how to Tweetinvi will handle RateLimits
        /// </summary>
        public static RateLimitTrackerMode RateLimitTrackerMode
        {
            get { return TweetinviConfig.CurrentThreadSettings.RateLimitTrackerMode; }
            set { TweetinviConfig.CurrentThreadSettings.RateLimitTrackerMode = value; }
        }

        /// <summary>
        /// Clear all the RateLimits information stored in the cache
        /// </summary>
        public static void ClearRateLimitCache()
        {
            _rateLimitCache.ClearAll();
        }

        /// <summary>
        /// Clear a specific set of credentials RateLimits information stored in the cache
        /// </summary>
        public static void ClearRateLimitCache(ITwitterCredentials credentials)
        {
            _rateLimitCache.Clear(credentials);
        }
    }
}