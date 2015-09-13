using System.Collections.Generic;
using System.Net.Http;
using Tweetinvi.Core.Extensions;
using Tweetinvi.Core.Interfaces.Models;
using Tweetinvi.Core.Interfaces.WebLogic;

namespace Tweetinvi.WebLogic
{
    public class TwitterRequester : ITwitterRequester
    {
        private readonly IWebRequestExecutor _webRequestExecutor;

        public TwitterRequester(IWebRequestExecutor webRequestExecutor)
        {
            _webRequestExecutor = webRequestExecutor;
        }

        public string ExecuteQuery(ITwitterQuery twitterQuery, TwitterClientHandler handler = null)
        {
            return _webRequestExecutor.ExecuteQuery(twitterQuery, handler);
        }

        public string ExecuteMultipartQuery(ITwitterQuery twitterQuery, string contentId, IEnumerable<byte[]> binaries)
        {
            if (binaries.IsNullOrEmpty())
            {
                return ExecuteQuery(twitterQuery);
            }

            return _webRequestExecutor.ExecuteMultipartQuery(twitterQuery, contentId, binaries);
        }
    }
}