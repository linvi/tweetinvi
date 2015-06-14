using Tweetinvi.Core.Interfaces.Models;
using Tweetinvi.Core.Interfaces.Parameters;

namespace Tweetinvi.Logic.Parameters.QueryParameters
{
    public interface IUserTimelineQueryParameters
    {
        IUserIdentifier UserIdentifier { get; }
        IUserTimelineParameters QueryParameters { get; }
    }
}
