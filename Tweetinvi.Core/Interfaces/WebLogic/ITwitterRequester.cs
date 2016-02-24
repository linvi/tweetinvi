using System.Collections.Generic;
using Tweetinvi.Core.Interfaces.Models;
using Tweetinvi.Core.Web;

namespace Tweetinvi.Core.Interfaces.WebLogic
{
    public interface ITwitterRequester
    {
        /// <summary>
        /// Prepare for the TwitterQuery to execute and then execute it to return the json data.
        /// </summary>
        string ExecuteQuery(ITwitterQuery twitterQuery, ITwitterClientHandler handler = null);

        /// <summary>
        /// Prepare for the multipart TwitterQuery to execute and then execute it to return the json data.
        /// </summary>
        string ExecuteMultipartQuery(ITwitterQuery twitterQuery, string contentId, IEnumerable<byte[]> binaries);
    }
}