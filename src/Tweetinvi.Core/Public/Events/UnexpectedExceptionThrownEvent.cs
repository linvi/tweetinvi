using System;

namespace Tweetinvi.Events
{
    /// <summary>
    /// An exception that could not be handled by Tweetinvi was thrown
    /// Please report such errors on github
    /// </summary>
    public class UnexpectedExceptionThrownEvent
    {
        public Exception Exception { get; }

        public UnexpectedExceptionThrownEvent(Exception exception)
        {
            Exception = exception;
        }
    }
}
