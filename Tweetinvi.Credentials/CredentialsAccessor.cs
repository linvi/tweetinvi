using System;
using System.Threading;
using Tweetinvi.Core.Credentials;
using Tweetinvi.Core.ExecutionContext;
using Tweetinvi.Models;

namespace Tweetinvi.Credentials
{
    public class CredentialsAccessor : ICredentialsAccessor
    {
        private static ITwitterCredentials _applicationCredentials;

        private static readonly AsyncLocal<ITwitterCredentials>
            _currentThreadCredentials = new AsyncLocal<ITwitterCredentials>();

        private ICrossExecutionContextPreparer _crossExecutionContextPreparer;

        public CredentialsAccessor(ICrossExecutionContextPreparer crossExecutionContextPreparer)
        {
            _crossExecutionContextPreparer = crossExecutionContextPreparer;

            CurrentThreadCredentials = _applicationCredentials;
        }

        public ITwitterCredentials ApplicationCredentials
        {
            get => _applicationCredentials;
            set => _applicationCredentials = value;
        }

        public ITwitterCredentials CurrentThreadCredentials
        {
            get
            {
                if (_currentThreadCredentials.Value == null)
                {
                    _currentThreadCredentials.Value = ApplicationCredentials;
                }

                return _currentThreadCredentials.Value;
            }
            set
            {
                _currentThreadCredentials.Value = value;

                if (!HasTheApplicationCredentialsBeenInitialized() && _currentThreadCredentials.Value != null)
                {
                    _applicationCredentials = value;
                }
            }
        }

        public T ExecuteOperationWithCredentials<T>(ITwitterCredentials credentials, Func<T> operation)
        {
            _crossExecutionContextPreparer.Prepare();

            ExecutionContext ec = ExecutionContext.Capture();
            T result = default(T);
            ExecutionContext.Run(ec, _ =>
            {
                // Setting a reference at an execution context level will not be carried back to the original EC.
                //  However, updates to objects on the heap (e.g. adding an item to a list) will do, as the EC is not
                //  deep copied.
                // So here, the CurrentThreadCredentials will only be updated within this ExecutionContext.
                CurrentThreadCredentials = credentials;
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

        private bool HasTheApplicationCredentialsBeenInitialized()
        {
            return _applicationCredentials != null;
        }
    }
}