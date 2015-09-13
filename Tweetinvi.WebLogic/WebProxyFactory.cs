using System;
using System.Net;
using Tweetinvi.Core.Web;
using Tweetinvi.WebLogic.Utils;

namespace Tweetinvi.WebLogic
{
    public class WebProxyFactory : IWebProxyFactory
    {
        public IWebProxy GetProxy(string proxyURL)
        {
            if (!string.IsNullOrEmpty(proxyURL))
            {
                var proxyUri = new Uri(proxyURL);
                var proxy = new WebProxy(string.Format("{0}://{1}:{2}", proxyUri.Scheme, proxyUri.Host, proxyUri.Port));

                // Check if Uri has user authentication specified
                if (!string.IsNullOrEmpty(proxyUri.UserInfo))
                {
                    var credentials = proxyUri.UserInfo.Split(':');

                    var username = credentials[0];
                    var password = credentials[1];

                    proxy.Credentials = new NetworkCredential(username, password);
                }

                // Assign proxy to handler
                return proxy;
            }

            return null;
        }
    }
}