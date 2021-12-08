using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Tweetinvi.Core.Helpers;
using Tweetinvi.Models;

namespace Tweetinvi.WebLogic
{
    public class WebHelper : IWebHelper
    {
        public Task<Stream> GetResponseStreamAsync(ITwitterRequest request)
        {
            var httpClient = new HttpClient();
            return httpClient.GetStreamAsync(request.Query.Url);
        }

        public Dictionary<string, string> GetURLParameters(string url)
        {
            return GetUriParameters(new Uri(url));
        }

        public Dictionary<string, string> GetUriParameters(Uri uri)
        {
            return GetQueryParameters(uri.Query);
        }

        public Dictionary<string, string> GetQueryParameters(string queryUrl)
        {
            var uriParameters = new Dictionary<string, string>();

            if (!string.IsNullOrEmpty(queryUrl))
            {
                foreach (Match variable in Regex.Matches(queryUrl, @"(?<varName>[^&?=]+)=(?<value>[^&?=]*)"))
                {
                    if (uriParameters.ContainsKey(variable.Groups["varName"].Value))
                        uriParameters[variable.Groups["varName"].Value] = variable.Groups["value"].Value;
                    else
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

            if (Uri.TryCreate(url, UriKind.Absolute, out var uri))
            {
                return GetBaseURL(uri);
            }

            return null;
        }

        public string GetBaseURL(Uri uri)
        {
            if (string.IsNullOrEmpty(uri.Query))
            {
                return uri.AbsoluteUri;
            }

            return uri.AbsoluteUri.Replace(uri.Query, string.Empty);
        }
    }
}
