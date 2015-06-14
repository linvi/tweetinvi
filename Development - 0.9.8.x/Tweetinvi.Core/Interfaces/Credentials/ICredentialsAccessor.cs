using System;
using Tweetinvi.Core.Interfaces.WebLogic;

namespace Tweetinvi.Core.Interfaces.Credentials
{
    public interface ICredentialsAccessor
    {
        IOAuthCredentials ApplicationCredentials { get; set; }
        IOAuthCredentials CurrentThreadCredentials { get; set; }

        T ExecuteOperationWithCredentials<T>(IOAuthCredentials credentials, Func<T> operation);
        void ExecuteOperationWithCredentials(IOAuthCredentials credentials, Action operation);
    }
}