using System.Net;

namespace Tweetinvi.Core.Web
{
    public interface IWebProxyFactory
    {
        IWebProxy GetProxy(string proxyURL);
    }
}