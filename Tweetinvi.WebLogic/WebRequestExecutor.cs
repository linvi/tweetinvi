using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Tweetinvi.Core.Exceptions;
using Tweetinvi.Core.Helpers;
using Tweetinvi.Core.Injectinvi;
using Tweetinvi.Core.Interfaces.Exceptions;
using Tweetinvi.Core.Interfaces.Models;
using Tweetinvi.Core.Interfaces.WebLogic;
using Tweetinvi.Core.Web;

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
        public string ExecuteQuery(ITwitterQuery twitterQuery, ITwitterClientHandler handler = null)
        {
            return ExecuteTwitterQuerySafely(twitterQuery, () =>
            {
                HttpResponseMessage httpResponseMessage = null;

                try
                {
                    httpResponseMessage = _httpClientWebHelper.GetHttpResponse(twitterQuery, null, handler).Result;

                    var result = GetWebResultFromResponse(twitterQuery.QueryURL, httpResponseMessage);

                    if (!result.IsSuccessStatusCode)
                    {
                            throw _exceptionHandler.TryLogFailedWebRequestResult(result);
                    }

                    var stream = result.ResultStream;

                    if (stream != null)
                    {
                        var responseReader = new StreamReader(stream);
                        return responseReader.ReadLine();
                    }

                    return null;
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
        public string ExecuteMultipartQuery(ITwitterQuery twitterQuery, string contentId, IEnumerable<byte[]> binaries)
        {
            return ExecuteTwitterQuerySafely(twitterQuery, () =>
            {
                HttpResponseMessage httpResponseMessage = null;

                try
                {
                    var multiPartContent = GetMultipartFormDataContent(contentId, binaries);
                    httpResponseMessage = _httpClientWebHelper.GetHttpResponse(twitterQuery, multiPartContent).Result;

                    var result = GetWebResultFromResponse(twitterQuery.QueryURL, httpResponseMessage);

                    var stream = result.ResultStream;

                    if (stream != null)
                    {
                        var responseReader = new StreamReader(stream);
                        return responseReader.ReadLine();
                    }

                    return null;
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

        private MultipartFormDataContent GetMultipartFormDataContent(string contentId, IEnumerable<byte[]> binaries)
        {
            var multiPartContent = new MultipartFormDataContent();

            int i = 0;
            foreach (var binary in binaries)
            {
                var byteArrayContent = new ByteArrayContent(binary);
                byteArrayContent.Headers.Add("Content-Type", "application/octet-stream");
                multiPartContent.Add(byteArrayContent, contentId, i.ToString(CultureInfo.InvariantCulture));
            }

            return multiPartContent;
        }

        // Helpers
        private IWebRequestResult GetWebResultFromResponse(string url, HttpResponseMessage httpResponseMessage)
        {
            var stream = httpResponseMessage.Content.ReadAsStreamAsync().Result;

            var webRequestResult = _webRequestResultFactory.Create();

            webRequestResult.ResultStream = stream;
            webRequestResult.StatusCode = (int)httpResponseMessage.StatusCode;
            webRequestResult.IsSuccessStatusCode = httpResponseMessage.IsSuccessStatusCode;
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