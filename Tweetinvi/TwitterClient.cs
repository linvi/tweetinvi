using Tweetinvi.Client;
using Tweetinvi.Client.Requesters;
using Tweetinvi.Core.Client;
using Tweetinvi.Models;

// ReSharper disable once CheckNamespace
namespace Tweetinvi
{
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

            Account = new AccountClient(this);
            Tweets = new TweetsClient(this);
            Users = new UsersClient(this);
        }

        public IAccountClient Account { get; }
        public ITweetsClient Tweets { get; }
        public IUsersClient Users { get; }

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
