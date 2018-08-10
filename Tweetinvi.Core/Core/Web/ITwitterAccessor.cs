using System;
using System.Collections.Generic;
using System.Net.Http;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Tweetinvi.Core.Public.Models.Authentication;
using Tweetinvi.Core.Public.Parameters;
using Tweetinvi.Models;
using Tweetinvi.Models.DTO.QueryDTO;
using HttpMethod = Tweetinvi.Models.HttpMethod;

namespace Tweetinvi.Core.Web
{
    public interface ITwitterAccessor
    {
        // Get Json
        string ExecuteGETQueryReturningJson(string query);
        string ExecutePOSTQueryReturningJson(string query);
        string ExecuteDELETEQueryReturningJson(string query);

        // Try Execute<Json>
        bool TryExecuteGETQuery(string query, out string json);
        bool TryExecutePOSTQuery(string query, out string json);
        bool TryExecuteDELETEQuery(string query, out string json);

        // Get unknown type of objects
        JObject ExecuteGETQuery(string query);
        JObject ExecutePOSTQuery(string query);
        JObject ExecuteDELETEQuery(string query);

        // Get specific type of object from path
        T ExecuteGETQueryWithPath<T>(string query, params string[] paths) where T : class;
        T ExecutePOSTQueryWithPath<T>(string query, params string[] paths) where T : class;
        T ExecuteDELETEQueryWithPath<T>(string query, params string[] paths) where T : class;

        // Get specific type of object
        T ExecuteGETQuery<T>(string query, JsonConverter[] converters = null) where T : class;
        T ExecutePOSTQuery<T>(string query, JsonConverter[] converters = null) where T : class;
        T ExecuteDELETEQuery<T>(string query, JsonConverter[] converters = null) where T : class;

        // Try Execute
        bool TryExecuteGETQuery(string query, JsonConverter[] converters = null);
        bool TryExecutePOSTQuery(string query, JsonConverter[] converters = null);
        bool TryExecuteDELETEQuery(string query, JsonConverter[] converters = null);

        // Try Get Result
        bool TryExecuteGETQuery<T>(string query, out T resultObject, JsonConverter[] converters = null) where T : class;
        bool TryExecutePOSTQuery<T>(string query, out T resultObject, JsonConverter[] converters = null) where T : class;
        bool TryExecuteDELETEQuery<T>(string query, out T resultObject, JsonConverter[] converters = null) where T : class;

        // Multipart Query

        /// <summary>
        /// Starts a multipart HttpWebRequest required by Twitter to upload binaries
        /// </summary>
        string ExecuteMultipartQuery(IMultipartHttpRequestParameters parameters);

        /// <summary>
        /// Starts a multipart HttpWebRequest required by Twitter to upload binaries
        /// </summary>
        T ExecuteMultipartQuery<T>(IMultipartHttpRequestParameters parameters, JsonConverter[] converters = null) where T : class;

        /// <summary>
        /// Starts a multipart HttpWebRequest required by Twitter to upload binaries
        /// </summary>
        bool TryExecuteMultipartQuery(IMultipartHttpRequestParameters parameters);

        // Cursor Query
        IEnumerable<string> ExecuteJsonCursorGETQuery<T>(
            string baseQuery,
            int maxObjectToRetrieve = Int32.MaxValue,
            long cursor = -1)
            where T : class, IBaseCursorQueryDTO;

        IEnumerable<T> ExecuteCursorGETCursorQueryResult<T>(
            string query,
            int maxObjectToRetrieve = Int32.MaxValue,
            long cursor = -1)
            where T : class, IBaseCursorQueryDTO;

        IEnumerable<T> ExecuteCursorGETQuery<T, T1>(
            string baseQuery,
            int maxObjectToRetrieve = Int32.MaxValue,
            long cursor = -1)
            where T1 : class, IBaseCursorQueryDTO<T>;

        // Http Content
        bool TryPOSTJsonContent(string url, string json);

        // POST JSON body & get JSON response
        T ExecutePOSTQueryJsonBody<T>(string query, object reqBody, JsonConverter[] converters = null) where T : class;
        string ExecutePOSTQueryJsonBody(string query, object reqBody, JsonConverter[] converters = null);

        // Get Json from Twitter

        IWebRequestResult ExecuteQuery(string query, HttpMethod method);
        IWebRequestResult ExecuteQuery(string query, HttpMethod method, ITwitterCredentials credentials, HttpContent httpContent = null);
        T ExecuteQuery<T>(string query, HttpMethod method, ITwitterCredentials credentials, HttpContent httpContent) where T : class;


        // Get Binary data from twitter URL
        byte[] DownloadBinary(string url);
        ITwitterRequestParameters GenerateTwitterRequestParameters(string url, HttpMethod method, ITwitterCredentials credentials, HttpContent httpContent);

        IEnumerable<T> ExecuteCursorGETQuery<T, T1>(
            string baseQuery,
            ICursorQueryParameters cursorQueryParameters)
            where T1 : class, IBaseCursorQueryDTO<T>;

        // Consumer Credentials Query
        IWebRequestResult ExecuteQuery(string query, HttpMethod method, IConsumerOnlyCredentials credentials, HttpContent httpContent = null);
        T ExecuteQuery<T>(string query, HttpMethod method, IConsumerOnlyCredentials credentials, HttpContent httpContent = null) where T : class;
    }
}