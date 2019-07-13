namespace Tweetinvi.Client
{
    public interface IRequestExecutor
    {
        ITweetsRequester Tweets { get; }
    }

    public interface IInternalRequestExecutor : IRequestExecutor
    {
        void Initialize(TwitterClient client);
    }

    public class RequestExecutor : IInternalRequestExecutor
    {
        private readonly IInternalTweetsRequester _tweetsRequester;

        private TwitterClient _client;

        public RequestExecutor(IInternalTweetsRequester tweetsRequester)
        {
            _tweetsRequester = tweetsRequester;
        }

        public void Initialize(TwitterClient client)
        {
            _client = client;
            _tweetsRequester.Initialize(_client.CreateRequest);
        }

        public ITweetsRequester Tweets => _tweetsRequester;
    }
}
