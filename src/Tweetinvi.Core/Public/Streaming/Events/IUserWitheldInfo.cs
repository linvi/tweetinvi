using System.Collections.Generic;

namespace Tweetinvi.Streaming.Events
{
    public interface IUserWitheldInfo
    {
        long Id { get; }
        IEnumerable<string> WitheldInCountries { get; }
    }
}