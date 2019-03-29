using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web;
using Tweetinvi.Core.Extensions;
using Tweetinvi.WebhooksShared.Core.Logic;

namespace Tweetinvi.AspNet
{
    public class WebhooksRequestHandlerForWebApi : IWebhooksRequestHandler
    {
        private readonly HttpRequestMessage _request;
        private readonly HttpResponseMessage _response;

        public WebhooksRequestHandlerForWebApi(HttpRequestMessage request)
        {
            _request = request;
            _response = new HttpResponseMessage();
        }
        
        public Task<string> GetJsonFromBody()
        {
            return _request.Content.ReadAsStringAsync();
        }

        public string GetPath()
        {
            return _request.RequestUri.AbsolutePath;
        }

        public IDictionary<string, string[]> GetHeaders()
        {
            return _request.Headers.ToDictionary(x => x.Key.ToLowerInvariant(), x => x.Value.ToArray());
        }

        public IDictionary<string, string[]> GetQuery()
        {
            var queryNameValuePairs = HttpUtility.ParseQueryString(_request.RequestUri.Query);
            var query = new Dictionary<string, string[]>();

            queryNameValuePairs.AllKeys.ForEach(key => query.Add(key, new[] { queryNameValuePairs[key] }));

            return query;
        }

        public void SetResponseStatusCode(int statusCode)
        {
            _response.StatusCode = (HttpStatusCode)statusCode;
        }

#pragma warning disable CS1998 // Async method lacks 'await' operators and will run synchronously
        public async Task WriteInResponseAsync(string content, string contentType)
#pragma warning restore CS1998 // Async method lacks 'await' operators and will run synchronously
        {
            _response.Content = new StringContent(content);
            _response.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
        }

        public HttpResponseMessage GetHttpResponseMessage()
        {
            return _response;
        }
    }
}
