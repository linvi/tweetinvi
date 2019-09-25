using Tweetinvi.Client.Requesters;

namespace Tweetinvi.Client
{
    public interface IInternalRequestExecutor : IRequestExecutor
    {
        void Initialize(TwitterClient client);
    }

    public class RequestExecutor : IInternalRequestExecutor
    {
        private readonly IInternalAccountsRequester _accountsRequester;
        private readonly IInternalTweetsRequester _tweetsRequester;
        private readonly IInternalUsersRequester _usersRequester;

        private TwitterClient _client;

        public RequestExecutor(
            IInternalAccountsRequester accountsRequester,
            IInternalTweetsRequester tweetsRequester,
            IInternalUsersRequester usersRequester)
        {
            _accountsRequester = accountsRequester;
            _tweetsRequester = tweetsRequester;
            _usersRequester = usersRequester;
        }

        public void Initialize(TwitterClient client)
        {
            _client = client;
            
            _accountsRequester.Initialize(client);
            _tweetsRequester.Initialize(client);
            _usersRequester.Initialize(client);
        }

        public IAccountsRequester Accounts => _accountsRequester;
        public ITweetsRequester Tweets => _tweetsRequester;
        public IUsersRequester Users => _usersRequester;
    }
}
