using Tweetinvi.Client.Requesters;

namespace Tweetinvi.Client
{
    public interface IRequestExecutor
    {
        IAccountsRequester Accounts { get; }
        ITweetsRequester Tweets { get; }
        IUsersRequester Users { get; }
    }
}