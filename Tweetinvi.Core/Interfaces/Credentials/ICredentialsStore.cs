using System;
using System.Collections.Generic;
using Tweetinvi.Core.Credentials;

namespace Tweetinvi.Core.Interfaces.Credentials
{
    public interface ICredentialsStore
    {
        Dictionary<Guid, IConsumerCredentials> CallbackCredentialsStore { get; }

        bool TryGetValue(Guid identifier, out IConsumerCredentials creds);
        bool TryGetValue(string identifier, out IConsumerCredentials creds);
    }
}