using System;
using System.Collections.Generic;
using Tweetinvi.Core.Authentication;
using Tweetinvi.Core.Interfaces.Credentials;

namespace Tweetinvi.Credentials
{
    public class CredentialsStore : ICredentialsStore
    {
        private static readonly Dictionary<Guid, IAuthenticationContext> _callbackAuthenticationContextStore = new Dictionary<Guid, IAuthenticationContext>();

        public Dictionary<Guid, IAuthenticationContext> CallbackAuthenticationContextStore
        {
            get { return _callbackAuthenticationContextStore; }
        }

        public bool TryGetValue(Guid identifier, out IAuthenticationContext creds)
        {
            return _callbackAuthenticationContextStore.TryGetValue(identifier, out creds);
        }

        public bool TryGetValue(string identifier, out IAuthenticationContext creds)
        {
            creds = null;

            if (string.IsNullOrEmpty(identifier))
            {
                return false;
            }

            Guid id;
            if (!Guid.TryParse(identifier, out id))
            {
                return false;
            }

            return TryGetValue(id, out creds);
        }
    }
}