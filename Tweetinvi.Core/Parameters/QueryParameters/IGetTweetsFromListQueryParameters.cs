using Tweetinvi.Core.Interfaces.Models;

namespace Tweetinvi.Core.Parameters.QueryParameters
{
    /// <summary>
    /// https://dev.twitter.com/rest/reference/get/lists/statuses
    /// </summary>
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