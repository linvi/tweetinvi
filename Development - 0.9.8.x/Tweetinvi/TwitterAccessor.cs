using System;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using Tweetinvi.Core.Enum;
using Tweetinvi.Core.Interfaces.Credentials;
using Tweetinvi.Core.Interfaces.DTO.QueryDTO;

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

        // Get json response from query
        public static string ExecuteJsonGETQuery(string query)
        {
            return Accessor.ExecuteJsonGETQuery(query);
        }

        public static string ExecuteJsonPOSTQuery(string query)
        {
            return Accessor.ExecuteJsonPOSTQuery(query);
        }

        // Get Json object from query
        public static JObject GetQueryableJsonObjectFromGETQuery(string query)
        {
            return Accessor.ExecuteGETQuery(query);
        }

        public static JObject GetQueryableJsonObjectFromPOSTQuery(string query)
        {
            return Accessor.ExecutePOSTQuery(query);
        }

        // Get object from path
        public static T ExecuteGETQueryFromPath<T>(string query, params string[] paths) where T : class
        {
            return Accessor.ExecuteGETQueryWithPath<T>(query, paths);
        }

        public static T ExecutePOSTQueryFromPath<T>(string query, params string[] paths) where T : class
        {
            return Accessor.ExecutePOSTQueryWithPath<T>(query, paths);
        }

        // Get object (DTO) from query
        public static T ExecuteGETQuery<T>(string query) where T : class
        {
            return Accessor.ExecuteGETQuery<T>(query);
        }

        public static T ExecutePOSTQuery<T>(string query) where T : class
        {
            return Accessor.ExecutePOSTQuery<T>(query);
        }

        // Try Get object (DTO) from query
        public static bool TryExecuteGETQuery<T>(string query, out T resultObject) where T : class
        {
            return Accessor.TryExecuteGETQuery(query, out resultObject);
        }

        public static bool TryExecutePOSTQuery<T>(string query, out T resultObject) where T : class
        {
            return Accessor.TryExecutePOSTQuery(query, out resultObject);
        }

        // Try Operation and check success
        public static bool TryExecuteGETQuery(string query)
        {
            return Accessor.TryExecuteGETQuery(query);
        }

        public static bool TryExecutePOSTQuery(string query)
        {
            return Accessor.TryExecutePOSTQuery(query);
        }

        // Cusror Query
        public static IEnumerable<string> ExecuteJsonCursorGETQuery<T>(
            string baseQuery,
            int maxObjectToRetrieve = Int32.MaxValue,
            long cursor = -1)
            where T : class, IBaseCursorQueryDTO
        {
            return Accessor.ExecuteJsonCursorGETQuery<T>(baseQuery, maxObjectToRetrieve, cursor);
        }

        public static IEnumerable<T> ExecuteCursorGETCursorQueryResult<T>(
            string query,
            int maxObjectToRetrieve = Int32.MaxValue,
            long cursor = -1)
            where T : class, IBaseCursorQueryDTO
        {
            return Accessor.ExecuteCursorGETCursorQueryResult<T>(query, maxObjectToRetrieve, cursor);
        }

        public static IEnumerable<T> ExecuteCursorGETQuery<T, T1>(
            string baseQuery,
            int maxObjectToRetrieve = Int32.MaxValue,
            long cursor = -1)
            where T1 : class, IBaseCursorQueryDTO<T>
        {
            return Accessor.ExecuteCursorGETQuery<T, T1>(baseQuery, maxObjectToRetrieve, cursor);
        }

        // Base call
        public static string ExecuteQuery(string query, HttpMethod method)
        {
            return Accessor.ExecuteQuery(query, method);
        }
    }
}