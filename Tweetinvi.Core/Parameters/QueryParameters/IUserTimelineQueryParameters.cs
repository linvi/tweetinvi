using Tweetinvi.Core.Interfaces.Models;

namespace Tweetinvi.Core.Parameters.QueryParameters
{
    /// <summary>
    /// https://dev.twitter.com/rest/reference/get/statuses/user_timeline
    /// </summary>
    public interface IUserTimelineQueryParameters
    {
        /// <summary>
        /// User identifier.
        /// </summary>
        IUserIdentifier UserIdentifier { get; }

        /// <summary>
        /// Query optional parameters.
        /// </summary>
        IUserTimelineParameters Parameters { get; }
    }
}