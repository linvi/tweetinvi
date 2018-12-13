using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using Tweetinvi.Core.Exceptions;
using Tweetinvi.Core.ExecutionContext;
using Tweetinvi.Exceptions;

namespace Tweetinvi.Logic.Exceptions
{
    public class ExceptionHandlerFactory : IExceptionHandlerFactory, ICrossExecutionContextPreparable
    {
        private static readonly AsyncLocal<IExceptionHandler> _exceptionHandler = new AsyncLocal<IExceptionHandler>();

        private readonly ITwitterExceptionFactory _twitterExceptionFactory;

        public ExceptionHandlerFactory(ITwitterExceptionFactory twitterExceptionFactory)
        {
            _twitterExceptionFactory = twitterExceptionFactory;
        }

        public IExceptionHandler Create()
        {
            if (_exceptionHandler.Value == null)
            {
                // TODO: Lock on creation??
                // Theoretically, we could be making the exception handler at two times within the same async flow on two different threads...
                // Also check what happens here as the pointer will change & it may be that the pointer doesn't propagate back upwards.
                _exceptionHandler.Value = new ExceptionHandler(_twitterExceptionFactory);
            }

            return _exceptionHandler.Value;
        }

        public void PrepareExecutionContext()
        {
            Create();
        }
    }
}
