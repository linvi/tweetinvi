using System;

namespace Tweetinvi.Exceptions
{
    public class TwitterAuthException : Exception
    {
        public TwitterAuthException(string message) : base(message)
        {
        }
    }
}