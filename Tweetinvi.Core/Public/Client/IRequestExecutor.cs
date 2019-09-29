using Tweetinvi.Client.Requesters;

namespace Tweetinvi.Client
{
    public interface IRequestExecutor
    {
        IAccountRequester Account { get; }
        ITweetsRequester Tweets { get; }
        IUsersRequester Users { get; }
    }
}