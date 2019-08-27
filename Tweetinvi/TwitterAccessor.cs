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
        public static Task<string> ExecuteGETQueryReturningJson(string query)
        {
            return Accessor.ExecuteGETQueryReturningJson(query);
        }

        /// <summary>
        /// Execute POST query and return json response
        /// </summary>
        public static Task<string> ExecutePOSTQueryReturningJson(string query)
        {
            return Accessor.ExecutePOSTQueryReturningJson(query);
        }

        /// <summary>
        /// Execute GET query and return 'dynamic' JObject
        /// </summary>
        public static Task<JObject> GetQueryableJsonObjectFromGETQuery(string query)
        {
            return Accessor.ExecuteGETQuery(query);
        }

        /// <summary>
        /// Execute POST query and return 'dynamic' JObject
        /// </summary>
        public static Task<JObject> GetQueryableJsonObjectFromPOSTQuery(string query)
        {
            return Accessor.ExecutePOSTQuery(query);
        }

        /// <summary>
        /// Execute GET query and return an object of type T located in a specific path of the json
        /// </summary>
        public static Task<T> ExecuteGETQueryFromPath<T>(string query, params string[] paths) where T : class
        {
            return Accessor.ExecuteGETQueryWithPath<T>(query, paths);
        }

        /// <summary>
        /// Execute POST query and return an object of type T located in a specific path of the json
        /// </summary>
        public static Task<T> ExecutePOSTQueryFromPath<T>(string query, params string[] paths) where T : class
        {
            return Accessor.ExecutePOSTQueryWithPath<T>(query, paths);
        }

        /// <summary>
        /// Execute GET query an return an object of type T
        /// </summary>
        public static Task<T> ExecuteGETQuery<T>(string query) where T : class
        {
            return Accessor.ExecuteGETQuery<T>(query);
        }

        /// <summary>
        /// Download binary media from twitter urls
        /// </summary>
        /// <param name="url">URL to binary</param>
        /// <returns>byte[] array of binary data</returns>
        public static Task<byte[]> DownloadBinary(string url)
        {
            return Accessor.DownloadBinary(url);
        }

        /// <summary>
        /// Execute POST query an return an object of type T
        /// </summary>
        public static Task<T> ExecutePOSTQuery<T>(string query) where T : class
        {
            return Accessor.ExecutePOSTQuery<T>(query);
        }

        /// <summary>
        /// Try to execute a GET query an return an object of type T as well as if the query has succeeded
        /// </summary>
        public static async Task<bool> TryExecuteGETQuery<T>(string query) where T : class
        {
            var asyncOperation = await Accessor.TryExecuteGETQuery<T>(query);

            return asyncOperation.Success;
        }

        /// <summary>
        /// Try to execute a POST query an return an object of type T as well as if the query has succeeded
        /// </summary>
        public static async Task<bool> TryExecutePOSTQuery<T>(string query) where T : class
        {
            var asyncOperation = await Accessor.TryExecutePOSTQuery<T>(query);

            return asyncOperation.Success;
        }

        /// <summary>
        /// Try to execute a GET query an return whether the query has succeeded
        /// </summary>
        public static async Task<bool> TryExecuteGETQuery(string query)
        {
            return (await Accessor.TryExecuteGETQuery(query)).Success;
        }

        /// <summary>
        /// Try to execute a POST query an return whether the query has succeeded.
        /// </summary>
        public static async Task<bool> TryExecutePOSTQuery(string query)
        {
            return (await Accessor.TryExecutePOSTQuery(query)).Success;
        }

        // MultiPart Query

        /// <summary>
        /// Execute a POST mutlipart query an return the result as an object of type T.
        /// </summary>
        public static Task<T> ExecutePOSTMultipartQuery<T>(IMultipartHttpRequestParameters parameters) where T : class
        {
            return Accessor.ExecuteMultipartQuery<T>(parameters);
        }

        /// <summary>
        /// Execute a POST mutlipart query an return the json result.
        /// </summary>
        public static Task<string> ExecuteJsonPOSTMultipartQuery(IMultipartHttpRequestParameters parameters)
        {
            return Accessor.ExecuteMultipartQuery(parameters);
        }

        /// <summary>
        /// Execute a GET cursor query that returns a list of json
        /// </summary>
        public static Task<IEnumerable<string>> ExecuteCursorGETQueryReturningJson<T>(
            string baseQuery,
            int maxObjectToRetrieve = Int32.MaxValue,
            string cursor = null)
            where T : class, IBaseCursorQueryDTO
        {
            return Accessor.ExecuteJsonCursorGETQuery<T>(baseQuery, maxObjectToRetrieve, cursor);
        }

        /// <summary>
        /// Execute a GET cursor query that returns a list of objects of type T
        /// </summary>
        public static Task<IEnumerable<T>> ExecuteCursorGETCursorQueryResult<T>(
            string query,
            int maxObjectToRetrieve = Int32.MaxValue,
            string cursor = null)
            where T : class, IBaseCursorQueryDTO
        {
            return Accessor.ExecuteCursorGETCursorQueryResult<T>(query, maxObjectToRetrieve, cursor);
        }

        /// <summary>
        /// Execute a GET cursor query that returns a list of objects of type T
        /// </summary>
        public static Task<IEnumerable<T>> ExecuteCursorGETQuery<T, T1>(
            string baseQuery,
            int maxObjectToRetrieve = Int32.MaxValue,
            string cursor = null)
            where T1 : class, IBaseCursorQueryDTO<T>
        {
            return Accessor.ExecuteCursorGETQuery<T, T1>(baseQuery, maxObjectToRetrieve, cursor);
        }

        // POST HTTP Content
        public static Task<bool> TryPOSTJsonContent(string url, string json)
        {
            return Accessor.TryPOSTJsonContent(url, json);
        }

        // Base call
        /// <summary>
        /// Execute a query that returns json
        /// </summary>
        public static async Task<string> ExecuteQuery(string query, HttpMethod method)
        {
            var result = await Accessor.ExecuteQuery(query, method);
            return result?.Text;
        }

        /// <summary>
        /// Execute a query that returns Response
        /// </summary>
        public static Task<ITwitterResponse> ExecuteQueryWithDetails(string query, HttpMethod method)
        {
            return Accessor.ExecuteQuery(query, method);
        }

        public static Task<ITwitterResponse> ExecuteConsumerQuery(string query, HttpMethod method, IConsumerOnlyCredentials credentials)
        {
            return Accessor.ExecuteQuery(query, method, credentials, null);
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