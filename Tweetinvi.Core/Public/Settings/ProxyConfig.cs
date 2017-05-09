using System;
using System.Net;

namespace Tweetinvi
{
    public interface IProxyConfig : IWebProxy
    {
        Uri Address { get; }
    }

    public class ProxyConfig : IProxyConfig
    {
        private Uri _proxyAddress;
        private NetworkCredential _networkCredentials;

        public ProxyConfig(Uri proxyAddress, NetworkCredential credentials = null)
        {
            _proxyAddress = proxyAddress;
            Credentials = credentials;
        }

        public ProxyConfig(string url, NetworkCredential credentials = null) : this(new Uri(url), credentials)
        {
        }

        public ProxyConfig(IProxyConfig proxyConfig)
        {
            if (proxyConfig != null)
            {
                _proxyAddress = proxyConfig.Address;

                var networkCredentials = proxyConfig.Credentials as NetworkCredential;
                if (networkCredentials != null)
                {
                    Credentials = new NetworkCredential(networkCredentials.UserName, networkCredentials.Password, networkCredentials.Domain);
                }
            }
        }

        public ICredentials Credentials
        {
            get { return _networkCredentials; }
            set
            {
                var isCompatibleWithTweetinvi = value == null || value is NetworkCredential;
                if (!isCompatibleWithTweetinvi)
                {
                    throw new Exception("Tweetinvi proxy credentials can only use System.Net.NetworkCredential");
                }

                _networkCredentials = (NetworkCredential)value;
            }
        }

        public Uri Address
        {
            get { return _proxyAddress; }
        }

        public Uri GetProxy(Uri destination)
        {
            return _proxyAddress;
        }

        public bool IsBypassed(Uri host)
        {
            return false;
        }
    }
}
