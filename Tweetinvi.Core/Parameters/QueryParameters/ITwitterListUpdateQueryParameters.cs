using Tweetinvi.Core.Interfaces.Models;

namespace Tweetinvi.Core.Parameters.QueryParameters
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