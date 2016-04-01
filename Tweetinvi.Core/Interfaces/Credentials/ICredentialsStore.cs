using System;
using System.Collections.Generic;
using Tweetinvi.Core.Authentication;

namespace Tweetinvi.Core.Interfaces.Credentials
{
    public interface ICredentialsStore
    {
        Dictionary<Guid, IAuthenticationContext> CallbackAuthenticationContextStore { get; }

        bool TryGetValue(Guid identifier, out IAuthenticationContext creds);
        bool TryGetValue(string identifier, out IAuthenticationContext creds);
    }
}