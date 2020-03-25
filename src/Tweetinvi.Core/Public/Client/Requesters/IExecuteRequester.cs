using System;
using System.Threading.Tasks;
using Tweetinvi.Core.Web;
using Tweetinvi.Models;

namespace Tweetinvi.Client.Requesters
{
    public interface IExecuteRequester
    {
        Task<ITwitterResult<T>> Request<T>(Action<ITwitterRequest> configureRequest) where T : class;
        Task<ITwitterResult> Request(Action<ITwitterRequest> configureRequest);

        Task<ITwitterResult<T>> Request<T>(Action<ITwitterQuery> configureQuery) where T : class;
        Task<ITwitterResult> Request(Action<ITwitterQuery> configureQuery);

        Task<ITwitterRequest> PrepareTwitterRequest(Action<ITwitterQuery> configureQuery);
        Task<ITwitterRequest> PrepareTwitterRequest(Action<ITwitterRequest> configureRequest);
    }
}