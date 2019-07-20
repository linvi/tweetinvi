using System;
using System.Threading.Tasks;
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
