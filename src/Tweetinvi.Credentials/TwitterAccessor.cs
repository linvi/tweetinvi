using System.Threading.Tasks;
using Tweetinvi.Core.Web;
using Tweetinvi.Models;
using Tweetinvi.WebLogic;

namespace Tweetinvi.Credentials
{
    public class TwitterAccessor : ITwitterAccessor
    {
        private readonly ITwitterRequestHandler _twitterRequestHandler;
        private readonly ITwitterResultFactory _twitterResultFactory;

        public TwitterAccessor(ITwitterRequestHandler twitterRequestHandler, ITwitterResultFactory twitterResultFactory)
        {
            _twitterRequestHandler = twitterRequestHandler;
            _twitterResultFactory = twitterResultFactory;
        }

        public async Task<ITwitterResult> ExecuteRequest(ITwitterRequest request)
        {
            var response = await _twitterRequestHandler.ExecuteQuery(request).ConfigureAwait(false);
            return _twitterResultFactory.Create(request, response);
        }

        public async Task<ITwitterResult<T>> ExecuteRequest<T>(ITwitterRequest request) where T : class
        {
            var response = await _twitterRequestHandler.ExecuteQuery(request).ConfigureAwait(false);
            return _twitterResultFactory.Create<T>(request, response);
        }

        // Download
        public async Task<byte[]> DownloadBinary(ITwitterRequest request)
        {
            var response = await _twitterRequestHandler.ExecuteQuery(request).ConfigureAwait(false);
            if (response.IsSuccessStatusCode)
            {
                return response.Binary;
            }

            return null;
        }

        // Sign
        public Task PrepareTwitterRequest(ITwitterRequest request)
        {
            return _twitterRequestHandler.PrepareTwitterRequest(request);
        }
    }
}