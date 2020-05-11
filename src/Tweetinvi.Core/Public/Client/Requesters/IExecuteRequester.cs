using System;
using System.Threading.Tasks;
using Tweetinvi.Core.Web;
using Tweetinvi.Models;

namespace Tweetinvi.Client.Requesters
{
    public interface IExecuteRequester
    {
        /// <summary>
        /// Execute a custom request
        /// </summary>
        /// <returns>The raw response from twitter with the json parsed into a Data Transfer Object</returns>
        Task<ITwitterResult<T>> RequestAsync<T>(Action<ITwitterRequest> configureRequest) where T : class;

        /// <summary>
        /// Execute a custom request
        /// </summary>
        /// <returns>The raw response from Twitter</returns>
        Task<ITwitterResult> RequestAsync(Action<ITwitterRequest> configureRequest);

        /// <summary>
        /// Execute a custom query
        /// </summary>
        /// <returns>The raw response from twitter with the json parsed into a Data Transfer Object</returns>
        Task<ITwitterResult<T>> RequestAsync<T>(Action<ITwitterQuery> configureQuery) where T : class;

        /// <summary>
        /// Execute a custom query
        /// </summary>
        /// <returns>The raw response from Twitter</returns>
        Task<ITwitterResult> RequestAsync(Action<ITwitterQuery> configureQuery);

        Task<ITwitterRequest> PrepareTwitterRequestAsync(Action<ITwitterQuery> configureQuery);
        Task<ITwitterRequest> PrepareTwitterRequestAsync(Action<ITwitterRequest> configureRequest);
    }
}