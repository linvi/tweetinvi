using System.Threading;
using Tweetinvi.Core.Credentials;
using Tweetinvi.Core.ExecutionContext;
using Tweetinvi.Models;

namespace Tweetinvi.Credentials
{
    public class CredentialsAccessor : ICredentialsAccessor, IAsyncContextPreparable
    {
        private static ITwitterCredentials _applicationCredentials;

        private static readonly AsyncLocal<ITwitterCredentials> _currentThreadCredentials = new AsyncLocal<ITwitterCredentials>();

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
                InitializeAsyncContext();

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

        public void InitializeAsyncContext()
        {
            if (_currentThreadCredentials.Value == null)
            {
                _currentThreadCredentials.Value = ApplicationCredentials;
            }
        }
    }
}