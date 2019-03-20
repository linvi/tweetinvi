using System;

namespace Tweetinvi.Events
{
    /// <summary>
    /// An exception that could not be handled by Tweetinvi was thrown
    /// Please report such errors on github
    /// </summary>
    public class UnexpectedExceptionThrownEventArgs : EventArgs
    {
        public Exception Exception { get; }

        public UnexpectedExceptionThrownEventArgs(Exception exception)
        {
            Exception = exception;
        }
    }
}
