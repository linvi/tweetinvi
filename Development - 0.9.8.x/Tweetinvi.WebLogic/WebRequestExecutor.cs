using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using Tweetinvi.Core.Exceptions;
using Tweetinvi.Core.Injectinvi;
using Tweetinvi.Core.Interfaces.Exceptions;
using Tweetinvi.Core.Interfaces.WebLogic;

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
        private readonly IFactory<ITwitterTimeoutException> _twitterTimeoutExceptionFactory;

        /// <summary>
        /// Headers of the latest WebResponse
        /// </summary>
        protected Dictionary<string, string> _lastHeadersResult { get; private set; }

        public WebRequestExecutor(
            IExceptionHandler exceptionHandler,
            IHttpClientWebHelper httpClientWebHelper,
            IFactory<IWebRequestResult> webRequestResultFactory,
            IFactory<ITwitterTimeoutException> twitterTimeoutExceptionFactory)
        {
            _exceptionHandler = exceptionHandler;
            _httpClientWebHelper = httpClientWebHelper;
            _webRequestResultFactory = webRequestResultFactory;
            _twitterTimeoutExceptionFactory = twitterTimeoutExceptionFactory;
        }

        // Simple WebRequest
        public string ExecuteWebRequest(HttpWebRequest webRequest)
        {
            try
            {
                var webRequestResult = GetWebRequestResultFromHttpClient(webRequest);

                if (!webRequestResult.IsSuccessStatusCode)
                {
                    throw _exceptionHandler.TryLogFailedWebRequestResult(webRequestResult);
                }

                var stream = webRequestResult.ResultStream;

                if (stream != null)
                {
                    // Getting the result
                    var responseReader = new StreamReader(stream);
                    return responseReader.ReadLine();
                }

                // Closing the connection
                webRequest.Abort();
            }
            catch (AggregateException aex)
            {
                var webException = aex.InnerException as WebException;
                var httpRequestMessageException = aex.InnerException as HttpRequestException;

                if (httpRequestMessageException != null)
                {
                    webException = httpRequestMessageException.InnerException as WebException;
                }

                if (webException != null)
                {
                    if (webRequest != null)
                    {
                        webRequest.Abort();

                        throw _exceptionHandler.TryLogWebException(webException, webRequest.RequestUri.AbsoluteUri);
                    }

                    throw webException;
                }

                throw;
            }
            catch (TimeoutException)
            {
                var twitterTimeoutException = _twitterTimeoutExceptionFactory.Create(new ConstructorNamedParameter("url", webRequest.RequestUri.AbsoluteUri));
                if (_exceptionHandler.LogExceptions)
                {
                    _exceptionHandler.AddTwitterException(twitterTimeoutException);
                }

                throw (Exception)twitterTimeoutException;
            }

            return null;
        }

        private IWebRequestResult GetWebRequestResultFromHttpClient(HttpWebRequest webRequest)
        {
            HttpResponseMessage webResponse = null;

            try
            {
                webResponse = _httpClientWebHelper.GetResponseMessageFromWebRequest(webRequest);
                var stream = webResponse.Content.ReadAsStreamAsync().Result;

                _lastHeadersResult = new Dictionary<string, string>();

                var headers = webRequest.Headers;
                foreach (var headerKey in headers.AllKeys)
                {
                    _lastHeadersResult.Add(headerKey, headers[headerKey]);
                }

                var webRequestResult = _webRequestResultFactory.Create();

                webRequestResult.ResultStream = stream;
                webRequestResult.StatusCode = (int)webResponse.StatusCode;
                webRequestResult.IsSuccessStatusCode = webResponse.IsSuccessStatusCode;
                webRequestResult.URL = webRequest.RequestUri.AbsoluteUri;

                return webRequestResult;
            }
            catch (Exception)
            {
                if (webResponse != null)
                {
                    webResponse.Dispose();
                }

                throw;
            }
        }

        // Multi-Part WebRequest
        public string ExecuteMultipartRequest(IMultipartWebRequest multipartWebRequest)
        {
            try
            {
                return multipartWebRequest.GetResult();
            }
            catch (WebException wex)
            {
                if (!_exceptionHandler.LogExceptions)
                {
                    throw _exceptionHandler.GenerateTwitterException(wex, multipartWebRequest.WebRequest.RequestUri.AbsoluteUri);
                }

                _exceptionHandler.AddWebException(wex, multipartWebRequest.WebRequest.RequestUri.AbsoluteUri);
                return null;
            }
        }
    }
}