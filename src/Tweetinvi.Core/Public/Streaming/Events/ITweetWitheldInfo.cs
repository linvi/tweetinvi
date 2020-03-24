using System.Collections.Generic;

namespace Tweetinvi.Streaming.Events
{
    public interface ITweetWitheldInfo
    {
        long Id { get; }
        long UserId { get; }
        IEnumerable<string> WitheldInCountries { get; }
    }
}