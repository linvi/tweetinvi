using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Tweetinvi.Core;
using Tweetinvi.Core.Client;
using Tweetinvi.Core.Credentials;
using Tweetinvi.Core.Helpers;
using Tweetinvi.Core.Models.Properties;
using Tweetinvi.Core.Web;
using Tweetinvi.Core.Wrappers;
using Tweetinvi.Credentials.QueryJsonConverters;
using Tweetinvi.Exceptions;
using Tweetinvi.Models;
using Tweetinvi.Models.DTO.QueryDTO;
using Tweetinvi.Parameters;
using Tweetinvi.WebLogic;
using HttpMethod = Tweetinvi.Models.HttpMethod;

namespace Tweetinvi.Credentials
{
    public class TwitterAccessor : ITwitterAccessor
    {
        private readonly IJObjectStaticWrapper _jObjectStaticWrapper;
        private readonly IJsonObjectConverter _jsonObjectConverter;
        private readonly ICursorQueryHelper _cursorQueryHelper;
        private readonly ITwitterRequestHandler _twitterRequestHandler;
        private readonly ITwitterQueryFactory _twitterQueryFactory;
        private readonly ICredentialsAccessor _credentialsAccessor;
        private readonly ITweetinviSettingsAccessor _settingsAccessor;
        private readonly ITwitterResultFactory _twitterResultFactory;

        public TwitterAccessor(
            IJObjectStaticWrapper jObjectStaticWrapper,
            IJsonObjectConverter jsonObjectConverter,
            ICursorQueryHelper cursorQueryHelper,
            ITwitterRequestHandler twitterRequestHandler,
            ITwitterQueryFactory twitterQueryFactory,
            ICredentialsAccessor credentialsAccessor,
            ITweetinviSettingsAccessor settingsAccessor,
            ITwitterResultFactory twitterResultFactory)
        {
            _jObjectStaticWrapper = jObjectStaticWrapper;
            _jsonObjectConverter = jsonObjectConverter;
            _cursorQueryHelper = cursorQueryHelper;
            _twitterRequestHandler = twitterRequestHandler;
            _twitterQueryFactory = twitterQueryFactory;
            _credentialsAccessor = credentialsAccessor;
            _settingsAccessor = settingsAccessor;
            _twitterResultFactory = twitterResultFactory;
        }

        // Execute<Json>

        public Task<string> ExecuteGETQueryReturningJson(string query)
        {
            return ExecuteQueryReturningContent(query, HttpMethod.GET);
        }

        public Task<string> ExecutePOSTQueryReturningJson(string query)
        {
            return ExecuteQueryReturningContent(query, HttpMethod.POST);
        }

        public Task<string> ExecuteDELETEQueryReturningJson(string query)
        {
            return ExecuteQueryReturningContent(query, HttpMethod.DELETE);
        }

        // Try Execute<Json>
        public async Task<AsyncOperation<string>> TryExecuteGETQuery(string query)
        {
            try
            {
                var json = await ExecuteGETQueryReturningJson(query);

                return new AsyncOperation<string>
                {
                    Success = json != null,
                    Result = json
                };
            }
            catch (TwitterException)
            {
                return new FailedAsyncOperation<string>();
            }
        }

        public async Task<AsyncOperation<string>> TryExecutePOSTQuery(string query)
        {
            try
            {
                var json = await ExecutePOSTQueryReturningJson(query);

                return new AsyncOperation<string>
                {
                    Success = json != null,
                    Result = json
                };
            }
            catch (TwitterException)
            {
                return new FailedAsyncOperation<string>();
            }
        }

        public async Task<AsyncOperation<string>> TryExecuteDELETEQuery(string query)
        {
            try
            {
                var json = await ExecuteDELETEQueryReturningJson(query);

                return new AsyncOperation<string>
                {
                    Success = json != null,
                    Result = json
                };
            }
            catch (TwitterException)
            {
                return new FailedAsyncOperation<string>();
            }
        }

        // Execute<JObject>
        public async Task<JObject> ExecuteGETQuery(string query)
        {
            string jsonResponse = await ExecuteQueryReturningContent(query, HttpMethod.GET);
            return _jObjectStaticWrapper.GetJobjectFromJson(jsonResponse);
        }

        public async Task<JObject> ExecutePOSTQuery(string query)
        {
            string jsonResponse = await ExecuteQueryReturningContent(query, HttpMethod.POST);
            return _jObjectStaticWrapper.GetJobjectFromJson(jsonResponse);
        }

        // Get specific type of object from path
        public async Task<T> ExecuteGETQueryWithPath<T>(string query, params string[] paths) where T : class
        {
            var jObject = await ExecuteGETQuery(query);
            return GetResultFromPath<T>(jObject, paths);
        }

