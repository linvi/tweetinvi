using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Tweetinvi.Models;

namespace Tweetinvi.Core.Helpers
{
    public interface IWebHelper
    {
        Task<Stream> GetResponseStreamAsync(ITwitterRequest request);

        Dictionary<string, string> GetUriParameters(Uri uri);
        Dictionary<string, string> GetURLParameters(string url);

        string GetBaseURL(string url);
        string GetBaseURL(Uri uri);
    }
}