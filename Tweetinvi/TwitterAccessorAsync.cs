using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Tweetinvi.Models;
using Tweetinvi.Models.DTO.QueryDTO;

namespace Tweetinvi
{
    public static class TwitterAccessorAsync
    {
        // Get json response from query
        public static ConfiguredTaskAwaitable<string> ExecuteGETQueryReturningJson(string query)
        {
            return Sync.ExecuteTaskAsync(() => TwitterAccessor.ExecuteGETQueryReturningJson(query));
        }

        public static ConfiguredTaskAwaitable<string> ExecutePOSTQueryReturningJson(string query)
        {
            return Sync.ExecuteTaskAsync(() => TwitterAccessor.ExecutePOSTQueryReturningJson(query));
        }

        // Get object (DTO) form query
        public static ConfiguredTaskAwaitable<T> ExecuteGETQuery<T>(string query) where T : class
        {
            return Sync.ExecuteTaskAsync(() => TwitterAccessor.ExecuteGETQuery<T>(query));
        }

        public static ConfiguredTaskAwaitable<T> ExecutePOSTQuery<T>(string query) where T : class
        {
            return Sync.ExecuteTaskAsync(() => TwitterAccessor.ExecutePOSTQuery<T>(query));
        }

        // Try Get object (DTO) from query
        public static ConfiguredTaskAwaitable<Tuple<bool, T>> TryExecuteGETQuery<T>(string query) where T : class
        {
            
            return Sync.ExecuteTaskAsync(() =>
            {
                bool success = TwitterAccessor.TryExecuteGETQuery(query, out T resultObject);
                return Tuple.Create(success, resultObject);
            });
        }

        public static ConfiguredTaskAwaitable<Tuple<bool, T>> TryExecutePOSTQuery<T>(string query) where T : class
        {
            return Sync.ExecuteTaskAsync(() =>
            {
                bool success = TwitterAccessor.TryExecutePOSTQuery(query, out T resultObject);
                return Tuple.Create(success, resultObject);
            });
        }

        // Try Operation and check success
        public static ConfiguredTaskAwaitable<bool> TryExecuteGETQuery(string query)
        {
            return Sync.ExecuteTaskAsync(() => TwitterAccessor.TryExecuteGETQuery(query));
        }

        public static ConfiguredTaskAwaitable<bool> TryExecutePOSTQuery(string query)
        {
            return Sync.ExecuteTaskAsync(() => TwitterAccessor.TryExecutePOSTQuery(query));
        }

        // Cusror Query
        public static ConfiguredTaskAwaitable<IEnumerable<string>> ExecuteJsonCursorGETQuery<T>(
            string baseQuery,
            int maxObjectToRetrieve = Int32.MaxValue,
            long cursor = -1)
            where T : class, IBaseCursorQueryDTO
        {
            return Sync.ExecuteTaskAsync(() => TwitterAccessor.ExecuteCursorGETQueryReturningJson<T>(baseQuery, maxObjectToRetrieve, cursor));
        }

        public static ConfiguredTaskAwaitable<IEnumerable<T>> ExecuteCursorGETCursorQueryResult<T>(
            string query,
            int maxObjectToRetrieve = Int32.MaxValue,
            long cursor = -1)
            where T : class, IBaseCursorQueryDTO
        {
            return Sync.ExecuteTaskAsync(() => TwitterAccessor.ExecuteCursorGETCursorQueryResult<T>(query, maxObjectToRetrieve, cursor));
        }

        public static ConfiguredTaskAwaitable<IEnumerable<T>> ExecuteCursorGETQuery<T, T1>(
            string query,
            int maxObjectToRetrieve = Int32.MaxValue,
            long cursor = -1)
            where T1 : class, IBaseCursorQueryDTO<T>
        {
            return Sync.ExecuteTaskAsync(() => TwitterAccessor.ExecuteCursorGETQuery<T, T1>(query, maxObjectToRetrieve, cursor));
        }

        // POST HTTP Content
        public static ConfiguredTaskAwaitable<bool> TryPOSTJsonContent(string url, string json)
        {
            return Sync.ExecuteTaskAsync(() => TwitterAccessor.TryPOSTJsonContent(url, json));
        }

        // Base call
        public static ConfiguredTaskAwaitable<string> ExecuteQuery(string query, HttpMethod method)
        {
            return Sync.ExecuteTaskAsync(() => TwitterAccessor.ExecuteQuery(query, method));
        }
    }
}
