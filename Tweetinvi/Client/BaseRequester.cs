using System;
using System.Threading.Tasks;
using Tweetinvi.Exceptions;
using Tweetinvi.Models.Interfaces;

namespace Tweetinvi.Client
{
    public abstract class BaseRequester
    {
        protected async Task<T> ExecuteRequest<T>(Func<Task<T>> action, ITwitterRequest request) where T : class
        {
            try
            {
                return await action();
            }
            catch (TwitterException)
            {
                if (request.Config.ErrorHandlerType == ErrorHandlerType.ReturnNull)
                {
                    return null;
                }

                throw;
            }
        }
    }
}
