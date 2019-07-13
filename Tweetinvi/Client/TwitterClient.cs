using Tweetinvi.Client;
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
        Tweets Tweets { get; }

        ITwitterCredentials Credentials { get; }
        ITweetinviSettings Config { get; }
        IRequestExecutor RequestExecutor { get; }
        ITwitterRequest CreateRequest();
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
