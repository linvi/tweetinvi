using System;
using System.Collections.Generic;
using Tweetinvi.Core.Authentication;
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
        private readonly ITweetinviContainer _container;
        private readonly Dictionary<ITwitterCredentials, ICredentialsRateLimits> _credentialsRateLimits;

        private readonly object _lockRefresh = new Object();
        private readonly object _lockCredentialsRateLimitsDictionary = new object();

        public RateLimitCache(
            IHelpQueryGenerator helpQueryGenerator,
            IJsonObjectConverter jsonObjectConverter,
            ITweetinviContainer container)
        {
            _helpQueryGenerator = helpQueryGenerator;
            _jsonObjectConverter = jsonObjectConverter;
            _container = container;
            _credentialsRateLimits = new Dictionary<ITwitterCredentials, ICredentialsRateLimits>();
        }

        public void Clear(ITwitterCredentials credentials)
        {
            // We want to lock both the refresh dictionary access so that we ensure the dictionary
            // is not cleared during a refresh or when it is being accessed
            lock (_lockRefresh)
            {
                lock (_lockCredentialsRateLimitsDictionary)
                {
                    if (_credentialsRateLimits.ContainsKey(credentials))
                    {
                        _credentialsRateLimits.Remove(credentials);
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
                lock (_lockCredentialsRateLimitsDictionary)
                {
                    _credentialsRateLimits.Clear();
                }
            }
        }

        public ICredentialsRateLimits GetCredentialsRateLimits(ITwitterCredentials credentials)
        {
            lock (_lockCredentialsRateLimitsDictionary)
            {
                ICredentialsRateLimits credentialsRateLimits;
                if (_credentialsRateLimits.TryGetValue(credentials, out credentialsRateLimits))
                {
                    return credentialsRateLimits;
                }
                else
                {
                    return null;
                }
            }
        }

        public void RefreshEntry(ITwitterCredentials credentials, ICredentialsRateLimits credentialsRateLimits)
        {
            lock (_lockCredentialsRateLimitsDictionary)
            {
                if (_credentialsRateLimits.ContainsKey(credentials))
                {
                    _credentialsRateLimits[credentials] = credentialsRateLimits;
                }
                else
                {
                    _credentialsRateLimits.Add(credentials, credentialsRateLimits);
                }
            }
        }
    }
}