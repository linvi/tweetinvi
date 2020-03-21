using System;
using System.Threading.Tasks;
using Tweetinvi.Core.Web;
using Tweetinvi.Models;

namespace Tweetinvi.Client
{
    public interface IExecuteClient
    {
        Task<ITwitterResult<T>> AdvanceRequest<T>(Action<ITwitterRequest> configureRequest) where T : class;
        Task<ITwitterResult> AdvanceRequest(Action<ITwitterRequest> configureRequest);
        Task<ITwitterResult<T>> Request<T>(Action<ITwitterQuery> configureQuery) where T : class;
        Task<ITwitterResult> Request(Action<ITwitterQuery> configureQuery);
    }
}