using System;
using Tweetinvi.Models.Interfaces;

namespace Tweetinvi.Exceptions
{
    public class TwitterRequestException : Exception
    {
        public ITwitterRequest Request { get; }
        public Exception Exception { get; }

        public TwitterRequestException(ITwitterRequest request, Exception exception) : base(exception.Message)
        {
            Request = request;
            Exception = exception;
        }
    }
}
