using System;
using Tweetinvi.Core.Interfaces.Credentials;
using Tweetinvi.Core.Interfaces.Factories;
using Tweetinvi.Core.Interfaces.WebLogic;

namespace Tweetinvi
{
    public class TwitterCredentials
    {
        private static readonly ICredentialsFactory _credentialsFactory;

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

        static TwitterCredentials()
        {
            _credentialsFactory = TweetinviContainer.Resolve<ICredentialsFactory>();
        }

        private static void Initialize()
        {
            _credentialsAccessor = TweetinviContainer.Resolve<ICredentialsAccessor>();
        }

        public static IOAuthCredentials ApplicationCredentials
        {
            get { return CredentialsAccessor.ApplicationCredentials; }
            set { CredentialsAccessor.ApplicationCredentials = value; }
        }

        public static IOAuthCredentials Credentials
        {
            get { return CredentialsAccessor.CurrentThreadCredentials; }
            set { CredentialsAccessor.CurrentThreadCredentials = value; }
        }

        public static IOAuthCredentials CreateCredentials(string userAccessToken, string userAccessSecret, string consumerKey, string consumerSecret)
        {
            return _credentialsFactory.CreateOAuthCredentials(userAccessToken, userAccessSecret, consumerKey, consumerSecret);
        }

        public static void SetCredentials(string userAccessToken, string userAccessSecret, string consumerKey, string consumerSecret)
        {
            Credentials = CreateCredentials(userAccessToken, userAccessSecret, consumerKey, consumerSecret);
        }

        public static void SetCredentials(IOAuthCredentials credentials)
        {
            Credentials = credentials;
        }

        public static T ExecuteOperationWithCredentials<T>(IOAuthCredentials credentials, Func<T> operation)
        {
            return CredentialsAccessor.ExecuteOperationWithCredentials(credentials, operation);
        }

        public static void ExecuteOperationWithCredentials(IOAuthCredentials credentials, Action operation)
        {
            CredentialsAccessor.ExecuteOperationWithCredentials(credentials, operation);
        }
    }
}