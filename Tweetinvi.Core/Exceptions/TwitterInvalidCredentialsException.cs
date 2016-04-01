using System;
using Tweetinvi.Core.Authentication;

namespace Tweetinvi.Core.Exceptions
{
    public class TwitterInvalidCredentialsException : Exception
    {
#pragma warning disable 108,114
        public string Message { get; set; }
#pragma warning restore 108,114

        public IConsumerCredentials Credentials { get; set; }

        public TwitterInvalidCredentialsException(string message)
        {
            Message = message;
        }

        public TwitterInvalidCredentialsException(IConsumerCredentials credentials) 
            : base("The consumer key and consumer secret must be defined!")
        {
            Credentials = credentials;
        }
    }
}