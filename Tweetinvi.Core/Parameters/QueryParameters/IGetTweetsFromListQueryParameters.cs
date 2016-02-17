using Tweetinvi.Core.Interfaces.Models;

namespace Tweetinvi.Core.Parameters.QueryParameters
{
    public interface IGetTweetsFromListQueryParameters
    {
        /// <summary>
        /// List identifier.
        /// </summary>
        ITwitterListIdentifier TwitterListIdentifier { get; }

        /// <summary>
        /// Query optional parameters.
        /// </summary>
        IGetTweetsFromListParameters Parameters { get; }
    }
}