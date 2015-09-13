using Tweetinvi.Core.Interfaces.Models;

namespace Tweetinvi.Core.Interfaces.Parameters.QueryParameters
{
    public interface IUserTimelineQueryParameters
    {
        IUserIdentifier UserIdentifier { get; }
        IUserTimelineParameters Parameters { get; }
    }
}