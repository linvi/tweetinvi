using Tweetinvi.Core.Exceptions;
using Tweetinvi.Models;

namespace Tweetinvi.Exceptions
{
    public interface ITwitterTimeoutException : ITwitterException
    {
    }

    public class TwitterTimeoutException : TwitterException, ITwitterTimeoutException
    {
        public TwitterTimeoutException(ITwitterRequest request)
            : base(request, $"{request.Query.Url} request timed out.")
        {
            TwitterDescription = $"Twitter was not able to perform your query within the Timeout limit of {request.Query.Timeout.TotalMilliseconds} ms.";
            WebException = null;
            StatusCode = 408;
            TwitterExceptionInfos = new ITwitterExceptionInfo[0];;
        }
    }
}