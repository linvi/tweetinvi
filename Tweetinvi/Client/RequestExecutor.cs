using Tweetinvi.Client.Requesters;

namespace Tweetinvi.Client
{
    public interface IRequestExecutor
    {
        ITweetsRequester Tweets { get; }
        IUsersRequester Users { get; }
    }

    public interface IInternalRequestExecutor : IRequestExecutor
    {
        void Initialize(TwitterClient client);
    }

    public class RequestExecutor : IInternalRequestExecutor
    {
        private readonly IInternalTweetsRequester _tweetsRequester;
        private readonly IInternalUsersRequester _usersRequester;

        private TwitterClient _client;

        public RequestExecutor(
            IInternalTweetsRequester tweetsRequester,
            IInternalUsersRequester usersRequester)
        {
            _tweetsRequester = tweetsRequester;
            _usersRequester = usersRequester;
        }

        public void Initialize(TwitterClient client)
        {
            _client = client;
            _tweetsRequester.Initialize(client);
            _usersRequester.Initialize(client);
        }

        public ITweetsRequester Tweets => _tweetsRequester;
        public IUsersRequester Users => _usersRequester;
    }
}
