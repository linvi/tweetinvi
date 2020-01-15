using Tweetinvi.Client.Requesters;

namespace Tweetinvi.Client
{
    public interface IRequestExecutor
    {
        IAccountRequester Account { get; }
        IAuthRequester Auth { get; }
        IAccountSettingsRequester AccountSettings { get; }
        IExecuteRequester Execute { get; }
        IHelpRequester Help { get; }
        ITimelinesRequester Timelines { get; }
        ITweetsRequester Tweets { get; }
        IUploadRequester Upload { get; }
        IUsersRequester Users { get; }
    }
}