namespace Tweetinvi.Client
{
    public interface IRequestExecutor
    {
        ITweetRequester TweetRequester { get; }
    }

    public interface IInternalRequestExecutor : IRequestExecutor
    {
        void Initialize(TwitterClient client);
    }

    public class RequestExecutor : IInternalRequestExecutor
    {
        private readonly IInternalTweetRequester _tweetRequester;

        private TwitterClient _client;

        public RequestExecutor(IInternalTweetRequester tweetRequester)
        {
            _tweetRequester = tweetRequester;
        }

        public void Initialize(TwitterClient client)
        {
            _client = client;
            _tweetRequester.Initialize(_client);
        }

        public ITweetRequester TweetRequester => _tweetRequester;
    }
}
