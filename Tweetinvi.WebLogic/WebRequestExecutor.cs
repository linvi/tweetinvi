using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Tweetinvi.Core.Exceptions;
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
        private readonly IExceptionHandler _exceptionHandler;
        private readonly IHttpClientWebHelper _httpClientWebHelper;
        private readonly IFactory<IWebRequestResult> _webRequestResultFactory;

        public WebRequestExecutor(
            IExceptionHandler exceptionHandler,
            IHttpClientWebHelper httpClientWebHelper,
            IFactory<IWebRequestResult> webRequestResultFactory)
        {
            _exceptionHandler = exceptionHandler;
            _httpClientWebHelper = httpClientWebHelper;
            _webRequestResultFactory = webRequestResultFactory;
        }

        // Simple Query
        public IWebRequestResult ExecuteQuery(ITwitterQuery twitterQuery, ITwitterClientHandler handler = null)
        {
            return ExecuteTwitterQuerySafely(twitterQuery, () =>
            {
                HttpResponseMessage httpResponseMessage = null;

                try
                {
                    httpResponseMessage = _httpClientWebHelper.GetHttpResponse(twitterQuery, handler).Result;

                    var result = GetWebResultFromResponse(twitterQuery.QueryURL, httpResponseMessage);

                    if (!result.IsSuccessStatusCode)
                    {
                        throw _exceptionHandler.TryLogFailedWebRequestResult(result);
                    }

                    var stream = result.ResultStream;

                    if (stream != null)
                    {
                        var responseReader = new StreamReader(stream);
                        result.Response = responseReader.ReadLine();
                    }

                    return result;
                }
                catch (Exception)
                {
                    if (httpResponseMessage != null)
                    {
                        httpResponseMessage.Dispose();
                    }

                    throw;
                }
            });
        }

        // Multipart
        public IWebRequestResult ExecuteMultipartQuery(ITwitterQuery twitterQuery, string contentId, IEnumerable<byte[]> binaries)
        {
            return ExecuteTwitterQuerySafely(twitterQuery, () =>
            {
                HttpResponseMessage httpResponseMessage = null;

                try
                {
                    httpResponseMessage = _httpClientWebHelper.GetHttpResponse(twitterQuery).Result;

                    var result = GetWebResultFromResponse(twitterQuery.QueryURL, httpResponseMessage);

                    var stream = result.ResultStream;

                    if (stream != null)
                    {
                        var responseReader = new StreamReader(stream);
                        result.Response =  responseReader.ReadLine();
                    }

                    return result;
                }
                catch (Exception)
                {
                    if (httpResponseMessage != null)
                    {
                        httpResponseMessage.Dispose();
                    }

                    throw;
                }
            });
        }

        // Helpers
        private IWebRequestResult GetWebResultFromResponse(string url, HttpResponseMessage httpResponseMessage)
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

        private T ExecuteTwitterQuerySafely<T>(ITwitterQuery twitterQuery, Func<T> action)
        {
            try
            {
                return action();
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
                    throw _exceptionHandler.TryLogWebException(webException, twitterQuery.QueryURL);
                }

                if (taskCanceledException != null)
                {
                    var twitterTimeoutException = new TwitterTimeoutException(twitterQuery);
                    if (_exceptionHandler.LogExceptions)
                    {
                        _exceptionHandler.AddTwitterException(twitterTimeoutException);
                    }

                    throw twitterTimeoutException;
                }

                throw;
            }
        }
    }
}