        public async Task<T> ExecutePOSTQueryWithPath<T>(string query, params string[] paths) where T : class
        {
            var jObject = await ExecutePOSTQuery(query);
            return GetResultFromPath<T>(jObject, paths);
        }

        private T GetResultFromPath<T>(JObject jObject, string[] paths) where T : class
        {
            if (paths != null && paths.Length > 0)
            {
                var path = string.Join(".", paths);
                var token = jObject.SelectToken(path);
                return _jObjectStaticWrapper.ToObject<T>(token);
            }

            return _jObjectStaticWrapper.ToObject<T>(jObject);
        }

        // Execute<T>
        public async Task<T> ExecuteGETQuery<T>(string query, JsonConverter[] converters = null) where T : class
        {
            string jsonResponse = await ExecuteQueryReturningContent(query, HttpMethod.GET);
            return _jsonObjectConverter.DeserializeObject<T>(jsonResponse, converters);
        }

        public async Task<T> ExecutePOSTQuery<T>(string query, JsonConverter[] converters = null) where T : class
        {
            string jsonResponse = await ExecuteQueryReturningContent(query, HttpMethod.POST);
            return _jsonObjectConverter.DeserializeObject<T>(jsonResponse, converters);
        }

        private async Task<T> ExecuteDELETEQuery<T>(string query, JsonConverter[] converters = null) where T : class
        {
            string jsonResponse = await ExecuteQueryReturningContent(query, HttpMethod.DELETE);
            return _jsonObjectConverter.DeserializeObject<T>(jsonResponse, converters);
        }

        // Try Execute<T>
        public async Task<AsyncOperation<T>> TryExecuteGETQuery<T>(string query, JsonConverter[] converters = null)
            where T : class
        {
            try
            {
                var resultObject = await ExecuteGETQuery<T>(query, converters);

                return new AsyncOperation<T>
                {
                    Success = resultObject != null,
                    Result = resultObject
                };
            }
            catch (TwitterException)
            {
                return new FailedAsyncOperation<T>();
            }
        }

        public async Task<AsyncOperation<T>> TryExecutePOSTQuery<T>(string query, JsonConverter[] converters = null)
            where T : class
        {
            try
            {
                var resultObject = await ExecutePOSTQuery<T>(query, converters);

                return new AsyncOperation<T>
                {
                    Success = resultObject != null,
                    Result = resultObject
                };
            }
            catch (TwitterException)
            {
                return new FailedAsyncOperation<T>();
            }
        }

        public async Task<AsyncOperation<T>> TryExecuteDELETEQuery<T>(string query, JsonConverter[] converters = null)
            where T : class
        {
            try
            {
                var resultObject = await ExecuteDELETEQuery<T>(query, converters);

                return new AsyncOperation<T>
                {
                    Success = resultObject != null,
                    Result = resultObject
                };
            }
            catch (TwitterException)
            {
                return new FailedAsyncOperation<T>();
            }
        }

        // Cursor Query
        public async Task<IEnumerable<string>> ExecuteJsonCursorGETQuery<T>(
            string baseQuery,
            int maxObjectToRetrieve = Int32.MaxValue,
            string cursor = null)
            where T : class, IBaseCursorQueryDTO
        {
            int nbOfObjectsProcessed = 0;
            string previousCursor = null;
            string nextCursor = cursor;

            // add & for query parameters
            baseQuery = FormatBaseQuery(baseQuery);

            var result = new List<string>();
            while ((previousCursor != nextCursor || previousCursor == null) && nbOfObjectsProcessed < maxObjectToRetrieve)
            {
                T cursorResult = await ExecuteCursorQuery<T>(baseQuery, cursor, true);

                if (!CanCursorQueryContinue(cursorResult))
                {
                    return result;
                }

                nbOfObjectsProcessed += cursorResult.GetNumberOfObjectRetrieved();
                previousCursor = cursorResult.PreviousCursorStr;
                nextCursor = cursorResult.NextCursorStr;

                result.Add(cursorResult.RawResult);
            }

            return result;
        }

