using System;
using System.Collections.Generic;
using System.Text;
using Tweetinvi.Models;

namespace Tweetinvi.Core.Credentials
{
    /// <summary>
    /// Run operations with specified credentials
    /// </summary>
    public interface ICredentialsRunner
    {
        T ExecuteOperationWithCredentials<T>(ITwitterCredentials credentials, Func<T> operation);
        void ExecuteOperationWithCredentials(ITwitterCredentials credentials, Action operation);
    }
}
