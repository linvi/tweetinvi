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
        private readonly IInternalAuthRequester _authRequester;
        private readonly IInternalAccountSettingsRequester _accountSettingsRequester;
        private readonly IInternalExecuteRequester _executeRequester;
        private readonly IInternalHelpRequester _helpRequester;
        private readonly IInternalTimelineRequester _timelineRequester;
        private readonly IInternalTweetsRequester _tweetsRequester;
        private readonly IInternalUploadRequester _uploadRequester;
        private readonly IInternalUsersRequester _usersRequester;

        public RequestExecutor()
        {
        }

        public RequestExecutor(
            IInternalAccountRequester accountRequester,
            IInternalAuthRequester authRequester,
            IInternalAccountSettingsRequester accountSettingsRequester,
            IInternalExecuteRequester executeRequester,
            IInternalHelpRequester helpRequester,
            IInternalTimelineRequester timelineRequester,
            IInternalTweetsRequester tweetsRequester,
            IInternalUploadRequester uploadRequester,
            IInternalUsersRequester usersRequester)
        {
            _accountRequester = accountRequester;
            _authRequester = authRequester;
            _accountSettingsRequester = accountSettingsRequester;
            _executeRequester = executeRequester;
            _helpRequester = helpRequester;
            _timelineRequester = timelineRequester;
            _tweetsRequester = tweetsRequester;
            _uploadRequester = uploadRequester;
            _usersRequester = usersRequester;
        }

        public void Initialize(TwitterClient client)
        {
            _accountRequester.Initialize(client);
            _authRequester.Initialize(client);
            _accountSettingsRequester.Initialize(client);
            _executeRequester.Initialize(client);
            _helpRequester.Initialize(client);
            _timelineRequester.Initialize(client);
            _tweetsRequester.Initialize(client);
            _uploadRequester.Initialize(client);
            _usersRequester.Initialize(client);
        }

        public IAccountRequester Account => _accountRequester;
        public IAuthRequester Auth => _authRequester;
        public IAccountSettingsRequester AccountSettings => _accountSettingsRequester;
        public IExecuteRequester Execute => _executeRequester;
        public IHelpRequester Help => _helpRequester;
        public ITimelineRequester Timeline => _timelineRequester;
        public ITweetsRequester Tweets => _tweetsRequester;
        public IUploadRequester Upload => _uploadRequester;
        public IUsersRequester Users => _usersRequester;
    }
}
