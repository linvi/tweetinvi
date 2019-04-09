using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using Tweetinvi.Core.Public.Models.Authentication;
using Tweetinvi.Core.Web;
using Tweetinvi.Models;
using Tweetinvi.Models.DTO.QueryDTO;
using HttpMethod = Tweetinvi.Models.HttpMethod;

namespace Tweetinvi
{
    /// <summary>
    /// Build your own requests and get back results in 1 line.
    /// (Are you feeling the power already?)
    /// </summary>
    public static class TwitterAccessor
    {
        [ThreadStatic]
        private static ITwitterAccessor _twitterAccessor;
        public static ITwitterAccessor Accessor
        {
            get
            {
                if (_twitterAccessor == null)
                {
                    Initialize();
                }

                return _twitterAccessor;
            }
        }

        static TwitterAccessor()
        {
            Initialize();
        }

        private static void Initialize()
        {
            _twitterAccessor = TweetinviContainer.Resolve<ITwitterAccessor>();
        }

        /// <summary>
        /// Execute GET query and return json response
        /// </summary>
        public static string ExecuteGETQueryReturningJson(string query)
        {
            return Accessor.ExecuteGETQueryReturningJson(query).Result;
        }

        /// <summary>
        /// Execute POST query and return json response
        /// </summary>
        public static string ExecutePOSTQueryReturningJson(string query)
        {
            return Accessor.ExecutePOSTQueryReturningJson(query).Result;
        }

        /// <summary>
        /// Execute GET query and return 'dynamic' JObject
        /// </summary>
        public static JObject GetQueryableJsonObjectFromGETQuery(string query)
        {
            return Accessor.ExecuteGETQuery(query).Result;
        }

        /// <summary>
        /// Execute POST query and return 'dynamic' JObject
        /// </summary>
        public static JObject GetQueryableJsonObjectFromPOSTQuery(string query)
        {
            return Accessor.ExecutePOSTQuery(query).Result;
        }

        /// <summary>
        /// Execute GET query and return an object of type T located in a specific path of the json
        /// </summary>
        public static T ExecuteGETQueryFromPath<T>(string query, params string[] paths) where T : class
        {
            return Accessor.ExecuteGETQueryWithPath<T>(query, paths).Result;
        }

        /// <summary>
        /// Execute POST query and return an object of type T located in a specific path of the json
        /// </summary>
        public static T ExecutePOSTQueryFromPath<T>(string query, params string[] paths) where T : class
        {
            return Accessor.ExecutePOSTQueryWithPath<T>(query, paths).Result;
        }

        /// <summary>
        /// Execute GET query an return an object of type T
        /// </summary>
        public static T ExecuteGETQuery<T>(string query) where T : class
        {
            return Accessor.ExecuteGETQuery<T>(query).Result;
        }

        /// <summary>
        /// Download binary media from twitter urls
        /// </summary>
        /// <param name="url">URL to binary</param>
        /// <returns>byte[] array of binary data</returns>
        public static byte[] DownloadBinary(string url)
        {
            return Accessor.DownloadBinary(url).Result;
        }

        /// <summary>
        /// Execute POST query an return an object of type T
        /// </summary>
        public static T ExecutePOSTQuery<T>(string query) where T : class
        {
            return Accessor.ExecutePOSTQuery<T>(query).Result;
        }

        /// <summary>
        /// Try to execute a GET query an return an object of type T as well as if the query has succeeded
        /// </summary>
        public static bool TryExecuteGETQuery<T>(string query, out T resultObject) where T : class
        {
            var asyncOperation = Accessor.TryExecuteGETQuery<T>(query).Result;

            resultObject = asyncOperation.Result;

            return asyncOperation.Success;
        }

        /// <summary>
        /// Try to execute a POST query an return an object of type T as well as if the query has succeeded
        /// </summary>
        public static bool TryExecutePOSTQuery<T>(string query, out T resultObject) where T : class
        {
            var asyncOperation = Accessor.TryExecutePOSTQuery<T>(query).Result;

            resultObject = asyncOperation.Result;

            return asyncOperation.Success;
        }

