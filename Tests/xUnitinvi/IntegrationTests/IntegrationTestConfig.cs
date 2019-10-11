using Tweetinvi.Models;

namespace xUnitinvi.IntegrationTests
{
    public static class IntegrationTestConfig
    {
        public static readonly bool ShouldRunIntegrationTests = false;
        
        public static readonly ITwitterCredentials TweetinviApiCredentials = new TwitterCredentials();
        public static readonly ITwitterCredentials TweetinviTestCredentials = new TwitterCredentials();
        public static readonly ITwitterCredentials ProtectedUserCredentials = new TwitterCredentials();
    }
}