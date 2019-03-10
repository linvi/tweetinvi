using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using Tweetinvi.Core.Credentials;
using Tweetinvi.Core.ExecutionContext;
using Tweetinvi.Models;

namespace Tweetinvi.Credentials
{
    public class CredentialsRunner : ICredentialsRunner
    {
        private readonly IAsyncContextPreparer _asyncContextPreparer;
        private readonly ICredentialsAccessor _credentialsAccessor;

        public CredentialsRunner(IAsyncContextPreparer asyncContextPreparer,
            ICredentialsAccessor credentialsAccessor)
        {
            _asyncContextPreparer = asyncContextPreparer;
            _credentialsAccessor = credentialsAccessor;
        }

        public T ExecuteOperationWithCredentials<T>(ITwitterCredentials credentials, Func<T> operation)
        {
            _asyncContextPreparer.PrepareFromParentAsyncContext();

            ExecutionContext ec = ExecutionContext.Capture();
            T result = default(T);
            // ReSharper disable once AssignNullToNotNullAttribute
            ExecutionContext.Run(ec, _ =>
            {
                _asyncContextPreparer.PrepareFromChildAsyncContext();

                // Setting a reference at an execution context level will not be carried back to the original EC.
                //  However, updates to objects on the heap (e.g. adding an item to a list) will do, as the EC is not
                //  deep copied.
                // So here, the CurrentThreadCredentials will only be updated within this ExecutionContext.
                _credentialsAccessor.CurrentThreadCredentials = credentials;
                result = operation();
            }, null);

            return result;
        }

        public void ExecuteOperationWithCredentials(ITwitterCredentials credentials, Action operation)
        {
            // Reuse Func<T> implementation
            ExecuteOperationWithCredentials(credentials, () =>
            {
                operation();
                return 0;
            });
        }
    }
}
