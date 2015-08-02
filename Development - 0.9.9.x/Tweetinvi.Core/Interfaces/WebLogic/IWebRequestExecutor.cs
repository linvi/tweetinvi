using System.Collections.Generic;
using Tweetinvi.Core.Interfaces.Models;
using Tweetinvi.WebLogic;

namespace Tweetinvi.Core.Interfaces.WebLogic
{
    /// <summary>
    /// Generate a Token that can be used to perform OAuth queries
    /// </summary>
    public interface IWebRequestExecutor
    {
        string ExecuteQuery(ITwitterQuery twitterQuery, TwitterClientHandler handler = null);
        string ExecuteMultipartQuery(ITwitterQuery twitterQuery, string contentId, IEnumerable<byte[]> binaries);
    }
}