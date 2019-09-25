using Tweetinvi.Client;
using Tweetinvi.Client.Requesters;
using Tweetinvi.Core.Client;
using Tweetinvi.Models;

namespace Tweetinvi
{
    public interface ITwitterClient
    {
        /// <summary>
        /// Client to execute all actions related the account associated with the clients' credentials
        /// </summary>
        IAccountsClient Accounts { get; }
        
        /// <summary>
        /// Client to execute all actions related with tweets
        /// </summary>
        ITweetsClient Tweets { get; }

        /// <summary>
        /// Client to execute all actions related with users
        /// </summary>
        IUsersClient Users { get; }

        ITwitterCredentials Credentials { get; }
        ITweetinviSettings Config { get; }
        IRequestExecutor RequestExecutor { get; }
        ITwitterRequest CreateRequest();
        ITwitterExecutionContext CreateTwitterExecutionContext();
    }
}
