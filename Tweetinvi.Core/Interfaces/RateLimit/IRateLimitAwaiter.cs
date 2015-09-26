using System;
using Tweetinvi.Core.Credentials;
using Tweetinvi.Core.Events.EventArguments;
using Tweetinvi.Core.Interfaces.Credentials;
using Tweetinvi.Core.Interfaces.WebLogic;

namespace Tweetinvi.Core.Interfaces.RateLimit
{
    public interface IRateLimitAwaiter
    {
        event EventHandler<QueryAwaitingEventArgs> QueryAwaitingForRateLimit;

        void WaitForCurrentCredentialsRateLimit(string query);
        void WaitForCredentialsRateLimit(string query, ITwitterCredentials credentials);
        void WaitForCredentialsRateLimit(ITokenRateLimit tokenRateLimit);

        void Wait(int timeToWait);

        int TimeToWaitBeforeTwitterRequest(string query, ITwitterCredentials credentials);
    }
}