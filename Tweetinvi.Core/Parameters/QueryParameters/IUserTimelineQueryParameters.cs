using Tweetinvi.Core.Interfaces.Models;

namespace Tweetinvi.Core.Parameters.QueryParameters
{
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