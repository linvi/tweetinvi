using System.Collections.Generic;
using System.Net;
using Tweetinvi.Core.Interfaces.DTO;
using Tweetinvi.Core.Interfaces.Models;

namespace Tweetinvi.Core.Interfaces.WebLogic
{
    public interface ITwitterRequestGenerator
    {
        /// <summary>
        /// Get the HttpWebRequest expected from the given parameters
        /// </summary>
        HttpWebRequest GetQueryWebRequest(ITwitterQuery twitterQuery);

        /// <summary>
        /// Get the Multipart HttpWebRequest to execute for publishing medias
        /// </summary>
        IMultipartWebRequest GenerateMultipartWebRequest(ITwitterQuery twitterQuery, string contentId, IEnumerable<IMedia> medias);
    }
}
