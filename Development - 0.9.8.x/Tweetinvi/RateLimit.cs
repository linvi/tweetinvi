using System;
using Tweetinvi.Core;
using Tweetinvi.Core.Events.EventArguments;
using Tweetinvi.Core.Interfaces.Controllers;
using Tweetinvi.Core.Interfaces.Credentials;
using Tweetinvi.Core.Interfaces.RateLimit;
using Tweetinvi.Core.Interfaces.WebLogic;

namespace Tweetinvi
{
    public static class RateLimit
    {
        [ThreadStatic]
        private static IHelpController _helpController;
        public static IHelpController HelpController
        {
            get
            {
                if (_helpController == null)
                {
                    Initialize();
                }

                return _helpController;
            }
        }

        private static readonly IRateLimitAwaiter _rateLimitAwaiter;
        private static readonly IRateLimitCacheManager _rateLimitCacheManager;
        private static readonly IRateLimitCache _rateLimitCache;

        static RateLimit()
        {
            Initialize();

            _rateLimitAwaiter = TweetinviContainer.Resolve<IRateLimitAwaiter>();
            _rateLimitCacheManager = TweetinviContainer.Resolve<IRateLimitCacheManager>();
            _rateLimitCache = TweetinviContainer.Resolve<IRateLimitCache>();
        }

        static void Initialize()
        {
            _helpController = TweetinviContainer.Resolve<IHelpController>();
        }

        public static event EventHandler<QueryAwaitingEventArgs> QueryAwaitingForRateLimit
        {
            add { _rateLimitAwaiter.QueryAwaitingForRateLimit += value; }
            remove { _rateLimitAwaiter.QueryAwaitingForRateLimit -= value; }
        }

        public static RateLimitTrackerOptions RateLimitTrackerOption
        {
            get { return TweetinviConfig.CURRENT_RATELIMIT_TRACKER_OPTION; }
            set { TweetinviConfig.CURRENT_RATELIMIT_TRACKER_OPTION = value; }
        }

        public static void ClearRateLimitCache()
        {
            _rateLimitCache.ClearAll();
        }

        public static void ClearRateLimitCache(IOAuthCredentials credentials)
        {
            _rateLimitCache.Clear(credentials);
        }

        public static void AwaitForQueryRateLimit(string query)
        {
            AwaitForQueryRateLimit(query, TwitterCredentials.Credentials);
        }

        public static void AwaitForQueryRateLimit(string query, IOAuthCredentials credentials)
        {
            _rateLimitAwaiter.WaitForCredentialsRateLimit(query, credentials);
        }

        public static void AwaitForQueryRateLimit(ITokenRateLimit tokenRateLimit)
        {
            _rateLimitAwaiter.WaitForCredentialsRateLimit(tokenRateLimit);
        }

        public static ITokenRateLimit GetQueryRateLimit(string query)
        {
            return _rateLimitCacheManager.GetQueryRateLimit(query, TwitterCredentials.Credentials);
        }

        public static ITokenRateLimit GetQueryRateLimit(string query, IOAuthCredentials credentials)
        {
            return _rateLimitCacheManager.GetQueryRateLimit(query, credentials);
        }

        public static ITokenRateLimits GetCurrentCredentialsRateLimits(bool useRateLimitCache = false)
        {
            ITokenRateLimits tokenRateLimits = null;
            if (!useRateLimitCache)
            {
                tokenRateLimits = HelpController.GetCurrentCredentialsRateLimits();
                _rateLimitCacheManager.UpdateTokenRateLimits(TwitterCredentials.Credentials, tokenRateLimits);
            }
            else
            {
                tokenRateLimits = _rateLimitCacheManager.GetTokenRateLimits(TwitterCredentials.Credentials);
            }

            return tokenRateLimits;
        }

        public static ITokenRateLimits GetCredentialsRateLimits(IOAuthCredentials credentials, bool useRateLimitCache = false)
        {
            if (useRateLimitCache)
            {
                return _rateLimitCacheManager.GetTokenRateLimits(credentials);
            }
            else
            {
                return HelpController.GetCredentialsRateLimits(credentials);
            }
        }
    }
}