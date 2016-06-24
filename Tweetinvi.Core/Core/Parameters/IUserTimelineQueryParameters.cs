using Tweetinvi.Models;
using Tweetinvi.Parameters;

namespace Tweetinvi.Core.Parameters
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