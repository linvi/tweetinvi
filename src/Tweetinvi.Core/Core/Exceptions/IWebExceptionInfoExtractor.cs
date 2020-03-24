using System.IO;
using System.Net;

namespace Tweetinvi.Core.Exceptions
{
    public interface IWebExceptionInfoExtractor
    {
        int GetWebExceptionStatusNumber(WebException wex);
        int GetWebExceptionStatusNumber(WebException wex, int defaultStatusCode);
        string GetStatusCodeDescription(int statusCode);
        ITwitterExceptionInfo[] GetTwitterExceptionInfo(WebException wex);
        ITwitterExceptionInfo[] GetTwitterExceptionInfosFromStream(Stream stream);
    }
}