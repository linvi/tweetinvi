using System;
using System.Threading;
using Tweetinvi.Core.Credentials;
using Tweetinvi.Core.ExecutionContext;
using Tweetinvi.Models;

namespace Tweetinvi.Credentials
{
    public class CredentialsAccessor : ICredentialsAccessor, ICrossExecutionContextPreparable
    {
        private static ITwitterCredentials _applicationCredentials;

        private static readonly AsyncLocal<ITwitterCredentials>
            _currentThreadCredentials = new AsyncLocal<ITwitterCredentials>();

        public CredentialsAccessor()
        {
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
                initialiseCurrentThreadCredentials();
                return _currentThreadCredentials.Value;
            }
            set
            {
                _currentThreadCredentials.Value = value;

                if (_applicationCredentials == null && _currentThreadCredentials.Value != null)
                {
                    _applicationCredentials = value;
                }
            }
        }

        public void PrepareExecutionContext() => initialiseCurrentThreadCredentials();

        private void initialiseCurrentThreadCredentials()
        {
            if (_currentThreadCredentials.Value == null)
            {
                _currentThreadCredentials.Value = ApplicationCredentials;
            }
        }
    }
}