using Tweetinvi.Core.Authentication;

namespace Examplinvi.Web
{
    public static class MyCredentials
    {
        public static string CONSUMER_KEY = "MY_CONSUMER_KEY";
        public static string CONSUMER_SECRET = "MY_CONSUMER_SECRET";
        public static string ACCESS_TOKEN = "MY_ACCESS_TOKEN";
        public static string ACCESS_TOKEN_SECRET = "MY_ACCESS_TOKEN_SECRET";

        public static ITwitterCredentials GenerateCredentials()
        {
            return new TwitterCredentials(CONSUMER_KEY, CONSUMER_SECRET, ACCESS_TOKEN, ACCESS_TOKEN_SECRET);
        }
    }
}