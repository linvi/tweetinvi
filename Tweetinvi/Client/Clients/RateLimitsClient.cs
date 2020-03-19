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

        public async Task InitializeRateLimitsManager()
        {
            if (await _rateLimitCacheManager.RateLimitCache.GetCredentialsRateLimits(_client.Credentials) == null)
            {
                await _rateLimitCacheManager.RefreshCredentialsRateLimits(_client.Credentials).ConfigureAwait(false);
            }
        }

        public Task<ICredentialsRateLimits> GetRateLimits()
        {
            return GetRateLimits(new GetRateLimitsParameters());
        }

        public Task<ICredentialsRateLimits> GetRateLimits(RateLimitsSource from)
        {
            return GetRateLimits(new GetRateLimitsParameters
            {
                From = from
            });
        }

        public async Task<ICredentialsRateLimits> GetRateLimits(IGetRateLimitsParameters parameters)
        {
            switch (parameters.From)
            {
                case RateLimitsSource.CacheOnly:
                    return await _rateLimitCacheManager.RateLimitCache.GetCredentialsRateLimits(_client.Credentials).ConfigureAwait(false);
                case RateLimitsSource.TwitterApiOnly:
                    var twitterResult = await _helpRequester.GetRateLimits(parameters).ConfigureAwait(false);
                    return _client.Factories.CreateRateLimits(twitterResult?.DataTransferObject);
                case RateLimitsSource.CacheOrTwitterApi:
                    return await _rateLimitCacheManager.GetCredentialsRateLimits(_client.Credentials).ConfigureAwait(false);
                default:
                    throw new ArgumentException(nameof(parameters.From));
            }
        }

        public Task<IEndpointRateLimit> GetEndpointRateLimit(string url)
        {
            return GetEndpointRateLimit(new GetEndpointRateLimitsParameters(url));
        }

        public Task<IEndpointRateLimit> GetEndpointRateLimit(string url, RateLimitsSource from)
        {
            return GetEndpointRateLimit(new GetEndpointRateLimitsParameters(url)
            {
                From = from
            });
        }

        public Task<IEndpointRateLimit> GetEndpointRateLimit(IGetEndpointRateLimitsParameters parameters)
        {
            return _rateLimitCacheManager.GetQueryRateLimit(parameters, _client.Credentials);
        }

        public Task WaitForQueryRateLimit(string url)
        {
            return _rateLimitAwaiter.WaitForCredentialsRateLimit(url, _client.Credentials, _client.CreateTwitterExecutionContext());
        }

        public Task WaitForQueryRateLimit(IEndpointRateLimit endpointRateLimit)
        {
            return _rateLimitAwaiter.WaitForCredentialsRateLimit(endpointRateLimit, _client.Credentials, _client.CreateTwitterExecutionContext());
        }

        public Task ClearRateLimitCache(IReadOnlyTwitterCredentials credentials)
        {
            return _rateLimitCacheManager.RateLimitCache.Clear(credentials);
        }

        public Task ClearRateLimitCache()
        {
            return _rateLimitCacheManager.RateLimitCache.Clear(_client.Credentials);
        }

        public Task ClearAllRateLimitCache()
        {
            return _rateLimitCacheManager.RateLimitCache.ClearAll();
        }
    }
}