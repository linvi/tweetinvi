using System;
using Tweetinvi.Core;
using Tweetinvi.Core.Client;
using Tweetinvi.Models;

namespace Tweetinvi.Factories
{
    public class TwitterRequestFactory : ITwitterRequestFactory
    {
        private readonly ITweetinviSettingsAccessor _tweetinviSettingsAccessor;

        public TwitterRequestFactory(ITweetinviSettingsAccessor tweetinviSettingsAccessor)
        {
            _tweetinviSettingsAccessor = tweetinviSettingsAccessor;
        }

        public ITwitterRequest Create(ITwitterCredentials credentials)
        {
            var twitterQuery = new TwitterQuery
            {
                ProxyConfig = _tweetinviSettingsAccessor.ProxyConfig,
                Timeout = TimeSpan.FromMilliseconds(_tweetinviSettingsAccessor.HttpRequestTimeout),
                TwitterCredentials = credentials
            };

            var twitterRequest = new TwitterRequest
            {
                Query = twitterQuery,
                ExecutionContext = new TwitterExecutionContext
                {
                    RateLimitTrackerMode = _tweetinviSettingsAccessor.RateLimitTrackerMode
                }
            };

            return twitterRequest;
        }
    }
}
