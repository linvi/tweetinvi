using System;
using System.Threading.Tasks;
using Tweetinvi.Core.Web;
using Tweetinvi.Models;

namespace Tweetinvi.Client.Requesters
{
    public interface IInternalExecuteRequester : IExecuteRequester, IBaseRequester
    {
    }

    public class ExecuteRequester : BaseRequester, IInternalExecuteRequester
    {
        private readonly ITwitterAccessor _accessor;

        public ExecuteRequester(ITwitterAccessor accessor)
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
    }
}