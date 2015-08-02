using System;
using System.Collections.Generic;
using Tweetinvi.Core.Credentials;
using Tweetinvi.Core.Helpers;
using Tweetinvi.Core.Injectinvi;
using Tweetinvi.Core.Interfaces.Controllers;
using Tweetinvi.Core.Interfaces.Credentials;
using Tweetinvi.Core.Interfaces.RateLimit;
using Tweetinvi.Core.Interfaces.WebLogic;

namespace Tweetinvi.Credentials.RateLimit
{
    /// <summary>
    /// IMPORTANT : This class needs to be ThreadSafe because it is registered as InstancePerApplication
    /// We want the cache to be updated once the second thread has completed the operation
    /// </summary>
    public class RateLimitCache : IRateLimitCache
    {
        private readonly IHelpQueryGenerator _helpQueryGenerator;
        private readonly IJsonObjectConverter _jsonObjectConverter;
        private readonly ITweetinviContainer _tweetinviContainer;
        private readonly Dictionary<ITwitterCredentials, ITokenRateLimits> _tokenRateLimits;

        private readonly object _lockRefresh = new Object();
        private readonly object _lockTokenRateLimitsDictionary = new object();

        public RateLimitCache(
            IHelpQueryGenerator helpQueryGenerator,
            IJsonObjectConverter jsonObjectConverter,
            ITweetinviContainer tweetinviContainer)
        {
            _helpQueryGenerator = helpQueryGenerator;
            _jsonObjectConverter = jsonObjectConverter;
            _tweetinviContainer = tweetinviContainer;
            _tokenRateLimits = new Dictionary<ITwitterCredentials, ITokenRateLimits>();
        }

        public void Clear(ITwitterCredentials credentials)
        {
            // We want to lock both the refresh dictionary access so that we ensure the dictionary
            // is not cleared during a refresh or when it is being accessed
            lock (_lockRefresh)
            {
                lock (_lockTokenRateLimitsDictionary)
                {
                    if (_tokenRateLimits.ContainsKey(credentials))
                    {
                        _tokenRateLimits.Remove(credentials);
                    }
                }
            }
        }

        public void ClearAll()
        {
            // We want to lock both the refresh dictionary access so that we ensure the dictionary
            // is not cleared during a refresh or when it is being accessed
            lock (_lockRefresh)
            {
                lock (_lockTokenRateLimitsDictionary)
                {
                    _tokenRateLimits.Clear();
                }
            }
        }

        public ITokenRateLimits GetTokenRateLimits(ITwitterCredentials credentials)
        {
            lock (_lockTokenRateLimitsDictionary)
            {
                ITokenRateLimits credentialsRateLimits;
                if (_tokenRateLimits.TryGetValue(credentials, out credentialsRateLimits))
                {
                    return credentialsRateLimits;
                }
                else
                {
                    return null;
                }
            }
        }

        public void RefreshEntry(ITwitterCredentials credentials, ITokenRateLimits credentialsRateLimits)
        {
            lock (_lockTokenRateLimitsDictionary)
            {
                if (_tokenRateLimits.ContainsKey(credentials))
                {
                    _tokenRateLimits[credentials] = credentialsRateLimits;
                }
                else
                {
                    _tokenRateLimits.Add(credentials, credentialsRateLimits);
                }
            }
        }
    }
}