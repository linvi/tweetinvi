using System;
using System.Net;
using Tweetinvi.Core.Credentials;
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

        public ITokenRateLimit GetQueryRateLimit(string query, ITwitterCredentials credentials)
        {
            var rateLimits = _rateLimitCache.GetTokenRateLimits(credentials);
            var queryRateLimit = _rateLimitHelper.GetTokenRateLimitFromQuery(query, rateLimits);

            if (rateLimits == null || DoesQueryNeedsToRefreshTheCacheInformation(queryRateLimit))
            {
                rateLimits = RefreshCredentialsRateLimits(credentials);
                queryRateLimit = _rateLimitHelper.GetTokenRateLimitFromQuery(query, rateLimits);
            }

            return queryRateLimit;
        }

        public ITokenRateLimits GetTokenRateLimits(ITwitterCredentials credentials)
        {
            var rateLimits = _rateLimitCache.GetTokenRateLimits(credentials);
            if (rateLimits == null)
            {
                rateLimits = RefreshCredentialsRateLimits(credentials);
            }

            return rateLimits;
        }

        public void UpdateTokenRateLimits(ITwitterCredentials credentials, ITokenRateLimits tokenRateLimits)
        {
            _rateLimitCache.RefreshEntry(credentials, tokenRateLimits);
        }

        private ITokenRateLimits RefreshCredentialsRateLimits(ITwitterCredentials credentials)
        {
            var tokenRateLimits = GetTokenRateLimitsFromTwitter(credentials);
            _rateLimitCache.RefreshEntry(credentials, tokenRateLimits);
            return _rateLimitCache.GetTokenRateLimits(credentials);
        }

        private ITokenRateLimits GetTokenRateLimitsFromTwitter(ITwitterCredentials credentials)
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
                    return _jsonObjectConverter.DeserializeObject<ITokenRateLimits>(jsonResponse);
                }
                catch (TwitterException)
                {
                    return null;
                }
            });

            _isRetrievingData = false;
            return result;
        }

        private bool DoesQueryNeedsToRefreshTheCacheInformation(ITokenRateLimit rateLimit)
        {
            return rateLimit != null && rateLimit.ResetDateTime < DateTime.Now;
        }
    }
}