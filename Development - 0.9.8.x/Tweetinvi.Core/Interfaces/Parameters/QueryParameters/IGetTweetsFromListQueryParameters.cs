using Tweetinvi.Core.Interfaces.Models;
using Tweetinvi.Core.Interfaces.Parameters;

namespace Tweetinvi.Logic.Parameters.QueryParameters
{
    public interface IGetTweetsFromListQueryParameters
    {
        ITwitterListIdentifier TwitterListIdentifier { get; }
        IGetTweetsFromListParameters QueryParameters { get; }
    }
}
