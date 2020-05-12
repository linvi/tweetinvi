using System;
using System.Threading.Tasks;
using Tweetinvi.Client.Requesters;
using Tweetinvi.Core.RateLimit;
using Tweetinvi.Models;
using Tweetinvi.Parameters;

namespace Tweetinvi.Client
{
    public class RateLimitsClient : IRateLimitsClient
    {
        private readonly ITwitterClient _client;
        private readonly IRateLimitCacheManager _rateLimitCacheManager;
        private readonly IRateLimitAwaiter _rateLimitAwaiter;
        private readonly IHelpRequester _helpRequester;

        public RateLimitsClient(ITwitterClient client)
        {
            var executionContext = client.CreateTwitterExecutionContext();

            _client = client;
            _helpRequester = client.Raw.Help;
            _rateLimitCacheManager = executionContext.Container.Resolve<IRateLimitCacheManager>();
            _rateLimitAwaiter = executionContext.Container.Resolve<IRateLimitAwaiter>();
        }

        public async Task InitializeRateLimitsManagerAsync()
        {
            var credentialsRateLimits = await _rateLimitCacheManager.RateLimitCache.GetCredentialsRateLimitsAsync(_client.Credentials).ConfigureAwait(false);
            if (credentialsRateLimits == null)
            {
                await _rateLimitCacheManager.RefreshCredentialsRateLimitsAsync(_client.Credentials).ConfigureAwait(false);
            }
        }

        public Task<ICredentialsRateLimits> GetRateLimitsAsync()
        {
            return GetRateLimitsAsync(new GetRateLimitsParameters());
        }

        public Task<ICredentialsRateLimits> GetRateLimitsAsync(RateLimitsSource from)
        {
            return GetRateLimitsAsync(new GetRateLimitsParameters
            {
                From = from
            });
        }

        public async Task<ICredentialsRateLimits> GetRateLimitsAsync(IGetRateLimitsParameters parameters)
        {
            switch (parameters.From)
            {
                case RateLimitsSource.CacheOnly:
                    return await _rateLimitCacheManager.RateLimitCache.GetCredentialsRateLimitsAsync(_client.Credentials).ConfigureAwait(false);
                case RateLimitsSource.TwitterApiOnly:
                    var twitterResult = await _helpRequester.GetRateLimitsAsync(parameters).ConfigureAwait(false);
                    return _client.Factories.CreateRateLimits(twitterResult?.Model);
                case RateLimitsSource.CacheOrTwitterApi:
                    return await _rateLimitCacheManager.GetCredentialsRateLimitsAsync(_client.Credentials).ConfigureAwait(false);
                default:
                    throw new ArgumentException(nameof(parameters.From));
            }
        }

        public Task<IEndpointRateLimit> GetEndpointRateLimitAsync(string url)
        {
            return GetEndpointRateLimitAsync(new GetEndpointRateLimitsParameters(url));
        }

        public Task<IEndpointRateLimit> GetEndpointRateLimitAsync(string url, RateLimitsSource from)
        {
            return GetEndpointRateLimitAsync(new GetEndpointRateLimitsParameters(url)
            {
                From = from
            });
        }

        public Task<IEndpointRateLimit> GetEndpointRateLimitAsync(IGetEndpointRateLimitsParameters parameters)
        {
            return _rateLimitCacheManager.GetQueryRateLimitAsync(parameters, _client.Credentials);
        }

        public Task WaitForQueryRateLimitAsync(string url)
        {
            return _rateLimitAwaiter.WaitForCredentialsRateLimitAsync(url, _client.Credentials, _client.CreateTwitterExecutionContext());
        }

        public Task WaitForQueryRateLimitAsync(IEndpointRateLimit endpointRateLimit)
        {
            return _rateLimitAwaiter.WaitForCredentialsRateLimitAsync(endpointRateLimit, _client.Credentials, _client.CreateTwitterExecutionContext());
        }

        public Task ClearRateLimitCacheAsync(IReadOnlyTwitterCredentials credentials)
        {
            return _rateLimitCacheManager.RateLimitCache.ClearAsync(credentials);
        }

        public Task ClearRateLimitCacheAsync()
        {
            return _rateLimitCacheManager.RateLimitCache.ClearAsync(_client.Credentials);
        }

        public Task ClearAllRateLimitCacheAsync()
        {
            return _rateLimitCacheManager.RateLimitCache.ClearAllAsync();
        }
    }
}