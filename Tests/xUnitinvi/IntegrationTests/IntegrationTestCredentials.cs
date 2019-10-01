using Tweetinvi.Models;

namespace xUnitinvi.IntegrationTests
{
    public static class IntegrationTestCredentials
    {
        public static readonly ITwitterCredentials NormalUserCredentials = new TwitterCredentials();
        public static readonly ITwitterCredentials ProtectedUserCredentials = new TwitterCredentials();

    }
}