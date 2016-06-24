using System.Collections.Generic;

namespace Tweetinvi.Streaming.Events
{
    public interface IWarningMessageTooManyFollowers : IWarningMessage
    {
        IEnumerable<long> UserIds { get; }
    }
}