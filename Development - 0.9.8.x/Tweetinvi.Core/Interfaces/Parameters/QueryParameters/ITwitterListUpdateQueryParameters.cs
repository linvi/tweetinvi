using Tweetinvi.Core.Interfaces.Models;

namespace Tweetinvi.Core.Interfaces.Parameters.QueryParameters
{
    public interface ITwitterListUpdateQueryParameters
    {
        ITwitterListIdentifier TwitterListIdentifier { get; }
        ITwitterListUpdateParameters QueryParameters { get; }
    }
}
