using System;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using Tweetinvi.Core.Enum;
using Tweetinvi.Core.Interfaces.Credentials;
using Tweetinvi.Core.Interfaces.DTO.QueryDTO;
using Tweetinvi.Core.Web;

namespace Tweetinvi
{
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
        public static string ExecuteJsonGETQuery(string query)
        {
            return Accessor.ExecuteJsonGETQuery(query);
        }

        /// <summary>
        /// Execute POST query and return json response
        /// </summary>
        public static string ExecuteJsonPOSTQuery(string query)
        {
            return Accessor.ExecuteJsonPOSTQuery(query);
        }

        /// <summary>
        /// Execute GET query and return 'dynamic' JObject
        /// </summary>
        public static JObject GetQueryableJsonObjectFromGETQuery(string query)
        {
            return Accessor.ExecuteGETQuery(query);
        }

        /// <summary>
        /// Execute POST query and return 'dynamic' JObject
        /// </summary>
        public static JObject GetQueryableJsonObjectFromPOSTQuery(string query)
        {
            return Accessor.ExecutePOSTQuery(query);
        }

        /// <summary>
        /// Execute GET query and return an object of type T located in a specific path of the json
        /// </summary>
        public static T ExecuteGETQueryFromPath<T>(string query, params string[] paths) where T : class
        {
            return Accessor.ExecuteGETQueryWithPath<T>(query, paths);
        }

        /// <summary>
        /// Execute POST query and return an object of type T located in a specific path of the json
        /// </summary>
        public static T ExecutePOSTQueryFromPath<T>(string query, params string[] paths) where T : class
        {
            return Accessor.ExecutePOSTQueryWithPath<T>(query, paths);
        }

        /// <summary>
        /// Execute GET query an return an object of type T
        /// </summary>
        public static T ExecuteGETQuery<T>(string query) where T : class
        {
            return Accessor.ExecuteGETQuery<T>(query);
        }

        /// <summary>
        /// Execute POST query an return an object of type T
        /// </summary>
        public static T ExecutePOSTQuery<T>(string query) where T : class
        {
            return Accessor.ExecutePOSTQuery<T>(query);
        }

        /// <summary>
        /// Try to execute a GET query an return an object of type T as well as if the query has succeeded
        /// </summary>
        public static bool TryExecuteGETQuery<T>(string query, out T resultObject) where T : class
        {
            return Accessor.TryExecuteGETQuery(query, out resultObject);
        }

        /// <summary>
        /// Try to execute a POST query an return an object of type T as well as if the query has succeeded
        /// </summary>
        public static bool TryExecutePOSTQuery<T>(string query, out T resultObject) where T : class
        {
            return Accessor.TryExecutePOSTQuery(query, out resultObject);
        }

        /// <summary>
        /// Try to execute a GET query an return whether the query has succeeded
        /// </summary>
        public static bool TryExecuteGETQuery(string query)
        {
            return Accessor.TryExecuteGETQuery(query);
        }

        /// <summary>
        /// Try to execute a POST query an return whether the query has succeeded.
        /// </summary>
        public static bool TryExecutePOSTQuery(string query)
        {
            return Accessor.TryExecutePOSTQuery(query);
        }

        // MultiPart Query

        /// <summary>
        /// Execute a POST mutlipart query an return the result as an object of type T.
        /// </summary>
        public static T ExecutePOSTMultipartQuery<T>(IMultipartHttpRequestParameters parameters) where T : class
        {
            return Accessor.ExecuteMultipartQuery<T>(parameters);
        }

        /// <summary>
        /// Execute a POST mutlipart query an return the json result.
        /// </summary>
        public static string ExecuteJsonPOSTMultipartQuery(IMultipartHttpRequestParameters parameters)
        {
            return Accessor.ExecuteMultipartQuery(parameters);
        }

        /// <summary>
        /// Execute a GET cursor query that returns a list of json
        /// </summary>
        public static IEnumerable<string> ExecuteJsonCursorGETQuery<T>(
            string baseQuery,
            int maxObjectToRetrieve = Int32.MaxValue,
            long cursor = -1)
            where T : class, IBaseCursorQueryDTO
        {
            return Accessor.ExecuteJsonCursorGETQuery<T>(baseQuery, maxObjectToRetrieve, cursor);
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
            return Accessor.ExecuteCursorGETCursorQueryResult<T>(query, maxObjectToRetrieve, cursor);
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
            return Accessor.ExecuteCursorGETQuery<T, T1>(baseQuery, maxObjectToRetrieve, cursor);
        }

        // Base call

        /// <summary>
        /// Execute a query that returns json
        /// </summary>
        public static string ExecuteQuery(string query, HttpMethod method)
        {
            return Accessor.ExecuteQuery(query, method);
        }
    }
}