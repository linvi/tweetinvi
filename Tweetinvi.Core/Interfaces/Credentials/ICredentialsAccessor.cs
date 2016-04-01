using System;
using Tweetinvi.Core.Authentication;
using Tweetinvi.Core.Interfaces.WebLogic;

namespace Tweetinvi.Core.Interfaces.Credentials
{
    public interface ICredentialsAccessor
    {
        ITwitterCredentials ApplicationCredentials { get; set; }
        ITwitterCredentials CurrentThreadCredentials { get; set; }

        T ExecuteOperationWithCredentials<T>(ITwitterCredentials credentials, Func<T> operation);
        void ExecuteOperationWithCredentials(ITwitterCredentials credentials, Action operation);
    }
}