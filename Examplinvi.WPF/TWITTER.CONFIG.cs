using Tweetinvi;

namespace Examplinvi.WPF
{
    public static class TwitterConfig
    {
        public static void InitApp()
        {
            Auth.SetUserCredentials("CONSUMER_KEY", "CONSUMER_SECRET", "ACCESS_TOKEN", "ACCESS_TOKEN_SECRET");
        }
    }
}