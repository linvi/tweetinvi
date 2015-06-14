using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Tweetinvi.Core;
using Tweetinvi.Core.Enum;
using Tweetinvi.Core.Exceptions;
using Tweetinvi.Core.Helpers;
using Tweetinvi.Core.Interfaces.Credentials;
using Tweetinvi.Core.Interfaces.DTO;
using Tweetinvi.Core.Interfaces.Models;
using Tweetinvi.Core.Interfaces.WebLogic;

namespace Tweetinvi.WebLogic
{
    public class TwitterRequestGenerator : ITwitterRequestGenerator
    {
        public const int BUFFER_SIZE = 4096;

        private readonly IOAuthWebRequestGenerator _webRequestGenerator;
        private readonly ICredentialsAccessor _credentialsAccessor;
        private readonly IWebHelper _webHelper;
        private readonly ITweetinviSettingsAccessor _tweetinviSettingsAccessor;

        public TwitterRequestGenerator(
            IOAuthWebRequestGenerator webRequestGenerator,
            ICredentialsAccessor credentialsAccessor,
            IWebHelper webHelper,
            ITweetinviSettingsAccessor tweetinviSettingsAccessor)
        {
            _webRequestGenerator = webRequestGenerator;
            _credentialsAccessor = credentialsAccessor;
            _webHelper = webHelper;
            _tweetinviSettingsAccessor = tweetinviSettingsAccessor;
        }

        public HttpWebRequest GetQueryWebRequest(ITwitterQuery twitterQuery)
        {
            var url = twitterQuery.QueryURL;
            var httpMethod = twitterQuery.HttpMethod;
            var queryParameters = GetTwitterQueryParameters(twitterQuery);

            return GetQueryWebRequestInternal(url, httpMethod, queryParameters);
        }

        private IEnumerable<IOAuthQueryParameter> GetTwitterQueryParameters(ITwitterQuery twitterQuery)
        {
            var queryParameters = twitterQuery.QueryParameters;

            if (twitterQuery.OAuthCredentials == null)
            {
                if (twitterQuery.TemporaryCredentials != null)
                {
                    return _webRequestGenerator.GenerateApplicationParameters(twitterQuery.TemporaryCredentials, queryParameters);
                }

                if (_credentialsAccessor.CurrentThreadCredentials == null)
                {
                    throw new TwitterNullCredentialsException();
                }

                twitterQuery.OAuthCredentials = _credentialsAccessor.CurrentThreadCredentials;
            }

            if (queryParameters == null)
            {
                queryParameters = _webRequestGenerator.GenerateParameters(twitterQuery.OAuthCredentials);
            }

            return queryParameters;
        }

        private HttpWebRequest GetQueryWebRequestInternal(string url, HttpMethod httpMethod, IEnumerable<IOAuthQueryParameter> headers = null)
        {
            return _webRequestGenerator.GenerateWebRequest(url, httpMethod, headers);
        }

        private IMultipartElement GenerateMultipartElement(IMedia media, string contentId, IMultipartRequestConfiguration configuration)
        {
            var additionalParameters = new Dictionary<string, string>();

            var element = new MultipartElement
            {
                Boundary = configuration.Boundary,
                ContentId = contentId,
                ContentDispositionType = "form-data",
                ContentType = "application/octet-stream",
                AdditionalParameters = additionalParameters,
                Data = configuration.EncodingAlgorithm.GetString(media.Data, 0, media.Data.Length),
            };

            return element;
        }

        public IMultipartWebRequest GenerateMultipartWebRequest(ITwitterQuery twitterQuery, string contentId, IEnumerable<IMedia> medias)
        {
            var baseURL = _webHelper.GetBaseURL(twitterQuery.QueryURL);
            var requestConfiguration = new MultipartRequestConfiguration();
            var multipartElements = medias.Select(media => GenerateMultipartElement(media, contentId, requestConfiguration));

            var requestContent = _webRequestGenerator.GenerateMultipartContent(twitterQuery.QueryURL, twitterQuery.HttpMethod, requestConfiguration, multipartElements);

            var baseTwitterQuery = twitterQuery.Clone();
            baseTwitterQuery.QueryURL = baseURL;

            var request = GetQueryWebRequest(baseTwitterQuery);
            request.ContentType = "multipart/form-data;boundary=" + requestConfiguration.Boundary;

            return new MultipartWebRequest(request, requestContent, _tweetinviSettingsAccessor);
        }

        private class MultipartWebRequest : IMultipartWebRequest
        {
            private readonly ITweetinviSettingsAccessor _tweetinviSettingsAccessor;

            public MultipartWebRequest(HttpWebRequest httpWebRequest, byte[] content, ITweetinviSettingsAccessor tweetinviSettingsAccessor)
            {
                _tweetinviSettingsAccessor = tweetinviSettingsAccessor;
                WebRequest = httpWebRequest;
                Content = content;
            }

            public HttpWebRequest WebRequest { get; private set; }
            public byte[] Content { get; private set; }

            private WebException _lastWebException;

            public string GetResult()
            {
                _lastWebException = null;
                var manualResetEvent = new ManualResetEvent(false);
                string result = null;

                WebRequest.BeginGetRequestStream(asyncResult =>
                {
                    HttpWebRequest request = (HttpWebRequest)asyncResult.AsyncState;
                    using (var reqStream = request.EndGetRequestStream(asyncResult))
                    {
                        int offset = 0;

                        while (offset < Content.Length)
                        {
                            int bytesToWrite = Math.Min(BUFFER_SIZE, Content.Length - offset);
                            reqStream.Write(Content, offset, bytesToWrite);
                            offset += bytesToWrite;
                        }

                        reqStream.Flush();
                    }

                    request.BeginGetResponse(a =>
                    {
                        try
                        {
                            var response = request.EndGetResponse(a);
                            using (StreamReader streamReader = new StreamReader(response.GetResponseStream()))
                            {
                                result = streamReader.ReadToEnd();
                                manualResetEvent.Set();
                            }
                        }
                        catch (Exception ex)
                        {
# if DEBUG
                            // ReSharper disable once ConditionIsAlwaysTrueOrFalse
                            if (_tweetinviSettingsAccessor.ShowDebug)
                            // ReSharper disable once CSharpWarnings::CS0162
                            {
                                Debug.WriteLine(ex);
                            }
# endif

                            _lastWebException = ex as WebException;
                            manualResetEvent.Set();
                        }
                    }, null);
                }, WebRequest);

                manualResetEvent.WaitOne();

                if (_lastWebException != null)
                {
                    throw _lastWebException;
                }

                return result;
            }

            public Task<string> GetResultAsync()
            {
                throw new NotImplementedException();
            }
        }
    }
}