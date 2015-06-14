using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Tweetinvi.Core;
using Tweetinvi.WebLogic.Utils;

namespace Tweetinvi.WebLogic
{
    public interface IHttpClientWebHelper
    {
        HttpClient GenerateHttpClientFromWebRequest(WebRequest webRequest);
        HttpResponseMessage GetResponseMessageFromWebRequest(WebRequest webRequest);

        Task<HttpResponseMessage> GetResponseMessageFromWebRequestAsync(WebRequest webRequest, int timeout = 0);
    }

    public class HttpClientWebHelper : IHttpClientWebHelper
    {
        private readonly ITweetinviSettingsAccessor _tweetinviSettingsAccessor;

        public HttpClientWebHelper(ITweetinviSettingsAccessor tweetinviSettingsAccessor)
        {
            _tweetinviSettingsAccessor = tweetinviSettingsAccessor;
        }

        public HttpClient GenerateHttpClientFromWebRequest(WebRequest webRequest)
        {
            var handler = new HttpClientHandler
            {
                UseCookies = false,
                UseDefaultCredentials = false,
            };

            if (!string.IsNullOrEmpty(_tweetinviSettingsAccessor.ProxyURL))
            {
                var proxyUri = new Uri(_tweetinviSettingsAccessor.ProxyURL);
                var proxy = new WebProxy(string.Format("{0}://{1}:{2}", proxyUri.Scheme, proxyUri.Host, proxyUri.Port));

                // Check if Uri has user authentication specified
                if (!string.IsNullOrEmpty(proxyUri.UserInfo))
                {
                    var credentials = proxyUri.UserInfo.Split(':');

                    var username = credentials[0];
                    var password = credentials[1];
                    
                    proxy.Credentials = new NetworkCredential(username, password);
                }

                // Assign proxy to handler
                handler.Proxy = proxy;
                handler.UseProxy = true;
            }

            var httpClient = new HttpClient(handler);

            var webRequestHeaders = webRequest.Headers;
            foreach (var headerKey in webRequestHeaders.AllKeys)
            {
                httpClient.DefaultRequestHeaders.Add(headerKey, webRequestHeaders[headerKey]);
            }

            return httpClient;
        }

        public HttpResponseMessage GetResponseMessageFromWebRequest(WebRequest webRequest)
        {
            var requestTask = TaskEx.Run(() => GetResponseMessageFromWebRequestAsync(webRequest, _tweetinviSettingsAccessor.WebRequestTimeout));

            if (_tweetinviSettingsAccessor.WebRequestTimeout > 0)
            {
                var resultingTask = TaskEx.WhenAny(requestTask, TaskEx.Delay(_tweetinviSettingsAccessor.WebRequestTimeout)).Result;
                if (resultingTask != requestTask)
                {
                    throw new TimeoutException("The operation could not complete");
                }
            }

            try
            {
                return requestTask.Result;
            }
            catch (AggregateException aex)
            {
                if (aex.InnerExceptions.Count == 1)
                {
                    if (aex.InnerExceptions[0] is TaskCanceledException)
                    {
                        throw new TimeoutException("The operation could not complete");
                    }
                }

                throw;
            }
            catch (TaskCanceledException)
            {
                throw new TimeoutException("The operation could not complete");
            }
        }

        public async Task<HttpResponseMessage> GetResponseMessageFromWebRequestAsync(WebRequest webRequest, int timeout)
        {
            var httpClient = GenerateHttpClientFromWebRequest(webRequest);

            if (timeout > 0)
            {
                httpClient.Timeout = new TimeSpan(0, 0, 0, 0, timeout);
            }

            var httpMethod = new HttpMethod(webRequest.Method);

            return await httpClient.SendAsync(new HttpRequestMessage(httpMethod, webRequest.RequestUri));
        }
    }
}