using Tweetinvi.Client;
using Tweetinvi.Core.Client;
using Tweetinvi.Models;
using Tweetinvi.Models.Interfaces;

// ReSharper disable once CheckNamespace
namespace Tweetinvi
{
    public interface ITwitterClient
    {
        /// <summary>
        /// Client to use in order to execute all actions related with tweets
        /// </summary>
        TweetsClient Tweets { get; }
        UsersClient Users { get; }


        ITwitterCredentials Credentials { get; }
        ITweetinviSettings Config { get; }
        ITwitterRequest CreateRequest();
        ITwitterExecutionContext CreateTwitterExecutionContext();
    }

    public class TwitterClient : ITwitterClient
    {
        public ITwitterCredentials Credentials { get; }
        public ITweetinviSettings Config { get; }

        public TwitterClient(ITwitterCredentials credentials)
        {
            Credentials = credentials;
            Config = new TweetinviSettings();

            var requestExecutor = TweetinviContainer.Resolve<IInternalRequestExecutor>();
            requestExecutor.Initialize(this);

            RequestExecutor = requestExecutor;

            Tweets = new TweetsClient(this);
            Users = new UsersClient(this);
        }

        public TweetsClient Tweets { get; }
        public UsersClient Users { get; }

        public IRequestExecutor RequestExecutor { get; }

        public ITwitterExecutionContext CreateTwitterExecutionContext()
        {
            return new TwitterExecutionContext
            {
                RequestFactory = CreateRequest
            };
        }

        public ITwitterRequest CreateRequest()
        {
            var request = new TwitterRequest
            {
                ExecutionContext = new TwitterExecutionContext
                {
                    RequestFactory = CreateRequest
                },
                Query =
                {
                    TwitterCredentials = Credentials
                }
            };

            request.ExecutionContext.InitialiseFrom(Config);

            return request;
        }
    }
}
