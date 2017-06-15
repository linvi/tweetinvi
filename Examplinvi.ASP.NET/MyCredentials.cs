using Tweetinvi.Models;

namespace Examplinvi.ASP.NET
{
    public static class MyCredentials
    {
        public static string CONSUMER_KEY = "CONSUMER_KEY";
        public static string CONSUMER_SECRET = "CONSUMER_SECRET";
        public static string ACCESS_TOKEN = "ACCESS_TOKEN";
        public static string ACCESS_TOKEN_SECRET = "ACCESS_TOKEN_SECRET";

        public static ITwitterCredentials GenerateCredentials()
        {
            return new TwitterCredentials(CONSUMER_KEY, CONSUMER_SECRET, ACCESS_TOKEN, ACCESS_TOKEN_SECRET);
        }
    }
}