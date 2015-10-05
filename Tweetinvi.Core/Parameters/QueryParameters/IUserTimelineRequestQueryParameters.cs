using Tweetinvi.Core.Interfaces.Models;

namespace Tweetinvi.Core.Parameters.QueryParameters
{
    public interface IUserTimelineQueryParameters
    {
        IUserIdentifier UserIdentifier { get; }
        IUserTimelineParameters Parameters { get; }
    }
}