using System;
using Tweetinvi.Core.Authentication;
using Tweetinvi.Core.Exceptions;
using Tweetinvi.Core.Interfaces.Credentials;
using Tweetinvi.Credentials;

namespace Tweetinvi
{
    public class Auth
    {
        [ThreadStatic]
        private static ICredentialsAccessor _credentialsAccessor;

        /// <summary>
        /// Object responsible to retrieve the current thread credentials.
        /// </summary>
        public static ICredentialsAccessor CredentialsAccessor
        {
            get
            {
                if (_credentialsAccessor == null)
                {
                    Initialize();
                }

                return _credentialsAccessor;
            }
        }

        [ThreadStatic]
        private static IAuthFactory _authFactoryForCurrentThread;
        private static IAuthFactory AuthFactory
        {
            get
            {
                if (_authFactoryForCurrentThread == null)
                {
                    Initialize();
                }

                return _authFactoryForCurrentThread;
            }
        }

        static Auth()
        {
            Initialize();
        }

        private static void Initialize()
        {
            _credentialsAccessor = TweetinviContainer.Resolve<ICredentialsAccessor>();
            _authFactoryForCurrentThread = TweetinviContainer.Resolve<IAuthFactory>();
        }

        /// <summary>
        /// Application wide credentials. When a new thread is starting, the thread credentials will be defaulted to this value.
        /// </summary>
        public static ITwitterCredentials ApplicationCredentials
        {
            get { return CredentialsAccessor.ApplicationCredentials; }
            set { CredentialsAccessor.ApplicationCredentials = value; }
        }

        /// <summary>
        /// Current Thread credentials.
        /// </summary>
        public static ITwitterCredentials Credentials
        {
            get { return CredentialsAccessor.CurrentThreadCredentials; }
            set { CredentialsAccessor.CurrentThreadCredentials = value; }
        }

        /// <summary>
        /// Let you create new TwitterCredentials
        /// </summary>
        public static ITwitterCredentials CreateCredentials(string consumerKey, string consumerSecret, string userAccessToken, string userAccessSecret)
        {
            return new TwitterCredentials(consumerKey, consumerSecret, userAccessToken, userAccessSecret);
        }

        /// <summary>
        /// Set the credentials of the running thread
        /// </summary>
        public static void SetCredentials(ITwitterCredentials credentials)
        {
            Credentials = credentials;
        }

        /// <summary>
        /// Set the current thread credentials based on user account credentials
        /// </summary>
        public static ITwitterCredentials SetUserCredentials(string consumerKey, string consumerSecret, string userAccessToken, string userAccessSecret)
        {
            Credentials = new TwitterCredentials(consumerKey, consumerSecret, userAccessToken, userAccessSecret);

            return Credentials;
        }

        /// <summary>
        /// Set the current thread credentials based on application only credentials.
        /// To execute http requests, application only credentials needs a bearer token.
        /// Setting  the initializeBearerToken to true will initialize your credentials so that they are ready to be used.
        /// </summary>
        public static ITwitterCredentials SetApplicationOnlyCredentials(string consumerKey, string consumerSecret, bool initializeBearerToken = false)
        {
            Credentials = new TwitterCredentials(consumerKey, consumerSecret);

            if (initializeBearerToken)
            {
                InitializeApplicationOnlyCredentials(Credentials);
            }

            return Credentials;
        }

        /// <summary>
        /// Set the current thread credentials based on application only credentials.
        /// To execute http requests, application only credentials needs a bearer token.
        /// </summary>
        public static ITwitterCredentials SetApplicationOnlyCredentials(string consumerKey, string consumerSecret, string bearerToken)
        {
            Credentials = new TwitterCredentials(consumerKey, consumerSecret)
            {
                ApplicationOnlyBearerToken = bearerToken
            };

            return Credentials;
        }

        /// <summary>
        /// Set the bearer token onto a set of credentials
        /// </summary>
        /// <param name="credentials">Credentials to update</param>
        /// <param name="force">Set the bearer token even if it is not required for executing queries</param>
        public static void InitializeApplicationOnlyCredentials(ITwitterCredentials credentials = null, bool force = false)
        {
            credentials = credentials ?? CredentialsAccessor.CurrentThreadCredentials;

            if (credentials == null)
            {
                throw new TwitterNullCredentialsException("Initialize Application Bearer needs to either have a" +
                                                          " credentials parameter or have the thread credentials set up.");
            }

            var isBearerAlreadySet = !string.IsNullOrEmpty(credentials.ApplicationOnlyBearerToken);
            var isBearerRequired = string.IsNullOrEmpty(credentials.AccessToken) || string.IsNullOrEmpty(credentials.AccessTokenSecret);

            if (force || (isBearerRequired && !isBearerAlreadySet))
            {
                AuthFactory.InitializeApplicationBearer(credentials);
            }
        }

        /// <summary>
        /// Invalidate application only credentials so that they can no longer be used to access Twitter.
        /// </summary>
        public static bool InvalidateCredentials(ITwitterCredentials credentials = null)
        {
            return AuthFactory.InvalidateCredentials(credentials ?? CredentialsAccessor.CurrentThreadCredentials);
        }

        /// <summary>
        /// Execute an action with a specific set of credentials and returns the value.
        /// </summary>
        public static T ExecuteOperationWithCredentials<T>(ITwitterCredentials credentials, Func<T> operation)
        {
            return CredentialsAccessor.ExecuteOperationWithCredentials(credentials, operation);
        }

        /// <summary>
        /// Execute an action with a specific set of credentials.
        /// </summary>
        public static void ExecuteOperationWithCredentials(ITwitterCredentials credentials, Action operation)
        {
            CredentialsAccessor.ExecuteOperationWithCredentials(credentials, operation);
        }
    }
}