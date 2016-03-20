using System;

namespace Tweetinvi.Core.Exceptions
{
    public interface ISingleAggregateExceptionThrower
    {
        void ExecuteActionAndThrowJustOneExceptionIfExist(Action action);
    }

    public class SingleAggregateExceptionThrower : ISingleAggregateExceptionThrower
    {
        public void ExecuteActionAndThrowJustOneExceptionIfExist(Action action)
        {
            try
            {
                action();
            }
            catch (AggregateException aex)
            {
                if (aex.InnerException != null && aex.InnerExceptions.Count == 1)
                {
                    var expectedExceptionType = aex.InnerExceptions[0];
                    throw expectedExceptionType;
                }

                throw;
            }
        }
    }
}