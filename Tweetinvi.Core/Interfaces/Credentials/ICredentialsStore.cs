using System;
using System.Collections.Generic;
using Tweetinvi.Core.Authentication;

namespace Tweetinvi.Core.Interfaces.Credentials
{
    public interface ICredentialsStore
    {
        Dictionary<string, IAuthenticationContext> CallbackAuthenticationContextStore { get; }

        bool TryGetValue(string identifier, out IAuthenticationContext creds);
    }
}