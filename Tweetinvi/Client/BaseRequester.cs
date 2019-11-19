using System;
using System.Threading.Tasks;
using Tweetinvi.Models;

namespace Tweetinvi.Client
{
    public interface IBaseRequester
    {
        void Initialize(ITwitterClient client);
    }

    public abstract class BaseRequester : IBaseRequester
    {
        protected ITwitterClient TwitterClient { get; private set; }

        public void Initialize(ITwitterClient client)
        {
            if (TwitterClient != null)
            {
                throw new InvalidOperationException("createRequest cannot be changed");
            }

            TwitterClient = client;
        }

        protected async Task<T> ExecuteRequest<T>(Func<Task<T>> action, ITwitterRequest request) where T : class
        {
            try
            {
                return await action().ConfigureAwait(false);
            }
            catch (Exception)
            {
                if (request.ExecutionContext.ErrorHandlerType == ErrorHandlerType.ReturnNull)
                {
                    return null;
                }

                throw;
            }
        }
    }
}
