using System.Collections.Generic;

namespace Tweetinvi.Core.Interfaces.Models.StreamMessages
{
    public interface ITweetWitheldInfo
    {
        long Id { get; }
        long UserId { get; }
        IEnumerable<string> WitheldInCountries { get; }
    }
}