using System.Net;

namespace Tweetinvi.Core.Web
{
    /// <summary>
    /// Class allowing to create a proxy from its url.
    /// </summary>
    public interface IWebProxyFactory
    {
        /// <summary>
        /// Create a WebProxy from an url with the following format :
        /// {protocol/scheme}://{username:password@}{host}:{port}
        /// </summary>
        IWebProxy GetProxy(string proxyURL);
    }
}