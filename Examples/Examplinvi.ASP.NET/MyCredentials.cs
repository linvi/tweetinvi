using Tweetinvi.Models;

namespace Examplinvi.ASP.NET
{
    public static class MyCredentials
    {
        public static IConsumerOnlyCredentials GetAppCredentials()
        {
            return new ConsumerOnlyCredentials("S7zdhisQjfVeyxev4upGaDS6P","hbgWl5XHWizuJbajBKq7xobhfW4aC3xAmz3xzaUL9NiBmrWG5t");
        }
        
        public static ITwitterCredentials LastAuthenticatedCredentials { get; set; }
    }
}