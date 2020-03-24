using System;
using Tweetinvi.Core.Web;

namespace Tweetinvi.Exceptions
{
    public class TwitterResponseException : Exception
    {
        public ITwitterResult TwitterResult { get; }

        public TwitterResponseException(ITwitterResult twitterResult)
        {
            TwitterResult = twitterResult;
        }

        public TwitterResponseException(ITwitterResult twitterResult, string message) : base(message)
        {
            TwitterResult = twitterResult;
        }
    }
}