using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Threading.Tasks;

namespace Tweetinvi.Core.Helpers
{
    public interface IWebHelper
    {
        WebResponse GetWebResponse(WebRequest webRequest);
        Stream GetResponseStream(string url);
        Stream GetResponseStream(WebRequest webRequest);

        Task<WebResponse> GetWebResponseAsync(WebRequest webRequest);
        Task<Stream> GetResponseStreamAsync(string url);
        Task<Stream> GetResponseStreamAsync(WebRequest webRequest);

        Dictionary<string, string> GetUriParameters(Uri uri);
        Dictionary<string, string> GetURLParameters(string url);

        string GetBaseURL(string url);
        string GetBaseURL(Uri uri);
    }
}