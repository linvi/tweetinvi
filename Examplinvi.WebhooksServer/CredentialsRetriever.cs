using System.Threading.Tasks;
using Tweetinvi.Models;

namespace Examplinvi.WebhooksServer
{
    public static class CredentialsRetriever
    {
        public static async Task<ITwitterCredentials> GetUserCredentials(string userId)
        {
            // Implement your own logic for user credentials data retrieval
            return await Task.FromResult(new TwitterCredentials("CONSUMER_KEY", "CONSUMER_SECRET", "ACCESS_TOKEN", "ACCESS_TOKEN_SECRET"));
        }
    }
}
