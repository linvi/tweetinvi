using System.Collections.Generic;
using System.Threading.Tasks;
using Tweetinvi.Core.Extensions;
using Tweetinvi.Models;

namespace Examplinvi.WebhooksServer
{
    public static class CredentialsRetriever
    {
        public static readonly Dictionary<long, ITwitterCredentials> CredentialsByUserId = new Dictionary<long, ITwitterCredentials>();

        public static async Task<ITwitterCredentials> GetUserCredentials(long userId)
        {
            // Implement your own logic for user credentials data retrieval
            if (CredentialsByUserId.TryGetValue(userId, out var credentials))
            {
                return await Task.FromResult(credentials);
            }

            return null;
        }

        public static async Task SetUserCredentials(long userId, ITwitterCredentials credentials)
        {
            await Task.Run(() =>
            {
                CredentialsByUserId.AddOrUpdate(userId, credentials);
            });
        }
    }
}
