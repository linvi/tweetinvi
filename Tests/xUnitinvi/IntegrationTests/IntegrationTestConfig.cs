using Tweetinvi.Models;

namespace xUnitinvi.IntegrationTests
{
    public static class IntegrationTestConfig
    {
        public static readonly bool ShouldRunIntegrationTests = true;
        public static readonly ITwitterCredentials NormalUserCredentials = new TwitterCredentials();
        public static readonly ITwitterCredentials ProtectedUserCredentials = new TwitterCredentials();
    }
}