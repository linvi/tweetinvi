using System;
using System.Net;

namespace Tweetinvi.WebLogic.Utils
{
    public class WebProxy : IWebProxy
    {
        private readonly Uri _proxyUri;

        public WebProxy(Uri proxyUri)
        {
            _proxyUri = proxyUri;
        }

        public WebProxy(string url) : this(new Uri(url))
        {
        }

        public ICredentials Credentials { get; set; }

        public Uri GetProxy(Uri destination)
        {
            return _proxyUri;
        }

        public bool IsBypassed(Uri host)
        {
            return false;
        }
    }
}