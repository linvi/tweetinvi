using System.Collections.Generic;

namespace Tweetinvi.Core.Interfaces.Models.StreamMessages
{
    public interface IUserWitheldInfo
    {
        long Id { get; }
        IEnumerable<string> WitheldInCountries { get; }
    }
}