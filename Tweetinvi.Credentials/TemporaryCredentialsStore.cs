using System.Collections.Generic;
using Tweetinvi.Core.Authentication;
using Tweetinvi.Core.Interfaces.Credentials;

namespace Tweetinvi.Credentials
{
    public class CredentialsStore : ICredentialsStore
    {
        private static readonly Dictionary<string, IAuthenticationContext> _callbackAuthenticationContextStore = new Dictionary<string, IAuthenticationContext>();

        public Dictionary<string, IAuthenticationContext> CallbackAuthenticationContextStore
        {
            get { return _callbackAuthenticationContextStore; }
        }

        public bool TryGetValue(string identifier, out IAuthenticationContext creds)
        {
            return _callbackAuthenticationContextStore.TryGetValue(identifier, out creds);
        }
    }
}