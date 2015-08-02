using System;
using Tweetinvi.Core.Credentials;

namespace Tweetinvi.Core.Exceptions
{
    public class TwitterInvalidCredentialsException : Exception
    {
        public IConsumerCredentials Credentials { get; set; }

        public TwitterInvalidCredentialsException(IConsumerCredentials credentials) 
            : base("The consumer key and consumer secret must be defined!")
        {
            Credentials = credentials;
        }
    }
}