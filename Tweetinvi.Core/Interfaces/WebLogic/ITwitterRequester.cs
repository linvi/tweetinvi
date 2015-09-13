using System.Collections.Generic;
using Tweetinvi.Core.Interfaces.Models;
using Tweetinvi.WebLogic;

namespace Tweetinvi.Core.Interfaces.WebLogic
{
    public interface ITwitterRequester
    {
        string ExecuteQuery(ITwitterQuery twitterQuery, TwitterClientHandler handler = null);
        string ExecuteMultipartQuery(ITwitterQuery twitterQuery, string contentId, IEnumerable<byte[]> binaries);
    }
}