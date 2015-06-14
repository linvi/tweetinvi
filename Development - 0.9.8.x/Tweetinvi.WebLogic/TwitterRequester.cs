using System.Collections.Generic;
using Tweetinvi.Core.Extensions;
using Tweetinvi.Core.Interfaces.DTO;
using Tweetinvi.Core.Interfaces.Models;
using Tweetinvi.Core.Interfaces.WebLogic;

namespace Tweetinvi.WebLogic
{
    public class TwitterRequester : ITwitterRequester
    {
        private readonly ITwitterRequestGenerator _twitterRequestGenerator;
        private readonly IWebRequestExecutor _webRequestExecutor;

        public TwitterRequester(
            ITwitterRequestGenerator twitterRequestGenerator,
            IWebRequestExecutor webRequestExecutor)
        {
            _twitterRequestGenerator = twitterRequestGenerator;
            _webRequestExecutor = webRequestExecutor;
        }

        public string ExecuteQuery(ITwitterQuery twitterQuery)
        {
            var webRequest = _twitterRequestGenerator.GetQueryWebRequest(twitterQuery);
            return _webRequestExecutor.ExecuteWebRequest(webRequest);
        }

        public string ExecuteMultipartQuery(ITwitterQuery twitterQuery, string contentId, IEnumerable<IMedia> medias)
        {
            if (medias == null || medias.IsEmpty())
            {
                return ExecuteQuery(twitterQuery);
            }

            var webRequest = _twitterRequestGenerator.GenerateMultipartWebRequest(twitterQuery, contentId, medias);
            var result = _webRequestExecutor.ExecuteMultipartRequest(webRequest);

            return result;
        }
    }
}