using System.Collections.Generic;

namespace Tweetinvi.Core.Interfaces.Models.StreamMessages
{
    public interface IWarningMessageTooManyFollowers : IWarningMessage
    {
        IEnumerable<long> UserIds { get; }
    }
}