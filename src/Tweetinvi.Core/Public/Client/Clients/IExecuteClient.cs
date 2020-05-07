using System;
using System.Threading.Tasks;
using Tweetinvi.Core.Web;
using Tweetinvi.Models;

namespace Tweetinvi.Client
{
    public interface IExecuteClient
    {
        Task<ITwitterResult<T>> AdvanceRequestAsync<T>(Action<ITwitterRequest> configureRequest) where T : class;
        Task<ITwitterResult> AdvanceRequestAsync(Action<ITwitterRequest> configureRequest);

        Task<ITwitterResult<T>> RequestAsync<T>(Action<ITwitterQuery> configureQuery) where T : class;
        Task<ITwitterResult> RequestAsync(Action<ITwitterQuery> configureQuery);
    }
}