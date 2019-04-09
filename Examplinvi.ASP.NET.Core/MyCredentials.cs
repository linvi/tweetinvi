using Tweetinvi.Models;

namespace Examplinvi.ASP.NET.Core
{
    public static class MyCredentials
    {
        public const string CONSUMER_KEY = "CONSUMER_KEY";
        public const string CONSUMER_SECRET = "CONSUMER_SECRET";
        private const string ACCESS_TOKEN = "ACCESS_TOKEN";
        private const string ACCESS_TOKEN_SECRET = "ACCESS_TOKEN_SECRET";

        public static ITwitterCredentials GenerateCredentials()
        {
            return new TwitterCredentials(CONSUMER_KEY, CONSUMER_SECRET, ACCESS_TOKEN, ACCESS_TOKEN_SECRET);
        }
    }
}