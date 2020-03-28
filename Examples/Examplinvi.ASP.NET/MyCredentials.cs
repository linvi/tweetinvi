using Tweetinvi.Models;

namespace Examplinvi.ASP.NET
{
    public static class MyCredentials
    {
        public static IConsumerOnlyCredentials GetAppCredentials()
        {
            return new ConsumerOnlyCredentials("CONSUMER_KEY","CONSUMER_SECRET");
        }
        
        public static ITwitterCredentials LastAuthenticatedCredentials { get; set; }
    }
}