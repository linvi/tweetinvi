using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Tweetinvi.Core.Helpers;
using Tweetinvi.Core.Models;
using Tweetinvi.Core.Public.Models.Authentication;
using Tweetinvi.Core.Public.Parameters;
using Tweetinvi.Models;
using Tweetinvi.Models.DTO.QueryDTO;
using Tweetinvi.Models.Interfaces;
using HttpMethod = Tweetinvi.Models.HttpMethod;

namespace Tweetinvi.Core.Web
{
   
    public interface ITwitterAccessor
    {
        // Get Json
        Task<string> ExecuteGETQueryReturningJson(string query);
        Task<string> ExecutePOSTQueryReturningJson(string query);
        Task<string> ExecuteDELETEQueryReturningJson(string query);

        // Try Execute<Json>
        Task<AsyncOperation<string>> TryExecuteGETQuery(string query);
        Task<AsyncOperation<string>> TryExecutePOSTQuery(string query);
        Task<AsyncOperation<string>> TryExecuteDELETEQuery(string query);

        // Get unknown type of objects
        Task<JObject> ExecuteGETQuery(string query);
        Task<JObject> ExecutePOSTQuery(string query);
        Task<JObject> ExecuteDELETEQuery(string query);

        // Get specific type of object from path
        Task<T> ExecuteGETQueryWithPath<T>(string query, params string[] paths) where T : class;
        Task<T> ExecutePOSTQueryWithPath<T>(string query, params string[] paths) where T : class;
        Task<T> ExecuteDELETEQueryWithPath<T>(string query, params string[] paths) where T : class;

        // Get specific type of object
        Task<T> ExecuteGETQuery<T>(string query, JsonConverter[] converters = null) where T : class;
        Task<T> ExecutePOSTQuery<T>(string query, JsonConverter[] converters = null) where T : class;
        Task<T> ExecuteDELETEQuery<T>(string query, JsonConverter[] converters = null) where T : class;

        // Try Execute
        Task<bool> TryExecuteGETQuery(string query, JsonConverter[] converters = null);
        Task<bool> TryExecutePOSTQuery(string query, JsonConverter[] converters = null);
        Task<bool> TryExecuteDELETEQuery(string query, JsonConverter[] converters = null);

        // Try Get Result
        Task<AsyncOperation<T>> TryExecuteGETQuery<T>(string query, JsonConverter[] converters = null) where T : class;
        Task<AsyncOperation<T>> TryExecutePOSTQuery<T>(string query, JsonConverter[] converters = null) where T : class;
        Task<AsyncOperation<T>> TryExecuteDELETEQuery<T>(string query, JsonConverter[] converters = null) where T : class;

        // Multipart Query

        /// <summary>
        /// Starts a multipart HttpWebRequest required by Twitter to upload binaries
        /// </summary>
        Task<string> ExecuteMultipartQuery(IMultipartHttpRequestParameters parameters);

        /// <summary>
        /// Starts a multipart HttpWebRequest required by Twitter to upload binaries
        /// </summary>
        Task<T> ExecuteMultipartQuery<T>(IMultipartHttpRequestParameters parameters, JsonConverter[] converters = null) where T : class;

        /// <summary>
        /// Starts a multipart HttpWebRequest required by Twitter to upload binaries
        /// </summary>
        Task<AsyncOperation<string>> TryExecuteMultipartQuery(IMultipartHttpRequestParameters parameters);

        // Cursor Query
        Task<IEnumerable<string>> ExecuteJsonCursorGETQuery<T>(
            string baseQuery,
            int maxObjectToRetrieve = Int32.MaxValue,
            string cursor = null)
            where T : class, IBaseCursorQueryDTO;

        Task<IEnumerable<T>> ExecuteCursorGETCursorQueryResult<T>(
            string query,
            int maxObjectToRetrieve = Int32.MaxValue,
            string cursor = null)
            where T : class, IBaseCursorQueryDTO;

        Task<IEnumerable<T>> ExecuteCursorGETQuery<T, T1>(
            string baseQuery,
            int maxObjectToRetrieve = Int32.MaxValue,
            string cursor = null)
            where T1 : class, IBaseCursorQueryDTO<T>;

        // Http Content
        Task<bool> TryPOSTJsonContent(string url, string json);

        // POST JSON body & get JSON response
        Task<T> ExecutePOSTQueryJsonBody<T>(string query, object reqBody, JsonConverter[] converters = null) where T : class;
        Task<string> ExecutePOSTQueryJsonBody(string query, object reqBody, JsonConverter[] converters = null);

        // Get Json from Twitter

        Task<ITwitterResponse> ExecuteQuery(string query, HttpMethod method);
        Task<ITwitterResponse> ExecuteQuery(string query, HttpMethod method, ITwitterCredentials credentials, HttpContent httpContent = null);
        Task<T> ExecuteQuery<T>(string query, HttpMethod method, ITwitterCredentials credentials, HttpContent httpContent) where T : class;


        // Get Binary data from twitter URL
        Task<byte[]> DownloadBinary(string url);
        Task<ITwitterRequestParameters> GenerateTwitterRequestParameters(string url, HttpMethod method,
            ITwitterCredentials credentials, HttpContent httpContent);

        Task<IEnumerable<T>> ExecuteCursorGETQuery<T, T1>(
            string baseQuery,
            ICursorQueryParameters cursorQueryParameters)
            where T1 : class, IBaseCursorQueryDTO<T>;

        // Consumer Credentials Query
        Task<ITwitterResponse> ExecuteQuery(string query, HttpMethod method, IConsumerOnlyCredentials credentials, HttpContent httpContent = null);
        Task<T> ExecuteQuery<T>(string query, HttpMethod method, IConsumerOnlyCredentials credentials, HttpContent httpContent = null) where T : class;
        Task<ITwitterResult> ExecuteRequest(ITwitterRequest request);
        Task<ITwitterResult<T>> ExecuteRequest<T>(ITwitterRequest request) where T : class;
    }
}