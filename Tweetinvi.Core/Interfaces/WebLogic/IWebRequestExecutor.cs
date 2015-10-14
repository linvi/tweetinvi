using System.Collections.Generic;
using Tweetinvi.Core.Interfaces.Models;
using Tweetinvi.Core.Web;

namespace Tweetinvi.Core.Interfaces.WebLogic
{
    /// <summary>
    /// Generate a Token that can be used to perform OAuth queries
    /// </summary>
    public interface IWebRequestExecutor
    {
        string ExecuteQuery(ITwitterQuery twitterQuery, ITwitterClientHandler handler = null);
        string ExecuteMultipartQuery(ITwitterQuery twitterQuery, string contentId, IEnumerable<byte[]> binaries);
    }
}