using System.Collections.Generic;
using System.Net.Http;
using Tweetinvi.Core.Web;
using Tweetinvi.Models;

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
        IWebRequestResult ExecuteQuery(ITwitterQuery twitterQuery, ITwitterClientHandler handler = null);

        /// <summary>
        /// Execute a multipart TwitterQuery and return the resulting json data.
        /// </summary>
        IWebRequestResult ExecuteMultipartQuery(ITwitterQuery twitterQuery, string contentId, IEnumerable<byte[]> binaries);
    }
}