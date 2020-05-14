using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Tweetinvi.Core.Extensions;
using Tweetinvi.Core.Models.Authentication;
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
        private readonly Dictionary<IReadOnlyTwitterCredentials, ICredentialsRateLimits> _credentialsRateLimits;

        private readonly object _lockRefresh = new Object();
        private readonly object _lockCredentialsRateLimitsDictionary = new object();

        public RateLimitCache()
        {
            _credentialsRateLimits = new Dictionary<IReadOnlyTwitterCredentials, ICredentialsRateLimits>();
        }

        public Task ClearAsync(IReadOnlyTwitterCredentials credentials)
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

                return Task.CompletedTask;
            }
        }

        public Task ClearAllAsync()
        {
            // We want to lock both the refresh dictionary access so that we ensure the dictionary
            // is not cleared during a refresh or when it is being accessed
            lock (_lockRefresh)
            {
                lock (_lockCredentialsRateLimitsDictionary)
                {
                    _credentialsRateLimits.Clear();
                }

                return Task.CompletedTask;
            }
        }

        public Task<ICredentialsRateLimits> GetCredentialsRateLimitsAsync(IReadOnlyTwitterCredentials credentials)
        {
            lock (_lockCredentialsRateLimitsDictionary)
            {
                if (credentials != null && _credentialsRateLimits.TryGetValue(credentials, out var credentialsRateLimits))
                {
                    return Task.FromResult(credentialsRateLimits);
                }

                return Task.FromResult<ICredentialsRateLimits>(null);
            }
        }

        public Task RefreshEntryAsync(IReadOnlyTwitterCredentials credentials, ICredentialsRateLimits newCredentialsRateLimits)
        {
            lock (_lockCredentialsRateLimitsDictionary)
            {
                if (newCredentialsRateLimits == null)
                {
                    return Task.CompletedTask;
                }

                if (_credentialsRateLimits.TryGetValue(credentials, out var currentRateLimits))
                {
                    var existingCustomEndpoints = currentRateLimits.OtherEndpointRateLimits;
                    existingCustomEndpoints.ForEach(x => newCredentialsRateLimits.OtherEndpointRateLimits.AddOrUpdate(x));
                }

                _credentialsRateLimits.AddOrUpdate(credentials, newCredentialsRateLimits);

                return Task.CompletedTask;
            }
        }
    }
}