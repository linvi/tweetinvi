using Tweetinvi.Core.Interfaces.Models;

namespace Tweetinvi.Core.Interfaces.Parameters.QueryParameters
{
    public interface IGetTweetsFromListQueryParameters
    {
        ITwitterListIdentifier TwitterListIdentifier { get; }
        IGetTweetsFromListParameters Parameters { get; }
    }
}
