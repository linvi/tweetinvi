using System;
using System.Collections.Generic;
using Tweetinvi.Core.Credentials;
using Tweetinvi.Core.Interfaces.Credentials;

namespace Tweetinvi.Credentials
{
    public class CredentialsStore : ICredentialsStore
    {
        private static readonly Dictionary<Guid, IConsumerCredentials> _callbackCredentialsStore = new Dictionary<Guid, IConsumerCredentials>();

        public Dictionary<Guid, IConsumerCredentials> CallbackCredentialsStore
        {
            get { return _callbackCredentialsStore; }
        }

        public bool TryGetValue(Guid identifier, out IConsumerCredentials creds)
        {
            return _callbackCredentialsStore.TryGetValue(identifier, out creds);
        }

        public bool TryGetValue(string identifier, out IConsumerCredentials creds)
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