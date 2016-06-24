using System.Collections.Generic;
using Tweetinvi.Models;

namespace Tweetinvi.Core.Credentials
{
    public interface ICredentialsStore
    {
        Dictionary<string, IAuthenticationContext> CallbackAuthenticationContextStore { get; }

        bool TryGetValue(string identifier, out IAuthenticationContext creds);
    }
}