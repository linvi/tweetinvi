using System;
using System.Threading.Tasks;
using Tweetinvi.Core.Iterators;
using Tweetinvi.Core.Web;
using Tweetinvi.Models;
using Tweetinvi.Models.DTO;
using Tweetinvi.Parameters;

namespace Tweetinvi.Client
{
    public interface IBaseRequester
    {
        void Initialize(ITwitterClient client);
    }

    public abstract class BaseRequester : IBaseRequester
    {
        protected ITwitterClient _twitterClient;

        public void Initialize(ITwitterClient client)
        {
            if (_twitterClient != null)
            {
                throw new InvalidOperationException("createRequest cannot be changed");
            }

            _twitterClient = client;
        }

        protected async Task<T> ExecuteRequest<T>(Func<Task<T>> action, ITwitterRequest request) where T : class
        {
            try
            {
                return await action();
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
