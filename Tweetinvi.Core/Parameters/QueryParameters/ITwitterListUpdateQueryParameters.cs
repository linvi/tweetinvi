using Tweetinvi.Core.Interfaces.Models;

namespace Tweetinvi.Core.Parameters.QueryParameters
{
    public interface ITwitterListUpdateQueryParameters
    {
        ITwitterListIdentifier TwitterListIdentifier { get; }
        ITwitterListUpdateParameters Parameters { get; }
    }
}
