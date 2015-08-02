using System.Collections.Generic;
using System.IO;
using System.Net;

namespace Tweetinvi.Core.Exceptions
{
    public interface IWebExceptionInfoExtractor
    {
        int GetWebExceptionStatusNumber(WebException wex);
        string GetStatusCodeDescription(int statusCode);
        IEnumerable<ITwitterExceptionInfo> GetTwitterExceptionInfo(WebException wex);
        IEnumerable<ITwitterExceptionInfo> GetTwitterExceptionInfosFromStream(Stream stream);
    }
}