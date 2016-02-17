using Tweetinvi.Core.Interfaces.Models;

namespace Tweetinvi.Core.Parameters.QueryParameters
{
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