using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Tweetinvi.Core;
using Tweetinvi.Core.Helpers;

namespace Tweetinvi.WebLogic
{
    public class WebHelper : IWebHelper
    {
        private readonly ITweetinviSettingsAccessor _tweetinviSettingsAccessor;

        public WebHelper(ITweetinviSettingsAccessor tweetinviSettingsAccessor)
        {
            _tweetinviSettingsAccessor = tweetinviSettingsAccessor;
        }

        public Stream GetResponseStream(string url)
        {
            if (!ValidateUrl(url))
            {
                return null;
            }

            WebRequest httpWebRequest = WebRequest.Create(url);
            var webResponse = GetWebResponse(httpWebRequest);

            return webResponse.GetResponseStream();
        }

        private bool ValidateUrl(string url)
        {
            return !String.IsNullOrEmpty(url);
        }

        public WebResponse GetWebResponse(WebRequest webRequest)
        {
            Task<WebResponse> requestTask = Task.Factory.FromAsync<WebResponse>(webRequest.BeginGetResponse, webRequest.EndGetResponse, webRequest);

            if (_tweetinviSettingsAccessor.HttpRequestTimeout > 0)
            {
                var resultingTask = TaskEx.WhenAny(requestTask, TaskEx.Delay(_tweetinviSettingsAccessor.HttpRequestTimeout)).Result;
                if (resultingTask == requestTask)
                {
                    return requestTask.Result;
                }

                throw new TimeoutException("The operation could not complete");
            }

            return requestTask.Result;
        }

        public Stream GetResponseStream(WebRequest webRequest)
        {
            var webResponse = GetWebResponse(webRequest);
            return webResponse.GetResponseStream();
        }

        public async Task<WebResponse> GetWebResponseAsync(WebRequest webRequest)
        {
            return await webRequest.GetResponseAsync();
        }

        public async Task<Stream> GetResponseStreamAsync(string url)
        {
            WebRequest webRequest = WebRequest.Create(url);
            return await GetResponseStreamAsync(webRequest);
        }

        public async Task<Stream> GetResponseStreamAsync(WebRequest webRequest)
        {
            var webResponse = await GetWebResponseAsync(webRequest);
            return webResponse.GetResponseStream();
        }

        public Dictionary<string, string> GetURLParameters(string url)
        {
            return GetUriParameters(new Uri(url));
        }

        public Dictionary<string, string> GetUriParameters(Uri uri)
        {
            Dictionary<string, string> uriParameters = new Dictionary<string, string>();
            if (!string.IsNullOrEmpty(uri.Query))
            {
                foreach (Match variable in Regex.Matches(uri.Query, @"(?<varName>[^&?=]+)=(?<value>[^&?=]*)"))
                {
                    uriParameters.Add(variable.Groups["varName"].Value, variable.Groups["value"].Value);
                }
            }
            return uriParameters;
        }

        public string GetBaseURL(string url)
        {
            if (string.IsNullOrWhiteSpace(url))
            {
                return null;
            }

            Uri uri;
            if (Uri.TryCreate(url, UriKind.Absolute, out uri))
            {
                return GetBaseURL(uri);
            }
            else
            {
                return null;
            }
        }

        public string GetBaseURL(Uri uri)
        {
            if (string.IsNullOrEmpty(uri.Query))
            {
                return uri.AbsoluteUri;
            }

            return uri.AbsoluteUri.Replace(uri.Query, String.Empty);
        }
    }
}