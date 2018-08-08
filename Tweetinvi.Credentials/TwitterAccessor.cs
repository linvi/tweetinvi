using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Tweetinvi.Core.Exceptions;
using Tweetinvi.Core.Helpers;
using Tweetinvi.Core.Public.Models.Authentication;
using Tweetinvi.Core.Public.Parameters;
using Tweetinvi.Core.Web;
using Tweetinvi.Core.Wrappers;
using Tweetinvi.Credentials.QueryJsonConverters;
using Tweetinvi.Exceptions;
using Tweetinvi.Models;
using Tweetinvi.Models.DTO.QueryDTO;
using Tweetinvi.WebLogic;
using HttpMethod = Tweetinvi.Models.HttpMethod;

namespace Tweetinvi.Credentials
{
    public class TwitterAccessor : ITwitterAccessor
    {
        private readonly IJObjectStaticWrapper _jObjectStaticWrapper;
        private readonly IJsonObjectConverter _jsonObjectConverter;
        private readonly IExceptionHandler _exceptionHandler;
        private readonly ICursorQueryHelper _cursorQueryHelper;
        private readonly ITwitterRequestHandler _twitterRequestHandler;

        public TwitterAccessor(
            IJObjectStaticWrapper jObjectStaticWrapper,
            IJsonObjectConverter jsonObjectConverter,
            IExceptionHandler exceptionHandler,
            ICursorQueryHelper cursorQueryHelper,
            ITwitterRequestHandler twitterRequestHandler)
        {
            _jObjectStaticWrapper = jObjectStaticWrapper;
            _jsonObjectConverter = jsonObjectConverter;
            _exceptionHandler = exceptionHandler;
            _cursorQueryHelper = cursorQueryHelper;
            _twitterRequestHandler = twitterRequestHandler;
        }

        // Execute<Json>
        public string ExecuteGETQueryReturningJson(string query)
        {
            return ExecuteQueryReturningContent(query, HttpMethod.GET);
        }

        public string ExecutePOSTQueryReturningJson(string query)
        {
            return ExecuteQueryReturningContent(query, HttpMethod.POST);
        }

        public string ExecuteDELETEQueryReturningJson(string query)
        {
            return ExecuteQueryReturningContent(query, HttpMethod.DELETE);
        }

        // Try Execute<Json>
        public bool TryExecuteGETQuery(string query, out string json)
        {
            try
            {
                json = ExecuteGETQueryReturningJson(query);
                return json != null;
            }
            catch (TwitterException)
            {
                if (!_exceptionHandler.SwallowWebExceptions)
                {
                    throw;
                }

                json = null;
                return false;
            }
        }

        public bool TryExecutePOSTQuery(string query, out string json)
        {
            try
            {
                json = ExecutePOSTQueryReturningJson(query);
                return json != null;
            }
            catch (TwitterException)
            {
                json = null;
                return false;
            }
        }

        public bool TryExecuteDELETEQuery(string query, out string json)
        {
            try
            {
                json = ExecuteDELETEQueryReturningJson(query);
                return json != null;
            }
            catch (TwitterException)
            {
                json = null;
                return false;
            }
        }

        // Execute<JObject>
        public JObject ExecuteGETQuery(string query)
        {
            string jsonResponse = ExecuteQueryReturningContent(query, HttpMethod.GET);
            return _jObjectStaticWrapper.GetJobjectFromJson(jsonResponse);
        }

        public JObject ExecutePOSTQuery(string query)
        {
            string jsonResponse = ExecuteQueryReturningContent(query, HttpMethod.POST);
            return _jObjectStaticWrapper.GetJobjectFromJson(jsonResponse);
        }

        public JObject ExecuteDELETEQuery(string query)
        {
            string jsonResponse = ExecuteQueryReturningContent(query, HttpMethod.DELETE);
            return _jObjectStaticWrapper.GetJobjectFromJson(jsonResponse);
        }

        // Get specific type of object from path
        public T ExecuteGETQueryWithPath<T>(string query, string[] paths) where T : class
        {
            var jObject = ExecuteGETQuery(query);
            return GetResultFromPath<T>(jObject, paths);
        }

        public T ExecutePOSTQueryWithPath<T>(string query, string[] paths) where T : class
        {
            var jObject = ExecutePOSTQuery(query);
            return GetResultFromPath<T>(jObject, paths);
        }

