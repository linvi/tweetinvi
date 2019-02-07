using System.Collections.Generic;
using System.Runtime.InteropServices.ComTypes;
using System.Threading.Tasks;
using Tweetinvi.Core.Extensions;
using Tweetinvi.Models;

namespace Examplinvi.WebhooksServer
{
    public static class CredentialsRetriever
    {
        public static Dictionary<long, ITwitterCredentials> _credentialsByUserId = new Dictionary<long, ITwitterCredentials>();

        public static async Task<ITwitterCredentials> GetUserCredentials(long userId)
        {
            // Implement your own logic for user credentials data retrieval
            if (_credentialsByUserId.TryGetValue(userId, out var credentials))
            {
                return await Task.FromResult(credentials);
            }

            return null;
        }

        public static async Task SetUserCredentials(long userId, ITwitterCredentials credentials)
        {
            await Task.Run(() =>
            {
                _credentialsByUserId.AddOrUpdate(userId, credentials);
            });
        }
    }
}