        public async Task<IEnumerable<T>> ExecuteCursorGETCursorQueryResult<T>(
            string baseQuery,
            int maxObjectToRetrieve = Int32.MaxValue,
            string cursor = null)
            where T : class, IBaseCursorQueryDTO
        {
            int nbOfObjectsProcessed = 0;
            string previousCursor = null;
            string nextCursor = cursor;

            // add & for query parameters
            baseQuery = FormatBaseQuery(baseQuery);

            var result = new List<T>();

            while ((previousCursor != nextCursor || previousCursor == null) && nbOfObjectsProcessed < maxObjectToRetrieve)
            {
                T cursorResult = await ExecuteCursorQuery<T>(baseQuery, nextCursor, false);

                if (!CanCursorQueryContinue(cursorResult))
                {
                    return result;
                }

                nbOfObjectsProcessed += cursorResult.GetNumberOfObjectRetrieved();
                previousCursor = cursorResult.PreviousCursorStr;
                nextCursor = cursorResult.NextCursorStr;

                result.Add(cursorResult);
            }

            return result;
        }

        private bool CanCursorQueryContinue<T>(T cursorResult) where T : class, IBaseCursorQueryDTO
        {
            if (cursorResult == null)
            {
                return false;
            }

            if (cursorResult.GetNumberOfObjectRetrieved() == 0 && cursorResult.NextCursor == 0 && cursorResult.PreviousCursor == -1)
            {
                return false;
            }

            return true;
        }

        public Task<IEnumerable<T>> ExecuteCursorGETQuery<T, T1>(
            string baseQuery,
            ICursorQueryParameters cursorQueryParameters)
            where T1 : class, IBaseCursorQueryDTO<T>
        {
            return ExecuteCursorGETQuery<T, T1>(baseQuery, cursorQueryParameters.PageSize, cursorQueryParameters.Cursor);
        }

        public async Task<IEnumerable<T>> ExecuteCursorGETQuery<T, T1>(
            string baseQuery,
            int maxObjectToRetrieve = int.MaxValue,
            string cursor = null)
            where T1 : class, IBaseCursorQueryDTO<T>
        {
            var cursorQueryResult = await ExecuteCursorGETCursorQueryResult<T1>(baseQuery, maxObjectToRetrieve, cursor);
            return _cursorQueryHelper.GetResultsFromCursorQuery(cursorQueryResult, maxObjectToRetrieve);
        }

        private string FormatBaseQuery(string baseQuery)
        {
            if (baseQuery.Contains("?") && baseQuery[baseQuery.Length - 1] != '?')
            {
                baseQuery += "&";
            }

            return baseQuery;
        }

        private async Task<T> ExecuteCursorQuery<T>(string baseQuery, string cursor, bool storeJson) where T : class, IBaseCursorQueryDTO
        {
            var query = $"{baseQuery}cursor={cursor}";

            var request = await TryExecuteGETQuery(query);

            if (request.Success)
            {
                var json = request.Result;
                var dtoResult = _jsonObjectConverter.DeserializeObject<T>(json, JsonQueryConverterRepository.Converters);

                if (storeJson)
                {
                    dtoResult.RawResult = json;
                }

                return dtoResult;
            }

            return null;
        }

        private async Task<string> ExecuteQueryReturningContent(string query, HttpMethod method, HttpContent httpContent = null, bool forceThrow = false)
        {
            try
            {
                var webRequestResult = await ExecuteQuery(query, method, (ITwitterCredentials) null, httpContent);
                return webRequestResult.Text;
            }
            catch (TwitterException)
            {
                if (forceThrow)
                {
                    throw;
                }

                return null;
            }
        }

        public Task<ITwitterResponse> ExecuteQuery(string query, HttpMethod method)
        {
            return ExecuteQuery(query, method, null);
        }

        public async Task<T> ExecuteQuery<T>(
            string query,
            HttpMethod method,
            ITwitterCredentials credentials,
            HttpContent httpContent) where T : class
        {
            var webRequestResult = await ExecuteQuery(query, method, credentials, httpContent);
            var jsonResponse = webRequestResult.Text;
            var deserializedObject = _jsonObjectConverter.DeserializeObject<T>(jsonResponse);
            return deserializedObject;
        }

        public async Task<ITwitterResult> ExecuteRequest(ITwitterRequest request)
        {
            var response = await _twitterRequestHandler.ExecuteQuery(request).ConfigureAwait(false);
            return _twitterResultFactory.Create(request, response);
        }

        public async Task<ITwitterResult<T>> ExecuteRequest<T>(ITwitterRequest request) where T : class
        {
            var response = await _twitterRequestHandler.ExecuteQuery(request).ConfigureAwait(false);
            return _twitterResultFactory.Create<T>(request, response);
        }


