using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using FakeItEasy;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Tweetinvi.Core.Models.Properties;
using Tweetinvi.Core.Web;
using Tweetinvi.Models;
using Tweetinvi.Models.DTO.QueryDTO;
using Tweetinvi.Parameters;
using HttpMethod = Tweetinvi.Models.HttpMethod;

namespace xUnitinvi.TestHelpers
{
    public class TwitterAccessorStub : Fake<ITwitterAccessor>, ITwitterAccessor
    {
        private readonly ITwitterAccessor _twitterAccessor;

        public TwitterAccessorStub(ITwitterAccessor twitterAccessor)
        {
            _twitterAccessor = twitterAccessor;
        }

        public Task<ITwitterResult> ExecuteRequest(ITwitterRequest request)
        {
            A.CallTo(() => FakedObject.ExecuteRequest(request))
                .ReturnsLazily((ITwitterRequest requestPassed) => _twitterAccessor.ExecuteRequest(requestPassed));

            return FakedObject.ExecuteRequest(request);
        }

        public Task<ITwitterResult<T>> ExecuteRequest<T>(ITwitterRequest request) where T : class
        {
            A.CallTo(() => FakedObject.ExecuteRequest<T>(request))
                .ReturnsLazily((ITwitterRequest requestPassed) => _twitterAccessor.ExecuteRequest<T>(requestPassed));

            return FakedObject.ExecuteRequest<T>(request);
        }

        public Task<string> ExecuteGETQueryReturningJson(string query)
        {
            throw new NotImplementedException();
        }

        public Task<string> ExecutePOSTQueryReturningJson(string query)
        {
            throw new NotImplementedException();
        }

        public Task<AsyncOperation<string>> TryExecuteGETQuery(string query)
        {
            throw new NotImplementedException();
        }

        public Task<AsyncOperation<string>> TryExecutePOSTQuery(string query)
        {
            throw new NotImplementedException();
        }

        public Task<AsyncOperation<string>> TryExecuteDELETEQuery(string query)
        {
            throw new NotImplementedException();
        }

        public Task<JObject> ExecuteGETQuery(string query)
        {
            throw new NotImplementedException();
        }

        public Task<JObject> ExecutePOSTQuery(string query)
        {
            throw new NotImplementedException();
        }

        public Task<T> ExecuteGETQueryWithPath<T>(string query, params string[] paths) where T : class
        {
            throw new NotImplementedException();
        }

        public Task<T> ExecutePOSTQueryWithPath<T>(string query, params string[] paths) where T : class
        {
            throw new NotImplementedException();
        }

        public Task<T> ExecuteGETQuery<T>(string query, JsonConverter[] converters = null) where T : class
        {
            throw new NotImplementedException();
        }

        public Task<T> ExecutePOSTQuery<T>(string query, JsonConverter[] converters = null) where T : class
        {
            throw new NotImplementedException();
        }

        public Task<AsyncOperation<T>> TryExecuteGETQuery<T>(string query, JsonConverter[] converters = null) where T : class
        {
            throw new NotImplementedException();
        }

        public Task<AsyncOperation<T>> TryExecutePOSTQuery<T>(string query, JsonConverter[] converters = null) where T : class
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<string>> ExecuteJsonCursorGETQuery<T>(string baseQuery, int maxObjectToRetrieve = 2147483647, string cursor = null) where T : class, IBaseCursorQueryDTO
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<T>> ExecuteCursorGETCursorQueryResult<T>(string query, int maxObjectToRetrieve = 2147483647, string cursor = null) where T : class, IBaseCursorQueryDTO
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<T>> ExecuteCursorGETQuery<T, T1>(string baseQuery, int maxObjectToRetrieve = 2147483647, string cursor = null) where T1 : class, IBaseCursorQueryDTO<T>
        {
            throw new NotImplementedException();
        }

        public Task<bool> TryPOSTJsonContent(string url, string json)
        {
            throw new NotImplementedException();
        }

        public Task<T> ExecutePOSTQueryJsonBody<T>(string query, object reqBody, JsonConverter[] converters = null) where T : class
        {
            throw new NotImplementedException();
        }

        public Task<string> ExecutePOSTQueryJsonBody(string query, object reqBody, JsonConverter[] converters = null)
        {
            throw new NotImplementedException();
        }

        public Task<ITwitterResponse> ExecuteQuery(string query, HttpMethod method)
        {
            throw new NotImplementedException();
        }

        public Task<ITwitterResponse> ExecuteQuery(string query, HttpMethod method, ITwitterCredentials credentials, HttpContent httpContent = null)
        {
            throw new NotImplementedException();
        }

        public Task<T> ExecuteQuery<T>(string query, HttpMethod method, ITwitterCredentials credentials, HttpContent httpContent) where T : class
        {
            throw new NotImplementedException();
        }

        public Task<byte[]> DownloadBinary(string url)
        {
            throw new NotImplementedException();
        }

        public Task<ITwitterRequestParameters> GenerateTwitterRequestParameters(string url, HttpMethod method, ITwitterCredentials credentials, HttpContent httpContent)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<T>> ExecuteCursorGETQuery<T, T1>(string baseQuery, ICursorQueryParameters cursorQueryParameters) where T1 : class, IBaseCursorQueryDTO<T>
        {
            throw new NotImplementedException();
        }

        public Task<ITwitterResponse> ExecuteQuery(string query, HttpMethod method, IConsumerOnlyCredentials credentials)
        {
            throw new NotImplementedException();
        }

        public Task<ITwitterResponse> ExecuteQuery(string query, HttpMethod method, IConsumerOnlyCredentials credentials, HttpContent httpContent)
        {
            throw new NotImplementedException();
        }

        public Task<T> ExecuteQuery<T>(string query, HttpMethod method, IConsumerOnlyCredentials credentials, HttpContent httpContent = null) where T : class
        {
            throw new NotImplementedException();
        }
    }
}