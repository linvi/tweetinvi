using System;
using System.Collections.Generic;
using Tweetinvi.Core;
using Tweetinvi.Core.Enum;
using Tweetinvi.Core.Events.EventArguments;
using Tweetinvi.Core.Interfaces;
using Tweetinvi.Core.Interfaces.Credentials;
using Tweetinvi.Core.Interfaces.DTO;
using Tweetinvi.Core.Interfaces.Models;
using Tweetinvi.Core.Interfaces.RateLimit;
using Tweetinvi.Core.Interfaces.WebLogic;
using Tweetinvi.Logic.Exceptions;

namespace Tweetinvi.WebLogic
{
    public interface ITwitterRequestHandler
    {
        string ExecuteQuery(string queryURL, HttpMethod method, IEnumerable<IOAuthQueryParameter> queryParameters = null);
        string ExecuteMultipartQuery(string queryURL, string contentId, HttpMethod httpMethod, IEnumerable<IMedia> medias);
        string ExecuteQueryWithTemporaryCredentials(string queryURL, HttpMethod httpMethod, ITemporaryCredentials temporaryCredentials, IEnumerable<IOAuthQueryParameter> queryParameters);
    }

    public class TwitterRequestHandler : ITwitterRequestHandler
    {
        private readonly ITweetinviEvents _tweetinviEvents;
        private readonly IRateLimitAwaiter _rateLimitAwaiter;
        private readonly IRateLimitUpdater _rateLimitUpdater;
        private readonly ITwitterRequester _twitterRequester;
        private readonly ICredentialsAccessor _credentialsAccessor;
        private readonly ITweetinviSettingsAccessor _tweetinviSettingsAccessor;
        private readonly ITwitterQueryFactory _twitterQueryFactory;

        public TwitterRequestHandler(
            ITweetinviEvents tweetinviEvents,
            IRateLimitAwaiter rateLimitAwaiter,
            IRateLimitUpdater rateLimitUpdater,
            ITwitterRequester twitterRequester,
            ICredentialsAccessor credentialsAccessor,
            ITweetinviSettingsAccessor tweetinviSettingsAccessor,
            ITwitterQueryFactory twitterQueryFactory)
        {
            _tweetinviEvents = tweetinviEvents;
            _rateLimitAwaiter = rateLimitAwaiter;
            _rateLimitUpdater = rateLimitUpdater;
            _twitterRequester = twitterRequester;
            _credentialsAccessor = credentialsAccessor;
            _tweetinviSettingsAccessor = tweetinviSettingsAccessor;
            _twitterQueryFactory = twitterQueryFactory;
        }

        public string ExecuteQuery(string queryURL, HttpMethod httpMethod, IEnumerable<IOAuthQueryParameter> queryParameters = null)
        {
            CleanupQueryURL(ref queryURL);
            var rateLimitTrackerOption = _tweetinviSettingsAccessor.RateLimitTrackerOption;
            
            ITwitterQuery twitterQuery;
            if (!TryPrepareRequest(queryURL, httpMethod, queryParameters, rateLimitTrackerOption, out twitterQuery))
            {
                return null;
            }

            try
            {
                var jsonResult = _twitterRequester.ExecuteQuery(twitterQuery);

                QueryCompleted(twitterQuery, jsonResult, rateLimitTrackerOption);

                return jsonResult;
            }
            catch (TwitterException ex)
            {
                HandleException(queryURL, rateLimitTrackerOption, ex.StatusCode, twitterQuery);
                
                throw;
            }
        }

        public string ExecuteMultipartQuery(string queryURL, string contentId, HttpMethod httpMethod, IEnumerable<IMedia> medias)
        {
            CleanupQueryURL(ref queryURL);
            var rateLimitTrackerOption = _tweetinviSettingsAccessor.RateLimitTrackerOption;
            
            ITwitterQuery twitterQuery;
            if (!TryPrepareRequest(queryURL, httpMethod, null, rateLimitTrackerOption, out twitterQuery))
            {
                return null;
            }

            try
            {
                var jsonResult = _twitterRequester.ExecuteMultipartQuery(twitterQuery, contentId, medias);

                QueryCompleted(twitterQuery, jsonResult, rateLimitTrackerOption);

                return jsonResult;
            }
            catch (TwitterException ex)
            {
                HandleException(queryURL, rateLimitTrackerOption, ex.StatusCode, twitterQuery);

                throw;
            }
        }

        public string ExecuteQueryWithTemporaryCredentials(string queryURL, HttpMethod httpMethod, ITemporaryCredentials temporaryCredentials, IEnumerable<IOAuthQueryParameter> queryParameters)
        {
            CleanupQueryURL(ref queryURL);

            var twitterQuery = _twitterQueryFactory.Create(queryURL, httpMethod, temporaryCredentials);
            twitterQuery.QueryParameters = queryParameters;

            _tweetinviEvents.RaiseBeforeQueryExecute(new QueryBeforeExecuteEventArgs(twitterQuery));

            var result = _twitterRequester.ExecuteQuery(twitterQuery);
            
            _tweetinviEvents.RaiseAfterQueryExecuted(new QueryAfterExecuteEventArgs(twitterQuery, result));
            
            return result;
        }

        private void CleanupQueryURL(ref string query)
        {
            var index = query.IndexOf("?", StringComparison.OrdinalIgnoreCase);
            if (index > 0)
            {
                if (query.Length == index + 1)
                {
                    query = query.Remove(index);
                    return;
                }

                if (query.Length > index && query[index + 1] == '&')
                {
                    query = query.Remove(index + 1, 1);
                }
            }
        }

        private bool TryPrepareRequest(string query, HttpMethod httpMethod, IEnumerable<IOAuthQueryParameter> queryParameters, RateLimitTrackerOptions rateLimitTrackerOption, out ITwitterQuery twitterQuery)
        {
            twitterQuery = _twitterQueryFactory.Create(query, httpMethod, _credentialsAccessor.CurrentThreadCredentials);
            twitterQuery.QueryParameters = queryParameters;

            var beforeQueryExecuteEventArgs = new QueryBeforeExecuteEventArgs(twitterQuery);
            _tweetinviEvents.RaiseBeforeQueryExecute(beforeQueryExecuteEventArgs);

            if (beforeQueryExecuteEventArgs.Cancel)
            {
                twitterQuery = null;
                return false;
            }

            if (rateLimitTrackerOption == RateLimitTrackerOptions.TrackAndAwait)
            {
                _rateLimitAwaiter.WaitForCurrentCredentialsRateLimit(query);
            }

            return true;
        }

        private void QueryCompleted(ITwitterQuery twitterQuery, string jsonResult, RateLimitTrackerOptions rateLimitTrackerOptions)
        {
            if (rateLimitTrackerOptions != RateLimitTrackerOptions.None)
            {
                _rateLimitUpdater.QueryExecuted(twitterQuery.QueryURL, _credentialsAccessor.CurrentThreadCredentials);
            }

            _tweetinviEvents.RaiseAfterQueryExecuted(new QueryAfterExecuteEventArgs(twitterQuery, jsonResult));
        }

        private void HandleException(string queryURL, RateLimitTrackerOptions rateLimitTrackerOption, int statusCode, ITwitterQuery queryParameter)
        {
            if (rateLimitTrackerOption != RateLimitTrackerOptions.None && statusCode == TweetinviConsts.STATUS_CODE_TOO_MANY_REQUEST)
            {
                _rateLimitUpdater.ClearRateLimitsForQuery(queryURL);
            }

            _tweetinviEvents.RaiseAfterQueryExecuted(new QueryAfterExecuteEventArgs(queryParameter, null));
        }
    }
}