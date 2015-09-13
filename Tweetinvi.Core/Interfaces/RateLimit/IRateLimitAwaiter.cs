using System;
using Tweetinvi.Core.Credentials;
using Tweetinvi.Core.Events.EventArguments;
using Tweetinvi.Core.Interfaces.Credentials;
using Tweetinvi.Core.Interfaces.WebLogic;

namespace Tweetinvi.Core.Interfaces.RateLimit
{
    public interface IRateLimitAwaiter
    {
        void WaitForCurrentCredentialsRateLimit(string query);
        void WaitForCredentialsRateLimit(string query, ITwitterCredentials credentials);
        event EventHandler<QueryAwaitingEventArgs> QueryAwaitingForRateLimit;
        void WaitForCredentialsRateLimit(ITokenRateLimit tokenRateLimit);
    }
}