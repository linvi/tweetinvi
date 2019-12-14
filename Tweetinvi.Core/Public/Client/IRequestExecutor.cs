using Tweetinvi.Client.Requesters;

namespace Tweetinvi.Client
{
    public interface IRequestExecutor
    {
        IAccountRequester Account { get; }
        IAuthRequester Auth { get; }
        IAccountSettingsRequester AccountSettings { get; }
        IHelpRequester Help { get; }
        ITimelineRequester Timeline { get; }
        ITweetsRequester Tweets { get; }
        IUploadRequester Upload { get; }
        IUsersRequester Users { get; }
    }
}