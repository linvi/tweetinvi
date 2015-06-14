using System.Collections.Generic;
using Tweetinvi.Core.Interfaces.DTO;
using Tweetinvi.Core.Interfaces.Models;

namespace Tweetinvi.Core.Interfaces.WebLogic
{
    public interface ITwitterRequester
    {
        string ExecuteQuery(ITwitterQuery twitterQuery);
        string ExecuteMultipartQuery(ITwitterQuery twitterQuery, string contentId, IEnumerable<IMedia> medias);
    }
}