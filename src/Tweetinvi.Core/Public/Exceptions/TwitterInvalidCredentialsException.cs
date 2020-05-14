using System;
using Tweetinvi.Core.Models.Authentication;
using Tweetinvi.Models;

namespace Tweetinvi.Exceptions
{
    /// <summary>
    /// Exception raised when attempting to perform operations without the proper credentials
    /// </summary>
    public class TwitterInvalidCredentialsException : Exception
    {

        public IReadOnlyConsumerCredentials Credentials { get; }

        public TwitterInvalidCredentialsException(string message): base(message)
        {
        }

        public TwitterInvalidCredentialsException(IReadOnlyConsumerCredentials credentials)
            : base("The consumer key and consumer secret must be defined!")
        {
            Credentials = credentials;
        }
    }
}