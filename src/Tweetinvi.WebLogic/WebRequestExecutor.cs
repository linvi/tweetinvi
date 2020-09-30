using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Tweetinvi.Core.Helpers;
using Tweetinvi.Core.Injectinvi;
using Tweetinvi.Core.Web;
using Tweetinvi.Exceptions;
using Tweetinvi.Models;

namespace Tweetinvi.WebLogic
{
    /// <summary>
    /// Generate a Token that can be used to perform OAuth queries
    /// </summary>
    public class WebRequestExecutor : IWebRequestExecutor
    {
        private readonly ITwitterExceptionFactory _twitterExceptionFactory;
        private readonly IHttpClientWebHelper _httpClientWebHelper;
        private readonly IFactory<ITwitterResponse> _webRequestResultFactory;

        public WebRequestExecutor(
            ITwitterExceptionFactory twitterExceptionFactory,
            IHttpClientWebHelper httpClientWebHelper,
            IFactory<ITwitterResponse> webRequestResultFactory)
        {
            _twitterExceptionFactory = twitterExceptionFactory;
            _httpClientWebHelper = httpClientWebHelper;
            _webRequestResultFactory = webRequestResultFactory;
        }

        // Simple Query
        public Task<ITwitterResponse> ExecuteQueryAsync(ITwitterRequest request, ITwitterClientHandler handler = null)
        {
            return ExecuteTwitterQuerySafelyAsync(request, async () =>
            {
                HttpResponseMessage httpResponseMessage = null;

                try
                {
                    httpResponseMessage = await _httpClientWebHelper.GetHttpResponseAsync(request.Query, handler).ConfigureAwait(false);

                    var result = GetWebResultFromResponse(request.Query.Url, httpResponseMessage);
                    if (!result.IsSuccessStatusCode)
                    {
                        throw _twitterExceptionFactory.Create(result, request);
                    }

                    var stream = result.ResultStream;
                    if (stream != null)
                    {
                        result.Binary = StreamToBinary(stream);
                        result.Content = Encoding.UTF8.GetString(result.Binary);
                    }

                    return result;
                }
                catch (Exception)
                {
                    httpResponseMessage?.Dispose();

                    throw;
                }
            });
        }

        // Multipart

        public static byte[] StreamToBinary(Stream stream)
        {
            if (stream == null)
            {
                return null;
            }

            byte[] binary;

            using (var tempMemStream = new MemoryStream())
            {
                byte[] buffer = new byte[128];

                while (true)
                {
                    int read = stream.Read(buffer, 0, buffer.Length);

                    if (read <= 0)
                    {
                        binary = tempMemStream.ToArray();
                        break;
                    }

                    tempMemStream.Write(buffer, 0, read);
                }
            }

            return binary;
        }

        public Task<ITwitterResponse> ExecuteMultipartQueryAsync(ITwitterRequest request)
        {
            return ExecuteTwitterQuerySafelyAsync(request, async () =>
            {
                HttpResponseMessage httpResponseMessage = null;

                try
                {
                    httpResponseMessage = await _httpClientWebHelper.GetHttpResponseAsync(request.Query).ConfigureAwait(false);

                    var result = GetWebResultFromResponse(request.Query.Url, httpResponseMessage);

                    var stream = result.ResultStream;

                    if (stream != null)
                    {
                        result.Binary = StreamToBinary(stream);
                        result.Content = Encoding.UTF8.GetString(result.Binary);
                    }

                    return result;
                }
                catch (Exception)
                {
                    httpResponseMessage?.Dispose();

                    throw;
                }
            });
        }

        // Helpers
        private ITwitterResponse GetWebResultFromResponse(string url, HttpResponseMessage httpResponseMessage)
        {
            var stream = httpResponseMessage.Content.ReadAsStreamAsync().Result;

            var webRequestResult = _webRequestResultFactory.Create();

            webRequestResult.ResultStream = stream;
            webRequestResult.StatusCode = (int)httpResponseMessage.StatusCode;

            const int TON_API_SUCCESS_STATUS_CODE = 308;

            var isTonApiRequest = url.StartsWith("https://ton.twitter.com");
            var isTonApiRequestSuccess = (int) httpResponseMessage.StatusCode == TON_API_SUCCESS_STATUS_CODE;

            webRequestResult.IsSuccessStatusCode = httpResponseMessage.IsSuccessStatusCode || (isTonApiRequest && isTonApiRequestSuccess);

            webRequestResult.URL = url;
            webRequestResult.Headers = httpResponseMessage.Headers.ToDictionary(x => x.Key, x => x.Value);

            return webRequestResult;
        }

        private async Task<T> ExecuteTwitterQuerySafelyAsync<T>(ITwitterRequest request, Func<Task<T>> action)
        {
            try
            {
                return await action().ConfigureAwait(false);
            }
            catch (AggregateException aex)
            {
                var webException = aex.InnerException as WebException;
                var httpRequestMessageException = aex.InnerException as HttpRequestException;
                var taskCanceledException = aex.InnerException as TaskCanceledException;

                if (httpRequestMessageException != null)
                {
                    webException = httpRequestMessageException.InnerException as WebException;
                }

                if (webException != null)
                {
                    throw _twitterExceptionFactory.Create(webException, request);
                }

                if (taskCanceledException != null)
                {
                    throw new TwitterTimeoutException(request);
                }

                throw;
            }
        }
    }
}