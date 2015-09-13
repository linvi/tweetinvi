using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Tweetinvi.Core.Enum;
using Tweetinvi.Core.Interfaces.DTO.QueryDTO;

namespace Tweetinvi
{
    public static class TwitterAccessorAsync
    {
        // Get json response from query
        public static async Task<string> ExecuteJsonGETQuery(string query)
        {
            return await Sync.ExecuteTaskAsync(() => TwitterAccessor.ExecuteJsonGETQuery(query));
        }

        public static async Task<string> ExecuteJsonPOSTQuery(string query)
        {
            return await Sync.ExecuteTaskAsync(() => TwitterAccessor.ExecuteJsonPOSTQuery(query));
        }

        // Get object (DTO) form query
        public static async Task<T> ExecuteGETQuery<T>(string query) where T : class
        {
            return await Sync.ExecuteTaskAsync(() => TwitterAccessor.ExecuteGETQuery<T>(query));
        }

        public static async Task<T> ExecutePOSTQuery<T>(string query) where T : class
        {
            return await Sync.ExecuteTaskAsync(() => TwitterAccessor.ExecutePOSTQuery<T>(query));
        }

        // Try Get object (DTO) from query
        public static async Task<Tuple<bool, T>> TryExecuteGETQuery<T>(string query) where T : class
        {
            T resultObject = null;
            var result = await Sync.ExecuteTaskAsync(() => TwitterAccessor.TryExecuteGETQuery(query, out resultObject));
            return new Tuple<bool, T>(result, resultObject);
        }

        public static async Task<Tuple<bool, T>> TryExecutePOSTQuery<T>(string query) where T : class
        {
            T resultObject = null;
            var result = await Sync.ExecuteTaskAsync(() => TwitterAccessor.TryExecutePOSTQuery(query, out resultObject));
            return new Tuple<bool, T>(result, resultObject);
        }

        // Try Operation and check success
        public static async Task<bool> TryExecuteGETQuery(string query)
        {
            return await Sync.ExecuteTaskAsync(() => TwitterAccessor.TryExecuteGETQuery(query));
        }

        public static async Task<bool> TryExecutePOSTQuery(string query)
        {
            return await Sync.ExecuteTaskAsync(() => TwitterAccessor.TryExecutePOSTQuery(query));
        }

        // Cusror Query
        public static async Task<IEnumerable<string>> ExecuteJsonCursorGETQuery<T>(
            string baseQuery,
            int maxObjectToRetrieve = Int32.MaxValue,
            long cursor = -1)
            where T : class, IBaseCursorQueryDTO
        {
            return await Sync.ExecuteTaskAsync(() => TwitterAccessor.ExecuteJsonCursorGETQuery<T>(baseQuery, maxObjectToRetrieve, cursor));
        }

        public static async Task<IEnumerable<T>> ExecuteCursorGETCursorQueryResult<T>(
            string query,
            int maxObjectToRetrieve = Int32.MaxValue,
            long cursor = -1)
            where T : class, IBaseCursorQueryDTO
        {
            return await Sync.ExecuteTaskAsync(() => TwitterAccessor.ExecuteCursorGETCursorQueryResult<T>(query, maxObjectToRetrieve, cursor));
        }

        public static async Task<IEnumerable<T>> ExecuteCursorGETQuery<T, T1>(
            string query,
            int maxObjectToRetrieve = Int32.MaxValue,
            long cursor = -1)
            where T1 : class, IBaseCursorQueryDTO<T>
        {
            return await Sync.ExecuteTaskAsync(() => TwitterAccessor.ExecuteCursorGETQuery<T, T1>(query, maxObjectToRetrieve, cursor));
        }

        // Base call
        public static async Task<string> ExecuteQuery(string query, HttpMethod method)
        {
            return await Sync.ExecuteTaskAsync(() => TwitterAccessor.ExecuteQuery(query, method));
        }
    }
}
