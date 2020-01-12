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

        public ExecuteClient(ITwitterClient client)
        {
            _executeRequester = client.RequestExecutor.Execute;
        }

        public Task<ITwitterResult<T>> Request<T>(Action<ITwitterRequest> configureRequest) where T : class
        {
            return _executeRequester.Request<T>(configureRequest);
        }

        public Task<ITwitterResult> Request(Action<ITwitterRequest> configureRequest)
        {
            return _executeRequester.Request(configureRequest);
        }
    }
}