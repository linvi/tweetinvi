using Tweetinvi.Models;

namespace Examplinvi.ASP.NET.Core
{
    public class MyCredentials
    {
        public static ITwitterCredentials GenerateCredentials()
        {
            return new TwitterCredentials("CONSUMER_KEY", "CONSUMER_SECRET", "ACCESS_TOKEN", "ACCESS_TOKEN_SECRET");
        }
    }
}