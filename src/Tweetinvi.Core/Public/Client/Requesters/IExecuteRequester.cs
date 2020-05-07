using System;
using System.Threading.Tasks;
using Tweetinvi.Core.Web;
using Tweetinvi.Models;

namespace Tweetinvi.Client.Requesters
{
    public interface IExecuteRequester
    {
        Task<ITwitterResult<T>> RequestAsync<T>(Action<ITwitterRequest> configureRequest) where T : class;
        Task<ITwitterResult> RequestAsync(Action<ITwitterRequest> configureRequest);

        Task<ITwitterResult<T>> RequestAsync<T>(Action<ITwitterQuery> configureQuery) where T : class;
        Task<ITwitterResult> RequestAsync(Action<ITwitterQuery> configureQuery);

        Task<ITwitterRequest> PrepareTwitterRequestAsync(Action<ITwitterQuery> configureQuery);
        Task<ITwitterRequest> PrepareTwitterRequestAsync(Action<ITwitterRequest> configureRequest);
    }
}