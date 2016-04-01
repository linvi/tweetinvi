using System;
using System.Net;
using Tweetinvi.Core.Authentication;
using Tweetinvi.Core.Enum;
using Tweetinvi.Core.Exceptions;
using Tweetinvi.Core.Helpers;
using Tweetinvi.Core.Interfaces.Controllers;
using Tweetinvi.Core.Interfaces.Credentials;
using Tweetinvi.Core.Interfaces.Models;
using Tweetinvi.Core.Interfaces.RateLimit;
using Tweetinvi.Core.Interfaces.WebLogic;

namespace Tweetinvi.Credentials.RateLimit
{
    public class RateLimitCacheManager : IRateLimitCacheManager
    {
        private readonly ICredentialsAccessor _credentialsAccessor;
        private readonly ITwitterRequester _twitterRequester;
        private readonly IHelpQueryGenerator _helpQueryGenerator;
        private readonly IJsonObjectConverter _jsonObjectConverter;
        private readonly IRateLimitCache _rateLimitCache;
        private readonly IRateLimitHelper _rateLimitHelper;
        private readonly ITwitterQueryFactory _twitterQueryFactory;

        private bool _isRetrievingData;

        public RateLimitCacheManager(
            ICredentialsAccessor credentialsAccessor,
            ITwitterRequester twitterRequester,
            IHelpQueryGenerator helpQueryGenerator,
            IJsonObjectConverter jsonObjectConverter,
            IRateLimitCache rateLimitCache,
            IRateLimitHelper rateLimitHelper,
            ITwitterQueryFactory twitterQueryFactory)
        {
            _credentialsAccessor = credentialsAccessor;
            _twitterRequester = twitterRequester;
            _helpQueryGenerator = helpQueryGenerator;
            _jsonObjectConverter = jsonObjectConverter;
            _rateLimitCache = rateLimitCache;
            _rateLimitHelper = rateLimitHelper;
            _twitterQueryFactory = twitterQueryFactory;
        }

        public IEndpointRateLimit GetQueryRateLimit(string query, ITwitterCredentials credentials)
        {
            var rateLimits = _rateLimitCache.GetCredentialsRateLimits(credentials);
            var queryRateLimit = _rateLimitHelper.GetEndpointRateLimitFromQuery(query, rateLimits);

            if (rateLimits == null || DoesQueryNeedsToRefreshTheCacheInformation(queryRateLimit))
            {
                rateLimits = RefreshCredentialsRateLimits(credentials);
                queryRateLimit = _rateLimitHelper.GetEndpointRateLimitFromQuery(query, rateLimits);
            }

            return queryRateLimit;
        }

        public ICredentialsRateLimits GetCredentialsRateLimits(ITwitterCredentials credentials)
        {
            var rateLimits = _rateLimitCache.GetCredentialsRateLimits(credentials);
            if (rateLimits == null)
            {
                rateLimits = RefreshCredentialsRateLimits(credentials);
            }

            return rateLimits;
        }

        public void UpdateCredentialsRateLimits(ITwitterCredentials credentials, ICredentialsRateLimits credentialsRateLimits)
        {
            _rateLimitCache.RefreshEntry(credentials, credentialsRateLimits);
        }

        private ICredentialsRateLimits RefreshCredentialsRateLimits(ITwitterCredentials credentials)
        {
            var tokenRateLimits = GetTokenRateLimitsFromTwitter(credentials);
            _rateLimitCache.RefreshEntry(credentials, tokenRateLimits);
            return _rateLimitCache.GetCredentialsRateLimits(credentials);
        }

        private ICredentialsRateLimits GetTokenRateLimitsFromTwitter(ITwitterCredentials credentials)
        {
            if (_isRetrievingData)
            {
                return null;
            }

            var isApplicationOnlyCreds = string.IsNullOrEmpty(credentials.AccessToken) || string.IsNullOrEmpty(credentials.AccessTokenSecret);
            if (isApplicationOnlyCreds && string.IsNullOrEmpty(credentials.ApplicationOnlyBearerToken))
            {
                return null;
            }

            _isRetrievingData = true;
            var result = _credentialsAccessor.ExecuteOperationWithCredentials(credentials, () =>
            {
                var twitterQuery = _twitterQueryFactory.Create(_helpQueryGenerator.GetCredentialsLimitsQuery(), HttpMethod.GET, credentials);

                try
                {
                    string jsonResponse = _twitterRequester.ExecuteQuery(twitterQuery);
                    return _jsonObjectConverter.DeserializeObject<ICredentialsRateLimits>(jsonResponse);
                }
                catch (TwitterException)
                {
                    return null;
                }
            });

            _isRetrievingData = false;
            return result;
        }

        private bool DoesQueryNeedsToRefreshTheCacheInformation(IEndpointRateLimit rateLimit)
        {
            return rateLimit != null && rateLimit.ResetDateTime < DateTime.Now;
        }
    }
}