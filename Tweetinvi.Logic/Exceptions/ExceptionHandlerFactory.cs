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
                // Note: it is safe to set _exceptionHandler.Value without locking, as even though it could be accessed
                //  concurrently from multiple places within an async flow, it can only be set in one.
                //  This is because the execution context doesn't get copied back to the calling thread.
                //
                // Example:
                //  Method A starts method B asynchronously without an ExceptionHandler in the execution context.
                //  Method A doesn't await method B, so they both run concurrently.
                //  Both call IExceptionHandlerFactory.Create()
                //  Method B has a shallow copy of the execution context of method A, and method A didn't have
                //      an ExceptionHandler, so neither method has an ExceptionHandler.
                //  An ExceptionHandler is made for method A and stored in its Execution Context
                //  An ExceptionHandler is made for method B and stored in its Execution Context
                //  Despite being in the same async flow, they each get their own ExceptionHandler, so there's no locking
                //  required.
                //
                // Note: the above example also demonstrates why external calls must use the methods in Sync
                //  which would ensure an ExceptionHandler exists in A before spawning B, thus sharing them.
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
