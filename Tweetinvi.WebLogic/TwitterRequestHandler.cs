using System;
using System.Collections.Generic;
using Tweetinvi.Core;
using Tweetinvi.Core.Credentials;
using Tweetinvi.Core.Events;
using Tweetinvi.Core.Events.EventArguments;
using Tweetinvi.Core.Exceptions;
using Tweetinvi.Core.Interfaces.Credentials;
using Tweetinvi.Core.Interfaces.Models;
using Tweetinvi.Core.Interfaces.RateLimit;
using Tweetinvi.Core.Interfaces.WebLogic;
using HttpMethod = Tweetinvi.Core.Enum.HttpMethod;

namespace Tweetinvi.WebLogic
{
    public interface ITwitterRequestHandler
    {
        string ExecuteQuery(string queryURL, HttpMethod method, TwitterClientHandler handler = null, ITwitterCredentials twitterCredentials = null);
        string ExecuteMultipartQuery(string queryURL, string contentId, HttpMethod httpMethod, IEnumerable<byte[]> binaries);
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

        public string ExecuteQuery(
            string queryURL, 
            HttpMethod httpMethod, 
            TwitterClientHandler handler = null,
            ITwitterCredentials credentials = null)
        {
            CleanupQueryURL(ref queryURL);
            var rateLimitTrackerOption = _tweetinviSettingsAccessor.RateLimitTrackerOption;
            
            ITwitterQuery twitterQuery;
            if (!TryPrepareRequest(queryURL, httpMethod, rateLimitTrackerOption, credentials, out twitterQuery))
            {
                return null;
            }

            try
            {
                var jsonResult = _twitterRequester.ExecuteQuery(twitterQuery, handler);

                QueryCompleted(twitterQuery, jsonResult, rateLimitTrackerOption);

                return jsonResult;
            }
            catch (TwitterException ex)
            {
                HandleException(queryURL, rateLimitTrackerOption, ex.StatusCode, twitterQuery);
                
                throw;
            }
        }

        public string ExecuteMultipartQuery(string queryURL, string contentId, HttpMethod httpMethod, IEnumerable<byte[]> binaries)
        {
            CleanupQueryURL(ref queryURL);
            var rateLimitTrackerOption = _tweetinviSettingsAccessor.RateLimitTrackerOption;
            
            ITwitterQuery twitterQuery;
            if (!TryPrepareRequest(queryURL, httpMethod, rateLimitTrackerOption, null, out twitterQuery))
            {
                return null;
            }

            try
            {
                var jsonResult = _twitterRequester.ExecuteMultipartQuery(twitterQuery, contentId, binaries);

                QueryCompleted(twitterQuery, jsonResult, rateLimitTrackerOption);

                return jsonResult;
            }
            catch (TwitterException ex)
            {
                HandleException(queryURL, rateLimitTrackerOption, ex.StatusCode, twitterQuery);

                throw;
            }
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

        private bool TryPrepareRequest(
            string query, 
            HttpMethod httpMethod, 
            RateLimitTrackerOptions rateLimitTrackerOption,
            ITwitterCredentials credentials,
            out ITwitterQuery twitterQuery)
        {
            credentials = credentials ?? _credentialsAccessor.CurrentThreadCredentials;

            if (credentials == null)
            {
                throw new TwitterNullCredentialsException();
            }

            twitterQuery = _twitterQueryFactory.Create(query, httpMethod, credentials);

            var beforeQueryExecuteEventArgs = new QueryBeforeExecuteEventArgs(twitterQuery);


            if (rateLimitTrackerOption == RateLimitTrackerOptions.TrackOnly ||
                rateLimitTrackerOption == RateLimitTrackerOptions.TrackAndAwait)
            {
                var timeToWait = _rateLimitAwaiter.TimeToWaitBeforeTwitterRequest(query, twitterQuery.TwitterCredentials);

                twitterQuery.DateWhenCredentialsWillHaveRequiredRateLimits = DateTime.Now.AddMilliseconds(timeToWait);
                _tweetinviEvents.RaiseBeforeQueryExecute(beforeQueryExecuteEventArgs);

                if (beforeQueryExecuteEventArgs.Cancel)
                {
                    twitterQuery = null;
                    return false;
                }

                if (rateLimitTrackerOption == RateLimitTrackerOptions.TrackAndAwait)
                {
                    _rateLimitAwaiter.Wait(timeToWait);
                }
            }
            else
            {
                _tweetinviEvents.RaiseBeforeQueryExecute(beforeQueryExecuteEventArgs);

                if (beforeQueryExecuteEventArgs.Cancel)
                {
                    twitterQuery = null;
                    return false;
                }
            }

            _tweetinviEvents.RaiseBeforeExecuteAfterRateLimitAwait(beforeQueryExecuteEventArgs);

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