        /// <summary>
        /// Try to execute a GET query an return whether the query has succeeded
        /// </summary>
        public static bool TryExecuteGETQuery(string query)
        {
            return Accessor.TryExecuteGETQuery(query).Result.Success;
        }

        /// <summary>
        /// Try to execute a POST query an return whether the query has succeeded.
        /// </summary>
        public static bool TryExecutePOSTQuery(string query)
        {
            return Accessor.TryExecutePOSTQuery(query).Result.Success;
        }

        // MultiPart Query

        /// <summary>
        /// Execute a POST mutlipart query an return the result as an object of type T.
        /// </summary>
        public static T ExecutePOSTMultipartQuery<T>(IMultipartHttpRequestParameters parameters) where T : class
        {
            return Accessor.ExecuteMultipartQuery<T>(parameters).Result;
        }

        /// <summary>
        /// Execute a POST mutlipart query an return the json result.
        /// </summary>
        public static string ExecuteJsonPOSTMultipartQuery(IMultipartHttpRequestParameters parameters)
        {
            return Accessor.ExecuteMultipartQuery(parameters).Result;
        }

        /// <summary>
        /// Execute a GET cursor query that returns a list of json
        /// </summary>
        public static IEnumerable<string> ExecuteCursorGETQueryReturningJson<T>(
            string baseQuery,
            int maxObjectToRetrieve = Int32.MaxValue,
            long cursor = -1)
            where T : class, IBaseCursorQueryDTO
        {
            return Accessor.ExecuteJsonCursorGETQuery<T>(baseQuery, maxObjectToRetrieve, cursor).Result;
        }

        /// <summary>
        /// Execute a GET cursor query that returns a list of objects of type T
        /// </summary>
        public static IEnumerable<T> ExecuteCursorGETCursorQueryResult<T>(
            string query,
            int maxObjectToRetrieve = Int32.MaxValue,
            long cursor = -1)
            where T : class, IBaseCursorQueryDTO
        {
            return Accessor.ExecuteCursorGETCursorQueryResult<T>(query, maxObjectToRetrieve, cursor).Result;
        }

        /// <summary>
        /// Execute a GET cursor query that returns a list of objects of type T
        /// </summary>
        public static IEnumerable<T> ExecuteCursorGETQuery<T, T1>(
            string baseQuery,
            int maxObjectToRetrieve = Int32.MaxValue,
            long cursor = -1)
            where T1 : class, IBaseCursorQueryDTO<T>
        {
            return Accessor.ExecuteCursorGETQuery<T, T1>(baseQuery, maxObjectToRetrieve, cursor).Result;
        }

        // POST HTTP Content
        public static bool TryPOSTJsonContent(string url, string json)
        {
            return Accessor.TryPOSTJsonContent(url, json);
        }

        // Base call
        /// <summary>
        /// Execute a query that returns json
        /// </summary>
        public static string ExecuteQuery(string query, HttpMethod method)
        {
            return Accessor.ExecuteQuery(query, method).Result?.Text;
        }

        /// <summary>
        /// Execute a query that returns WebRequestResult
        /// </summary>
        public static IWebRequestResult ExecuteQueryWithDetails(string query, HttpMethod method)
        {
            return Accessor.ExecuteQuery(query, method).Result;
        }

        public static IWebRequestResult ExecuteConsumerQuery(string query, HttpMethod method, IConsumerOnlyCredentials credentials)
        {
            return Accessor.ExecuteQuery(query, method, credentials, null).Result;
        }

        // Sign
        public static Task<ITwitterRequestParameters> GenerateTwitterRequestParameters(
            string query,
            HttpMethod method,
            ITwitterCredentials credentials = null,
            HttpContent content = null)
        {
            return Accessor.GenerateTwitterRequestParameters(query, method, credentials, content);
        }
    }
}