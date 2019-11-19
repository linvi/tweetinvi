using System;
using Tweetinvi.Models;

namespace Tweetinvi.Exceptions
{
    public class TwitterInvalidCredentialsException : Exception
    {

        public IConsumerCredentials Credentials { get; }

        public TwitterInvalidCredentialsException(string message): base(message)
        {
        }

        public TwitterInvalidCredentialsException(IConsumerCredentials credentials)
            : base("The consumer key and consumer secret must be defined!")
        {
            Credentials = credentials;
        }
    }
}