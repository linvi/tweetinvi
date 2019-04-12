using System;
using System.Collections.Generic;
using System.Text;
using Tweetinvi.Core;
using Tweetinvi.Models;
using Tweetinvi.Models.Interfaces;

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
                Config = new TweetinviSettings
                {
                    RateLimitTrackerMode = _tweetinviSettingsAccessor.RateLimitTrackerMode
                }
            };

            return twitterRequest;
        }
    }
}