        public T ExecuteDELETEQueryWithPath<T>(string query, string[] paths) where T : class
        {
            var jObject = ExecuteDELETEQuery(query);
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
        public T ExecuteGETQuery<T>(string query, JsonConverter[] converters = null) where T : class
        {
            string jsonResponse = ExecuteQueryReturningContent(query, HttpMethod.GET);
            return _jsonObjectConverter.DeserializeObject<T>(jsonResponse, converters);
        }

        public T ExecutePOSTQuery<T>(string query, JsonConverter[] converters = null) where T : class
        {
            string jsonResponse = ExecuteQueryReturningContent(query, HttpMethod.POST);
            return _jsonObjectConverter.DeserializeObject<T>(jsonResponse, converters);
        }

        public T ExecuteDELETEQuery<T>(string query, JsonConverter[] converters = null) where T : class
        {
            string jsonResponse = ExecuteQueryReturningContent(query, HttpMethod.DELETE);
            return _jsonObjectConverter.DeserializeObject<T>(jsonResponse, converters);
        }

        // Try Execute
        public bool TryExecuteGETQuery(string query, JsonConverter[] converters = null)
        {
            try
            {
                // Call ExecuteQuery so that we get the string response rather than a JObject, allowing us to differentiate
                //  between the empty string (successful request with no response) and null (error)
                string strResponse = ExecuteQueryReturningContent(query, HttpMethod.GET);
                return strResponse != null;
            }
            catch (TwitterException)
            {
                if (!_exceptionHandler.SwallowWebExceptions)
                {
                    throw;
                }

                return false;
            }
        }

        public bool TryExecutePOSTQuery(string query, JsonConverter[] converters = null)
        {
            try
            {
                // Call ExecuteQuery so that we get the string response rather than a JObject, allowing us to differentiate
                //  between the empty string (successful request with no response) and null (error)
                string strResponse = ExecuteQueryReturningContent(query, HttpMethod.POST);
                return strResponse != null;
            }
            catch (TwitterException)
            {
                if (!_exceptionHandler.SwallowWebExceptions)
                {
                    throw;
                }

                return false;
            }
        }

        public bool TryExecuteDELETEQuery(string query, JsonConverter[] converters = null)
        {
            try
            {
                // Call ExecuteQuery so that we get the string response rather than a JObject, allowing us to differentiate
                //  between the empty string (successful request with no response) and null (error)
                string strResponse = ExecuteQueryReturningContent(query, HttpMethod.DELETE);
                return strResponse != null;
            }
            catch (TwitterException)
            {
                if (!_exceptionHandler.SwallowWebExceptions)
                {
                    throw;
                }

                return false;
            }
        }

        // Try Execute<T>
        public bool TryExecuteGETQuery<T>(string query, out T resultObject, JsonConverter[] converters = null)
            where T : class
        {
            try
            {
                resultObject = ExecuteGETQuery<T>(query, converters);
                return resultObject != null;
            }
            catch (TwitterException)
            {
                if (!_exceptionHandler.SwallowWebExceptions)
                {
                    throw;
                }

                resultObject = null;
                return false;
            }
        }

        public bool TryExecutePOSTQuery<T>(string query, out T resultObject, JsonConverter[] converters = null)
            where T : class
        {
            try
            {
                resultObject = ExecutePOSTQuery<T>(query, converters);
                return resultObject != null;
            }
            catch (TwitterException)
            {
                if (!_exceptionHandler.SwallowWebExceptions)
                {
                    throw;
                }

                resultObject = null;
                return false;
            }
        }

        public bool TryExecuteDELETEQuery<T>(string query, out T resultObject, JsonConverter[] converters = null)
            where T : class
        {
            try
            {
                resultObject = ExecuteDELETEQuery<T>(query, converters);
                return resultObject != null;
            }
            catch (TwitterException)
            {
                if (!_exceptionHandler.SwallowWebExceptions)
                {
                    throw;
                }

                resultObject = null;
                return false;
            }
        }

        // Multipart Query
        public T ExecuteMultipartQuery<T>(IMultipartHttpRequestParameters parameters, JsonConverter[] converters = null) where T : class
        {
            string jsonResponse = ExecuteMultipartQuery(parameters);
            return _jsonObjectConverter.DeserializeObject<T>(jsonResponse, converters);
        }

        public bool TryExecuteMultipartQuery(IMultipartHttpRequestParameters parameters)
        {
            string unused;
            return TryExecuteMultipartQuery(parameters, out unused);
        }

        public string ExecuteMultipartQuery(IMultipartHttpRequestParameters parameters)
        {
            string result;
            TryExecuteMultipartQuery(parameters, out result);

            return result;
        }

        // Cursor Query
        public IEnumerable<string> ExecuteJsonCursorGETQuery<T>(
                string baseQuery,
                int maxObjectToRetrieve = Int32.MaxValue,
                long cursor = -1)
            where T : class, IBaseCursorQueryDTO
        {
            int nbOfObjectsProcessed = 0;
            long previousCursor = -2;
            long nextCursor = cursor;

            // add & for query parameters
            baseQuery = FormatBaseQuery(baseQuery);

            var result = new List<string>();
            while (previousCursor != nextCursor && nbOfObjectsProcessed < maxObjectToRetrieve)
            {
                T cursorResult = ExecuteCursorQuery<T>(baseQuery, cursor, true);

                if (!CanCursorQueryContinue(cursorResult))
                {
                    return result;
                }

                nbOfObjectsProcessed += cursorResult.GetNumberOfObjectRetrieved();
                previousCursor = cursorResult.PreviousCursor;
                nextCursor = cursorResult.NextCursor;

                result.Add(cursorResult.RawJson);
            }

            return result;
        }

        public IEnumerable<T> ExecuteCursorGETCursorQueryResult<T>(
                string baseQuery,
                int maxObjectToRetrieve = Int32.MaxValue,
                long cursor = -1)
            where T : class, IBaseCursorQueryDTO
        {
            int nbOfObjectsProcessed = 0;
            long previousCursor = -2;
            long nextCursor = cursor;

            // add & for query parameters
            baseQuery = FormatBaseQuery(baseQuery);

            var result = new List<T>();
            while (previousCursor != nextCursor && nbOfObjectsProcessed < maxObjectToRetrieve)
            {
                T cursorResult = ExecuteCursorQuery<T>(baseQuery, nextCursor, false);

                if (!CanCursorQueryContinue(cursorResult))
                {
                    return result;
                }

                nbOfObjectsProcessed += cursorResult.GetNumberOfObjectRetrieved();
                previousCursor = cursorResult.PreviousCursor;
                nextCursor = cursorResult.NextCursor;

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

        public IEnumerable<T> ExecuteCursorGETQuery<T, T1>(
            string baseQuery,
            ICursorQueryParameters cursorQueryParameters)
            where T1 : class, IBaseCursorQueryDTO<T>
        {
            return ExecuteCursorGETQuery<T, T1>(baseQuery, cursorQueryParameters.MaximumNumberOfResults,
                cursorQueryParameters.Cursor);
        }

        public IEnumerable<T> ExecuteCursorGETQuery<T, T1>(
                string baseQuery,
                int maxObjectToRetrieve = Int32.MaxValue,
                long cursor = -1)
            where T1 : class, IBaseCursorQueryDTO<T>
        {
            var cursorQueryResult = ExecuteCursorGETCursorQueryResult<T1>(baseQuery, maxObjectToRetrieve, cursor);
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

        private T ExecuteCursorQuery<T>(string baseQuery, long cursor, bool storeJson) where T : class, IBaseCursorQueryDTO
        {
            var query = string.Format("{0}cursor={1}", baseQuery, cursor);

            string json;
            if (TryExecuteGETQuery(query, out json))
            {
                var dtoResult = _jsonObjectConverter.DeserializeObject<T>(json, JsonQueryConverterRepository.Converters);

                if (storeJson)
                {
                    dtoResult.RawJson = json;
                }

                return dtoResult;
            }

            return null;
        }

        // POST Http Content
        public bool TryPOSTJsonContent(string url, string json)
        {
            try
            {
                ExecuteQuery(url, (HttpMethod) HttpMethod.POST, (ITwitterCredentials) null, new StringContent(json));
                return true;
            }
            catch (TwitterException)
            {
                return false;
            }
        }

        public string ExecuteQueryReturningContent(string query, HttpMethod method, HttpContent httpContent = null, bool forceThrow = false)
        {
            try
            {
                return ExecuteQuery(query, method, (ITwitterCredentials) null, httpContent).Text;
            }
            catch (TwitterException ex)
            {
                if (forceThrow)
                {
                    throw;
                }

                HandleQueryException(ex);
                return null;
            }
        }

        public IWebRequestResult ExecuteQuery(string query, HttpMethod method)
        {
            return ExecuteQuery(query, method, null);
        }

        public T ExecuteQuery<T>(
            string query, 
            HttpMethod method, 
            ITwitterCredentials credentials,
            HttpContent httpContent) where T : class
        {
            var webRequestResult = ExecuteQuery(query, method, credentials, httpContent);
            var jsonResponse = webRequestResult.Text;
            return _jsonObjectConverter.DeserializeObject<T>(jsonResponse);
        }

        // Concrete Execute
        public IWebRequestResult ExecuteQuery(
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

            return _twitterRequestHandler.ExecuteQuery(query, method, httpContent: httpContent, credentials: credentials);
        }

        // Consumer Credentials
        public IWebRequestResult ExecuteQuery(string query, HttpMethod method,
            IConsumerOnlyCredentials credentials, HttpContent httpContent)
        {
            return ExecuteQuery(query, method, new TwitterCredentials(credentials), httpContent);
        }

        public T ExecuteQuery<T>(string query, HttpMethod method, IConsumerOnlyCredentials credentials,
            HttpContent httpContent) where T : class
        {
            return ExecuteQuery<T>(query, method, new TwitterCredentials(credentials), httpContent);
        }


        private bool TryExecuteMultipartQuery(IMultipartHttpRequestParameters parameters, out string result)
        {
            if (parameters.Query == null)
            {
                throw new ArgumentException("At least one of the arguments provided to the query was invalid.");
            }

            try
            {
                result = _twitterRequestHandler.ExecuteMultipartQuery(parameters);
                return true;
            }
            catch (TwitterException ex)
            {
                HandleQueryException(ex);

                result = null;
                return false;
            }
        }

        // POST JSON body & get JSON response
        public T ExecutePOSTQueryJsonBody<T>(string query, object reqBody, JsonConverter[] converters = null) where T : class
        {
            string jsonResponse = ExecutePOSTQueryJsonBody(query, reqBody, converters);
            return _jsonObjectConverter.DeserializeObject<T>(jsonResponse, converters);
        }

        public string ExecutePOSTQueryJsonBody(string query, object reqBody, JsonConverter[] converters = null)
        {
            string jsonBody = _jsonObjectConverter.SerializeObject(reqBody, converters);

            return ExecuteQueryReturningContent(query, HttpMethod.POST,
                new StringContent(jsonBody, Encoding.UTF8, "application/json"));
        }

        // Download
        public byte[] DownloadBinary(string url)
        {
            if (url == null)
            {
                throw new ArgumentNullException("URL", "Url cannot be null.");
            }

            try
            {
                return _twitterRequestHandler.DownloadBinary(url);
            }
            catch (TwitterException ex)
            {
                HandleQueryException(ex);
                return null;
            }
        }

        private void HandleQueryException(TwitterException ex)
        {
            if (_exceptionHandler.SwallowWebExceptions)
            {
                return;
            }

            throw ex;
        }

        // Sign
        public ITwitterRequestParameters GenerateTwitterRequestParameters(string url, HttpMethod method, ITwitterCredentials credentials, HttpContent httpContent)
        {
            var requestParameters = new HttpRequestParameters
            {
                Query = url,
                HttpMethod = method,
                HttpContent = httpContent
            };

            var twitterQuery = _twitterRequestHandler.GetTwitterQuery(requestParameters, RateLimitTrackerMode.None, credentials);

            return new TwitterRequestParameters()
            {
                QueryURL = twitterQuery.QueryURL,
                HttpMethod = twitterQuery.HttpMethod,
                HttpContent = twitterQuery.HttpContent,
                AuthorizationHeader = twitterQuery.AuthorizationHeader,
                AcceptHeaders = twitterQuery.AcceptHeaders,
                CustomHeaders = twitterQuery.CustomHeaders
            };
        }
    }
}