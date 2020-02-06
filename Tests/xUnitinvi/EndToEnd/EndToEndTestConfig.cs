using Tweetinvi.Models;

namespace xUnitinvi.EndToEnd
{
    public static class EndToEndTestConfig
    {
        public static readonly bool ShouldRunEndToEndTests = false;
        public static readonly bool ShouldRunRateLimitHungryTests = false;

        public static readonly IntegrationTestAccount TweetinviApi = new IntegrationTestAccount
        {
            Credentials = new TwitterCredentials("", "", "",    ""),
            AccountId = "tweetinviapi"
        };

        public static readonly IntegrationTestAccount TweetinviTest = new IntegrationTestAccount
        {
            Credentials = new TwitterCredentials("", "", "",    ""),
            AccountId = "tweetinvitest"
        };

        public static readonly IntegrationTestAccount ProtectedUser = new IntegrationTestAccount
        {
            Credentials = new TwitterCredentials("", "", "",    ""),
            AccountId = "artwolkt"
        };
    }

    public class IntegrationTestAccount
    {
        public ITwitterCredentials Credentials { get; set; }
        public string AccountId { get; set; }

        public static implicit operator string (IntegrationTestAccount account)
        {
            return account.AccountId;
        }
    }
}