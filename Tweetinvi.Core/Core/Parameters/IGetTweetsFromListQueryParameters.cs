using Tweetinvi.Models;
using Tweetinvi.Parameters;

namespace Tweetinvi.Core.Parameters
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