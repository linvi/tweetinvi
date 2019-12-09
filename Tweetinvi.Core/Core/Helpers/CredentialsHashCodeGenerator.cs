using Tweetinvi.Models;

namespace Tweetinvi.Core.Helpers
{
    public static class CredentialsHashCodeGenerator
    {
        public static string CreateHash(IReadOnlyConsumerCredentials credentials)
        {
            var hash = $"{credentials.ConsumerKey} - {credentials.ConsumerSecret} - {credentials.BearerToken}";
            var twitterCredentials = credentials as IReadOnlyTwitterCredentials;

            if (twitterCredentials == null)
            {
                return hash;
            }

            return $"{hash} - {twitterCredentials.AccessToken}  - {twitterCredentials.AccessTokenSecret}";
        }
    }
}