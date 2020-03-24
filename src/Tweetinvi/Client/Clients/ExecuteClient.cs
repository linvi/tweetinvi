using System;
using System.Threading.Tasks;
using Tweetinvi.Client.Requesters;
using Tweetinvi.Core.Web;
using Tweetinvi.Models;

namespace Tweetinvi.Client
{
    public class ExecuteClient : IExecuteClient
    {
        private readonly IExecuteRequester _executeRequester;

        public ExecuteClient(IExecuteRequester executeRequester)
        {
            _executeRequester = executeRequester;
        }

        public Task<ITwitterResult<T>> AdvanceRequest<T>(Action<ITwitterRequest> configureRequest) where T : class
        {
            return _executeRequester.Request<T>(configureRequest);
        }

        public Task<ITwitterResult> AdvanceRequest(Action<ITwitterRequest> configureRequest)
        {
            return _executeRequester.Request(configureRequest);
        }

        public Task<ITwitterResult<T>> Request<T>(Action<ITwitterQuery> configureQuery) where T : class
        {
            return _executeRequester.Request<T>(configureQuery);
        }

        public Task<ITwitterResult> Request(Action<ITwitterQuery> configureQuery)
        {
            return _executeRequester.Request(configureQuery);
        }
    }
}