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

        public async Task<ITwitterResult> ExecuteRequestAsync(ITwitterRequest request)
        {
            var response = await _twitterRequestHandler.ExecuteQueryAsync(request).ConfigureAwait(false);
            return _twitterResultFactory.Create(request, response);
        }

        public async Task<ITwitterResult<T>> ExecuteRequestAsync<T>(ITwitterRequest request) where T : class
        {
            var response = await _twitterRequestHandler.ExecuteQueryAsync(request).ConfigureAwait(false);
            return _twitterResultFactory.Create<T>(request, response);
        }

        // Sign
        public Task PrepareTwitterRequestAsync(ITwitterRequest request)
        {
            return _twitterRequestHandler.PrepareTwitterRequestAsync(request);
        }
    }
}