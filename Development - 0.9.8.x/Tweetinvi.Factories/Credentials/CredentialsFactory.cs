using Tweetinvi.Core.Injectinvi;
using Tweetinvi.Core.Interfaces.Factories;
using Tweetinvi.Core.Interfaces.WebLogic;

namespace Tweetinvi.Factories.Credentials
{
    public class CredentialsFactory : ICredentialsFactory
    {
        private readonly IFactory<IOAuthCredentials> _oauthCredentialsUnityFactory;

        public CredentialsFactory(IFactory<IOAuthCredentials> oauthCredentialsUnityFactory)
        {
            _oauthCredentialsUnityFactory = oauthCredentialsUnityFactory;
        }

        public IOAuthCredentials CreateOAuthCredentials(string userAccessToken, string userAccessSecret, string consumerKey, string consumerSecret)
        {
            var credentials = _oauthCredentialsUnityFactory.Create();
            credentials.AccessToken = userAccessToken;
            credentials.AccessTokenSecret = userAccessSecret;
            credentials.ConsumerKey = consumerKey;
            credentials.ConsumerSecret = consumerSecret;
            return credentials;
        }
    }
}