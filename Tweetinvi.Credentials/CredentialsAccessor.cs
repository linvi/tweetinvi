using System.Diagnostics;
using System.Threading;
using Tweetinvi.Core.Credentials;
using Tweetinvi.Core.ExecutionContext;
using Tweetinvi.Models;

namespace Tweetinvi.Credentials
{
    public class CredentialsAccessor : ICredentialsAccessor, IAsyncContextPreparable
    {
        private static ITwitterCredentials _applicationCredentials;
        private static readonly AsyncLocal<ITwitterCredentials> _executionContextCredentials = new AsyncLocal<ITwitterCredentials>();

        private static readonly ThreadLocal<ITwitterCredentials> _threadCredentials = new ThreadLocal<ITwitterCredentials>(() => null);

        /// <summary>
        /// Whether or not the ThreadCredentials were initialized. As ThreadCredentials can be null, we need another property to identify
        /// if the initialization has occurred to improve performances.
        /// </summary>
        private static readonly ThreadLocal<bool> _initialized = new ThreadLocal<bool>();
        private static readonly ThreadLocal<bool> _fromAwait = new ThreadLocal<bool>(() => false);

        public CredentialsAccessor()
        {
            _initialized.Value = true;
            _fromAwait.Value = true;
            CurrentThreadCredentials = _applicationCredentials;
        }

        public ITwitterCredentials ApplicationCredentials
        {
            get => _applicationCredentials;
            set
            {
                _applicationCredentials = value;

                InitializeFromParentAsyncContext();
            }
        }

        public ITwitterCredentials CurrentThreadCredentials
        {
            get
            {
                InitializeThreadExecutionContext();
                return _threadCredentials.Value ?? _executionContextCredentials.Value;
            }
            set
            {
                if (_fromAwait.Value)
                {
                    _executionContextCredentials.Value = value;
                }
                else
                {
                    _threadCredentials.Value = value;
                }
            }
        }

        private void InitializeThreadExecutionContext()
        {
            if (_initialized.Value)
            {
                Debug.WriteLine("InitializeThreadExecutionContext already initialized");

                return;
            }

            Debug.WriteLine("InitializeThreadExecutionContext");

            _initialized.Value = true;

            var isToLevelExecutionContext = _executionContextCredentials.Value == null;
            if (isToLevelExecutionContext)
            {
                Debug.WriteLine("using await");

                _fromAwait.Value = true;

                if (ApplicationCredentials != null)
                {
                    _executionContextCredentials.Value = ApplicationCredentials.Clone();
                }
                
                return;
            }

            if (!_fromAwait.Value && _threadCredentials.Value == null && _executionContextCredentials.Value == null)
            {
                Debug.WriteLine("Duplicating Application value");
                _threadCredentials.Value = _executionContextCredentials.Value.Clone();
            }
        }

        public void InitializeFromParentAsyncContext()
        {
           
        }

        public void InitializeFromChildAsyncContext()
        {
            _fromAwait.Value = true;

            if (_executionContextCredentials.Value == null && ApplicationCredentials != null)
            {
                Debug.WriteLine("Duplicating Application value");
                _executionContextCredentials.Value = ApplicationCredentials.Clone();
                return;
            }

            Debug.WriteLine("Credentials InitializeFromParentAsyncContext");

            if (_executionContextCredentials.Value != null)
            {
                Debug.WriteLine("Duplicating parent value");
                _executionContextCredentials.Value = _executionContextCredentials.Value.Clone();
            }
        }
    }
}