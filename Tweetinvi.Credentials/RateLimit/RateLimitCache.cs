using System;
using System.Collections.Generic;
using Tweetinvi.Core.Extensions;
using Tweetinvi.Core.RateLimit;
using Tweetinvi.Models;

namespace Tweetinvi.Credentials.RateLimit
{
    /// <summary>
    /// IMPORTANT : This class needs to be ThreadSafe because it is registered as InstancePerApplication
    /// We want the cache to be updated once the second thread has completed the operation
    /// </summary>
    public class RateLimitCache : IRateLimitCache
    {
        private readonly Dictionary<ITwitterCredentials, ICredentialsRateLimits> _credentialsRateLimits;

        private readonly object _lockRefresh = new Object();
        private readonly object _lockCredentialsRateLimitsDictionary = new object();

        public RateLimitCache()
        {
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
                if (credentials != null && _credentialsRateLimits.TryGetValue(credentials, out credentialsRateLimits))
                {
                    return credentialsRateLimits;
                }

                return null;
            }
        }

        public void RefreshEntry(ITwitterCredentials credentials, ICredentialsRateLimits newCredentialsRateLimits)
        {
            lock (_lockCredentialsRateLimitsDictionary)
            {
                if (newCredentialsRateLimits == null)
                {
                    return;
                }

                ICredentialsRateLimits currentRateLimits;
                if (_credentialsRateLimits.TryGetValue(credentials, out currentRateLimits))
                {
                    var existingCustomEndpoints = currentRateLimits.OtherEndpointRateLimits;
                    existingCustomEndpoints.ForEach(x => newCredentialsRateLimits.OtherEndpointRateLimits.AddOrUpdate(x));
                }

                _credentialsRateLimits.AddOrUpdate(credentials, newCredentialsRateLimits);
            }
        }
    }
}