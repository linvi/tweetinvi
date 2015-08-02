using System;
using Tweetinvi.Core.Credentials;
using Tweetinvi.Core.Exceptions;
using Tweetinvi.Core.Interfaces.Credentials;
using Tweetinvi.Credentials;

namespace Tweetinvi
{
    public class Auth
    {
        [ThreadStatic]
        private static ICredentialsAccessor _credentialsAccessor;
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
        private static ICredentialsCreator _credentialsCreatorForCurrentThread;
        private static ICredentialsCreator _credentialsCreator
        {
            get
            {
                if (_credentialsCreatorForCurrentThread == null)
                {
                    Initialize();
                }

                return _credentialsCreatorForCurrentThread;
            }
        }

        static Auth()
        {
            Initialize();
        }

        private static void Initialize()
        {
            _credentialsAccessor = TweetinviContainer.Resolve<ICredentialsAccessor>();
            _credentialsCreatorForCurrentThread = TweetinviContainer.Resolve<ICredentialsCreator>();
        }

        public static ITwitterCredentials ApplicationCredentials
        {
            get { return CredentialsAccessor.ApplicationCredentials; }
            set { CredentialsAccessor.ApplicationCredentials = value; }
        }

        public static ITwitterCredentials Credentials
        {
            get { return CredentialsAccessor.CurrentThreadCredentials; }
            set { CredentialsAccessor.CurrentThreadCredentials = value; }
        }

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

        public static ITwitterCredentials SetUserCredentials(string consumerKey, string consumerSecret, string userAccessToken, string userAccessSecret)
        {
            Credentials = new TwitterCredentials(consumerKey, consumerSecret, userAccessToken, userAccessSecret);

            return Credentials;
        }

        public static ITwitterCredentials SetApplicationOnlyCredentials(string consumerKey, string consumerSecret, bool initializeBearerToken = false)
        {
            Credentials = new TwitterCredentials(consumerKey, consumerSecret);

            if (initializeBearerToken)
            {
                InitializeApplicationOnlyCredentials(Credentials);
            }

            return Credentials;
        }

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
                _credentialsCreator.InitializeApplicationBearer(credentials);
            }
        }

        public static bool InvalidateCredentials(ITwitterCredentials credentials = null)
        {
            return _credentialsCreator.InvalidateToken(credentials ?? _credentialsAccessor.CurrentThreadCredentials);
        }

        public static T ExecuteOperationWithCredentials<T>(ITwitterCredentials credentials, Func<T> operation)
        {
            return CredentialsAccessor.ExecuteOperationWithCredentials(credentials, operation);
        }

        public static void ExecuteOperationWithCredentials(ITwitterCredentials credentials, Action operation)
        {
            CredentialsAccessor.ExecuteOperationWithCredentials(credentials, operation);
        }
    }
}