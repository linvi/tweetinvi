using Tweetinvi.Client;
using Tweetinvi.Client.Tools;
using Tweetinvi.Core.Client;
using Tweetinvi.Core.Client.Validators;
using Tweetinvi.Core.Events;
using Tweetinvi.Models;

namespace Tweetinvi
{
    public interface ITwitterClient
    {
        /// <summary>
        /// Client to execute all actions related with the account associated with the clients' credentials
        /// </summary>
        IAccountSettingsClient AccountSettings { get; }

        /// <summary>
        /// Client to execute all actions related with authentication
        /// </summary>
        IAuthClient Auth { get; }

        /// <summary>
        /// Client to execute custom requests
        /// </summary>
        IExecuteClient Execute { get; }

        /// <summary>
        /// Client to execute all actions from the help path
        /// </summary>
        IHelpClient Help { get; }

        /// <summary>
        /// Client to execute all actions related with twitter lists
        /// </summary>
        IListsClient Lists { get; }

        /// <summary>
        /// Client to execute all actions related with messages
        /// </summary>
        IMessagesClient Messages { get; }

        /// <summary>
        /// Client to execute all actions related with rate limits
        /// </summary>
        IRateLimitsClient RateLimits { get; }

        /// <summary>
        /// Client to execute all actions related with search
        /// </summary>
        ISearchClient Search { get; }

        /// <summary>
        /// Client to create all type of streams
        /// </summary>
        IStreamsClient Streams { get; }

        /// <summary>
        /// Client to execute all actions related with timelines
        /// </summary>
        ITimelinesClient Timelines { get; }

        /// <summary>
        /// Client to execute all actions related with trends
        /// </summary>
        ITrendsClient Trends { get; }

        /// <summary>
        /// Client to execute all actions related with tweets
        /// </summary>
        ITweetsClient Tweets { get; }

        /// <summary>
        /// Client to execute all actions related with media upload
        /// </summary>
        IUploadClient Upload { get; }

        /// <summary>
        /// Client to execute all actions related with users
        /// </summary>
        IUsersClient Users { get; }

        /// <summary>
        /// Client to execute all the actions related with webhooks
        /// </summary>
        IAccountActivityClient AccountActivity { get; }

        /// <summary>
        /// Execute Request and receive request results
        /// </summary>
        IRawExecutors Raw { get; }


        /// <summary>
        /// Client's settings
        /// </summary>
        ITweetinviSettings ClientSettings { get; }

        /// <summary>
        /// Client's credentials
        /// </summary>
        IReadOnlyTwitterCredentials Credentials { get; }

        /// <summary>
        /// Listen to events raised by actions performed by the client
        /// </summary>
        IExternalClientEvents Events { get; }

        /// <summary>
        /// Simple way to construct tweetinvi objects
        /// </summary>
        ITwitterClientFactories Factories { get; }

        /// <summary>
        /// Help you perform json operations with Tweetinvi objects
        /// </summary>
        IJsonClient Json { get; }

        /// <summary>
        /// Validate parameters to ensure that they meet the default criteria
        /// </summary>
        IParametersValidator ParametersValidator { get; }


        /// <summary>
        /// Creates skeleton request representing a request from the client
        /// </summary>
        ITwitterRequest CreateRequest();

        /// <summary>
        /// Create an execution context for a request to be sent to Twitter.
        /// </summary>
        ITwitterExecutionContext CreateTwitterExecutionContext();
    }
}
