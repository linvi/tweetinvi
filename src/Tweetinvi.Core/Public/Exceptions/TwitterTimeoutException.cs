using System;
using Tweetinvi.Core.Exceptions;
using Tweetinvi.Models;

namespace Tweetinvi.Exceptions
{
    public interface ITwitterTimeoutException : ITwitterException
    {
    }

    /// <summary>
    /// Exception raised when Twitter did not manage to respond to your request on time.
    /// </summary>
    public class TwitterTimeoutException : TwitterException, ITwitterTimeoutException
    {
        public TwitterTimeoutException(ITwitterRequest request, Exception e)
            : base(request, $"{request.Query.Url} request timed out.", e)
        {
            TwitterDescription = $"Twitter was not able to perform your query within the Timeout limit of {request.Query.Timeout.TotalMilliseconds}ms.";
            WebException = null;
            StatusCode = 408;
            TwitterExceptionInfos = new ITwitterExceptionInfo[0];
        }
    }
}