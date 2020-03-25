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

        public Task<ITwitterResult<T>> Request<T>(Action<ITwitterRequest> configureRequest) where T : class
        {
            return ExecuteRequest(request =>
            {
                configureRequest(request);
                return _accessor.ExecuteRequest<T>(request);
            });
        }

        public Task<ITwitterResult> Request(Action<ITwitterRequest> configureRequest)
        {
            return ExecuteRequest(request =>
            {
                configureRequest(request);
                return _accessor.ExecuteRequest(request);
            });
        }

        public Task<ITwitterResult<T>> Request<T>(Action<ITwitterQuery> configureQuery) where T : class
        {
            return ExecuteRequest(request =>
            {
                configureQuery(request.Query);
                return _accessor.ExecuteRequest<T>(request);
            });
        }

        public Task<ITwitterResult> Request(Action<ITwitterQuery> configureQuery)
        {
            return ExecuteRequest(request =>
            {
                configureQuery(request.Query);
                return _accessor.ExecuteRequest(request);
            });
        }

        public Task<ITwitterRequest> PrepareTwitterRequest(Action<ITwitterQuery> configureQuery)
        {
            return ExecuteRequest(async request =>
            {
                configureQuery(request.Query);
                await _accessor.PrepareTwitterRequest(request).ConfigureAwait(false);
                return request;
            });
        }

        public Task<ITwitterRequest> PrepareTwitterRequest(Action<ITwitterRequest> configureRequest)
        {
            return ExecuteRequest(async request =>
            {
                configureRequest(request);
                await _accessor.PrepareTwitterRequest(request).ConfigureAwait(false);
                return request;
            });
        }
    }
}