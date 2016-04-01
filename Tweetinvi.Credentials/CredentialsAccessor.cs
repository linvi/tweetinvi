using System;
using Tweetinvi.Core.Authentication;
using Tweetinvi.Core.Interfaces.Credentials;

namespace Tweetinvi.Credentials
{
    public class CredentialsAccessor : ICredentialsAccessor
    {
        private static ITwitterCredentials StaticApplicationCredentials { get; set; }

        public CredentialsAccessor()
        {
            CurrentThreadCredentials = StaticApplicationCredentials;
        }

        public ITwitterCredentials ApplicationCredentials
        {
            get { return StaticApplicationCredentials; }
            set
            {
                StaticApplicationCredentials = value;

                if (_currentThreadCredentials == null)
                {
                    _currentThreadCredentials = value;
                }
            }
        }

        private ITwitterCredentials _currentThreadCredentials;
        public ITwitterCredentials CurrentThreadCredentials
        {
            get { return _currentThreadCredentials; }
            set
            {
                _currentThreadCredentials = value;

                if (!HasTheApplicationCredentialsBeenInitialized() && _currentThreadCredentials != null)
                {
                    StaticApplicationCredentials = value;
                }
            }
        }

        public T ExecuteOperationWithCredentials<T>(ITwitterCredentials credentials, Func<T> operation)
        {
            // This operation does not need any lock because the Credentials are unique per thread
            // We can therefore change the value safely without affecting any other thread

            var initialCredentials = CurrentThreadCredentials;
            CurrentThreadCredentials = credentials;
            var result = operation();

            bool hasUserChangedCredentialsDuringOpertion = CurrentThreadCredentials != credentials;
            if (!hasUserChangedCredentialsDuringOpertion)
            {
                CurrentThreadCredentials = initialCredentials;
            }

            return result;
        }

        public void ExecuteOperationWithCredentials(ITwitterCredentials credentials, Action operation)
        {
            // This operation does not need any lock because the Credentials are unique per thread
            // We can therefore change the value safely without affecting any other thread

            var initialCredentials = CurrentThreadCredentials;
            CurrentThreadCredentials = credentials;
            operation();

            bool hasUserChangedCredentialsDuringOpertion = CurrentThreadCredentials != credentials;
            if (!hasUserChangedCredentialsDuringOpertion)
            {
                CurrentThreadCredentials = initialCredentials;
            }
        }

        private bool HasTheApplicationCredentialsBeenInitialized()
        {
            return StaticApplicationCredentials != null;
        }
    }
}