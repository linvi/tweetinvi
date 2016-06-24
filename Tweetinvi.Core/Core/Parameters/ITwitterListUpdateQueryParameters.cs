using Tweetinvi.Models;
using Tweetinvi.Parameters;

namespace Tweetinvi.Core.Parameters
{
    /// <summary>
    /// https://dev.twitter.com/rest/reference/post/lists/update
    /// </summary>
    public interface ITwitterListUpdateQueryParameters
    {
        /// <summary>
        /// List identifier.
        /// </summary>
        ITwitterListIdentifier TwitterListIdentifier { get; }

        /// <summary>
        /// Query optional parameters.
        /// </summary>
        ITwitterListUpdateParameters Parameters { get; }
    }
}