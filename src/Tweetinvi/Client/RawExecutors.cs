using Tweetinvi.Client.Requesters;
using Tweetinvi.Client.Requesters.V2;

namespace Tweetinvi.Client
{
    public class RawExecutors : IRawExecutors
    {
        private readonly IAuthRequester _authRequester;
        private readonly IAccountSettingsRequester _accountSettingsRequester;
        private readonly IHelpRequester _helpRequester;
        private readonly ISearchRequester _searchRequester;
        private readonly ITwitterListsRequester _listsRequester;
        private readonly ITimelinesRequester _timelinesRequester;
        private readonly ITrendsRequester _trendsRequester;
        private readonly ITweetsRequester _tweetsRequester;
        private readonly IUploadRequester _uploadRequester;
        private readonly IUsersRequester _usersRequester;
        private readonly IAccountActivityRequester _accountActivityRequester;

        private readonly ISearchV2Requester _searchV2Requester;
        private readonly ITweetsV2Requester _tweetsV2Requester;
        private readonly IUsersV2Requester _usersV2Requester;

        public RawExecutors(
            IAccountActivityRequester accountActivityRequester,
            IAuthRequester authRequester,
            IAccountSettingsRequester accountSettingsRequester,
            IHelpRequester helpRequester,
            ISearchRequester searchRequester,
            ITwitterListsRequester listsRequester,
            ITimelinesRequester timelinesRequester,
            ITrendsRequester trendsRequester,
            ITweetsRequester tweetsRequester,
            IUploadRequester uploadRequester,
            IUsersRequester usersRequester,
            ISearchV2Requester searchV2Requester,
            ITweetsV2Requester tweetsV2Requester,
            IUsersV2Requester usersV2Requester)
        {
            _accountActivityRequester = accountActivityRequester;
            _authRequester = authRequester;
            _accountSettingsRequester = accountSettingsRequester;
            _helpRequester = helpRequester;
            _searchRequester = searchRequester;
            _listsRequester = listsRequester;
            _timelinesRequester = timelinesRequester;
            _trendsRequester = trendsRequester;
            _tweetsRequester = tweetsRequester;
            _uploadRequester = uploadRequester;
            _usersRequester = usersRequester;

            _searchV2Requester = searchV2Requester;
            _tweetsV2Requester = tweetsV2Requester;
            _usersV2Requester = usersV2Requester;
        }

        public IAuthRequester Auth => _authRequester;
        public IAccountSettingsRequester AccountSettings => _accountSettingsRequester;
        public IHelpRequester Help => _helpRequester;
        public ISearchRequester Search => _searchRequester;
        public ITwitterListsRequester Lists => _listsRequester;
        public ITimelinesRequester Timelines => _timelinesRequester;
        public ITrendsRequester Trends => _trendsRequester;
        public ITweetsRequester Tweets => _tweetsRequester;
        public IUploadRequester Upload => _uploadRequester;
        public IUsersRequester Users => _usersRequester;
        public IAccountActivityRequester AccountActivity => _accountActivityRequester;


        public ISearchV2Requester SearchV2 => _searchV2Requester;
        public ITweetsV2Requester TweetsV2 => _tweetsV2Requester;
        public IUsersV2Requester UsersV2 => _usersV2Requester;
    }
}
