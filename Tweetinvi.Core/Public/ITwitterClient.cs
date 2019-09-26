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
        IAccountClient Account { get; }
        
        /// <summary>
        /// Client to execute all actions related with tweets
        /// </summary>
        ITweetsClient Tweets { get; }

        /// <summary>
        /// Client to execute all actions related with users
        /// </summary>
        IUsersClient Users { get; }

        /// <summary>
        /// Client's credentials
        /// </summary>
        ITwitterCredentials Credentials { get; }
        
        /// <summary>
        /// Client's settings
        /// </summary>
        ITweetinviSettings Config { get; }
        IRequestExecutor RequestExecutor { get; }
        ITwitterRequest CreateRequest();
        ITwitterExecutionContext CreateTwitterExecutionContext();
    }
}
