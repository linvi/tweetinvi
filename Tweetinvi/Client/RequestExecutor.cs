using Tweetinvi.Client.Requesters;

namespace Tweetinvi.Client
{
    public interface IInternalRequestExecutor : IRequestExecutor
    {
        void Initialize(TwitterClient client);
    }

    public class RequestExecutor : IInternalRequestExecutor
    {
        private readonly IInternalAccountRequester _accountRequester;
        private readonly IInternalTweetsRequester _tweetsRequester;
        private readonly IInternalUsersRequester _usersRequester;

        private TwitterClient _client;

        public RequestExecutor(
            IInternalAccountRequester accountRequester,
            IInternalTweetsRequester tweetsRequester,
            IInternalUsersRequester usersRequester)
        {
            _accountRequester = accountRequester;
            _tweetsRequester = tweetsRequester;
            _usersRequester = usersRequester;
        }

        public void Initialize(TwitterClient client)
        {
            _client = client;
            
            _accountRequester.Initialize(client);
            _tweetsRequester.Initialize(client);
            _usersRequester.Initialize(client);
        }

        public IAccountRequester Account => _accountRequester;
        public ITweetsRequester Tweets => _tweetsRequester;
        public IUsersRequester Users => _usersRequester;
    }
}
