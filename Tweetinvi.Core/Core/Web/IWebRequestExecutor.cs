using System.Collections.Generic;
using System.Threading.Tasks;
using Tweetinvi.Models;

namespace Tweetinvi.Core.Web
{
    /// <summary>
    /// Generate a Token that can be used to perform OAuth queries
    /// </summary>
    public interface IWebRequestExecutor
    {
        /// <summary>
        /// Execute a TwitterQuery and return the resulting json data.
        /// </summary>
        Task<IWebRequestResult> ExecuteQuery(ITwitterQuery twitterQuery, ITwitterClientHandler handler = null);

        /// <summary>
        /// Execute a multipart TwitterQuery and return the resulting json data.
        /// </summary>
        Task<IWebRequestResult> ExecuteMultipartQuery(ITwitterQuery twitterQuery, string contentId, IEnumerable<byte[]> binaries);
    }
}