        // Concrete Execute
        public async Task<ITwitterResponse> ExecuteQuery(
            string query,
            HttpMethod method,
            ITwitterCredentials credentials,
            HttpContent httpContent = null)
        {
            if (query == null)
            {
                // When a query is null and has been generated by Tweetinvi it implies that one of the query parameter was invalid
                throw new ArgumentException("At least one of the arguments provided to the query was invalid.");
            }

            credentials = credentials ?? _credentialsAccessor.CurrentThreadCredentials;

            if (credentials == null)
            {
                throw new TwitterNullCredentialsException();
            }

            var twitterQuery = _twitterQueryFactory.Create(query, method, credentials);
            twitterQuery.HttpContent = httpContent;

            var twitterRequest = new TwitterRequest
            {
                Query = twitterQuery,
                ExecutionContext = new TwitterExecutionContext
                {
                    RateLimitTrackerMode = _settingsAccessor.RateLimitTrackerMode
                }
            };

            var twitterResult = await ExecuteRequest(twitterRequest);

            return twitterResult.Response;
        }

        // Consumer Credentials
        public async Task<ITwitterResponse> ExecuteQuery(
            string query,
            HttpMethod method,
            IConsumerOnlyCredentials credentials,
            HttpContent httpContent)
        {
            return await ExecuteQuery(query, method, new TwitterCredentials(credentials), httpContent);
        }

        public Task<ITwitterResponse> ExecuteQuery(string query, HttpMethod method, IConsumerOnlyCredentials credentials)
        {
            return ExecuteQuery(query, method, credentials, null);
        }

        public async Task<T> ExecuteQuery<T>(
            string query,
            HttpMethod method,
            IConsumerOnlyCredentials credentials,
            HttpContent httpContent) where T : class
        {
            return await ExecuteQuery<T>(query, method, new TwitterCredentials(credentials), httpContent);
        }


        public async Task<AsyncOperation<string>> TryExecuteMultipartQuery(ITwitterRequest request)
        {
            try
            {
                var response = await _twitterRequestHandler.ExecuteQuery(request);

                return new AsyncOperation<string>
                {
                    Success = true,
                    Result = response.Text
                };
            }
            catch (TwitterException ex)
            {
                return new FailedAsyncOperation<string>();
            }
        }

        // POST JSON body & get JSON response
        public async Task<T> ExecutePOSTQueryJsonBody<T>(string query, object reqBody, JsonConverter[] converters = null) where T : class
        {
            string jsonResponse = await ExecutePOSTQueryJsonBody(query, reqBody, converters);
            return _jsonObjectConverter.DeserializeObject<T>(jsonResponse, converters);
        }

        public Task<string> ExecutePOSTQueryJsonBody(string query, object reqBody, JsonConverter[] converters = null)
        {
            string jsonBody = _jsonObjectConverter.SerializeObject(reqBody, converters);

            return ExecuteQueryReturningContent(
                query,
                HttpMethod.POST,
                new StringContent(jsonBody, Encoding.UTF8, "application/json"));
        }

        // Download
        public async Task<byte[]> DownloadBinary(string url)
        {
            if (url == null)
            {
                throw new ArgumentNullException(nameof(url), "Url cannot be null.");
            }

            var credentials = _credentialsAccessor.CurrentThreadCredentials;

            if (credentials == null)
            {
                throw new TwitterNullCredentialsException();
            }

            var twitterQuery = _twitterQueryFactory.Create(url, HttpMethod.GET, credentials);

            var twitterRequest = new TwitterRequest
            {
                Query = twitterQuery,

                ExecutionContext = new TwitterExecutionContext
                {
                    RateLimitTrackerMode = _settingsAccessor.RateLimitTrackerMode
                }
            };

            var response = await _twitterRequestHandler.ExecuteQuery(twitterRequest);

            if (response.IsSuccessStatusCode)
            {
                return response.Binary;
            }

            return null;
        }

        // Sign
        public async Task<ITwitterRequestParameters> GenerateTwitterRequestParameters(string url, HttpMethod method, ITwitterCredentials credentials, HttpContent httpContent)
        {
            var twitterQuery = _twitterQueryFactory.Create(url, method, credentials);
            twitterQuery.HttpContent = httpContent;
            var twitterRequest = new TwitterRequest
            {
                Query = twitterQuery,
                ExecutionContext = new TwitterExecutionContext
                {
                    RateLimitTrackerMode = _settingsAccessor.RateLimitTrackerMode
                }
            };

            await _twitterRequestHandler.PrepareTwitterRequest(twitterRequest);

            return new TwitterRequestParameters
            {
                Url = twitterQuery.Url,
                HttpMethod = twitterQuery.HttpMethod,
                HttpContent = twitterQuery.HttpContent,
                AuthorizationHeader = twitterQuery.AuthorizationHeader,
                AcceptHeaders = twitterQuery.AcceptHeaders,
                CustomHeaders = twitterQuery.CustomHeaders
            };
        }
    }
}