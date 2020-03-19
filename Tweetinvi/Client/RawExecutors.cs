using Tweetinvi.Client.Requesters;

namespace Tweetinvi.Client
{
    public class RawExecutors : IRawExecutors
    {
        private readonly IAuthRequester _authRequester;
        private readonly IAccountSettingsRequester _accountSettingsRequester;
        private readonly IExecuteRequester _executeRequester;
        private readonly IHelpRequester _helpRequester;
        private readonly ISearchRequester _searchRequester;
        private readonly ITwitterListsRequester _listsRequester;
        private readonly ITimelinesRequester _timelinesRequester;
        private readonly ITweetsRequester _tweetsRequester;
        private readonly IUploadRequester _uploadRequester;
        private readonly IUsersRequester _usersRequester;
        private readonly IAccountActivityRequester _accountActivityRequester;

        public RawExecutors(
            IAccountActivityRequester accountActivityRequester,
            IAuthRequester authRequester,
            IAccountSettingsRequester accountSettingsRequester,
            IExecuteRequester executeRequester,
            IHelpRequester helpRequester,
            ISearchRequester searchRequester,
            ITwitterListsRequester listsRequester,
            ITimelinesRequester timelinesRequester,
            ITweetsRequester tweetsRequester,
            IUploadRequester uploadRequester,
            IUsersRequester usersRequester)
        {
            _accountActivityRequester = accountActivityRequester;
            _authRequester = authRequester;
            _accountSettingsRequester = accountSettingsRequester;
            _executeRequester = executeRequester;
            _helpRequester = helpRequester;
            _searchRequester = searchRequester;
            _listsRequester = listsRequester;
            _timelinesRequester = timelinesRequester;
            _tweetsRequester = tweetsRequester;
            _uploadRequester = uploadRequester;
            _usersRequester = usersRequester;
        }

        public IAuthRequester Auth => _authRequester;
        public IAccountSettingsRequester AccountSettings => _accountSettingsRequester;
        public IExecuteRequester Execute => _executeRequester;
        public IHelpRequester Help => _helpRequester;
        public ISearchRequester Search => _searchRequester;
        public ITwitterListsRequester Lists => _listsRequester;
        public ITimelinesRequester Timelines => _timelinesRequester;
        public ITweetsRequester Tweets => _tweetsRequester;
        public IUploadRequester Upload => _uploadRequester;
        public IUsersRequester Users => _usersRequester;
        public IAccountActivityRequester AccountActivity => _accountActivityRequester;
    }
}
