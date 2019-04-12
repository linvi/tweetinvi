using Tweetinvi.Client;
using Tweetinvi.Models;
using Tweetinvi.Models.Interfaces;

namespace Tweetinvi
{
    public class TwitterClient
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

            Tweets = new Tweets(this);
        }

        public Tweets Tweets { get; }

        public IRequestExecutor RequestExecutor { get; }

        public ITwitterRequest CreateRequest()
        {
            var request = new TwitterRequest
            {
                Config = Config,
            };

            request.Query.TwitterCredentials = Credentials;

            return request;
        }
    }
}
