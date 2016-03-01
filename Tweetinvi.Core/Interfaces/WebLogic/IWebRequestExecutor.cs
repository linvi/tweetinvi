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
        /// <summary>
        /// Execute a TwitterQuery and return the resulting json data.
        /// </summary>
        string ExecuteQuery(ITwitterQuery twitterQuery, ITwitterClientHandler handler = null);

        /// <summary>
        /// Execute a multipart TwitterQuery and return the resulting json data.
        /// </summary>
        string ExecuteMultipartQuery(ITwitterQuery twitterQuery, string contentId, IEnumerable<byte[]> binaries);
    }
}