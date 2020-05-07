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

        public Task<ITwitterResult<T>> AdvanceRequestAsync<T>(Action<ITwitterRequest> configureRequest) where T : class
        {
            return _executeRequester.RequestAsync<T>(configureRequest);
        }

        public Task<ITwitterResult> AdvanceRequestAsync(Action<ITwitterRequest> configureRequest)
        {
            return _executeRequester.RequestAsync(configureRequest);
        }

        public Task<ITwitterResult<T>> RequestAsync<T>(Action<ITwitterQuery> configureQuery) where T : class
        {
            return _executeRequester.RequestAsync<T>(configureQuery);
        }

        public Task<ITwitterResult> RequestAsync(Action<ITwitterQuery> configureQuery)
        {
            return _executeRequester.RequestAsync(configureQuery);
        }

        public Task<ITwitterRequest> PrepareTwitterRequestAsync(Action<ITwitterQuery> configureQuery)
        {
            return _executeRequester.PrepareTwitterRequestAsync(configureQuery);
        }

        public Task<ITwitterRequest> PrepareTwitterRequestAsync(Action<ITwitterRequest> configureRequest)
        {
            return _executeRequester.PrepareTwitterRequestAsync(configureRequest);
        }
    }
}