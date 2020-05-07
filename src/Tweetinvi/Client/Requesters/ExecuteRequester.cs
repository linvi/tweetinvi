using System;
using System.Threading.Tasks;
using Tweetinvi.Core.Events;
using Tweetinvi.Core.Web;
using Tweetinvi.Models;

namespace Tweetinvi.Client.Requesters
{
    public class ExecuteRequester : BaseRequester, IExecuteRequester
    {
        private readonly ITwitterAccessor _accessor;

        public ExecuteRequester(
            ITwitterClient client,
            ITwitterClientEvents clientEvents,
            ITwitterAccessor accessor)
        : base(client, clientEvents)
        {
            _accessor = accessor;
        }

        public Task<ITwitterResult<T>> RequestAsync<T>(Action<ITwitterRequest> configureRequest) where T : class
        {
            return ExecuteRequestAsync(request =>
            {
                configureRequest(request);
                return _accessor.ExecuteRequestAsync<T>(request);
            });
        }

        public Task<ITwitterResult> RequestAsync(Action<ITwitterRequest> configureRequest)
        {
            return ExecuteRequestAsync(request =>
            {
                configureRequest(request);
                return _accessor.ExecuteRequestAsync(request);
            });
        }

        public Task<ITwitterResult<T>> RequestAsync<T>(Action<ITwitterQuery> configureQuery) where T : class
        {
            return ExecuteRequestAsync(request =>
            {
                configureQuery(request.Query);
                return _accessor.ExecuteRequestAsync<T>(request);
            });
        }

        public Task<ITwitterResult> RequestAsync(Action<ITwitterQuery> configureQuery)
        {
            return ExecuteRequestAsync(request =>
            {
                configureQuery(request.Query);
                return _accessor.ExecuteRequestAsync(request);
            });
        }

        public Task<ITwitterRequest> PrepareTwitterRequestAsync(Action<ITwitterQuery> configureQuery)
        {
            return ExecuteRequestAsync(async request =>
            {
                configureQuery(request.Query);
                await _accessor.PrepareTwitterRequestAsync(request).ConfigureAwait(false);
                return request;
            });
        }

        public Task<ITwitterRequest> PrepareTwitterRequestAsync(Action<ITwitterRequest> configureRequest)
        {
            return ExecuteRequestAsync(async request =>
            {
                configureRequest(request);
                await _accessor.PrepareTwitterRequestAsync(request).ConfigureAwait(false);
                return request;
            });
        }
    }
}